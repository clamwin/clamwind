//-----------------------------------------------------------------------------
// Name:        cwdb.h
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

#ifndef _CWDB_H_
#define _CWDB_H_
#include <clamav.h>

typedef enum
{
    DB_MAIN = 0,
    DB_DAILY
} database_t;

/* cwDB Class */
class cwDB
{
public:
    cwDB(const wchar_t *szPath);
    ~cwDB();
    uint32_t CVDVersion(const char *dbName);
    bool CVDLoad(const char *dbName, cl_engine **engine);

    struct cl_engine *GetEngine(database_t type) { return this->engines[type]; }
    uint32_t GetEngineVersion(database_t type) { return this->versions[type]; }

private:
    std::string BasePath;
    cl_engine *engines[2];
    uint32_t versions[2];
};

#endif /* _CWDB_H_ */
