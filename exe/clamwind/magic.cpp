//-----------------------------------------------------------------------------
// Name:        magic.cpp
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

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#include <magic.h>

struct cli_magic_s
{
    int offset;
    const char *magic;
    size_t length;
    const char *descr;
    cw_magic_t type;
};

struct cli_smagic_s
{
    const char *sig;
    const char *descr;
    cw_magic_t type;
};

static const struct cli_magic_s cli_magic[] =
{
    /* Executables */

    {0, "MZ",                   2,  "DOS/W32 executable/library/driver",    CW_TYPE_MSEXE       },
    {0, "\177ELF",              4,  "ELF",                                  CW_TYPE_ELF         },

    /* Archives */

    {0, "Rar!",                 4,  "RAR",                                  CW_TYPE_RAR         },
    {0, "PK\003\004",           4,  "ZIP",                                  CW_TYPE_ZIP         },
    {0, "PK00PK\003\004",       8,  "ZIP",                                  CW_TYPE_ZIP         },
    {0, "\037\213",             2,  "GZip",                                 CW_TYPE_GZ          },
    {0, "BZh",                  3,  "BZip",                                 CW_TYPE_BZ          },
    {0, "SZDD",                 4,  "compress.exe'd",                       CW_TYPE_MSSZDD      },
    {0, "MSCF",                 4,  "MS CAB",                               CW_TYPE_MSCAB       },
    {0, "ITSF",                 4,  "MS CHM",                               CW_TYPE_MSCHM       },
    {8, "\x19\x04\x00\x10",     4,  "SIS",                                  CW_TYPE_SIS         },
    {0, "#@~^",                 4,  "SCRENC",                               CW_TYPE_SCRENC      },

    /* Graphics (may contain exploits against MS systems) */

    {0,  "GIF",                 3,  "GIF",                                  CW_TYPE_GRAPHICS    },
    {0,  "BM",                  2,  "BMP",                                  CW_TYPE_GRAPHICS    },
    {0,  "\377\330\377",        3,  "JPEG",                                 CW_TYPE_GRAPHICS    },
    {6,  "JFIF",                4,  "JPEG",                                 CW_TYPE_GRAPHICS    },
    {6,  "Exif",                4,  "JPEG",                                 CW_TYPE_GRAPHICS    },
    {0,  "\x89PNG",             4,  "PNG",                                  CW_TYPE_GRAPHICS    },
    {0,  "RIFF",                4,  "RIFF",                                 CW_TYPE_RIFF        },
    {0,  "RIFX",                4,  "RIFX",                                 CW_TYPE_RIFF        },

    /* Others */

    {0,  "\320\317\021\340\241\261\032\341",    8, "OLE2 container",        CW_TYPE_MSOLE2      },
    {0,  "%PDF-",                5,  "PDF document",                        CW_TYPE_PDF         },
    {0,  "\266\271\254\256\376\377\377\377",    8, "CryptFF",               CW_TYPE_CRYPTFF     },

    /* Ignored types */

    {0,  "\000\000\001\263",    4,  "MPEG video stream",                    CW_TYPE_DATA        },
    {0,  "\000\000\001\272",    4,  "MPEG sys stream",                      CW_TYPE_DATA        },
    {0,  "OggS",                4,  "Ogg Stream",                           CW_TYPE_DATA        },
    {0,  "ID3",                 3,  "MP3",                                  CW_TYPE_DATA        },
    {0,  "\377\373\220",        3,  "MP3",                                  CW_TYPE_DATA        },
    {0,  "%!PS-Adobe-",         11, "PostScript",                           CW_TYPE_DATA        },
    {0,  "\060\046\262\165\216\146\317", 7, "WMA/WMV/ASF",                  CW_TYPE_DATA        },
    {0,  ".RMF" ,             4, "Real Media File",                         CW_TYPE_DATA        },

    {-1, NULL,                  0,  NULL,                                   CW_TYPE_UNKNOWN_DATA}
};

cw_magic_t cw_magic(const unsigned char* buf, size_t buflen)
{
    size_t i;

    for(i = 0; cli_magic[i].magic; i++)
        if(buflen >= cli_magic[i].offset+cli_magic[i].length)
            if(memcmp(buf+cli_magic[i].offset, cli_magic[i].magic, cli_magic[i].length) == 0)
                return cli_magic[i].type;
    return CW_TYPE_UNKNOWN_DATA;
}
