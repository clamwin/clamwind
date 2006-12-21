//-----------------------------------------------------------------------------
// Name:        pipeserver.h
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/03/21
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

#ifndef _PIPESERVER_H_
#define _PIPESERVER_H_

#include <clamwind.h>

#define PIPE_TIMEOUT 5000

inline uint32_t string_to_uint32(const char *str)
{
    if (!str) return 0;
    int64_t jobid64 = _atoi64(str);
    /* This checks if > 0 and < max int32 */
    if ((jobid64 > 0) && (jobid64 < ((uint32_t) ~0)))
        return (uint32_t) jobid64;
    else
        return 0;
}
class cwPipeServer
{
    typedef struct
    {
        OVERLAPPED oOverlap;
        HANDLE hPipeInst;
        char chRequest[XML_MSG_SIZE];
        DWORD cbRead;
        char chReply[XML_MSG_SIZE];
        DWORD cbToWrite;
        CClamWinD *svc;
    } PIPEINST, *LPPIPEINST;


    static void DisconnectAndClose(LPPIPEINST lpPipeInst);
    static void GetAnswerToRequest(LPPIPEINST pipe);
    static bool ConnectToNewClient(HANDLE hPipe, LPOVERLAPPED lpo);
    static bool CreateAndConnectInstance(LPOVERLAPPED lpoOverlap, HANDLE& hPipe);

    // completion routines need to be static
    static void WINAPI CompletedReadRoutine(DWORD dwErr, DWORD cbBytesRead, LPOVERLAPPED lpOverLap);
    static void WINAPI CompletedWriteRoutine(DWORD dwErr, DWORD cbWritten, LPOVERLAPPED lpOverLap);

public:
    // main pipe server thread
    static DWORD WINAPI Run(LPVOID lpvThreadParam);
};

#endif /* _PIPESERVER_H_ */
