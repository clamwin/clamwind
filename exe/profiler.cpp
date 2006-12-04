//-----------------------------------------------------------------------------
// Name:        profiler.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2005/12/15
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

#define _CRT_SECURE_NO_DEPRECATE
#define _CRT_NON_CONFORMING_SWPRINTFS
#include <stdio.h>
#include <windows.h>
#include <io.h>
#include <fcntl.h>
#include <clamav.h>

/*
    On my TB 1.2Ghz
    Average speed: 1.608 mb/s - main + daily
    Average speed: 6.752 mb/s - daily
*/

extern "C" int cl_loaddb(const char *filename, struct cl_engine **engine, unsigned int *signo);

#define BMTOMS  (1000.0 / 1048576.0)

//#define PROFILE_ADLER32
//#define PROFILE_PANAMA

#define DAILY_ONLY

#define FILESPEC L"*.dll"

#define PADDING_50 L"                                                  \n"
#define PADDING_48 L"                                                \n"

#if defined(PROFILE_ADLER32)
#include "clamwind/adler32.cpp"
#elif defined(PROFILE_PANAMA)
#include <panama.h>
#include "clamwind/panama_x.cpp"
#include "clamwind/hash.cpp"
#else
static struct cl_limits limits;
static struct cl_node *m_root;
#endif

static double speed = 0.0;
static int runs = 1;
static int cycles = 0;

static void console_ctrl_handler(DWORD ctrl_type)
{
    wprintf(L"\nControl+C pressed, aborting...\n");
    if (speed > 0.0f) wprintf(L"Average speed: %4.3f mb/s" PADDING_50, (speed * BMTOMS / runs));
    exit(1);
}

static void stop_console_handler(void)
{
    SetConsoleCtrlHandler((PHANDLER_ROUTINE) console_ctrl_handler, FALSE);
}

double benchmark(void)
{

#if !defined(PROFILE_ADLER32) && !defined(PROFILE_PANAMA)
    const char *virname;
#endif
#ifdef PROFILE_PANAMA
    uint32_t hash[PAN_STAGE_SIZE];
#endif

    double p_size = 0.0, p_speed = 0.0, r_speed = 0.0;
    WIN32_FIND_DATA info;
    HANDLE hFile = INVALID_HANDLE_VALUE;
    HANDLE fp = INVALID_HANDLE_VALUE;
    DWORD ticker, now;
    size_t i = 0, processed = 0, elapsed = 0;
    int fd = -1;
    wchar_t szWinDir[MAX_PATH], szFileSpec[MAX_PATH], szFileName[MAX_PATH];
    wchar_t szLine[MAX_PATH];

    GetWindowsDirectory(szWinDir, MAX_PATH);
    wcscat(szWinDir, L"\\System32\\");
    szFileSpec[0] = L'\0';
    wcscat(szFileSpec, szWinDir);
    wcscat(szFileSpec, FILESPEC);
    fp = FindFirstFile(szFileSpec, &info);

    do
    {
        if (!_wcsicmp(L".", info.cFileName) || !_wcsicmp(L"..", info.cFileName))
        {
            if (!FindNextFile(fp, &info)) break;
            continue;
        }

        szFileName[0] = L'\0';
        wcscat(szFileName, szWinDir);
        wcscat(szFileName, info.cFileName);

        hFile = CreateFile(szFileName, GENERIC_READ, FILE_SHARE_READ|FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_FLAG_RANDOM_ACCESS, NULL);
        if (hFile == INVALID_HANDLE_VALUE) break;

#if !defined(PROFILE_ADLER32) && !defined(PROFILE_PANAMA)
        fd = _open_osfhandle((intptr_t) hFile, O_RDONLY | O_BINARY);
#endif

        /* Process the file */
        ticker = timeGetTime();
#if defined(PROFILE_ADLER32)
        compute_checksum(hFile);
#elif defined(PROFILE_PANAMA)
        compute_hash(hFile, hash);
#else
        cl_scandesc(fd, &virname, NULL, m_root, &limits, CL_SCAN_STDOPT);
#endif
        now = timeGetTime();
        /* End processing */

        elapsed = now - ticker; /* millisec */
        p_size = (double) GetFileSize(hFile, NULL); /* bytes */
        p_speed = 0;

        /* Skip scans < 1 millisec */
        if (elapsed)
        {
            processed++;
            p_speed = p_size / (double) elapsed; /* bytes / millisec */
            r_speed += p_speed;

            _snwprintf(szLine, sizeof(szLine), L"[%02d/%02d] %4.3f mb/s Scanned %s", runs, cycles, p_speed * BMTOMS, szFileName);
            SetConsoleTitle(szLine);
            /* Padding */
            for (i = wcslen(szLine); i < 78; i++)
                szLine[i] = L' ';
            szLine[78] = L'\r';
            szLine[79] = L'\0';
            wprintf(szLine);
        }

#if defined(PROFILE_ADLER32) || defined(PROFILE_PANAMA)
        CloseHandle(hFile);
#else
        _close(fd);
#endif

        if (!FindNextFile(fp, &info)) break;

    } while (fp && fp != INVALID_HANDLE_VALUE);

    FindClose(fp);
    return (r_speed / processed);
}

int wmain(int argc, wchar_t *argv[])
{
    if (argc > 3)
    {
        wprintf(L"Usage %s: [cycles]\n", argv[0]);
        return -1;
    }

    if (argc == 2)
    {
        cycles = _wtoi(argv[1]);
        if ((cycles < 2) || (cycles > 20))
        {
            wprintf(L"Invalid number of cycles\n");
            return -1;
        }
    }

    SetConsoleCtrlHandler((PHANDLER_ROUTINE) console_ctrl_handler, TRUE);
    atexit(stop_console_handler);

#if !defined(PROFILE_ADLER32) && !defined(PROFILE_PANAMA)
    int ret = -1;
    unsigned int no = 0;

    wprintf(L"Loading Clamav DB...\n");

#ifndef DAILY_ONLY
    ret = cl_loaddb("main.cvd", &m_root, &no);
    if (ret || !m_root || !no)
    {
        wprintf(L"Error loading main db...\n");
        return 1;
    }
#endif

    if ((ret = cl_loaddb("daily.cvd", &m_root, &no)))
    {
        wprintf(L"Error loading daily.cvd...\n");
        return -1;
    }

    wprintf(L"Known viruses: %d\n", no);
    /* build engine */
    if((ret = cl_build(m_root)))
    {
        wprintf(L"Error initializing database: %s\n", cl_strerror(ret));
        cl_free(m_root);
        return 1;
    }

    /* Specify no callback */
    m_root->callback = NULL;

    /* FIXME: use better values */
    memset(&limits, 0, sizeof(struct cl_limits));
    limits.maxfiles = 1;                /* max files */
    limits.maxfilesize = 1 * 524288;    /* maximal archived file size == 5 Mb */
    limits.maxreclevel = 1;             /* maximal recursion level */
    limits.maxratio = 1000;             /* maximal compression ratio */
    limits.archivememlim = 0;           /* disable memory limit for bzip2 scanner */
#endif

    if (cycles)
    {
        wprintf(L"Warm up cycle\n");
        benchmark();
        wprintf(L"Warm up done... starting..." PADDING_48);

        for (runs = 1; runs <= cycles; runs++)
            speed += benchmark();
    }
    else
    {
        cycles = 1;
        speed = benchmark();
    }

#if !defined(PROFILE_ADLER32) && !defined(PROFILE_PANAMA)
    cl_free(m_root);
#endif

    wprintf(L"Average speed: %4.3f mb/s" PADDING_50, (speed * BMTOMS / cycles));
}

