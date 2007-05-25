//-----------------------------------------------------------------------------
// Name:        clamwind.h
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2005/12/15
// Copyright:   Copyright ClamWin Pty Ltd (c) 2005
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

#ifndef __CLAMWIND_H_
#define __CLAMWIND_H_

#define _CRT_SECURE_NO_DEPRECATE

#define WIN32_LEAN_AND_MEAN
#define _WIN32_WINNT 0x0501
#include <windows.h>

#include <stdio.h>
#include <stdlib.h>
#include <io.h>
#include <conio.h>
#include <fcntl.h>
#include <ctype.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <queue>
#include <string>
#include <map>
#include <list>

#include <cwtypes.h>
#include <cwxml.h>
#include <cwdb.h>
#include <cwctrl.h>
#include <cwcache.h>
#include <clamav.h>
#include <magic.h>
#include <service.h>
#include <logfile.h>
#include <pruning.h>

#define CORE_VERSION    "1.0"
#define XML_MSG_SIZE 1024
#define MIN_FILE_SIZE   512 /* If the file is too small it fools the hashing function */

#define HELP_MESSAGE \
    L"ClamWinD usage: /i - install service\n" \
    L"                /r - remove service\n"  \
    L"                /s - start\n"           \
    L"                /q - stop\n"            \
    L"                /t - console mode\n"

typedef enum
{
    LOCK_JOBID = 0,
    LOCK_JOBRES,
    LOCK_FILE,
    LOCK_ABORTED,
    LOCK_WORKER,
    LOCK_IMPERSONATE,
    LOCK_RELOAD,
    LOCK_MAX
} cs_t;

#define LOCK(x)     EnterCriticalSection(&this->cs[LOCK_##x])
#define UNLOCK(x)   LeaveCriticalSection(&this->cs[LOCK_##x])

typedef enum
{
    SCAN_RES_SCANNING,
    SCAN_RES_CLEAN,
    SCAN_RES_ABORTED,
    SCAN_RES_INFECTED,
    SCAN_RES_ERROR,
    SCAN_RES_NOSUCHJOB
} scan_res_t;

typedef enum
{
    JOB_IOCTL,
    JOB_ASYNC,
    JOB_RELOAD,
    JOB_END
} job_types_t;

typedef struct _job_t
{
    job_types_t type;

    /* Handle ioctl */
    std::wstring filename;
    DWORD dwIndex, dwPID, dwDBMajor, dwDBMinor;

    /* Handle async */
    int fd;
    uint32_t jobid;
} job_t;

typedef struct _jobresult_t
{
    DWORD ThreadId;
    HANDLE hFinished;
    int64_t timestamp;
    std::string message;
    scan_res_t result;
    int64_t count, total;
    int8_t percent;
    int fd;
} jobresult_t;

typedef std::list<uint32_t> joblist;
typedef std::map<uint32_t, jobresult_t> jobresult_hash;
typedef jobresult_hash::value_type jobresult_pair;

extern CLogFile log;

class CClamWinD : public CServiceBase
{
public:
    CClamWinD(): impersonate(true) {}
    ~CClamWinD();

    void Run(wchar_t *arg)
    {
        if (arg != NULL)
        {
            if      (IsArg(arg, L"/i")) Install();        // install
            else if (IsArg(arg, L"/r")) Remove();         // remove
            else if (IsArg(arg, L"/s")) Start();          // start
            else if (IsArg(arg, L"/q")) Stop();           // quit
            else if (IsArg(arg, L"/t"))
            {
                impersonate = false; /* Avoid to impersonate the client user */
                ConsoleMode();
            }
            else wprintf(HELP_MESSAGE);
        }
        else Execute();
    }

    static DWORD WINAPI HandleIOCTL(LPVOID lParam);
    static DWORD WINAPI Worker(LPVOID lParam);
    static int ScanCallback(int desc, int bytes);

    scan_res_t ProcessFile(int fd, database_t dbtype, std::wstring wsfilename, std::string *message);
    std::string  HandleXmlMessage(CwXmlMessage *msg);
    std::string  Escape(const char* str);
    void Push(job_t& job) { this->jobs.push(job); };
    void Signal(void) { ReleaseSemaphore(this->sem_worker, 1, NULL); };
    void ShutDown(void) { SetEvent(this->hStopEvent); };

    bool isReloading;
    uint32_t suspended;
    HANDLE sem_reload;
    HANDLE hReloadEvent;

    /* Synched */
    void InitEvents(void);
    void UninitEvents(void);
    bool AbortJob(uint32_t jobid);
    bool IsAborted(uint32_t jobid);
    uint32_t GenJobId(void);
    void InsertJobInList(jobresult_pair job);
    void UpdateJobResult(uint32_t jobid, scan_res_t res, DWORD ThreadId, std::string message);
    jobresult_t PopJobResult(uint32_t jobid, bool remove);
    jobresult_t GetJobResult(uint32_t jobid);
    job_t GetWorkerJob(void);
    int UpdateThreadJob(int desc, int bytes);

private:
    friend class cwPipeServer;
    friend class cwPruningService;

    bool impersonate;
    DWORD nthreads;
    HANDLE hStopEvent;

    wchar_t ourPath[MAX_PATH];
    DWORD pipesrv_tid; /* Needed to check for it in Scan CallBack */
    HANDLE pipesrv, pruningsvc;

    cwDB *db;
    cwCtrl *Filter;
    cwCache *cache;

    HANDLE sem_worker;
    std::queue<job_t> jobs;

    /* Synched stuff */
    CRITICAL_SECTION cs[LOCK_MAX];

    uint32_t jobid_generator;
    jobresult_hash jobresults;
    joblist aborted;

    void ServiceProc(void);
    const wchar_t *GetName(void){ return L"ClamWind"; }
    bool Init(void);
    inline bool IsArg(wchar_t *a, wchar_t *s) { return wcsstr(a, s) == a; }
};

/* Global pointer to pick service while in Scanning Callback */
extern CClamWinD svc;

/* Helpers */
extern size_t GetOurDir(LPWSTR szPath, DWORD cbPath);
extern bool IsSmall(HANDLE hFile);
extern bool IsRegularFile(LPCWSTR deviceName, LPCWSTR pathName);
extern bool GetCanonicalFilename(LPCWSTR devicename, LPCWSTR pathname, LPWSTR szPath, DWORD cbPath);
extern bool InitNtFunctions(void);
extern void UnintNtFunctions(void);
bool Impersonate(DWORD dwPID);
extern DWORD GetCPUCount(void);
extern const char *scanresult_to_string(scan_res_t result);
extern const char *scanresult_to_desc(scan_res_t result);

extern void compute_hash(HANDLE hFile, uint32_t *hashcode);

/* Dns Query */
extern wchar_t *txtquery(const wchar_t *domain, unsigned int *ttl);
#endif
