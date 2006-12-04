//-----------------------------------------------------------------------------
// Name:        cwctrl.h
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

#ifndef __CWCTRL_H_
#define __CWCTRL_H_

#include <windows.h>
#include <string.h>

class cwCtrl
{
public:
    // [in] impersonate parameter is used when running as service to impersonate process owner
    // when checking if the file is a regular file
    cwCtrl(bool impersonate);
    ~cwCtrl();

    // [out] parameter pid is used to impersonate the user opening the file
    bool GetNextFilename(std::wstring& filename, DWORD& index, DWORD& pid, DWORD& dbmajor, DWORD& dbminor);


    // tell the driver to allow CreateFile or not
    // [in] index is a record index returned form GetFilename
    // [in] main, [in]  daily are current db versions the file hjas been scanned with
    // if the file was not scanned (skipped) then the db versions should be set to -1
    // in this cas the fs filter will not remove those files from internal cache
    // when the database verson changes
    bool SendResult(DWORD index, int main, int daily, bool allowed);

    // start filtering IRP_MJ_CREATE
    bool StartFiltering(bool bEnableCache, int main, int daily);

    // stop filtering IRP_MJ_CREATE
    bool StopFiltering(void);

    // Set database version
    // call after each db reload if fsfilter cache is enabled
    bool SetDatabaseVersion(int main, int daily);

    // fsfilter cache control
    bool EnableCaching(bool bEnable);

    bool Present;
    bool Enabled;

    // Open an event for communication with the filter driver
    HANDLE OpenEvent(void);
private:
    HANDLE m_hDevice;
    bool m_impersonate;
};

#endif // __CWCTRL_H_
