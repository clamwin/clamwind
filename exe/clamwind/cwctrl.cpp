//-----------------------------------------------------------------------------
// Name:        cwctrl.cpp
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
#include <cwctrl.h>
#include <ioctl.h>
#include <ntdefs.h>

#define CHECK_RECORD(_size) \
    if(pBuf - buffer + _size > (INT) bytesReturned) \
    { \
        dbgprint(LOG_ALWAYS, L"Thread %u; Invalid Record\n", GetCurrentThreadId()); \
        SendResult(index, -1, -1, true); \
        continue; \
    }

cwCtrl::cwCtrl(bool impersonate) : m_hDevice(NULL), Present(false), Enabled(false), m_impersonate(impersonate)
{
    // Opening device
    dbgprint(LOG_ALWAYS, L"Opening device...\n");
    this->m_hDevice = CreateFile(L"\\\\.\\FileFilter",
        GENERIC_READ | GENERIC_WRITE,
        0,
        NULL,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        NULL);

    if (this->m_hDevice == INVALID_HANDLE_VALUE)
        dbgprint(LOG_ALWAYS, L"IFS Filter is not installed\n");
    else
        this->Present = true;
}

cwCtrl::~cwCtrl()
{
    if(this->m_hDevice) CloseHandle(this->m_hDevice);
}

HANDLE cwCtrl::OpenEvent(void)
{
    return ::OpenEvent(SYNCHRONIZE, FALSE, L"CreateFileEvent");
}


bool cwCtrl::GetNextFilename(std::wstring& filename, DWORD& index, DWORD& pid, DWORD& dbmajor, DWORD& dbminor)
{
    bool gotFile = false;
    // loop through records
    // and skip all but regular files
    while(true)
    {
        BYTE buffer[MAX_NAME_LENGTH * sizeof(wchar_t) + MAX_DEVICE_NAME * sizeof(wchar_t) +
                    4 * sizeof(ULONG) + 2 * sizeof(USHORT)];

        DWORD bytesReturned = 0;
        DWORD res;

        dbgprint(LOG_TRACE, L"Thread %u; Reading record...\n", GetCurrentThreadId());

        if(!DeviceIoControl(this->m_hDevice,
            FILTER_GETCREATERECORD,
            (PVOID) &res,
            sizeof(DWORD),
            buffer,
            sizeof(buffer),
            &bytesReturned,
            NULL))
        {
            dbgprint(LOG_ALWAYS, L"Thread %u; ERROR controlling device: 0x%x\n", GetCurrentThreadId(), GetLastError());
            break;
        }

        if (bytesReturned == 0)
        {
            dbgprint(LOG_TRACE, L"Thread %u; Reading complete...no more records!\n\n", GetCurrentThreadId());
            break;
        }

        dbgprint(LOG_TRACE, L"Thread %u; Finished Reading record, read %u bytes\n", GetCurrentThreadId(), bytesReturned);

        USHORT devnameLen = 0, pathnameLen = 0;
        wchar_t devicename[MAX_DEVICE_NAME + 1];
        wchar_t pathname[MAX_NAME_LENGTH + 1];


        // record format as folows:
        // ULONG(RecordIndex)ULONG(RequestProcessID)ULONG(DBVersionMajor)ULONG(DBVersionMinor)USHORT(DeviceNameLength) \
        // PBYTE((PWCHAR)DeviceName)USHORT(FileNameLength)PBYTE((PWCHAR)FileName)

        // DBVersionMajor amd DBVersionMainor are 0 if a file has not been scanned yet
        // and contain a last scanned version if the db version has changed

        // process record
        PBYTE pBuf = buffer;

        CHECK_RECORD(sizeof(ULONG));
        memcpy(&index, pBuf, sizeof(ULONG));
        pBuf += sizeof(ULONG);

        CHECK_RECORD(sizeof(ULONG));
        memcpy(&pid, pBuf,sizeof(ULONG));
        pBuf += sizeof(ULONG);

        CHECK_RECORD(sizeof(ULONG));
        memcpy(&dbmajor, pBuf,sizeof(ULONG));
        pBuf += sizeof(ULONG);

        CHECK_RECORD(sizeof(ULONG));
        memcpy(&dbminor, pBuf,sizeof(ULONG));
        pBuf += sizeof(ULONG);

        CHECK_RECORD(sizeof(USHORT));
        memcpy(&devnameLen, pBuf, sizeof(USHORT));
        pBuf += sizeof(USHORT);

        CHECK_RECORD(devnameLen);
        memcpy(devicename, pBuf, devnameLen);
        devicename[devnameLen/sizeof(devicename[0])]=L'\0';
        pBuf += devnameLen;

        CHECK_RECORD(sizeof(USHORT));
        memcpy(&pathnameLen, pBuf, sizeof(USHORT));
        pBuf += sizeof(USHORT);

        CHECK_RECORD(pathnameLen);
        memcpy(pathname, pBuf, pathnameLen);
        pathname[pathnameLen/sizeof(pathname[0])]=L'\0';

        dbgprint(LOG_TRACE, L"Thread %u; Record: devicename=%s, pathname=%s, PID=%i, Index=%i, Major=%i, Minor=%i\n\n", GetCurrentThreadId(), devicename, pathname, pid, index, dbmajor, dbminor);

        bool impersonate = this->m_impersonate;
        if(impersonate)
        {
            impersonate = Impersonate(pid);
            if(!impersonate)
                dbgprint(LOG_ALWAYS, L"Thread %u; Impersonation failed: devicename=%s, pathname=%s, PID=%i. LastError: %x\n", GetCurrentThreadId(), devicename, pathname, pid, GetLastError());
        }
        // now check if the object is a regular file (not a volume, directory or pipe, etc)
        bool bRegular = IsRegularFile(devicename, pathname);
        if(impersonate)
            RevertToSelf();

        if(!bRegular)
        {
             SendResult(index, -1, -1, true);
             continue;
        }

        // get the normal usermode filename
        wchar_t name[MAX_DEVICE_NAME + MAX_NAME_LENGTH + 1];
        if(!GetCanonicalFilename(devicename, pathname, name))
        {
             dbgprint(LOG_ALWAYS, L"Thread %u; Can't get filename for: %s%s\n", GetCurrentThreadId(), devicename, pathname);
             SendResult(index, -1, -1, true);
             continue;
        }

        dbgprint(LOG_TRACE, L"Thread: %u; filename=name: %s\n", GetCurrentThreadId(), name);
        filename = std::wstring(name);
        gotFile = true;
        break;
    }
    return gotFile;
}

bool cwCtrl::SendResult(DWORD index, int main, int daily, bool allowed)
{
    // Input: ULONG(Index)ULONG(Major)ULONG(Minor)

    DWORD unused;
    ULONG data[3] = {index, main, daily};

    dbgprint(LOG_TRACE, L"SendResult -- allowed: %i, Thread: %u; Index: 0x%x\n", allowed, GetCurrentThreadId(), index);
    if (!DeviceIoControl(this->m_hDevice,
        (allowed ? FILTER_ALLOWED : FILTER_DENIED),
        data,
        sizeof(data),
        0,
        0,
        &unused,
        NULL))
    {
        dbgprint(LOG_ALWAYS, L"SendResult -- Thread: %u; ERROR controlling device: 0x%x\n", GetCurrentThreadId(), GetLastError());
        return false;
    }
    return true;
}

bool cwCtrl::StopFiltering(void)
{
    DWORD unused;
    if (!this->Enabled) return false;

    if (!DeviceIoControl(this->m_hDevice,
        FILTER_STOPFILTERING,
        NULL,
        0,
        NULL,
        0,
        &unused,
        NULL
        ))
    {
        dbgprint(LOG_ALWAYS, L"StopFiltering -- Thread: %u; ERROR controlling device: 0x%x\n", GetCurrentThreadId(), GetLastError());
        return false;
    }
    this->Enabled = false;
    return true;
}

bool cwCtrl::StartFiltering(bool bEnableCache, int main, int daily)
{
    ULONG data[3] = { bEnableCache ? 1 : 0, main, daily };
    ULONG unused;

    if (this->Enabled) return false;

    if (!DeviceIoControl(this->m_hDevice,
        FILTER_STARTFILTERING,
        data,
        sizeof(data),
        NULL,
        0,
        &unused,
        NULL))
    {
          dbgprint(LOG_ALWAYS, L"StartFiltering -- Thread: %u; ERROR controlling device: 0x%x\n", GetCurrentThreadId(), GetLastError());
        return false;
    }
    this->Enabled = true;
    return true;
}

bool cwCtrl::SetDatabaseVersion(int main, int daily)
{
    ULONG data[2] = {main, daily};
    ULONG unused;

    if (!DeviceIoControl(this->m_hDevice,
        FILTER_SETDATABASEVERSION,
        data,
        sizeof(data),
        NULL,
        0,
        &unused,
        NULL))
    {
        dbgprint(LOG_ALWAYS, L"SetDatabaseVersion -- Thread: %u; ERROR controlling device: 0x%x\n", GetCurrentThreadId(), GetLastError());
        return false;
    }
    return true;
}

bool cwCtrl::EnableCaching(bool bEnable)
{
    ULONG data = bEnable ? 1 : 0;
    ULONG unused;

    if (!DeviceIoControl(this->m_hDevice,
        FILTER_SETCACHEENABLE,
        &data,
        sizeof(data),
        NULL,
        0,
        &unused,
        NULL))
    {
        dbgprint(LOG_ALWAYS, L"EnableCaching -- Thread: %u; ERROR controlling device: 0x%x\n", GetCurrentThreadId(), GetLastError());
        return false;
    }
    return true;
}

/* CClamWinD */

DWORD WINAPI CClamWinD::HandleIOCTL(LPVOID lParam)
{
    CClamWinD* pThis = static_cast<CClamWinD*>(lParam);
    BOOL bStop = FALSE;
    HANDLE hEventPool[2];

    dbgprint(LOG_ALWAYS, L"IOCTL Server :: Starting...\n");
    if ((hEventPool[0] = pThis->Filter->OpenEvent()) == INVALID_HANDLE_VALUE)
    {
        dbgprint(LOG_ALWAYS, L"IOCTL Server :: Could not open Filter Event. Exiting...\n");
        return 1;
    }

    hEventPool[1] = pThis->hStopEvent;

    /* Waiting for "need check" events */
    while (bStop == FALSE)
    {
        switch (WaitForMultipleObjects(2, hEventPool, FALSE, INFINITE))
        {
            case WAIT_OBJECT_0:
            {
                DWORD dwIndex=0, dwPID=0, dbMajor=0, dbMinor=0;
                std::wstring path;
                /* reading all records in the queue until GetFilename returns last record */
                while(pThis->Filter->GetNextFilename(path, dwIndex, dwPID, dbMajor, dbMinor))
                {
                    job_t iocjob;
                    iocjob.type = JOB_IOCTL;
                    iocjob.dwIndex = dwIndex;
                    iocjob.dwPID = dwPID;
                    iocjob.dwDBMajor = dbMajor;
                    iocjob.dwDBMinor = dbMinor;
                    iocjob.filename = path;

                    pThis->jobs.push(iocjob);
                    ReleaseSemaphore(pThis->sem_worker, 1, NULL);
                }
                break;
            }
            case WAIT_OBJECT_0 + 1:
                bStop = TRUE;
                break;
        }
    }
    CloseHandle(hEventPool[0]);
    dbgprint(LOG_ALWAYS, L"IOCTL Server :: Done...\n");
    return 0;
}
