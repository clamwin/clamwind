//-----------------------------------------------------------------------------
// Name:        clamwind.cpp
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
#include <cwdb.h>
#include <cwctrl.h>
#include <pipeserver.h>

CLogFile log;
CClamWinD svc;

CClamWinD::~CClamWinD()
{
    if (this->Filter) delete this->Filter;
    if (this->db) delete this->db;
    if (this->cache) delete this->cache;
    this->UninitEvents();
    dbgprint(LOG_ALWAYS, L"Exiting...\n");
}

bool CClamWinD::Init(void)
{
    /* no last slash here */
    GetOurDir(this->ourPath, sizeof(ourPath));

    /* init db */
    this->db = new cwDB(this->ourPath);
    if (!(this->db->GetEngine(DB_MAIN) && this->db->GetEngine(DB_DAILY)))
    {
        dbgprint(LOG_ALWAYS, L"Problems loading Virus Signatures, aborting...\n");
        return false;
    }

    /* BDB Database */
	DWORD disableCache = 0;
	this->cache = NULL;
    svc.GetConfigValue(L"DisableCache", &disableCache);
	if(!disableCache)
	{
		this->cache = new cwCache(this->ourPath);
		if (this->cache->error)
		{
			dbgprint(LOG_ALWAYS, L"Problems opening Persistant Cache Db, aborting...\n");
			delete this->cache;
			this->cache = NULL;
		}
	}

    /* Sync objects */
    this->InitEvents();

    /* Pipe Server */
    this->pipesrv = CreateThread(NULL, 0, cwPipeServer::Run, (LPVOID) this, 0, &this->pipesrv_tid);

    /* Pruning Service */
    DWORD dummy;
    this->pruningsvc = CreateThread(NULL, 0, cwPruningService::Run, (LPVOID) this, 0, &dummy);

    this->Filter = new cwCtrl(impersonate);
    return true; /* Allow to work without ifs filter */
}

void CClamWinD::ServiceProc(void)
{
    dbgprint(LOG_ALWAYS, L"Starting Service...\n");

    HANDLE threads[MAX_THREADS], ioctl;
    DWORD i, id;
    this->sem_worker = CreateSemaphore(NULL, 0, 1000, L"ClamWinD_Worker");
    this->isReloading = false;
    this->suspended = 0;

    for(i = 0; i < sizeof(threads)/sizeof(threads[0]); i++)
        threads[i] = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) Worker, (LPVOID) this, 0, &id);

    if (Filter->Present)
    {
        if (Filter->StartFiltering(true, this->db->GetEngineVersion(DB_MAIN), this->db->GetEngineVersion(DB_DAILY)))
            ioctl = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) HandleIOCTL, (LPVOID) this, 0, &id);
        else
            ShutDown();
    }

    WaitForMultipleObjects(sizeof(threads)/sizeof(threads[0]), threads, TRUE, INFINITE);
    for(i = 0; i < sizeof(threads)/sizeof(threads[0]); i++)
        CloseHandle(threads[i]);

    if (Filter->Present)
    {
        WaitForSingleObject(ioctl, INFINITE);
        CloseHandle(ioctl);
    }
    /* Shut down Pipe/Pruning Threads */
    ShutDown();

    /* Wait for Pipe/Pruning Threads termination */
    WaitForSingleObject(this->pipesrv, INFINITE);
    WaitForSingleObject(this->pruningsvc, INFINITE);

    this->Filter->StopFiltering();
    dbgprint(LOG_ALWAYS, L"Service Stopped...\n");
    return;
}

int wmain(int argc, wchar_t *argv[])
{
    // read loglevel from registry
    DWORD logLevel = LOG_ALWAYS;
    svc.GetConfigValue(L"LogLevel", &logLevel);

    if ((argc > 1) && wcsstr(argv[1], L"/t"))
        log.InitStdOut(logLevel);
    else
    {
        wchar_t szPath[MAX_PATH];
        GetOurDir(szPath, sizeof(szPath));
        wcscat(szPath, L"\\ClamWind.log");
        log.InitFile(logLevel, szPath);
    }

    if(!InitNtFunctions())
        dbgprint(LOG_ALWAYS, L"ERROR Loading NTDll Functions...\n");

    svc.Run(argv[1]);
    UnintNtFunctions();
    return 0;
}
