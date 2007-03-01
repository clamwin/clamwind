//-----------------------------------------------------------------------------
// Name:        worker.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/11/05
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
#include <pipeserver.h>
#include <panama.h>

scan_res_t CClamWinD::ProcessFile(int fd, database_t dbtype, std::wstring wsfilename, std::string *message)
{
    scan_res_t scanres = SCAN_RES_CLEAN;
    int result, i;
    uint32_t hash[PAN_STAGE_SIZE];
    entry_t *match = NULL;
    bool nocheck = !this->cache || IsSmall((HANDLE) _get_osfhandle(fd));

    /* If the file is too small threat always as unchecked */
    if (nocheck)
        if(this->cache)
        dbgprint(LOG_INFO, L"Thread %u; File: %s is too small to insert in cache\n", GetCurrentThreadId(), wsfilename.c_str());
    else
    {
        compute_hash((HANDLE) _get_osfhandle(fd), hash);
        match = this->cache->Get(hash);
    }

    if (!match || // TODO handle different db scans here
        (match->main != this->db->GetEngineVersion(DB_MAIN)) ||
        (match->daily != this->db->GetEngineVersion(DB_DAILY)))
    {
        const char *virname;
        struct cl_limits limits;
        wchar_t wszVirName[MAX_PATH];

        memset(&limits, 0, sizeof(struct cl_limits));
        limits.maxfiles = 1;                /* max files */
        limits.maxfilesize = 1 * 524288;    /* maximal archived file size == 5 Mb */
        limits.maxreclevel = 1;             /* maximal recursion level */
        limits.maxratio = 1000;             /* maximal compression ratio */
        limits.archivememlim = 0;           /* disable memory limit for bzip2 scanner */

        switch (result = cl_scandesc(fd, &virname, NULL, this->db->GetEngine(dbtype), &limits, CL_SCAN_STDOPT))
        {
            case CL_CLEAN:
                dbgprint(LOG_DEBUG, L"Thread %u; Scanned: CLEAN\n", GetCurrentThreadId());
                break;
            case CL_VIRUS:
            {
                MultiByteToWideChar(CP_ACP, 0, virname, -1, wszVirName, sizeof(wszVirName) / sizeof(wszVirName[0]));
                dbgprint(LOG_ALWAYS, L"Thread %u; Scanned: found %s\n", GetCurrentThreadId(), wszVirName);
                scanres = SCAN_RES_INFECTED;
                if (message) message->append(virname);
                break;
            }
            case CL_EUSERABORT:
                dbgprint(LOG_ALWAYS, L"Thread %u; Scanned: Aborted by user\n", GetCurrentThreadId());
                if (message) message->append("Scan aborted by user");
                return SCAN_RES_ABORTED;
            default:
                dbgprint(LOG_ALWAYS, L"Thread %u; Scanner returned an error %d\n", GetCurrentThreadId(), result);
                if (message) message->append(cl_strerror(result));
                return SCAN_RES_ERROR;
        }

        /* Avoid inserting the entry */
        if (nocheck) return scanres;

        /* Fill the entry, then insert in the cache */
        entry_t entry;
        entry.allowed = (scanres == SCAN_RES_INFECTED) ? false : true;
        entry.main = this->db->GetEngineVersion(DB_MAIN);
        entry.daily = this->db->GetEngineVersion(DB_DAILY);
        entry.filename[0] = L'\0';
        wcscat(entry.filename, wsfilename.c_str());

        if(this->cache)
        {
            this->cache->Insert(hash, entry);
            /* FIXME: ugly */
            dbgprint(LOG_DEBUG, L"Thread %u; Scanned: %s - Hash:", GetCurrentThreadId(), wsfilename.c_str());
            for (i = 0; i < PAN_STAGE_SIZE; i++)
                dbgprint(LOG_DEBUG, L" %08x", hash[i]);
            dbgprint(LOG_DEBUG, L"\n");
        }
    }
    else
    {
        if (match->allowed)
        {
            // FIXME -> LOG_DEBUG
            dbgprint(LOG_DEBUG, L"[ALLOWED] Thread %u; Filename: %s found in cache\n", GetCurrentThreadId(), wsfilename.c_str());
            scanres = SCAN_RES_CLEAN;
        }
        else
        {
            dbgprint(LOG_DEBUG, L"[DENIED] Thread %u; Filename: %s found in cache\n", GetCurrentThreadId(), wsfilename.c_str());
            scanres = SCAN_RES_INFECTED;
        }
        delete match;
    }
    return scanres;
}

DWORD WINAPI CClamWinD::Worker(LPVOID lParam)
{
    CClamWinD* pThis = static_cast<CClamWinD*>(lParam);
    dbgprint(LOG_ALWAYS, L"Starting Worker thread: %u\n", GetCurrentThreadId());
    while (true)
    {
        HANDLE handleArray[2];
        handleArray[0] = pThis->sem_worker;
        handleArray[1] = pThis->hStopEvent;

        if (WaitForMultipleObjects(2, handleArray, FALSE, INFINITE) == WAIT_OBJECT_0 + 1)
        {
            dbgprint(LOG_ALWAYS, L"Stopping Worker thread: %u\n", GetCurrentThreadId());
            return 0;
        }

        job_t job = pThis->GetWorkerJob();
        switch (job.type)
        {
            case JOB_RELOAD:
            {
                dbgprint(LOG_ALWAYS, L"Worker thread %u - Need to reload db\n", GetCurrentThreadId());
                EnterCriticalSection(&pThis->cs[LOCK_RELOAD]);
                pThis->suspended++;
                LeaveCriticalSection(&pThis->cs[LOCK_RELOAD]);
                SetEvent(pThis->hReloadEvent);
                WaitForSingleObject(pThis->sem_reload, INFINITE);
                break;
            }

            case JOB_ASYNC:
            {
                std::string message;
                DWORD ThreadId = GetCurrentThreadId();

                /* Avoid CPU burn */
                SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_BELOW_NORMAL);

                /* Link this thread with the job */
                pThis->UpdateJobResult(job.jobid, SCAN_RES_SCANNING, ThreadId, message);

                /* Do the scan */
                scan_res_t res = pThis->ProcessFile(job.fd, DB_MAIN, job.filename, &message); /* FIXME DBENGINE */

                if (res == SCAN_RES_ABORTED)
                    pThis->PopJobResult(job.jobid, true); /* Remove from Scan results */
                else
                {
                    if (pThis->IsAborted(job.jobid)) /* Raise Thread Priority to normal since abort lowered me ;) */
                        pThis->PopJobResult(job.jobid, true); /* Remove from Scan results */
                    else /* Update entry in Scan results */
                        pThis->UpdateJobResult(job.jobid, res, ThreadId, message);
                }

                /* reset priority */
                SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_NORMAL);
                _close(job.fd);
                break;
            }
            case JOB_IOCTL:
            {
                DWORD dwMRead;
                bool bAllowed = true;
                BYTE magicbuf[MAGIC_BUFFER_SIZE];

                bool impersonate = pThis->impersonate;
                if(impersonate)
                {
                    impersonate = Impersonate(job.dwPID);
                    if(!impersonate)
                        dbgprint(LOG_ALWAYS, L"Thread %u; Impersonation failed: filename=%s, PID=%i. LastError: %x\n", GetCurrentThreadId(), job.filename.c_str(), job.dwPID, GetLastError());
                }
                dbgprint(LOG_DEBUG, L"Thread: %u;\tOpening file: %s --- Index:%u\n", GetCurrentThreadId(), job.filename.c_str(), job.dwIndex);
                HANDLE hFile = CreateFile(job.filename.c_str(), GENERIC_READ, FILE_SHARE_READ/*|FILE_SHARE_WRITE*/, NULL, OPEN_EXISTING, FILE_FLAG_RANDOM_ACCESS, NULL);
                /* revert impersonation now */
                if (impersonate) RevertToSelf();

                if ((hFile == INVALID_HANDLE_VALUE) || (!ReadFile(hFile, magicbuf, MAGIC_BUFFER_SIZE, &dwMRead, NULL)))
                {
                    if ((::GetLastError() != ERROR_FILE_NOT_FOUND) && (::GetLastError() != ERROR_INVALID_NAME))
                        dbgprint(LOG_INFO, L"Thread %u; CreateFile failed on: %s. LastError: %x\n", GetCurrentThreadId(), job.filename.c_str(), ::GetLastError());
                }
                else
                {
                    int fd = _open_osfhandle((intptr_t) hFile, O_RDONLY | O_BINARY);
                    if(fd != -1)
                    {
                        cw_magic_t type = cw_magic(magicbuf, MAGIC_BUFFER_SIZE);
                        switch(type)
                        {
                        case CW_TYPE_MSEXE: /* Add handled filetypes here */
                            {
                                /* if the main datbase version this file was scanned with is
                                   identical with current then we rescan with daily only */
                                database_t dbType = (pThis->db->GetEngineVersion(DB_MAIN) == job.dwDBMajor) ? DB_DAILY : DB_MAIN;
                                dbgprint(LOG_DEBUG, L"Thread %u; Current db version {%i; %i}. Cached main db version: %i. Scanning with %i (0=FULL, 1=DAILY)\n", GetCurrentThreadId(), pThis->db->GetEngineVersion(DB_MAIN), pThis->db->GetEngineVersion(DB_DAILY), job.dwDBMajor, dbType);
                                bAllowed = (pThis->ProcessFile(fd, dbType, job.filename, NULL) == SCAN_RES_CLEAN); /* FIXME DBENGINE */
                            }
                            break;
                        default:
                            {
                                wchar_t szExt[_MAX_EXT];
                                _wsplitpath(job.filename.c_str(), NULL, NULL, NULL, szExt);

                                if (!_wcsicmp(szExt, L".com"))
                                    bAllowed = (pThis->ProcessFile(fd, DB_MAIN, job.filename, NULL) == SCAN_RES_CLEAN); /* FIXME DBENGINE */
                                else
                                    dbgprint(LOG_TRACE, L"Skipped %s Filetype %d\n", job.filename.c_str(), type);
                            }
                        }
                        _close(fd);
                    }
                    else
                    {
                        CloseHandle(hFile);
                        dbgprint(LOG_INFO, L"Thread %u; ERROR getting file descriptor for: %s\n", GetCurrentThreadId(), job.filename.c_str());
                    }
                }
                pThis->Filter->SendResult(job.dwIndex, pThis->db->GetEngineVersion(DB_MAIN), pThis->db->GetEngineVersion(DB_DAILY), bAllowed);
                break;
            }
        default:
            dbgprint(LOG_ALWAYS, L"Invalid job type\n");
        }
    }
}
std::string CClamWinD::Escape(const char* str) const
{
    size_t position;
    std::string replacewith = "";
    std::string search(str);
    // escape "'" and "&" characters as they are invalid in XML
    position = search.find("&");
    if (position > 0)
    {
        replacewith = "&amp;";
    }
    else
    {
        position = search.find("'");
        if (position > 0)
        {
            replacewith = "&apos;";
        }
    }
    if (position > 0)
        return search.substr(0, position) + replacewith + search.substr(position + 1);
    else
        return search;
}

/* Handle xml messsages recevied from the pipeserver */
std::string CClamWinD::HandleXmlMessage(CwXmlMessage *msg)
{
    char szToDigit[16];
    DWORD dwWritten = 0;
    std::string reply;
    reply.clear();
    reply.append(REPLY_HDR);

    switch(msg->action)
    {
        case ACTION_RELOADDB:
            dbgprint(LOG_INFO, L"XML::Reload DB\n");
            int i;

            LOCK(RELOAD);
            this->suspended = 0;
            this->isReloading = true;
            UNLOCK(RELOAD);

            /* Ask Workers to stop by faking a job */
            {
                job_t reloadjob;
                reloadjob.type = JOB_RELOAD;
                for (i = 0; i < MAX_THREADS; i++)
                    this->jobs.push(reloadjob);
            }

            ReleaseSemaphore(this->sem_worker, MAX_THREADS, NULL);

            /* Wait until all are ready */
            do WaitForSingleObject(this->hReloadEvent, INFINITE);
            while (this->suspended < MAX_THREADS);

            /* Reload */
            delete this->db;
            this->db = new cwDB(this->ourPath);

            /* Wake up all since we are done */
            LOCK(RELOAD);
            this->isReloading = false;
            UNLOCK(RELOAD);
            ReleaseSemaphore(this->sem_reload, this->suspended, NULL);

            reply.append("<action>reloaddb</action>");
            reply.append("<status>OK</status>");
            break;

        case ACTION_GETINFO:
            dbgprint(LOG_INFO, L"XML::Get Info\n");
            reply.append("<action>getinfo</action>");
            reply.append("<core>"CORE_VERSION"</core>");
            reply.append("<libclamav>");
            reply.append(cl_retver());
            reply.append("</libclamav>");
            reply.append("<main>");
            reply.append(_itoa(this->db->GetEngineVersion(DB_MAIN), szToDigit, 10));
            reply.append("</main>");
            reply.append("<daily>");
            reply.append(_itoa(this->db->GetEngineVersion(DB_DAILY), szToDigit, 10));
            reply.append("</daily>");
            reply.append("<fsfilter>");
            reply.append(this->Filter->Enabled ? "1" : "0");
            reply.append("</fsfilter>");
            break;

        case ACTION_FSFILTERCONTROL:
        {
            bool res = false;
            dbgprint(LOG_INFO, L"XML::FS Filter Control\n");
            if (msg->GetArgument(TAG_VALUE))
            {
                if (atoi(msg->GetArgument(TAG_VALUE)))
                    res = this->Filter->StartFiltering(true, this->db->GetEngineVersion(DB_MAIN), this->db->GetEngineVersion(DB_DAILY));
                else
                    res = this->Filter->StopFiltering();
            }
            reply.append("<action>fsfiltercontrol</action>");
            reply.append("<status code=\"0\">"); /* TODO fill status code */
            reply.append(res ? "OK" : "ERROR");
            reply.append("</status>");
        }
        break;

        case ACTION_SCAN:
        case ACTION_ASYNSCAN:
        {
            HANDLE hFile;
            int fd = -1;
            dbgprint(LOG_INFO, L"%sScan request for %s\n", ((msg->action == ACTION_ASYNSCAN) ? L"(Async)" : L""), msg->filename);

            reply.append("<action>");
            reply.append((msg->action == ACTION_ASYNSCAN) ? "asynscan" : "scan");
            reply.append("</action>");;

            reply.append("<filename>");
            reply.append(Escape(msg->GetArgument(TAG_FILENAME)));
            reply.append("</filename>");

            // need to lock Impersonation from concurrent access
            // as this may result in a security breach of opening file
            // under undesired user account
            LOCK(IMPERSONATE);
            if (this->impersonate) ImpersonateNamedPipeClient(msg->client);
            hFile = CreateFile(msg->filename, GENERIC_READ, FILE_SHARE_READ/*|FILE_SHARE_WRITE*/, NULL, OPEN_EXISTING, FILE_FLAG_RANDOM_ACCESS, NULL);
            if (this->impersonate) RevertToSelf();
            UNLOCK(IMPERSONATE);
            if ((hFile != INVALID_HANDLE_VALUE) && ((fd = _open_osfhandle((intptr_t) hFile, O_RDONLY | O_BINARY)) != -1))
            {
                std::string message;
                FILETIME ts;
                LARGE_INTEGER timestamp;
                if (msg->action == ACTION_ASYNSCAN)
                {
                    uint32_t jobid = this->GenJobId();
                    job_t async_scan;

                    jobresult_t job;
                    job.timestamp = 0;
                    job.ThreadId = 0;
                    job.hFinished = CreateEvent(NULL, FALSE, FALSE, NULL);
                    job.result = SCAN_RES_SCANNING;
                    job.percent = 100;
                    job.count = 0;
                    job.fd = fd;

                    /* Get File Size */
                    job.total = _lseeki64(fd, 0, SEEK_END);
                    _lseeki64(fd, 0, SEEK_SET);

                    /* TimeStamp */
                    GetSystemTimeAsFileTime(&ts);
                    timestamp.LowPart = ts.dwLowDateTime;
                    timestamp.HighPart = ts.dwHighDateTime;
                    job.timestamp = timestamp.QuadPart;

                    reply.append("<jobid>");
                    reply.append(_ui64toa(jobid, szToDigit, 10)); /* FIXME: Check range */
                    reply.append("</jobid>");

                    this->InsertJobInList(jobresult_pair(jobid, job));

                    /* Delegate it to the Thread Pool */
                    async_scan.type = JOB_ASYNC;
                    async_scan.jobid = jobid;
                    async_scan.fd = fd;
                    async_scan.filename = msg->filename;
                    this->Push(async_scan);
                    this->Signal();
                }
                else
                {
                    if (this->isReloading)
                    {
                        reply.append("<status>ABORTED</status>");
                        reply.append("<message>Reloading Virus DB</message>");
                    }
                    else
                    {
                        scan_res_t scanres = this->ProcessFile(fd, DB_MAIN, msg->filename, &message); /* FIXME DBENGINE */
                        reply.append("<status>");
                        reply.append(scanresult_to_string(scanres));
                        reply.append("</status>");

                        if (message.length() > 0)
                        {
                            reply.append("<message>");
                            reply.append(message);
                            reply.append("</message>");
                        }
                    }
                    _close(fd);
                }
            }
            else
            {
                reply.append("<status code=\"");
                reply.append(_itoa(::GetLastError(), szToDigit, 10));
                reply.append("\">ERROR</status>");
            }
        }
        break;

        case ACTION_ASYNRESULT:
        {
            uint32_t jobid = 0;
            jobresult_t jobres;
            jobres.result = SCAN_RES_NOSUCHJOB;
            jobres.percent = 0;

            dbgprint(LOG_INFO, L"XML::Async Scan Get Result\n");

            if (jobid = string_to_uint32(msg->GetArgument(TAG_JOBID)))
                    jobres = this->GetJobResult(jobid);

            reply.append("<action>asynresult</action>");

            reply.append("<jobid>");
            reply.append(_itoa(jobid, szToDigit, 10));
            reply.append("</jobid>");

            reply.append("<status>");
            reply.append(scanresult_to_string(jobres.result));
            reply.append("</status>");

            reply.append("<progress>");
            reply.append((jobres.result == SCAN_RES_NOSUCHJOB) ? "-1" : _itoa(jobres.percent, szToDigit, 10));
            reply.append("</progress>");

            reply.append("<message>");
            reply.append(scanresult_to_desc(jobres.result));
            reply.append("</message>");
            break;
        }

        case ACTION_ASYNABORTSCAN:
        {
            dbgprint(LOG_INFO, L"XML::Async Scan Abort\n");
            uint32_t jobid = string_to_uint32(msg->GetArgument(TAG_JOBID));
            reply.append("<action>asynabortscan</action>");
            reply.append("<jobid>");
            reply.append(_itoa(jobid, szToDigit, 10));
            reply.append("</jobid>");

            reply.append("<status>");
            if (jobid && !this->IsAborted(jobid) && this->AbortJob(jobid))
                reply.append("OK");
            else
                reply.append("NOSUCHJOB");
            reply.append("</status>");
            break;
        }

        default:
            dbgprint(LOG_ALWAYS, L"XML::Bad request\n");
            reply.append(REPLY_ERROR);
    }

    reply.append(REPLY_FOOTER);
    return reply;
}
