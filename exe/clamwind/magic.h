//-----------------------------------------------------------------------------
// Name:        magic.h
// Product:     ClamWin Antivirus
//
//
// Created:     2005/12/15
// Copyright (C) 2002 - 2006 Tomasz Kojm <tkojm@clamav.net>
// With enhancements from Thomas Lamy <Thomas.Lamy@in-online.net>
// Adopted for ClamWin from ClamAV code by sherpya <sherpya at users dot sourceforge dot net>
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

#ifndef __MAGIC_H_
#define __MAGIC_H_

#define MAGIC_BUFFER_SIZE 50

typedef enum
{
    CW_TYPE_UNKNOWN_TEXT = 0,
    CW_TYPE_UNKNOWN_DATA,
    CW_TYPE_MSEXE,
    CW_TYPE_ELF,
    CW_TYPE_DATA,
    CW_TYPE_POSIX_TAR,
    CW_TYPE_OLD_TAR,
    CW_TYPE_GZ,
    CW_TYPE_ZIP,
    CW_TYPE_BZ,
    CW_TYPE_RAR,
    CW_TYPE_MSSZDD,
    CW_TYPE_MSOLE2,
    CW_TYPE_MSCAB,
    CW_TYPE_MSCHM,
    CW_TYPE_SIS,
    CW_TYPE_SCRENC,
    CW_TYPE_GRAPHICS,
    CW_TYPE_RIFF,
    CW_TYPE_BINHEX,
    CW_TYPE_TNEF,
    CW_TYPE_CRYPTFF,
    CW_TYPE_PDF,

    /* bigger numbers have higher priority (in o-t-f detection) */
    CW_TYPE_HTML,   /* on the fly */
    CW_TYPE_MAIL,   /* magic + on the fly */
    CW_TYPE_ZIPSFX, /* on the fly */
    CW_TYPE_RARSFX  /* on the fly */
} cw_magic_t;

cw_magic_t cw_magic(const unsigned char* buf, size_t buflen);
#endif
