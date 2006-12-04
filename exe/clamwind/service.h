//-----------------------------------------------------------------------------
// Name:        service.h
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//                 this file is based on "NT Service Class" code by
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

#ifndef SERVICE_CLASS_INCLUDED
#define SERVICE_CLASS_INCLUDED

#define REG_CONFIG   L"SYSTEM\\CurrentControlSet\\Services\\%s\\Parameters"
#define REG_EVENTLOG L"SYSTEM\\CurrentControlSet\\Services\\EventLog\\System"

class CServiceBase
{
public:
    CServiceBase(void);
    ~CServiceBase();

    bool Execute(void);
    bool ConsoleMode(void);

    bool Start(void);
    bool Stop(void);
    bool Install(void);
    bool Remove(void);

    bool SetConfigValue(wchar_t *key, BYTE *val, DWORD nval);
    bool SetConfigValue(wchar_t *key, DWORD val);
    bool SetConfigValue(wchar_t *key, wchar_t *val);

    bool GetConfigValue(wchar_t *key, BYTE *val, DWORD *nval);
    bool GetConfigValue(wchar_t *key, DWORD *val);
    bool GetConfigValue(wchar_t *key, wchar_t *val, DWORD *nval);

    enum evLogType
    {
        evError   = EVENTLOG_ERROR_TYPE,
        evWarning = EVENTLOG_WARNING_TYPE,
        evInfo    = EVENTLOG_INFORMATION_TYPE
    };

    void LogEvent(wchar_t *e, evLogType t = evInfo, WORD cat = 0);

    virtual const wchar_t *GetName(void) = 0;
    virtual void ServiceProc(void) = 0;

    virtual const wchar_t *GetDisplayName(void) { return GetName(); }
    virtual bool Init(void) { return true; }
    virtual void Cleanup(void) { return; }
    virtual void LogoffEvent(void) { return; }
    virtual void ShutDown(void) = 0;

private:
   SERVICE_STATUS_HANDLE m_StatusHandle;
   DWORD m_StatusCode;
   HANDLE m_EventLog;
   wchar_t m_ErrorString[512];

   bool Open(SC_HANDLE &hService);
   void Close(SC_HANDLE hService);
   bool Succeeded(bool ReturnValue);

   void SetStatus(DWORD status);
   static void WINAPI ServiceMain(int argc, wchar_t *argv[]);
   static void WINAPI ServiceHandler(DWORD ServiceControl);
   static BOOL WINAPI ConsoleHandler(DWORD dwCtrlType);

   bool SetConfigValue (wchar_t *key, BYTE *data, DWORD size, DWORD type);
   bool GetConfigValue (wchar_t *key, BYTE *data, DWORD *size, DWORD *type);
};

#endif // SERVICE_CLASS_INCLUDED
