using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ClamWinApp
{
    #region Win32API class
    /// <summary>
    /// Windows application programing interface wrapper
    /// </summary>    
    class Win32API
    {
        #region Constants
        public const int WM_USER = 0x0400;
        public const int WM_CLOSE = 0x0010;
        public const int WM_DEVICECHANGE = 0x0219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVTYP_VOLUME = 0x00000002;
        public const int MAX_PATH = 260;

        public const uint GENERIC_READ = (0x80000000);        
        public const uint GENERIC_WRITE = (0x40000000);        
        public const uint GENERIC_EXECUTE = (0x20000000);        
        public const uint GENERIC_ALL = (0x10000000);
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        public const uint ERROR_IO_PENDING = 997;

        public const uint WAIT_OBJECT_0 = 0x00000000;

        public const uint CREATE_NEW = 1;        
        public const uint CREATE_ALWAYS = 2;        
        public const uint OPEN_EXISTING = 3;        
        public const uint OPEN_ALWAYS = 4;        
        public const uint TRUNCATE_EXISTING = 5;        
        public const int INVALID_HANDLE_VALUE = -1;        
        public const ulong ERROR_SUCCESS = 0;        
        public const ulong ERROR_CANNOT_CONNECT_TO_PIPE = 2;        
        public const ulong ERROR_PIPE_BUSY = 231;        
        public const ulong ERROR_NO_DATA = 232;        
        public const ulong ERROR_PIPE_NOT_CONNECTED = 233;        
        public const ulong ERROR_MORE_DATA = 234;        
        public const ulong ERROR_PIPE_CONNECTED = 535;        
        public const ulong ERROR_PIPE_LISTENING = 536;        

        public const Int32 HWND_TOPMOST = -1;
        public const Int32 HWND_TOP = 0;
        public const Int32 SWP_NOACTIVATE = 0x0010;
        public const Int32 SWP_NOSIZE = 0x0001;
        public const Int32 SWP_NOMOVE = 0x0002;
        public const Int32 SWP_SHOWWINDOW = 0x0040;  

        // ShowWindow commands
        public const int SW_HIDE             = 0;
        public const int SW_SHOWNORMAL       = 1;
        public const int SW_NORMAL           = 1;
        public const int SW_SHOWMINIMIZED    = 2;
        public const int SW_SHOWMAXIMIZED    = 3;
        public const int SW_MAXIMIZE         = 3;
        public const int SW_SHOWNOACTIVATE   = 4;
        public const int SW_SHOW             = 5;
        public const int SW_MINIMIZE         = 6;
        public const int SW_SHOWMINNOACTIVE  = 7;
        public const int SW_SHOWNA           = 8;
        public const int SW_RESTORE          = 9;
        public const int SW_SHOWDEFAULT      = 10;
        public const int SW_FORCEMINIMIZE    = 11;
        public const int SW_MAX              = 11;
     
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct _DEV_BROADCAST_HDR
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
        };          
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct OVERLAPPED 
        {  
            public IntPtr Internal;  
            public IntPtr InternalHigh;              
            public IntPtr Offset;      
            public IntPtr OffsetHigh;                
            public IntPtr hEvent;
        };
        #endregion

        #region Interfaces
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
         Guid("00000002-0000-0000-C000-000000000046")]
        public interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc([In] int cb);
            [PreserveSig]
            IntPtr Realloc([In] IntPtr pv, [In] int cb);
            [PreserveSig]
            void Free([In] IntPtr pv);
            [PreserveSig]
            int GetSize([In] IntPtr pv);
            [PreserveSig]
            int DidAlloc(IntPtr pv);
            [PreserveSig]
            void HeapMinimize();
        }

        #endregion

        #region DLL Import
        [DllImport("User32.DLL")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("User32.Dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, uint wParam, uint lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(String lpFileName,						  
                                                uint dwDesiredAccess,					  
                                                uint dwShareMode,						
                                                IntPtr attr,				
                                                uint dwCreationDisposition,			
                                                uint dwFlagsAndAttributes,			
                                                uint hTemplateFile
                                                );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(IntPtr hHandle,
                                            byte[] lpBuffer,						
                                            int nNumberOfBytesToRead,
                                            out UInt32 lpNumberOfBytesRead,
                                             ref OVERLAPPED lpOverlapped
                                            );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(IntPtr hHandle,
                                            byte[] lpBuffer,
                                            int nNumberOfBytesToWrite,
                                            byte[] lpNumberOfBytesWritten,
                                            ref OVERLAPPED lpOverlapped
                                            );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateEvent(IntPtr lpEventAttributes,
                                                bool bManualReset,
                                                bool bInitialState,
                                                string lpName);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ResetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WaitForSingleObject(IntPtr hHandle,int dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WaitForMultipleObjects(int nCount, IntPtr[] lpHandles, bool fWaitAll, int dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetOverlappedResult(IntPtr hFile, 
                                                      ref OVERLAPPED lpOverlapped,
                                                      ref IntPtr lpNumberOfBytesTransferred,
                                                      bool bWait
                                                      );

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(  IntPtr hWnd,
                                                    Int32 hWndInsertAfter, 
                                                    Int32 X, 
                                                    Int32 Y, 
                                                    Int32 cx, 
                                                    Int32 cy, 
                                                    uint uFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow( string lpClassName,
                                                string lpWindowName );
        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd,int nCmdShow);

        #endregion

        #region Shell32
        public class Shell32
        {
            #region Structs
            /// <summary>
            /// Struct used to store file information
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct SHFILEINFO
            {
                public IntPtr hIcon;
                public int iIcon;
                public int dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string szTypeName;
            }
            /// <summary>
            /// Struct used to pass parameters to SHBrowseForFolder
            /// </summary>
            [StructLayout(LayoutKind.Sequential, Pack = 8)]
            public struct BROWSEINFO
            {
                public IntPtr hwndOwner;
                public IntPtr pidlRoot;
                public IntPtr pszDisplayName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string lpszTitle;
                public int ulFlags;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public BFFCALLBACK lpfn;
                public IntPtr lParam;
                public int iImage;
            }
            #endregion

            #region Enums
            /// <summary>
            /// SHGetFileInfoConstants constants
            /// </summary>
            public enum SHGetFileInfoConstants : int
            {
                SHGFI_ICON = 0x100,                // get icon 
                SHGFI_DISPLAYNAME = 0x200,         // get display name 
                SHGFI_TYPENAME = 0x400,            // get type name 
                SHGFI_ATTRIBUTES = 0x800,          // get attributes 
                SHGFI_ICONLOCATION = 0x1000,       // get icon location 
                SHGFI_EXETYPE = 0x2000,            // return exe type 
                SHGFI_SYSICONINDEX = 0x4000,       // get system icon index 
                SHGFI_LINKOVERLAY = 0x8000,        // put a link overlay on icon 
                SHGFI_SELECTED = 0x10000,          // show icon in selected state 
                SHGFI_ATTR_SPECIFIED = 0x20000,    // get only specified attributes 
                SHGFI_LARGEICON = 0x0,             // get large icon 
                SHGFI_SMALLICON = 0x1,             // get small icon 
                SHGFI_OPENICON = 0x2,              // get open icon 
                SHGFI_SHELLICONSIZE = 0x4,         // get shell size icon 
                SHGFI_PIDL = 0x8,                  // pszPath is a pidl 
                SHGFI_USEFILEATTRIBUTES = 0x10,    // use passed dwFileAttribute 
                SHGFI_ADDOVERLAYS = 0x000000020,   // apply the appropriate overlays
                SHGFI_OVERLAYINDEX = 0x000000040   // Get the index of the overlay
            }
            
            /// <summary>
            /// Styles used in the BROWSEINFO.ulFlags field.
            /// </summary>
            [Flags]
            public enum BffStyles
            {
                RestrictToFilesystem = 0x0001, // BIF_RETURNONLYFSDIRS
                RestrictToDomain = 0x0002, // BIF_DONTGOBELOWDOMAIN
                RestrictToSubfolders = 0x0008, // BIF_RETURNFSANCESTORS
                ShowTextBox = 0x0010, // BIF_EDITBOX
                ValidateSelection = 0x0020, // BIF_VALIDATE
                NewDialogStyle = 0x0040, // BIF_NEWDIALOGSTYLE
                BrowseForComputer = 0x1000, // BIF_BROWSEFORCOMPUTER
                BrowseForPrinter = 0x2000, // BIF_BROWSEFORPRINTER
                BrowseForEverything = 0x4000, // BIF_BROWSEINCLUDEFILES
            }
            #endregion

            #region DLL Import
            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath,
                                                        int dwFileAttributes,
                                                        ref SHFILEINFO psfi,
                                                        uint cbFileInfo,
                                                        uint uFlags);

            [DllImport("Shell32.DLL")]
            public static extern int SHGetMalloc(out IMalloc ppMalloc);

            [DllImport("Shell32.DLL")]
            public static extern int SHGetSpecialFolderLocation(
                        IntPtr hwndOwner, int nFolder, out IntPtr ppidl);

            [DllImport("Shell32.DLL")]
            public static extern int SHGetPathFromIDList(
                        IntPtr pidl, StringBuilder Path);

            [DllImport("Shell32.DLL", CharSet = CharSet.Auto)]
            public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO bi);

            [DllImport("Shell32.DLL", CharSet = CharSet.Auto)]
            public static extern IntPtr ShellExecute(IntPtr hwnd,
                                                     string lpOperation,
                                                     string lpFile,
                                                     string lpParameters,
                                                     string lpDirectory,
                                                     int nShowCmd
                                                    );

            #endregion        
                                    
            #region Callbacks
            /// <summary>
            /// Delegate type used in BROWSEINFO.lpfn field.
            /// </summary>
            /// <param name="hwnd"></param>
            /// <param name="uMsg"></param>
            /// <param name="lParam"></param>
            /// <param name="lpData"></param>
            /// <returns></returns>
            public delegate int BFFCALLBACK(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData);
            #endregion                     
        }
        #endregion
    }
    #endregion
}
