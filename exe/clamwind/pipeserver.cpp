//-----------------------------------------------------------------------------
// Name:        pipeserver.cpp
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
#include <pipeserver.h>

void cwPipeServer::DisconnectAndClose(LPPIPEINST lpPipeInst)
{
    /* if (!DisconnectNamedPipe(lpPipeInst->hPipeInst))
        dbgprint(LOG_ALWAYS, L"PipeServer :: DisconnectNamedPipe failed with %d.\n", ::GetLastError());*/

    DisconnectNamedPipe(lpPipeInst->hPipeInst);
    CloseHandle(lpPipeInst->hPipeInst);
    if (lpPipeInst) GlobalFree(lpPipeInst);
}

void cwPipeServer::GetAnswerToRequest(LPPIPEINST pipe)
{
    CwXmlMessage *msg;
    /* Skip UTF-8 BOM */
    if (memcmp(pipe->chRequest, "\xef\xbb\xbf", 3))
        msg = new CwXmlMessage(pipe->chRequest, (DWORD) pipe->cbRead);
    else
        msg = new CwXmlMessage(&pipe->chRequest[3], (DWORD) pipe->cbRead - 3);

    if (msg->valid)
    {
        msg->client = pipe->hPipeInst;
        std::string reply = pipe->svc->HandleXmlMessage(msg);
        strcpy(pipe->chReply, reply.c_str());
        pipe->cbToWrite = (DWORD) reply.length();
    }
    else
    {
        strcpy(pipe->chReply, FULL_ERROR_REPLY);
        pipe->cbToWrite = (DWORD) sizeof(FULL_ERROR_REPLY);
    }
    delete msg;
}

bool cwPipeServer::ConnectToNewClient(HANDLE hPipe, LPOVERLAPPED lpo)
{
   bool fConnected, fPendingIO = false;
   fConnected = ConnectNamedPipe(hPipe, lpo)==TRUE;

   if (fConnected)
   {
      dbgprint(LOG_ALWAYS, L"PipeServer :: ConnectNamedPipe failed with with %d..\n", ::GetLastError());
      return false;
   }

    switch (GetLastError())
    {
        /* The overlapped connection in progress. */
        case ERROR_IO_PENDING:
            fPendingIO = true;
            break;

        /* Client is already connected, so signal an event. */
        case ERROR_PIPE_CONNECTED:
            if (SetEvent(lpo->hEvent))
            break;

        /* If an error occurs during the connect operation... */
        default:
        {
            dbgprint(LOG_ALWAYS, L"PipeServer :: ConnectNamedPipe failed with %d.\n", ::GetLastError());
            return false;
        }
    }
    return fPendingIO;
}

bool cwPipeServer::CreateAndConnectInstance(LPOVERLAPPED lpoOverlap, HANDLE& hPipe)
{
    hPipe = CreateNamedPipe(CLAMWIND_PIPE,
        PIPE_ACCESS_DUPLEX |         // read/write access
        FILE_FLAG_OVERLAPPED,        // overlapped mode
        PIPE_TYPE_MESSAGE |          // message-type pipe
        PIPE_READMODE_MESSAGE |      // message read mode
        PIPE_WAIT,                   // blocking mode
        PIPE_UNLIMITED_INSTANCES,    // unlimited instances
        XML_MSG_SIZE,                // output buffer size
        XML_MSG_SIZE,                // input buffer size
        PIPE_TIMEOUT,                // client time-out
        NULL);                       // default security attributes

    if (hPipe == INVALID_HANDLE_VALUE)
    {
        dbgprint(LOG_ALWAYS, L"PipeServer :: CreateNamedPipe failed with %d..\n", ::GetLastError());
        return false;
    }

    return ConnectToNewClient(hPipe, lpoOverlap);
}

VOID WINAPI cwPipeServer::CompletedReadRoutine(DWORD dwErr, DWORD cbBytesRead, LPOVERLAPPED lpOverLap)
{
    LPPIPEINST lpPipeInst;
    BOOL fWrite = FALSE;

    /* lpOverlap points to storage for this instance. */
    lpPipeInst = (LPPIPEINST) lpOverLap;

    /* The read operation has finished, so write a response (if no error occurred). */
    if ((dwErr == 0) && (cbBytesRead != 0))
    {
        lpPipeInst->cbRead = cbBytesRead;
        GetAnswerToRequest(lpPipeInst);

        fWrite = WriteFileEx(
            lpPipeInst->hPipeInst,
            lpPipeInst->chReply,
            lpPipeInst->cbToWrite,
            (LPOVERLAPPED) lpPipeInst,
            (LPOVERLAPPED_COMPLETION_ROUTINE) CompletedWriteRoutine);
    }

    /* Disconnect if an error occurred. */
    if (!fWrite) DisconnectAndClose(lpPipeInst);
}

/* CompletedWriteRoutine(DWORD, DWORD, LPOVERLAPPED)
 * This routine is called as a completion routine after writing to
 * the pipe, or when a new client has connected to a pipe instance.
 * It starts another read operation.
 */
VOID WINAPI cwPipeServer::CompletedWriteRoutine(DWORD dwErr, DWORD cbWritten, LPOVERLAPPED lpOverLap)
{
    LPPIPEINST lpPipeInst;
    BOOL fRead = FALSE;

    lpPipeInst = (LPPIPEINST) lpOverLap;

    /* The write operation has finished, so read the next request (if there is no error). */
    if ((dwErr == 0) && (cbWritten == lpPipeInst->cbToWrite))
        fRead = ReadFileEx(lpPipeInst->hPipeInst, lpPipeInst->chRequest,
                           XML_MSG_SIZE,
                           (LPOVERLAPPED) lpPipeInst,
                           (LPOVERLAPPED_COMPLETION_ROUTINE) CompletedReadRoutine);

    /* Disconnect if an error occurred. */
    if (!fRead) DisconnectAndClose(lpPipeInst);
}

DWORD WINAPI cwPipeServer::Run(LPVOID lpvThreadParam)
{
    CClamWinD *svc = static_cast<CClamWinD *>(lpvThreadParam);
    HANDLE hPipe = INVALID_HANDLE_VALUE;
    HANDLE hConnectEvent;
    HANDLE handleArray[2];
    BOOL fSuccess, fPendingIO;
    OVERLAPPED oConnect;
    LPPIPEINST lpPipeInst;
    DWORD dwWait, cbRet;

    dbgprint(LOG_ALWAYS, L"PipeServer :: Starting...\n");

    if ((hConnectEvent = CreateEvent(NULL, TRUE, TRUE, NULL)) == NULL)
    {
        dbgprint(LOG_ALWAYS, L"PipeServer :: CreateEvent failed with %d.\n", ::GetLastError());
        return -1;
    }

    oConnect.hEvent = hConnectEvent;
    fPendingIO = CreateAndConnectInstance(&oConnect, hPipe);

    handleArray[0] = hConnectEvent;
    handleArray[1] = svc->hStopEvent;

    while (true)
    {
        /* Wait for a client to connect, or for a read or write
         * operation to be completed, which causes a completion
         * routine to be queued for execution. */
        //dwWait = WaitForSingleObjectEx(hConnectEvent, INFINITE, TRUE);
        dwWait = WaitForMultipleObjectsEx(2, handleArray, FALSE, INFINITE, TRUE);

        switch (dwWait)
        {
            /* The wait conditions are satisfied by a completed connect operation. */
            case WAIT_OBJECT_0:
                /* If an operation is pending, get the result of the connect operation. */
                if (fPendingIO)
                {
                    fSuccess = GetOverlappedResult(hPipe, &oConnect, &cbRet, FALSE);
                    if (!fSuccess)
                    {
                        dbgprint(LOG_ALWAYS, L"PipeServer :: ConnectNamedPipe (%d)\n", ::GetLastError());
                        return -1;
                    }
                }

                /* Allocate storage for this instance. */
                lpPipeInst = (LPPIPEINST) GlobalAlloc(GPTR, sizeof(PIPEINST));
                if (!lpPipeInst)
                {
                    dbgprint(LOG_ALWAYS, L"PipeServer :: GlobalAlloc failed (%d)\n", ::GetLastError());
                    return -1;
                }

                lpPipeInst->hPipeInst = hPipe;
                lpPipeInst->svc = svc;

                /* Start the read operation for this client.
                 * Note that this same routine is later used as a
                 * completion routine after a write operation.
                 */

                lpPipeInst->cbToWrite = 0;
                CompletedWriteRoutine(0, 0, (LPOVERLAPPED) lpPipeInst);

                /* Create new pipe instance for the next client. */

                fPendingIO = CreateAndConnectInstance(&oConnect, hPipe);
                break;

            case WAIT_OBJECT_0 + 1: /* Done */
                dbgprint(LOG_ALWAYS, L"PipeServer :: Done...\n");
                //DisconnectAndClose(lpPipeInst);
                return 0;
                break;

                /* The wait is satisfied by a completed read or write
                 * operation. This allows the system to execute the
                 * completion routine.
                 */
            case WAIT_IO_COMPLETION:
                break;

            /* An error occurred in the wait function. */
            default:
                return -1;
        }
    }
    return 0; /* Never reached */
}
