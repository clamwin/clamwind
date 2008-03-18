//-----------------------------------------------------------------------------
// Name:        cwxml.h
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

#ifndef _CWXML_H_
#define _CWXML_H_

#define XML_STATIC
#include <expat.h>

#define CLAMWIND_PIPE   L"\\\\.\\pipe\\ClamWinD"

#define REPLY_HDR       "<?xml version=\"1.0\"?><clamwinreply>"
#define REPLY_FOOTER    "</clamwinreply>"
#define REPLY_ERROR     "<error>Bad Request</error>"

#define FULL_ERROR_REPLY   REPLY_HDR REPLY_ERROR REPLY_FOOTER

#define TAG_CLAMWINREQUEST  0x0a4c6654
#define TAG_CLAMWINREPLY    0x0afa43e9

#define TAG_ACTION      0x067ab05e

#define TAG_STATUS      0x07aa8bc3
#define TAG_MESSAGE     0x03ca97a5
#define TAG_PROGRESS    0x095e8bd3

/* Scan */
#define TAG_FILENAME    0x002c42e5
#define TAG_JOBID       0x007158f4

/* GetInfo */
#define TAG_CORE        0x0006a685
#define TAG_LIBCLAMAV   0x08a21176
#define TAG_MAIN        0x000737fe
#define TAG_DAILY       0x006a8039
#define TAG_FSFILTER    0x09d03c12

/* FS Filter Control */
#define TAG_VALUE       0x007c83b5

#define ACTION_SCAN             0x0000007e
#define ACTION_ASYNSCAN         0x000000fe
#define ACTION_ASYNRESULT       0x00000034
#define ACTION_ASYNABORTSCAN    0x0000008e
#define ACTION_FSFILTERCONTROL  0x0000004c
#define ACTION_GETINFO          0x000000af
#define ACTION_RELOADDB         0x00000022

/* String Hash */
#define HASHWORDBITS 32

/* Defines the so called `hashpjw' function by P.J. Weinberger
 *    [see Aho/Sethi/Ullman, COMPILERS: Principles, Techniques and Tools,
 *       1986, 1987 Bell Telephone Laboratories, Inc.]  */
inline uint32_t hash_string(const char *str_param)
{
    uint32_t hval, g;
    const char *str = str_param;

    hval = 0;
    while (*str)
    {
        hval <<= 4;
        hval += (uint32_t) tolower(*str++);
        g = hval & ((uint32_t) 0xf << (HASHWORDBITS - 4));
        if (g)
        {
            hval ^= g >> (HASHWORDBITS - 8);
            hval ^= g;
        }
    }
    return hval;
}

/* CwXmlMessage Class */
class CwXmlMessage
{
public:
    CwXmlMessage(char *buffer, int len, uint32_t type = TAG_CLAMWINREQUEST);
    ~CwXmlMessage();
    bool valid;
    HANDLE client;
    uint8_t action;
    wchar_t *filename;
    const char *GetArgument(uint32_t arg);
    wchar_t *GetFileName(void);
private:
    uint32_t type;
    uint32_t tag;
    bool inside;
    XML_Parser parser;
    std::map<uint32_t, std::string> arguments;
    void Validate(void);
    static void XMLCALL startElement(void *userData, const char *name, const char **atts);
    static void XMLCALL endElement(void *userData, const char *name);
    static void XMLCALL dataElement(void *userData, const XML_Char *s, int len);
};

/* To string functions */
extern const wchar_t *hash_to_action(uint32_t value);
extern const wchar_t *hash_to_element(uint32_t value);

#endif /* _CWXML_H_ */
