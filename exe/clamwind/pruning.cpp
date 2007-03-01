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

#define BREAKIFDONE(time) \
    if (WaitForSingleObject(svc->hStopEvent, time) == WAIT_OBJECT_0) \
    { if (cursorp) cursorp->close(); cursorp = NULL; break; }

/* FIXME: error checking, check multithreading - it needs tnx ?? */
DWORD WINAPI cwPruningService::Run(LPVOID lpvThreadParam)
{
    CClamWinD *svc = static_cast<CClamWinD *>(lpvThreadParam);
    if(!svc->cache) return 0;

    HANDLE hFile = INVALID_HANDLE_VALUE;
    /*uint32_t hash[PAN_STAGE_SIZE];
    uint32_t *dbhash = NULL;*/
    Dbc *cursorp = NULL;
    entry_t *entry = NULL;

    Dbt key, *dkey;
    uint32_t *hashres = new uint32_t[PAN_STAGE_SIZE];
    key.set_data((void *) hashres);
    key.set_size(PAN_STAGES);
    key.set_ulen(PAN_STAGES);
    key.set_flags(key.get_flags() | DB_DBT_USERMEM);

    Dbt data;
    entry_t *datares = new entry_t;
    data.set_data((void *) datares);
    data.set_size(sizeof(entry_t));
    data.set_ulen(sizeof(entry_t));
    data.set_flags(data.get_flags() | DB_DBT_USERMEM);

    dbgprint(LOG_ALWAYS, L"Pruning Service :: Starting...\n");
    /* This is a background service */
    SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_IDLE);

    while (true)
    {
        BREAKIFDONE(SLEEPTIME);

		if(!svc->cache)
			continue;

        svc->cache->database->cursor(NULL, &cursorp, DB_READ_UNCOMMITTED | DB_TXN_SNAPSHOT);
        dkey = NULL;

        if (!cursorp)
        {
            dbgprint(LOG_ALWAYS, L"Pruning Service :: Cannot get db cursor, aborting...\n");
            break;
        }

        // ret should be DB_NOTFOUND upon exiting the loop
        while (!cursorp->get(&key, &data, DB_NEXT | DB_READ_UNCOMMITTED))
        {
            BREAKIFDONE(0);
            entry = (entry_t *) data.get_data();
            dbgprint(LOG_TRACE, L"Pruning Service :: Found in CACHE: %s\n", entry->filename);
            hFile = CreateFile(entry->filename, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_FLAG_SEQUENTIAL_SCAN, NULL);
            if (hFile == INVALID_HANDLE_VALUE)
            {
                dbgprint(LOG_ALWAYS, L"Error opening file: %s (%d) - REMOVED\n", entry->filename, GetLastError());
                dkey = &key;
                break;
            }

            /*compute_hash(hFile, hash);
            dbhash = (uint32_t *) key.get_data();*/
            CloseHandle(hFile);
#if 0
            /* FIXME: ugly */
            dbgprint(LOG_TRACE, L"DBHash:");
            for (int i = 0; i < PAN_STAGE_SIZE; i++)
                dbgprint(LOG_TRACE, L" %08x", dbhash[i]);
            dbgprint(LOG_TRACE, L"\n");

            dbgprint(LOG_TRACE, L"FlHash:");
            for (int i = 0; i < PAN_STAGE_SIZE; i++)
                dbgprint(LOG_TRACE, L" %08x", hash[i]);
            dbgprint(LOG_TRACE, L"\n");
#endif
            /*if (memcmp(dbhash, hash, PAN_STAGES))
            {
                dbgprint(LOG_ALWAYS, L"File has different Checksum: %s - REMOVED\n", entry->filename);
                dkey = &key;
                break;
            }*/
        }
        cursorp->close();
        cursorp = NULL;
        if (dkey) svc->cache->Delete(dkey);
    }

    delete hashres;
    delete datares;
    dbgprint(LOG_ALWAYS, L"Pruning Service :: Done...\n");
    return 0;
}
