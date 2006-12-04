//-----------------------------------------------------------------------------
// Name:        ntdefs.h
// Product:     ClamWin Antivirus
//
// Author(s):   defs taken from various ReactOs files
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

#ifndef __NTDEFS_H_
#define __NTDEFS_H_

#include <clamwind.h>
#include <winternl.h>

// from ReactOS: w32api/include/ddk/ntstatus.h

#define STATUS_SUCCESS                  ((NTSTATUS) 0x00000000L)
#define STATUS_SHARING_VIOLATION        ((NTSTATUS) 0xC0000043L)
#define STATUS_DELETE_PENDING           ((NTSTATUS) 0xC0000056L)
#define STATUS_OBJECT_PATH_INVALID      ((NTSTATUS) 0xC0000039L)
#define STATUS_OBJECT_PATH_NOT_FOUND    ((NTSTATUS) 0xC000003AL)

#define FILE_OPEN                   0x00000001
#define FILE_DIRECTORY_FILE         0x00000001

#define OBJ_CASE_INSENSITIVE        0x00000040L


#define InitializeObjectAttributes(p, n, a, r, s)        \
{                                                        \
    (p)->Length = sizeof(OBJECT_ATTRIBUTES);           \
    (p)->RootDirectory = r;                             \
    (p)->Attributes = a;                                \
    (p)->ObjectName = n;                                \
    (p)->SecurityDescriptor = s;                        \
    (p)->SecurityQualityOfService = NULL;               \
}

typedef NTSYSAPI NTSTATUS (NTAPI *pfnRtlAppendUnicodeToString)(PUNICODE_STRING Destination, PCWSTR Source);
typedef NTSYSAPI NTSTATUS (NTAPI *pfnZwClose)(IN HANDLE Handle);

typedef NTSYSAPI NTSTATUS (NTAPI *pfnZwCreateFile)(
    OUT PHANDLE FileHandle,
    IN ACCESS_MASK DesiredAccess,
    IN POBJECT_ATTRIBUTES ObjectAttributes,
    OUT PIO_STATUS_BLOCK IoStatusBlock,
    IN PLARGE_INTEGER AllocationSize OPTIONAL,
    IN ULONG FileAttributes,
    IN ULONG ShareAccess,
    IN ULONG CreateDisposition,
    IN ULONG CreateOptions,
    IN PVOID EaBuffer OPTIONAL,
    IN ULONG EaLength
    );

#define FILE_DEVICE_DISK_FILE_SYSTEM    0x00000008
#define FILE_READ_ACCESS                (0x0001)
#define METHOD_BUFFERED                 0

#define CTL_CODE(DeviceType, Function, Method, Access) \
    (((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method))

#endif
