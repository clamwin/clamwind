//-----------------------------------------------------------------------------
// Name:        cwcache.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/09/10
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

#define HASH_BYTE_SIZE  32

#define BDB_CWOPEN_FLAGS (DB_CREATE | DB_INIT_MPOOL | DB_READ_UNCOMMITTED | DB_THREAD | DB_AUTO_COMMIT)
#define ENV_FLAGS (DB_CREATE | DB_RECOVER | DB_INIT_LOCK | DB_INIT_LOG | DB_INIT_TXN | DB_INIT_MPOOL | DB_THREAD)

#define BDB_FILENAME "cache.dat"

cwCache::cwCache(wchar_t *dbdir)
{
    char dbase[MAX_PATH] = "";
    std::wstring path(dbdir);
    WideCharToMultiByte(CP_ACP, 0, path.c_str(), -1, dbase, sizeof(dbase), NULL, NULL);

    this->envp = new DbEnv(0);
    //this->envp = new DbEnv(DB_CXX_NO_EXCEPTIONS);
    this->envp->set_lk_detect(DB_LOCK_MINWRITE);
    this->envp->open(dbase, ENV_FLAGS, 0);

    strncat(dbase, "\\" BDB_FILENAME, MAX_PATH);

    this->database = new Db(this->envp, 0);
    //this->database = new Db(this->envp, DB_CXX_NO_EXCEPTIONS);

    if ((this->error = this->database->open(NULL, dbase, NULL, DB_HASH, BDB_CWOPEN_FLAGS, 0)))
        dbgprint(LOG_ALWAYS, L"Persistent Cache :: Cache Db open failed\n");
    else
        dbgprint(LOG_ALWAYS, L"Persistent Cache :: Cache Db opened\n");
}

cwCache::~cwCache()
{

    if (this->database && this->envp && this->database->close(0) || this->envp->close(0))
        dbgprint(LOG_ALWAYS, L"Persistent Cache :: Cache Db Close() Failed\n");
    else
        dbgprint(LOG_ALWAYS, L"Persistent Cache :: Cache Db Closed\n");
}

int cwCache::Insert(const uint32_t *hash, entry_t &entry)
{
    int err = 0;
    Dbt key((void *) hash, HASH_BYTE_SIZE);
    Dbt data((void *) &entry, sizeof(entry));

    if (err = this->database->put(NULL, &key, &data, 0))
    {
        dbgprint(LOG_ALWAYS, L"Persistent Cache :: Insert Failed\n");
        this->database->err(err, "INSERT");
    }
    return err;
}

entry_t *cwCache::Get(const uint32_t *hash)
{
    int err = 0;
    entry_t *datares = new entry_t;
    Dbt key((void *) hash, HASH_BYTE_SIZE);

    Dbt data;
    data.set_data((void *) datares);
    data.set_size(sizeof(entry_t));
    data.set_ulen(sizeof(entry_t));
    data.set_flags(data.get_flags() | DB_DBT_USERMEM);

    if ((err = this->database->get(NULL, &key, &data, DB_READ_UNCOMMITTED))
        && err != DB_NOTFOUND)
    {
        dbgprint(LOG_ALWAYS, L"Persistent Cache :: Get Failed\n");
        this->database->err(err, "GET");
    }
    return err ? NULL : (entry_t *) data.get_data();
}

