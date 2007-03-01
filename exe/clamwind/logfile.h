//-----------------------------------------------------------------------------
// Name:        logfile.h
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

#ifndef __LOGFILE_H_
#define __LOGFILE_H_

#include <clamwind.h>
#include <share.h>
#include <stdio.h>

#define dbgprint log.Write

enum
{
    LOG_ALWAYS = 0,
    LOG_INFO,
    LOG_DEBUG,
    LOG_TRACE
};

class CLogFile
{
public:
    CLogFile(): m_dwLogLevel(0), m_pOutFile(stdout)
    {
        m_hMutex = ::CreateMutex(NULL, FALSE, L"{45428D53-A5DB-4168-BD3C-658419B60279}");
    }
    virtual ~CLogFile()
    {
        if(m_pOutFile && m_pOutFile!=stdout)
            fclose(m_pOutFile);
        ::CloseHandle(m_hMutex);
    }


    //
    // output to a file
    bool InitStdOut(DWORD dwLogLevel)
    {
        m_dwLogLevel = dwLogLevel;
        m_pOutFile = stdout;
        return true;
    }

    //
    // output to a file
    bool InitFile(DWORD dwLogLevel, wchar_t * pszFileName)
    {
        m_dwLogLevel = dwLogLevel;
        m_pOutFile = _wfsopen(pszFileName, L"w", _SH_DENYNO);
        return !!m_pOutFile;
    }

    void Write(INT level, LPCWSTR szFormat, ...)
    {
        ::WaitForSingleObject(m_hMutex, INFINITE);
        __try
        {
            if (ShouldWrite(level))
            {
                if (m_pOutFile)
                {
                        WCHAR szMsg[4096];
                        va_list marker;
                        va_start( marker, szFormat );
                        vswprintf_s(szMsg, sizeof(szMsg)/sizeof(szMsg[0]), szFormat, marker);
                        va_end(marker);
                        fputws(szMsg, m_pOutFile);
                        fflush(m_pOutFile);
                }

            }
        }
        __finally
        {
            ::ReleaseMutex(m_hMutex);
        }
    }

    void Write(INT level, LPCSTR szFormat, ...)
    {
        ::WaitForSingleObject(m_hMutex, INFINITE);
        __try
        {
            if (ShouldWrite(level))
            {
                if (m_pOutFile)
                {
                        CHAR szMsg[4096];
                        va_list marker;
                        va_start( marker, szFormat );
                        vsprintf_s(szMsg, sizeof(szMsg)/sizeof(szMsg[0]), szFormat, marker);
                        va_end(marker);
                        fputs(szMsg, m_pOutFile);
                        fflush(m_pOutFile);
                }

            }
        }
        __finally
        {
            ::ReleaseMutex(m_hMutex);
        }
    }


private:
    FILE *m_pOutFile;
    DWORD m_dwLogLevel;
    HANDLE m_hMutex;
    bool m_bConsole;

    inline bool ShouldWrite(DWORD level)
    {
        if(!m_pOutFile) return false;
        if (level <= m_dwLogLevel) return true;
        return false;
    }
};

#endif /* __LOGFILE_H_ */
