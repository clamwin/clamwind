//-----------------------------------------------------------------------------
// Name:        pipeclient.cpp
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

#define BUFSIZE 1024
CLogFile log;

int main(int argc, char *argv[])
{
    HANDLE hFile;
    BOOL flg;
    DWORD dwWrite, cbBytesRead;
    int i;
    char reply[BUFSIZE];
    char *buffer = NULL;
    FILE *fd = NULL;
    size_t flen = 0;

    if (argc < 2)
    {
        printf("Usage: %s file1 [file2] [file3] [...]\n", argv[0]);
        return -1;
    }

    log.InitStdOut(LOG_TRACE);

    hFile = CreateFile(CLAMWIND_PIPE, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
    if(hFile == INVALID_HANDLE_VALUE)
    {
        printf("CreateFile failed for Named Pipe client %d\n", ::GetLastError());
        return -1;
    }

    for (i = 1; i < argc; i ++)
    {
        if (!(fd = fopen(argv[i], "rb")))
        {
            printf("Invalid file %s - skipping\n", argv[i]);
            continue;
        }

        fseek(fd, 0L, SEEK_END);
        flen = ftell(fd);
        fseek(fd, 0L, SEEK_SET);

        buffer = new char[flen];
        if (fread(buffer, 1, flen, fd) != flen)
        {
            perror("fread()");
            delete buffer;
            fclose(fd);
            continue;
        }
        fclose(fd);

        printf("-------------------------------------\n");
        printf("Sending %s to the server\n", argv[i]);
        flg = WriteFile(hFile, buffer, (DWORD) flen, &dwWrite, NULL);
        FlushFileBuffers(hFile);
        if (FALSE == flg)
            printf("WriteFile failed for Named Pipe client: %d\n", ::GetLastError());
        else
            printf("WriteFile succeeded for Named Pipe client\n");
        delete buffer;

        cbBytesRead = 0;
        if (ReadFile(hFile, reply, BUFSIZE, &cbBytesRead, NULL))
        {
            reply[cbBytesRead] = 0;
            printf("Server reply: %s\n", reply);
            CwXmlMessage *msg = new CwXmlMessage(reply, (int) cbBytesRead, TAG_CLAMWINREPLY);
            delete msg;
        }
        else
            printf("Error reading from server %d\n", ::GetLastError());
        FlushFileBuffers(hFile);
    }

    CloseHandle(hFile);
    return 0;
}
