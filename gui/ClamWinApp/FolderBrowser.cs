// Name:        FolderBrowser.cs
// Product:     ClamWin Free Antivirus
//
// Author(s):      Garincho [garincho at users dot sourceforge dot net]
//
// Created:     2007/02/28
// Copyright:   Copyright ClamWin Pty Ltd (c) 2005-2007
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

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Text;

namespace ClamWinApp
{
    #region FolderBrowser class
    /// <summary>
    /// Component wrapper for BrowseForFolder Shell function
    /// </summary>
    public sealed class FolderBrowser : Component
    {
        #region Private Data
        /// <summary>
        /// // Root node of the tree view.
        /// </summary>
        private FolderID startLocation = FolderID.Desktop;                
        /// <summary>
        /// Browse info options.
        /// </summary>
        private int publicOptions = (int)Win32API.Shell32.BffStyles.RestrictToFilesystem |
             (int)Win32API.Shell32.BffStyles.RestrictToDomain;
        /// <summary>
        /// Browse info options.
        /// </summary>
        private int privateOptions = (int)Win32API.Shell32.BffStyles.NewDialogStyle;        
        /// <summary>
        /// Description text to show.
        /// </summary>
        private string descriptionText = "Please select an item(s) below:";        
        /// <summary>
        /// Folder chosen by the user.
        /// </summary>
        private string directoryPath = string.Empty;
        #endregion

        #region Enums
        /// <summary>
        /// Enum of CSIDLs identifying standard shell folders.
        /// </summary>
        public enum FolderID
        {
            Desktop = 0x0000,
            Printers = 0x0004,
            MyDocuments = 0x0005,
            Favorites = 0x0006,
            Recent = 0x0008,
            SendTo = 0x0009,
            StartMenu = 0x000b,
            MyComputer = 0x0011,
            NetworkNeighborhood = 0x0012,
            Templates = 0x0015,
            MyPictures = 0x0027,
            NetAndDialUpConnections = 0x0031,
        }
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Helper function that returns the IMalloc interface used by the shell.
        /// </summary>
        private static Win32API.IMalloc GetSHMalloc()
        {
            Win32API.IMalloc malloc;
            Win32API.Shell32.SHGetMalloc(out malloc);
            return malloc;
        }
        /// <summary>
        /// Helper function used to set and reset bits in the publicOptions bitfield.
        /// </summary>
        private void SetOptionField(int mask, bool turnOn)
        {
            if (turnOn)
            {
                publicOptions |= mask;
            }
            else
            {
                publicOptions &= ~mask;
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Shows the folder browser dialog box.
        /// </summary>
        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }
        /// <summary>
        /// Shows the folder browser dialog box with the specified owner window.
        /// </summary>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            IntPtr pidlRoot = IntPtr.Zero;

            // Get/find an owner HWND for this dialog.
            IntPtr hWndOwner;

            if (owner != null)
            {
                hWndOwner = owner.Handle;
            }
            else
            {
                hWndOwner = Win32API.GetActiveWindow();
            }

            // Get the IDL for the specific startLocation.
            Win32API.Shell32.SHGetSpecialFolderLocation(hWndOwner, (int)startLocation, out pidlRoot);

            if (pidlRoot == IntPtr.Zero)
            {
                return DialogResult.Cancel;
            }

            int mergedOptions = (int)publicOptions | (int)privateOptions;

            if ((mergedOptions & (int)Win32API.Shell32.BffStyles.NewDialogStyle) != 0)
            {
                if (System.Threading.ApartmentState.MTA == Application.OleRequired())
                {
                    mergedOptions = mergedOptions & (~(int)Win32API.Shell32.BffStyles.NewDialogStyle);
                }
            }

            IntPtr pidlRet = IntPtr.Zero;

            try
            {
                // Construct a BROWSEINFO.
                Win32API.Shell32.BROWSEINFO bi = new Win32API.Shell32.BROWSEINFO();
                IntPtr buffer = Marshal.AllocHGlobal(Win32API.MAX_PATH);

                bi.pidlRoot = pidlRoot;
                bi.hwndOwner = hWndOwner;
                bi.pszDisplayName = buffer;
                bi.lpszTitle = descriptionText;
                bi.ulFlags = mergedOptions;
                // The rest of the fields are initialized to zero by the constructor.
                // bi.lpfn = null;  bi.lParam = IntPtr.Zero;    bi.iImage = 0;

                // Show the dialog.
                pidlRet = Win32API.Shell32.SHBrowseForFolder(ref bi);

                // Free the buffer you've allocated on the global heap.
                Marshal.FreeHGlobal(buffer);

                if (pidlRet == IntPtr.Zero)
                {
                    // User clicked Cancel.
                    return DialogResult.Cancel;
                }

                // Then retrieve the path from the IDList.
                StringBuilder sb = new StringBuilder(Win32API.MAX_PATH);
                if (0 == Win32API.Shell32.SHGetPathFromIDList(pidlRet, sb))
                {
                    return DialogResult.Cancel;
                }

                // Convert to a string.
                directoryPath = sb.ToString();
            }
            finally
            {
                Win32API.IMalloc malloc = GetSHMalloc();
                malloc.Free(pidlRoot);

                if (pidlRet != IntPtr.Zero)
                {
                    malloc.Free(pidlRet);
                }
            }

            return DialogResult.OK;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Only return file system directories. If the user selects folders
        /// that are not part of the file system, the OK button is unavailable.
        /// </summary>
        [Category("Navigation")]
        [Description("Only return file system directories. If the user selects folders " +
                       "that are not part of the file system, the OK button is unavailable.")]
        [DefaultValue(true)]
        public bool OnlyFilesystem
        {
            get
            {
                return (publicOptions & (int)Win32API.Shell32.BffStyles.RestrictToFilesystem) != 0;
            }
            set
            {
                SetOptionField((int)Win32API.Shell32.BffStyles.RestrictToFilesystem, value);
            }
        }
        /// <summary>
        /// If network folder will be shown in tree view
        /// </summary>
        [Category("Navigation")]
        [Description("Determines if network folder will be shown in tree view.")]
        [DefaultValue(true)]
        public bool ShowNetworkFolders
        {
            get
            {
                return (publicOptions & (int)Win32API.Shell32.BffStyles.RestrictToDomain) != 0;
            }
            set
            {
                SetOptionField((int)Win32API.Shell32.BffStyles.RestrictToDomain, value);
            }
        }
        /// <summary>
        /// Determines if user will be able select only that subfolders which are ancestors of root path.
        /// </summary>
        [Category("Navigation")]
        [Description("Determines if user will be able select only that subfolders which are ancestors of root path.")]
        [DefaultValue(false)]
        public bool OnlySubfolders
        {
            get
            {
                return (publicOptions & (int)Win32API.Shell32.BffStyles.RestrictToSubfolders) != 0;
            }
            set
            {
                SetOptionField((int)Win32API.Shell32.BffStyles.RestrictToSubfolders, value);
            }
        }
        /// <summary>
        /// Determine if text box will be shown.
        /// </summary>
        [Category("Navigation")]
        [Description("Determine if text box will be shown.")]
        [DefaultValue(false)]
        public bool ShowTextBox
        {
            get
            {
                return (publicOptions & (int)Win32API.Shell32.BffStyles.ShowTextBox) != 0;
            }
            set
            {
                SetOptionField((int)Win32API.Shell32.BffStyles.ShowTextBox, value);
            }
        }
        /// <summary>
        /// If typed in item name will be validated.
        /// </summary>
        [Category("Navigation")]
        [Description("Determine if typed in item name will be validated.")]
        [DefaultValue(false)]
        public bool ValidateUserInput
        {
            get
            {
                return (publicOptions & (int)Win32API.Shell32.BffStyles.ValidateSelection) != 0;
            }
            set
            {
                SetOptionField((int)Win32API.Shell32.BffStyles.ValidateSelection, value);
            }
        }
        /// <summary>
        /// Only computers are allowed for selection, ok button will be disabled otherwise.
        /// </summary>
        [Category("Navigation")]
        [Description("Only computers are allowed for selection.")]
        [DefaultValue(false)]
        public bool SelectComputer
        {
            get
            {
                return (publicOptions & (int)Win32API.Shell32.BffStyles.BrowseForComputer) != 0;
            }
            set
            {
                SetOptionField((int)Win32API.Shell32.BffStyles.BrowseForComputer, value);
            }
        }
        /// <summary>
        /// Only printers are allowed for selection, ok button will be disabled otherwise.
        /// </summary>
        [Category("Navigation")]
        [Description("Only printers are allowed for selection.")]
        [DefaultValue(false)]
        public bool SelectPrinter
        {
            get
            {
                return (publicOptions & (int)Win32API.Shell32.BffStyles.BrowseForPrinter) != 0;
            }
            set
            {
                SetOptionField((int)Win32API.Shell32.BffStyles.BrowseForPrinter, value);
            }
        }
        /// <summary>
        /// The browse dialog box will display files as well as folders
        /// </summary>
        [Category("Navigation")]
        [Description("The browse dialog box will display files as well as folders.")]
        [DefaultValue(false)]
        public bool SelectFiles
        {
            get
            {
                return (publicOptions & (int)Win32API.Shell32.BffStyles.BrowseForEverything) != 0;
            }
            set
            {
                SetOptionField((int)Win32API.Shell32.BffStyles.BrowseForEverything, value);
            }
        }
        /// <summary>
        /// Location of the root folder from which to start browsing. Only the specified
        /// folder and any folders beneath it in the namespace hierarchy  appear
        /// in the dialog box.
        /// </summary>
        [Category("Navigation")]
        [Description("Location of the root folder from which to start browsing. Only the specified " +
                       "folder and any folders beneath it in the namespace hierarchy appear " +
                       "in the dialog box.")]
        [DefaultValue(typeof(FolderID), "0")]
        public FolderID StartLocation
        {
            get
            {
                return startLocation;
            }
            set
            {
                new UIPermission(UIPermissionWindow.AllWindows).Demand();
                startLocation = value;
            }
        }
        /// <summary>
        /// Full path to the folder selected by the user.
        /// </summary>
        [Category("Navigation")]
        [Description("Full path to the folder slected by the user.")]
        public string DirectoryPath
        {
            get
            {
                return directoryPath;
            }
        } 
        #endregion 
    }
    #endregion
}
