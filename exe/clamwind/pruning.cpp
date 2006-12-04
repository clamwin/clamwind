//-----------------------------------------------------------------------------
// Name:        pruning.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/10/12
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
#include <panama.h>
#include <bdb/db_cxx.h>

#define SLEEPTIME   5000 /* FIXME this should be an option */

#define BREAKIFDONE(time) if (WaitForSingleObject(svc->hStopEvent, time) == WAIT_OBJECT_0) break;

/* FIXME: error checking */
DWORD WINAPI cwPruningService::Run(LPVOID lpvThreadParam)
{
    CClamWinD *svc = static_cast<CClamWinD *>(lpvThreadParam);
    HANDLE hFile = INVALID_HANDLE_VALUE;
    uint32_t hash[PAN_STAGE_SIZE];
    uint32_t *dbhash = NULL;
    dbgprint(LOG_ALWAYS, L"Pruning Service :: Starting...\n");
    /* This is a background service */
    SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_IDLE);

    while (true)
    {
        Dbt key, data;
        entry_t *entry = NULL;
        int ret;

        BREAKIFDONE(SLEEPTIME);

        Dbc *cursorp = svc->cache->GetCursor();
        if (!cursorp)
        {
            dbgprint(LOG_ALWAYS, L"Pruning Service :: Cannot get db cursor, aborting...\n");
            break;
        }

        // ret should be DB_NOTFOUND upon exiting the loop
        // FIXME it can throw an exception
        // FIXME need to lock?
        while ((ret = cursorp->get(&key, &data, DB_NEXT)) == 0)
        {
            BREAKIFDONE(0);
            entry = (entry_t *) data.get_data();
            dbgprint(LOG_TRACE, L"Pruning Service :: Found in CACHE: %s\n", entry->filename);
            hFile = CreateFile(entry->filename, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_FLAG_SEQUENTIAL_SCAN, NULL);
            if (hFile == INVALID_HANDLE_VALUE)
            {
                dbgprint(LOG_INFO, L"Error opening file: %s (%d) - REMOVED\n", entry->filename, GetLastError());
                cursorp->del(0);
                continue;
            }

            compute_hash(hFile, hash);
            dbhash = (uint32_t *) key.get_data(); /* Bound check ?? */
            CloseHandle(hFile);

            /* FIXME: ugly */
            dbgprint(LOG_TRACE, L"DBHash:");
            for (int i = 0; i < PAN_STAGE_SIZE; i++)
                dbgprint(LOG_TRACE, L" %08x", dbhash[i]);
            dbgprint(LOG_TRACE, L"\n");

            dbgprint(LOG_TRACE, L"FlHash:");
            for (int i = 0; i < PAN_STAGE_SIZE; i++)
                dbgprint(LOG_TRACE, L" %08x", hash[i]);
            dbgprint(LOG_TRACE, L"\n");


            if (memcmp(dbhash, hash, PAN_STAGE_SIZE))
            {
                dbgprint(LOG_INFO, L"File has different Checksum: %s - REMOVED\n", entry->filename);
                cursorp->del(0);
                continue;
            }
        }
        cursorp->close();
    }

    dbgprint(LOG_ALWAYS, L"Pruning Service :: Done...\n");
    return 0;
}
