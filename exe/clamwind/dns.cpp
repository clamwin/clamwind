//-----------------------------------------------------------------------------
// Name:        dns.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/07/24
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
#include <windns.h>

wchar_t *txtquery(const wchar_t *domain, unsigned int *ttl)
{
    PDNS_RECORD pRec = NULL, pRecOrig = NULL;
    wchar_t *txt = NULL;
    if (DnsQuery(domain, DNS_TYPE_TEXT, DNS_QUERY_STANDARD | DNS_QUERY_BYPASS_CACHE, NULL, &pRec, NULL) != ERROR_SUCCESS)
    {
        dbgprint(LOG_ALWAYS, L"Dns Query Failed...");
        return NULL;
    }

    pRecOrig = pRec;

    while (pRec)
    {
        if ((pRec->wType == DNS_TYPE_TEXT) && pRec->wDataLength && pRec->Data.TXT.dwStringCount)
        {
            /* yes strlen is unsafe but win32 doesn't tell me the right length */
            size_t len = wcslen(pRec->Data.TXT.pStringArray[0]);
            txt = (wchar_t *) malloc(len + 1);
            wcsncpy(txt, (wchar_t *) pRec->Data.TXT.pStringArray[0], len);
            txt[len] = 0;
            *ttl = pRec->dwTtl;
            break;
        }
        pRec = pRec->pNext;
    }
    DnsRecordListFree(pRecOrig, DnsFreeRecordList);
    return txt;
}
