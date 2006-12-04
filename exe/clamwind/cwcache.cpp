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

cwCache::cwCache(wchar_t *dbdir)
{
    char dbasefile[MAX_PATH] = "";
    std::wstring path(dbdir);
    path.append(L"\\cache.dat");
    WideCharToMultiByte(CP_ACP, 0, path.c_str(), -1, dbasefile, sizeof(dbasefile), NULL, NULL);

    this->database = new Db(NULL, 0);

    try // FIXME: error handling
    {
        //this->database->open(NULL, dbasefile, NULL, DB_HASH, DB_CREATE | DB_THREAD | DB_INIT_MPOOL, 0);
        // humm DB_THREAD needs some stuff to be done in a different way, but no examples with DB_HASH
        // are using also DB_THREAD, maybe not needed?
        this->database->open(NULL, dbasefile, NULL, DB_HASH, DB_CREATE | DB_INIT_MPOOL, 0);
        dbgprint(LOG_ALWAYS, L"Persistent Cache :: Cache Db opened\n");
    } catch(DbException &e)
    {
        // Error handling code goes here
        MessageBoxA(NULL, e.what(), "cwCache::{ctor}", MB_OK);
    }
}

cwCache::~cwCache()
{
    try
    {
        this->database->close(0);
        dbgprint(LOG_ALWAYS, L"Persistent Cache :: Cache Db Closed\n");
    } catch(DbException &e)
    {
        MessageBoxA(NULL, e.what(), "cwCache::{dtor}", MB_OK);
        // Error handling code goes here
    }
    // FIXME delete database crashes
}

void cwCache::Insert(const uint32_t *hash, entry_t &entry)
{
    int ret = 0;
    Dbt key((void *) hash, HASH_BYTE_SIZE);
    Dbt data(&entry, sizeof(entry));
    try
    {
        ret = this->database->put(NULL, &key, &data, 0);
    } catch(DbException &e)
    {
        MessageBoxA(NULL, e.what(), "cwCache::Insert", MB_OK);
        // Error handling code goes here
    }
}

entry_t *cwCache::Get(const uint32_t *hash)
{
    int ret = -1;
    Dbt key((void *) hash, HASH_BYTE_SIZE);
    Dbt data;
    try
    {
        ret = this->database->get(NULL, &key, &data, 0);
    } catch(DbException &e)
    {
        // Error handling code goes here
        MessageBoxA(NULL, e.what(), "cwCache::Get", MB_OK);
    }
    return ret ? NULL : (entry_t *) data.get_data();
}
/* TODO: error checking */
Dbc *cwCache::GetCursor(void)
{
    Dbc *cursorp;
    this->database->cursor(NULL, &cursorp, 0);
    return cursorp;
}
