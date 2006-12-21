//-----------------------------------------------------------------------------
// Name:        cwcache.h
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

#ifndef _CWCACHE_H_
#define _CWCACHE_H_

#include <windows.h>
#include <cwtypes.h>
#include <bdb/db_cxx.h>

#pragma pack(1)
typedef struct _entry
{
    bool allowed;
    wchar_t filename[512]; /* MAX_PATH is 260 but I think ^2 will perform better */
    uint32_t main, daily;
} entry_t;
#pragma pack()

class cwCache
{
public:
    cwCache(wchar_t *dbdir);
    ~cwCache();
    int Insert(const uint32_t *hash, entry_t &entry);
    entry_t *Get(const uint32_t *hash);
    int error;
private:
    friend class cwPruningService;
    DbEnv *envp;
    Db *database;
};

#endif // _CWCACHE_H_
