//-----------------------------------------------------------------------------
// Name:        synced.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/03/09
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

/* Init Mutex Events */
void CClamWinD::InitEvents(void)
{
    this->jobid_generator = 0;
    for (int i = 0; i < LOCK_MAX; i++)
        InitializeCriticalSection(&this->cs[i]);

    this->hStopEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
    this->hReloadEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
    this->sem_reload = CreateSemaphore(NULL, 0, this->nthreads, NULL);
}

void CClamWinD::UninitEvents(void)
{
    for (int i = 0; i < LOCK_MAX; i++)
        DeleteCriticalSection(&this->cs[i]);
    CloseHandle(this->hStopEvent);
    CloseHandle(this->hReloadEvent);
    CloseHandle(this->sem_reload);
}

/* Generate a serial jobid */
uint32_t CClamWinD::GenJobId(void)
{
    uint32_t j = 0;
    LOCK(JOBID);
    this->jobid_generator++;
    if (!this->jobid_generator) this->jobid_generator++; /* Reserve 0 for sync scans */
    j = this->jobid_generator;
    UNLOCK(JOBID);
    return j;
}

/* Insert a job in the jobs bucket */
void CClamWinD::InsertJobInList(jobresult_pair job)
{
    LOCK(JOBRES);
    this->jobresults.insert(job);
    UNLOCK(JOBRES);
}

/* Update an existing job with the scan result or with the Thread link reference  */
void CClamWinD::UpdateJobResult(uint32_t jobid, scan_res_t res, DWORD ThreadId, std::string message)
{
    jobresult_hash::iterator match;
    LOCK(JOBRES);
    match = this->jobresults.find(jobid);
    if (match == this->jobresults.end())
    {
        dbgprint(LOG_ALWAYS, L"Warning job id %u not found!!!\n", jobid);
        UNLOCK(JOBRES);
        return;
    }
    match->second.result = res;
    match->second.message = message;
    match->second.ThreadId = ThreadId;
    UNLOCK(JOBRES);
    if (res != SCAN_RES_SCANNING) SetEvent(match->second.hFinished); /* Signal the termination */
}

/* Pick a job from the joblist hash, if remove is true the entry is also removed */
jobresult_t CClamWinD::PopJobResult(uint32_t jobid, bool remove)
{
    jobresult_t res;
    jobresult_hash::iterator match;
    res.result = SCAN_RES_NOSUCHJOB;

    match = this->jobresults.find(jobid);
    if (match != this->jobresults.end())
    {
        res = this->jobresults[jobid];
        if (remove)
        {
            if (res.hFinished != INVALID_HANDLE_VALUE) CloseHandle(res.hFinished);
            this->jobresults.erase(match);
        }
    }
    return res;
}

/* Gets an async job results, and remove it if it's done */
jobresult_t CClamWinD::GetJobResult(uint32_t jobid)
{
    jobresult_t res = this->PopJobResult(jobid, false);
    if (res.result == SCAN_RES_NOSUCHJOB) return res;

    if (WaitForSingleObject(res.hFinished, 0) != WAIT_OBJECT_0)
        return res;

    return (this->PopJobResult(jobid, true));
}

/* Aborts an async job */
bool CClamWinD::AbortJob(uint32_t jobid)
{
    joblist::iterator i;
    jobresult_hash::iterator match;

    /* Short circuit */
    LOCK(JOBRES);
    if (this->jobresults.empty())
    {
        UNLOCK(JOBRES);
        return false;
    }
    UNLOCK(JOBRES);

    if (this->IsAborted(jobid)) return false;
    this->aborted.push_back(jobid);
    return true;
}

/* Check if job matching jobid is aborted or not */
bool CClamWinD::IsAborted(uint32_t jobid)
{
    joblist::iterator i;

    LOCK(ABORTED);
    /* Short circuit */
    if (this->aborted.empty())
    {
        UNLOCK(ABORTED);
        return false;
    }

    for (i = this->aborted.begin(); i != this->aborted.end(); i++)
    {
        if (jobid == *i)
        {
            this->aborted.erase(i);
            UNLOCK(ABORTED);
            return true;
        }
    }
    UNLOCK(ABORTED);
    return false;
}

/* Workers are using this method to pick a job to be worked */
job_t CClamWinD::GetWorkerJob(void)
{
    LOCK(WORKER);
    job_t job = this->jobs.front();
    this->jobs.pop();
    UNLOCK(WORKER);
    return job;
}

/* Used by the scan callback to update the progress and also to tell the scanner
 * to stop scanning if the job is aborted */
int CClamWinD::UpdateThreadJob(int desc, int bytes)
{
    jobresult_hash::iterator i;
    int ok = 1;
    DWORD ThreadId = GetCurrentThreadId();

    /* Check if the server is shutting down, so abort all scans */
    if (WaitForSingleObject(this->hStopEvent, 0) == WAIT_OBJECT_0)
        return 0;

    /* FIXME: ifs scans : return */

    /* return always ok when the thread is the pipeserver
     * this means sync scan, we are not providing any way to abort it for now.
     * FIXME: handle broken pipe from client by returning 0 (not ok) */
    if (!this->isReloading && (ThreadId == this->pipesrv_tid)) return ok;

    LOCK(JOBRES);
    for (i = this->jobresults.begin(); i != this->jobresults.end(); i++)
    {
        if (ThreadId == i->second.ThreadId)
        {
            if (this->IsAborted(i->first) || this->isReloading)
            {
                ok = 0;
                break; /* Break the libclamav scan */
            }

            if (i->second.fd != desc)
            {
                i->second.percent = -1;
                break;
            }

            i->second.count += bytes;
            i->second.percent = min(100, (uint8_t) (((double) i->second.count) * 100.0f / ((double) i->second.total)));
            break;
        }
    }
    dbgprint(LOG_DEBUG, L"Scan CallBack [Thread %u - JobID %u] %d%%\n", ThreadId, i->first, i->second.percent);
    UNLOCK(JOBRES);
    return ok;
}

/* Simple callback, it's wrapped since the only way to relationate it with
 * a running job is checking the thread. The service instance is needed
 * because of locks/unlocks */

int CClamWinD::ScanCallback(int desc, int bytes)
{
    return (svc.UpdateThreadJob(desc, bytes));
}
