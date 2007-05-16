//-----------------------------------------------------------------------------
// Name:        cwdb.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/04/01
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

/* upstream removed cl_loaddb from clamav.h */
extern "C" int cl_loaddb(const char *filename, struct cl_engine **engine, unsigned int *signo);

uint32_t cwDB::CVDVersion(const char *dbName)
{
    char dbPath[MAX_PATH];
    uint32_t res = 0;
    cl_cvd *cvd;

    dbPath[0] = 0;
    strncat(dbPath, this->BasePath.c_str(), MAX_PATH);
    strncat(dbPath, "\\", MAX_PATH);
    strncat(dbPath, dbName, MAX_PATH);

    if ((cvd = cl_cvdhead(dbPath)))
    {
        res = cvd->version;
        cl_cvdfree(cvd);
    }
    return res;
}

bool cwDB::CVDLoad(const char *dbName, cl_engine **engine)
{
    char dbPath[MAX_PATH];
    wchar_t dbNameW[MAX_PATH];
    uint32_t signo = 0;

    dbPath[0] = 0;
    strncat(dbPath, this->BasePath.c_str(), MAX_PATH);
    strncat(dbPath, "\\", MAX_PATH);
    strncat(dbPath, dbName, MAX_PATH);

    MultiByteToWideChar(CP_UTF8, 0, dbName, -1, dbNameW, MAX_PATH);

    if (cl_loaddb(dbPath, engine, &signo))
    {
        dbgprint(LOG_ALWAYS, L"Clamav DB not found: %s...\n", dbNameW);
        return false;
    }
    else
    {
        dbgprint(LOG_ALWAYS, L"Loaded Clamav DB: %s, %d signatures\n", dbNameW, signo);
        return true;
    }
}

/* FIXME: ugly for .inc directories */
cwDB::cwDB(const wchar_t *szPath)
{
    char szMBPath[MAX_PATH];
    this->engines[DB_MAIN] = NULL;
    this->engines[DB_DAILY] = NULL;

    /* get the path where the exe resides db must be there too */
    WideCharToMultiByte(CP_ACP, 0, szPath, -1, szMBPath, sizeof(szMBPath), NULL, NULL);
    this->BasePath = std::string(szMBPath);

    dbgprint(LOG_ALWAYS, L"Loading Clamav Signatures from %s...\n", szPath);

    // Full DB
    if (!(this->CVDLoad("main.cvd", &this->engines[DB_MAIN]) || this->CVDLoad("main.inc", &this->engines[DB_MAIN]))) return;
    if (!(this->CVDLoad("daily.cvd", &this->engines[DB_MAIN]) || this->CVDLoad("daily.inc", &this->engines[DB_MAIN]))) return;
    if (cl_build(this->engines[DB_MAIN]))
    {
        cl_free(this->engines[DB_MAIN]);
        this->engines[DB_MAIN] = NULL;
        dbgprint(LOG_ALWAYS, L"Error initializing Main database...");
        return;
    }

    // Daily Only
    if (!(this->CVDLoad("daily.cvd", &this->engines[DB_DAILY]) || this->CVDLoad("daily.inc", &this->engines[DB_DAILY]))) return;
    if (cl_build(this->engines[DB_DAILY]))
    {
        cl_free(this->engines[DB_DAILY]);
        this->engines[DB_DAILY] = NULL;
        dbgprint(LOG_ALWAYS, L"Error initializing Daily database...");
        return;
    }

    this->versions[DB_MAIN]  = this->CVDVersion("main.cvd");
    this->versions[DB_DAILY] = this->CVDVersion("daily.cvd");
    if (!this->versions[DB_DAILY]) this->versions[DB_DAILY] = this->CVDVersion("main.inc");
    if (!this->versions[DB_DAILY]) this->versions[DB_DAILY] = this->CVDVersion("daily.inc");

    /* Register the Scan Callbacks */
    this->engines[DB_MAIN]->callback = this->engines[DB_DAILY]->callback = CClamWinD::ScanCallback;
}

cwDB::~cwDB()
{
    dbgprint(LOG_ALWAYS, L"Unloading Virus DB\n");
    if (this->engines[DB_MAIN]) cl_free(this->engines[DB_MAIN]);
    if (this->engines[DB_DAILY]) cl_free(this->engines[DB_DAILY]);
}
