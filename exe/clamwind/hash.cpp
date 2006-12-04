//-----------------------------------------------------------------------------
// Name:        hash.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/09/11
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
#include <panama.h>

/* Timing referred to my TB 1.2Ghz */
#define BUFSIZE 8192    // ~63 mb/s

//#define BUFSIZE 4096    // ~60 mb/s
//#define BUFSIZE 65536   // ~63 mb/s
//#define BUFSIZE 131072  // ~63 mb/s
//#define BUFSIZE 262144  // ~55 mb/s

/* Adler32 -> ~75 mb/s */

void compute_hash(HANDLE hFile, uint32_t *hashcode)
{
    DWORD count = 0;
    unsigned char data[BUFSIZE];
    long pan_blocks = 0, rembits = 0, bitlength = 0;
    int i;

    uint32_t little_endian_remnant[PAN_STAGE_SIZE] = {0,0,0,0,0,0,0,0};

    PAN_BUFFER buffer;
    PAN_STATE state;

    /* initialize the Panama state machine for a fresh hashing operation */
    pan_reset(&buffer, &state);

    SetFilePointer(hFile, 0, NULL, FILE_BEGIN);

    while (ReadFile(hFile, data, sizeof(data), &count, NULL) && count)
    {
        bitlength = count * 8;

        /* divide the source array into full 256-bit blocks and a remnant */
        pan_blocks = bitlength / (PAN_STAGE_SIZE * WORDLENGTH);
        rembits = bitlength - pan_blocks * (PAN_STAGE_SIZE * WORDLENGTH);

        /* perform hashing operation on sourcetext array except for remnant */
        if (pan_blocks > 0)
            pan_push((uint32_t *) data, pan_blocks, &buffer, &state);
    }

    /* copy remnant bits to temporary array and append '1' */
    for (i = 0; i < (rembits + WORDLENGTH - 1) / WORDLENGTH; i++)
        little_endian_remnant[i] = *(data + pan_blocks * PAN_STAGE_SIZE + i);

    little_endian_remnant[rembits / WORDLENGTH] &= ~(~0L << (rembits % WORDLENGTH));
    little_endian_remnant[rembits / WORDLENGTH] |= 1L << (rembits % WORDLENGTH);

    /* operate on final remnant, padded out to 256-bit block */
    pan_push(little_endian_remnant, 1, &buffer, &state);

    /* perform 32 dummy PULL operations */
    pan_pull(NULL, NULL, 32, &buffer, &state);

    /* perform final PULL operation */
    pan_pull(NULL, hashcode, 1, &buffer, &state);

    SetFilePointer(hFile, 0, NULL, FILE_BEGIN);
}
