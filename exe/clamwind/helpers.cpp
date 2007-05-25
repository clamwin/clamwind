//-----------------------------------------------------------------------------
// Name:        helpers.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2005/12/15
// Copyright:   Copyright ClamWin Pty Ltd (c) 2005-2006
// Licence:
//   This program is free software; you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation; either version 2 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program; if not, write to the Free Software
//   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA

#include <clamwind.h>
#include <ntdefs.h>

static HMODULE hNtDll = NULL;
static pfnRtlAppendUnicodeToString pRtlAppendUnicodeToString = NULL;
static pfnZwClose pZwClose = NULL;
static pfnZwCreateFile pZwCreateFile = NULL;

#define GetProc(name, lib) p##name = (pfn##name) GetProcAddress(lib, #name)

bool InitNtFunctions(void)
{
    if (!(hNtDll = LoadLibrary(L"ntdll.dll"))) return false;

    GetProc(RtlAppendUnicodeToString, hNtDll);
    GetProc(ZwClose, hNtDll);
    GetProc(ZwCreateFile, hNtDll);

    if (pRtlAppendUnicodeToString && pZwClose && pZwCreateFile)
        return true;

    return false;
}

void UnintNtFunctions(void)
{
    if (hNtDll) FreeLibrary(hNtDll);
}

bool IsSmall(HANDLE hFile)
{
    DWORD size_low = 0, size_high = 0;
    size_low = GetFileSize(hFile, &size_high);
    if (!size_high && (size_low < MIN_FILE_SIZE)) return true;
    return false;
}

size_t GetOurDir(LPWSTR szPath, DWORD cbPath)
{
    wchar_t szDrive[_MAX_DRIVE], szDir[_MAX_DIR], szFile[_MAX_FNAME], szExt[_MAX_EXT];
    if(!GetModuleFileName(NULL, szPath, cbPath))
        return 0;
    _wsplitpath(szPath, szDrive, szDir, szFile, szExt);
    wcsncpy(szPath, szDrive, cbPath);
    wcsncat(szPath, szDir, cbPath);
    // remove last slash
    size_t len = wcslen(szPath);
    if(szPath[len-1] == L'\\')
    {
        szPath[len-1] = L'\0';
        --len;
    }
    return len;
}

bool IsRegularFile(LPCWSTR deviceName, LPCWSTR pathName)
{
    dbgprint(LOG_TRACE, L"Thread:%i; IsRegularFile for %s%s\n", GetCurrentThreadId(), deviceName, pathName);
    // check if file name ends with \, then it is a directory
    if(pathName[wcslen(pathName) - 1] == L'\\')
        return false;

    // buffered unicode string
    wchar_t nameBuffer[384];
    UNICODE_STRING name;
    name.Buffer = nameBuffer;
    name.MaximumLength = sizeof(nameBuffer);
    name.Length = 0;

    // concatenate device name and file name
    pRtlAppendUnicodeToString(&name, deviceName);
    pRtlAppendUnicodeToString(&name, pathName);

    OBJECT_ATTRIBUTES oa;
    InitializeObjectAttributes(&oa, &name, OBJ_CASE_INSENSITIVE, 0, 0);

    NTSTATUS rc;
    IO_STATUS_BLOCK ioStatus;
    HANDLE hFile;

    // check if the file is a regular file

    // we do not use FILE_NON_DIRECTORY_FILE CreateFlag here
    // as  ZwCreateFile would return success on a named pipe alias (i.e \\machine\PIPE\wkssvc)
    // it also returns success with FILE_DIRECTORY_FILE CreateFlag on such filename

    // there is a bug on vmware when you attempt to read from a named pipe alias (i.e \\machine\PIPE\wkssvc)
    // which has been opened as a file (FILE_DIRECTORY_FILE not set) then it hangs for couple of minutes.
    // therefore we have to open file as directory and if we succeed than the file is a directory and hence
    // not a regular file
    dbgprint(LOG_TRACE, L"Thread:%i; Calling ZwCreateFile for %s%s\n", GetCurrentThreadId(), deviceName, pathName);
    rc = pZwCreateFile(
        &hFile,
        FILE_READ_DATA,
        &oa,
        &ioStatus,
        0,
        FILE_ATTRIBUTE_NORMAL,
        FILE_SHARE_DELETE | FILE_SHARE_READ | FILE_SHARE_WRITE,
        FILE_OPEN,
        FILE_DIRECTORY_FILE,
        0, 0 );

    switch(rc)
    {
    case STATUS_SUCCESS:
    case STATUS_SHARING_VIOLATION:
    case STATUS_DELETE_PENDING:
    case STATUS_OBJECT_PATH_INVALID:
    case STATUS_OBJECT_PATH_NOT_FOUND:
        // file is a directory
        dbgprint(LOG_TRACE, L"Thread:%i; %s%s is NOT a regular file. rc=%x\n", GetCurrentThreadId(), deviceName, pathName, rc);
        pZwClose(hFile);
        return false;
    default: break;
    }
    dbgprint(LOG_TRACE, L"Thread:%i; %s%s is a regular file. rc=%x\n", GetCurrentThreadId(), deviceName, pathName, rc);
    return true;
}

BOOL GetNetworkDriveSharePath(LPCTSTR szDrive, LPTSTR szShare, PDWORD pdwShareSize)
{
    DWORD dwResult;

    dwResult = WNetGetConnection(szDrive, (LPTSTR) szShare, pdwShareSize);

    if(dwResult == NO_ERROR)
        return TRUE;
    else
    {
        SetLastError(dwResult);
        return FALSE;
    }
}

bool GetCanonicalFilename(LPCWSTR deviceName, LPCWSTR szFilename, LPWSTR szPath, DWORD cbPath)
{
    int i = 0;
    bool bSuccess = FALSE;
    const wchar_t szUNC[]= L"\\??\\UNC";

    dbgprint(LOG_TRACE, L"Thread %i; Getting canonical name for: %s:%s\n", GetCurrentThreadId(), deviceName, szFilename);
    if(wcsstr(deviceName, szUNC) == deviceName)
    {
        // network path
        const wchar_t szMappedDrive[] = L"\\;";
        if(wcsstr(szFilename, szMappedDrive) == szFilename)
        {
            // mapped disk
            bool bCopied = false;
            size_t pos = wcslen(szMappedDrive);
            //check if we are dealing with mapped drive (\\;X:\) then we
            // will use get the drive letter
            if(wcslen(szFilename) > pos)
            {
                wchar_t *pStart = wcschr((wchar_t *) &szFilename[pos], L'\\');
                if(pStart)
                {
                    wchar_t szTemp[512];
                    wcsncpy(szTemp, L"\\", sizeof(szTemp));
                    wcsncat(szTemp, pStart, sizeof(szTemp));
                    wcsncpy(szPath, szTemp, cbPath);
                    bCopied = true;
                }
            }
            return bCopied;
        }
        else
            wcsncpy(szPath, L"\\", cbPath);
    }
    else
    {
        dbgprint(LOG_TRACE, L"Thread %i; Querying device: %s\n", GetCurrentThreadId(), deviceName);
        wchar_t szDevice[MAX_PATH], szDrive[MAX_PATH];
        // get all available drive letters
        DWORD dwDrives = GetLogicalDrives();

        // loop through all 26 letters
        for(i = 0; i < 26; i++)
        {
            if(dwDrives & (1 << i))
            {
                _snwprintf(szDrive, MAX_PATH, L"%C:", i + 0x41);
                if(QueryDosDevice(szDrive, szDevice, sizeof(szDevice)))
                {
                    if(!_wcsicmp(szDevice, deviceName))
                        break;
                }
            }
        }
        if(i == 26)
        {
            dbgprint(LOG_INFO, L"Thread %i; Device Name: %s NOT FOUND\n", GetCurrentThreadId(), deviceName);
            return false;
        }
        wcsncpy(szPath, szDrive, cbPath);
    }

    // append back slash to the path
    if(szPath[wcslen(szPath)] != L'\\')
        wcsncat(szPath, L"\\", cbPath);
    // append filename, removing first slash if necessary
    if(szFilename[0] == L'\\')
        wcsncat(szPath, szFilename + 1, cbPath);
    else
        wcsncat(szPath, szFilename, cbPath);
    dbgprint(LOG_TRACE, L"Thread %i; Got canonical name: %s\n", GetCurrentThreadId(), szPath);
    return true;
}

bool Impersonate(DWORD dwPID)
{
    bool bImpersonated = false;

    HANDLE hToken = NULL, hProcess;
    hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, dwPID);

    if(hProcess)
        OpenProcessToken(hProcess, TOKEN_QUERY|TOKEN_IMPERSONATE|TOKEN_DUPLICATE, &hToken);
    if(hToken)
    {
        bImpersonated = !!ImpersonateLoggedOnUser(hToken);
        CloseHandle(hProcess);
        CloseHandle(hToken);
    }
    return bImpersonated;
}

DWORD GetCPUCount(void)
{
    SYSTEM_INFO sysinfo;
    memset(&sysinfo, 0, sizeof(sysinfo));
    GetSystemInfo(&sysinfo);
    return (sysinfo.dwNumberOfProcessors ? sysinfo.dwNumberOfProcessors : 1);
}

const char *scanresult_to_string(scan_res_t result)
{
    switch (result)
    {
        case SCAN_RES_SCANNING:
            return "SCANNING";
        case SCAN_RES_CLEAN:
            return "CLEAN";
        case SCAN_RES_ABORTED:
            return "ABORTED";
        case SCAN_RES_INFECTED:
            return "INFECTED";
        case SCAN_RES_ERROR:
            return "ERROR";
        case SCAN_RES_NOSUCHJOB:
            return "NOSUCHJOB";
        default:
            return "UNKNOWN";
    }
}

const char *scanresult_to_desc(scan_res_t result)
{
    switch (result)
    {
        case SCAN_RES_SCANNING:
            return "The file is being scanned";
        case SCAN_RES_CLEAN:
            return "The file is clean";
        case SCAN_RES_ABORTED:
            return "The job was aborted";
        case SCAN_RES_INFECTED:
            return "The file is infected";
        case SCAN_RES_ERROR:
            return "There was an error while scanning";
        case SCAN_RES_NOSUCHJOB:
            return "No jobid matching your request";
        default:
            return "Unknown result";
    }
}
