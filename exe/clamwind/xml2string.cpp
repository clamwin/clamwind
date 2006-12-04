//-----------------------------------------------------------------------------
// Name:        xml2string.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/01/20
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
#include <cwxml.h>

/* Codes to human readable strings, self commented */

const wchar_t *hash_to_action(uint32_t value)
{
    switch (value)
    {
        case ACTION_SCAN:
            return L"Scan";
        case ACTION_ASYNSCAN:
            return L"Async Scan";
        case ACTION_ASYNRESULT:
            return L"Async Scan Result";
        case ACTION_ASYNABORTSCAN:
            return L"Async Abort Scan";
        case ACTION_FSFILTERCONTROL:
            return L"FS Filter Control";
        case ACTION_GETINFO:
            return L"GetInfo";
        case ACTION_RELOADDB:
            return L"Reload DB";
        default:
            return L"Unknown";
    }
}

const wchar_t *hash_to_element(uint32_t value)
{
    switch (value)
    {
        case TAG_CLAMWINREQUEST:
            return L"ClamWin Request";
        case TAG_CLAMWINREPLY:
            return L"ClamWin Reply";
        case TAG_ACTION:
            return L"Action";

        /* Scan */
        case TAG_FILENAME:
            return L"Filename";
        case TAG_JOBID:
            return L"JobID";
        case TAG_STATUS:
            return L"Status";
        case TAG_MESSAGE:
            return L"Message";
        case TAG_PROGRESS:
            return L"Progress";

        /* GetInfo */
        case TAG_CORE:
            return L"Core Version";
        case TAG_LIBCLAMAV:
            return L"LibClamAV Version";
        case TAG_MAIN:
            return L"MainCVD";
        case TAG_DAILY:
            return L"DailyCVD";
        case TAG_FSFILTER:
            return L"FS Filter Installed";

        /* FS Filter Control */
        case TAG_VALUE:
           return L"Value";

        default:
            return L"Unknown tag";
    }
}
