
#ifndef __IOCTL_H__
#define __IOCTL_H__

#define MAX_NAME_LENGTH 2024
#define MAX_DEVICE_NAME 128


#define FILTER_DOSDEVICE_NAME   L"\\DosDevices\\FileFilter"
#define FILTER_STARTFILTERING (ULONG) CTL_CODE( FILE_DEVICE_DISK_FILE_SYSTEM, 0x01, METHOD_BUFFERED, FILE_READ_ACCESS )
#define FILTER_STOPFILTERING (ULONG) CTL_CODE( FILE_DEVICE_DISK_FILE_SYSTEM, 0x02, METHOD_BUFFERED, FILE_READ_ACCESS )
#define FILTER_GETCREATERECORD (ULONG) CTL_CODE( FILE_DEVICE_DISK_FILE_SYSTEM, 0x03, METHOD_BUFFERED, FILE_READ_ACCESS )
#define FILTER_ALLOWED (ULONG) CTL_CODE( FILE_DEVICE_DISK_FILE_SYSTEM, 0x04, METHOD_BUFFERED, FILE_READ_ACCESS )
#define FILTER_DENIED (ULONG) CTL_CODE( FILE_DEVICE_DISK_FILE_SYSTEM, 0x05, METHOD_BUFFERED, FILE_READ_ACCESS )

// <New stuff from 13.01.2006>
#define FILTER_SETDATABASEVERSION (ULONG) CTL_CODE( FILE_DEVICE_DISK_FILE_SYSTEM, 0x07, METHOD_BUFFERED, FILE_READ_ACCESS )
#define FILTER_SETCACHEENABLE (ULONG) CTL_CODE( FILE_DEVICE_DISK_FILE_SYSTEM, 0x08, METHOD_BUFFERED, FILE_READ_ACCESS )
// </New stuff from 13.01.2006>
#endif __IOCTL_H__
