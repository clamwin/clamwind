//-----------------------------------------------------------------------------
// Name:        service.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//                 this file is based on "NT This Class" code by
//                 copyright 2003 Stefan Voitel - Berlin (stefan.voitel@winways.de)
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
#include <service.h>

namespace NsServiceBase
{
   CServiceBase* This;
};

void WINAPI CServiceBase::ServiceMain(int argc, wchar_t *argv[])
{
   using namespace NsServiceBase;
   This->m_StatusHandle = RegisterServiceCtrlHandler(This->GetName(), ServiceHandler);
    if (This->Succeeded(This->m_StatusHandle != NULL))
    {
        This->SetStatus(SERVICE_START_PENDING);

        if (This->Init())
        {
            This->SetStatus(SERVICE_RUNNING);
            This->ServiceProc();
            This->SetStatus(SERVICE_STOP_PENDING);
            This->Cleanup();
            This->SetStatus(SERVICE_STOPPED);
        }
        else This->SetStatus(SERVICE_STOPPED);
    }
    else This->SetStatus(SERVICE_STOPPED);
}

void WINAPI CServiceBase::ServiceHandler(DWORD control)
{
   using namespace NsServiceBase;
   switch(control)
   {
        case SERVICE_CONTROL_INTERROGATE:
            This->SetStatus(This->m_StatusCode);
            break;
        case SERVICE_CONTROL_STOP:
        case SERVICE_CONTROL_SHUTDOWN:
            This->SetStatus(SERVICE_STOP_PENDING);
            This->ShutDown();
            break;
    }
}

BOOL WINAPI CServiceBase::ConsoleHandler(DWORD dwCtrlType)
{
    using namespace NsServiceBase;
    switch(dwCtrlType)
    {
        case CTRL_LOGOFF_EVENT:
            This->LogoffEvent();
            break;
        default:
            This->ShutDown();
    }
    return true;
}

CServiceBase::CServiceBase(void) : m_StatusHandle(NULL), m_StatusCode(SERVICE_STOPPED)
{
    using namespace NsServiceBase;
    NsServiceBase::This = this;
    memset(m_ErrorString, 0, sizeof(m_ErrorString));
    Succeeded(!!SetConsoleCtrlHandler(ConsoleHandler, TRUE));
}

CServiceBase::~CServiceBase()
{
    if (m_EventLog) DeregisterEventSource(m_EventLog);
}

bool CServiceBase::Open(SC_HANDLE &hService)
{
    bool success = false;
    SC_HANDLE hSCM;

    if((hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS)) != NULL)
    {
        hService = OpenService(hSCM, GetName(), SERVICE_ALL_ACCESS);
        success = Succeeded(hService != NULL);
        CloseServiceHandle(hSCM);
    }
    else Succeeded(false);
    return success;
}

void CServiceBase::Close(SC_HANDLE hService)
{
    CloseServiceHandle(hService);
}

void  CServiceBase::SetStatus(DWORD status)
{
    m_StatusCode  = status;

    SERVICE_STATUS ss;
    ss.dwServiceType              = SERVICE_WIN32_OWN_PROCESS;
    ss.dwCurrentState             = m_StatusCode;
    ss.dwControlsAccepted         = SERVICE_ACCEPT_STOP|SERVICE_ACCEPT_SHUTDOWN;
    ss.dwWin32ExitCode            = NOERROR;
    ss.dwServiceSpecificExitCode  = NOERROR;
    ss.dwCheckPoint               = 0;
    ss.dwWaitHint                 = 3000;

    if(!Succeeded(!!SetServiceStatus(m_StatusHandle, &ss)))
        LogEvent(m_ErrorString,evWarning);
}

bool CServiceBase::Succeeded(bool success)
{
    DWORD SysError = success ? ERROR_SUCCESS : ::GetLastError();
    FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, NULL, SysError,
                  MAKELANGID(LANG_NEUTRAL,SUBLANG_DEFAULT),
                  m_ErrorString, sizeof(m_ErrorString), NULL);
    return (SysError == ERROR_SUCCESS);
}

bool CServiceBase::Execute(void)
{
    m_EventLog = RegisterEventSource(NULL, GetName());

    SERVICE_TABLE_ENTRY entries[2];
    entries[0].lpServiceName = (wchar_t *) GetName();
    entries[0].lpServiceProc = (LPSERVICE_MAIN_FUNCTION) ServiceMain;
    entries[1].lpServiceName = NULL;
    entries[1].lpServiceProc = NULL;

    if(!Succeeded(!!StartServiceCtrlDispatcher(entries)))
    {
        LogEvent(m_ErrorString, evError);
        return false;
    }
    return true;
}

bool CServiceBase::ConsoleMode(void)
{
    m_EventLog = RegisterEventSource(NULL, GetName());

    if (Init())
    {
        ServiceProc();
        Cleanup();
        return true;
    }
    return false;
}

bool CServiceBase::Start(void)
{
    bool success = false;
    SC_HANDLE hService;
    if(Open(hService))
    {
        success = Succeeded(!!StartService(hService, 0, NULL));
        Close(hService);
    }
    return success;
}

bool CServiceBase::Stop(void)
{
    bool success = false;
    SC_HANDLE hService;

    if(Open(hService))
    {
        SERVICE_STATUS state;
        success = Succeeded(!!ControlService(hService, SERVICE_CONTROL_STOP, &state));
        Close(hService);
    }
    return success;
}

bool CServiceBase::Remove(void)
{
    Stop();
    bool success = false;
    SC_HANDLE hService;

    if(Open(hService))
    {
        success = Succeeded(!!DeleteService(hService));
        Close(hService);

        HKEY hKey;
        if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, REG_EVENTLOG, 0, KEY_ALL_ACCESS, &hKey) == ERROR_SUCCESS)
        {
                RegDeleteKey(hKey, GetName());
                RegCloseKey(hKey);
        }
    }
    return success;
}

bool CServiceBase::Install(void)
{
    bool success = false;
    SC_HANDLE hService, hSCManager;
    wchar_t imagePath[MAX_PATH];

    GetModuleFileName(NULL, imagePath, MAX_PATH);
    if(Succeeded((hSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS))!= NULL))
    {
        if(Succeeded((hService = ::CreateService(hSCManager, GetName(), GetDisplayName(),
                                                 SERVICE_ALL_ACCESS,
                                                 SERVICE_WIN32_OWN_PROCESS | SERVICE_INTERACTIVE_PROCESS,
                                                 SERVICE_DEMAND_START, SERVICE_ERROR_NORMAL,
                                                 imagePath, NULL, NULL, NULL, NULL, NULL)) != NULL))
        {
            CloseServiceHandle(hService);
            success = true;
        }
        CloseServiceHandle(hSCManager);
    }

    if(success)
    {
        UINT f = EVENTLOG_ERROR_TYPE | EVENTLOG_WARNING_TYPE | EVENTLOG_INFORMATION_TYPE;
        wchar_t szKey[MAX_PATH];
        _snwprintf(szKey, MAX_PATH, L"%s\\%s", REG_EVENTLOG, GetName());

        HKEY hKey;
        if (RegCreateKey(HKEY_LOCAL_MACHINE, szKey, &hKey) == ERROR_SUCCESS)
        {
            wchar_t mod[MAX_PATH];
            DWORD len = GetModuleFileName(NULL, mod, MAX_PATH);
            RegSetValueEx(hKey, L"TypesSupported", 0, REG_DWORD, (BYTE *) &f, sizeof(DWORD));
            RegSetValueEx(hKey, L"EventMessageFile", 0, REG_SZ, (BYTE *) mod, len + 1);
            RegCloseKey(hKey);
        }
    }
    return success;
}

void CServiceBase::LogEvent(wchar_t *str, evLogType type, WORD Category)
{
    if(m_EventLog)
    {
        const wchar_t *msgs[1];
        msgs[0] = str;
        ReportEvent(m_EventLog, (WORD) type, Category, 0, NULL, 1, 0, msgs, NULL);
    }
}

bool CServiceBase::SetConfigValue(wchar_t *key, BYTE *data, DWORD size, DWORD type)
{
    wchar_t RegPath[MAX_PATH];
    HKEY hKey;
    DWORD disp;
    bool success = false;

    _snwprintf(RegPath, MAX_PATH, REG_CONFIG, GetName());

    if(Succeeded(RegCreateKeyEx(HKEY_LOCAL_MACHINE, RegPath, 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &hKey, &disp) == ERROR_SUCCESS))
    {
        success = Succeeded(RegSetValueEx(hKey, key, 0, type, data, size) == ERROR_SUCCESS);
        RegCloseKey(hKey);
    }
    return success;
}

bool CServiceBase::GetConfigValue(wchar_t *key, BYTE *data, DWORD *size, DWORD *type)
{
    wchar_t RegPath[MAX_PATH];
    HKEY hKey;
    DWORD disp;
    bool success = false;

    _snwprintf(RegPath, MAX_PATH, REG_CONFIG, GetName());

    if(Succeeded(RegCreateKeyEx(HKEY_LOCAL_MACHINE, RegPath, 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &hKey, &disp) == ERROR_SUCCESS))
    {
        success = Succeeded(RegQueryValueEx(hKey, key, 0, type, data, size) == ERROR_SUCCESS);
        RegCloseKey(hKey);
    }
    return success;
}

bool CServiceBase::SetConfigValue(wchar_t *key, BYTE *val, DWORD nval)
{
    return SetConfigValue(key, val, nval, REG_BINARY);
}

bool CServiceBase::SetConfigValue(wchar_t *key, DWORD val)
{
    return SetConfigValue(key, (BYTE *) &val, sizeof(val), REG_DWORD);
}

bool CServiceBase::SetConfigValue(wchar_t *key, wchar_t *val)
{
    return SetConfigValue(key, (BYTE *) val, -1, REG_SZ);
}

bool CServiceBase::GetConfigValue(wchar_t *key, BYTE *val, DWORD *nval)
{
    DWORD type = REG_BINARY;
    return GetConfigValue(key, val, nval, &type);
}
bool CServiceBase::GetConfigValue(wchar_t *key, DWORD *val)
{
    DWORD type = REG_DWORD;
    DWORD size = sizeof(*val);
    return GetConfigValue(key, (BYTE *) val, &size, &type);
}
bool CServiceBase::GetConfigValue(wchar_t *key, wchar_t *val, DWORD *nval)
{
    DWORD type = REG_SZ;
    return GetConfigValue(key, (BYTE *) val, nval, &type);
}
