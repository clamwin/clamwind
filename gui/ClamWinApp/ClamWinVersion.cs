// Name:        ClamWinVersion.cs
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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace ClamWinApp
{
    #region ClamWinVersion class
    public class ClamWinVersion
    {
        #region
        /// <summary>
        /// Download
        /// </summary>
        private static DownloadDelegate Download = null;
        /// <summary>
        /// Result
        /// </summary>
        private static IAsyncResult Result = null;
        /// <summary>
        /// Current version
        /// </summary>
        private static Version CurrentVersion = new Version(0, 0, 0, 1);
        #endregion

        #region Hidden Constructor
        private ClamWinVersion()
        { 
        
        }
        #endregion

        #region Constants
        public const int UM_NEW_VERSION_AVAILABLE = Win32API.WM_USER + 301;
        #endregion

        #region Public Functions
        public static void CheckVersion(IntPtr[] Listeners)
        {
            if (Result != null)
            {
                if (!Result.AsyncWaitHandle.WaitOne(1, false))
                {
                    return;
                }
            }
            else
            {
                Download = new DownloadDelegate(DownloadWorker);
            }

            Result = Download.BeginInvoke(Listeners,null,null);
        }
        #endregion

        #region Private Helper Functions
        private static void DownloadWorker(IntPtr[] Listeners)
        {
            while (true)
            {
                WebClient client = new WebClient();
                byte[] data = client.DownloadData("http://clamwin.sourceforge.net/clamwin.ver");

                string helper = Encoding.ASCII.GetString(data, 0, data.Length);

                int pos = helper.IndexOf("=");

                if (pos == -1)
                {
                    return;
                }

                pos++;

                int pos1 = helper.IndexOf("[", pos);

                string version = helper.Substring(pos, pos1 - pos);
                Version Version = new Version();

                try
                {
                    pos = version.IndexOf(".");

                    if (pos != -1)
                    {
                        Version.part1 = int.Parse(version.Substring(0, pos));
                    }
                    else
                    {
                        throw new SystemException();
                    }

                    pos++;

                    pos1 = version.IndexOf(".", pos);

                    if (pos1 != -1)
                    {
                        Version.part2 = int.Parse(version.Substring(pos, pos1 - pos));
                    }
                    else
                    {
                        throw new SystemException();
                    }

                    pos1++;

                    pos = version.IndexOf(".", pos1);

                    if (pos != -1)
                    {
                        Version.part3 = int.Parse(version.Substring(pos1, pos - pos1));
                    }
                    else
                    {
                        throw new SystemException();
                    }

                    pos++;

                    if (pos != -1)
                    {
                        Version.part4 = int.Parse(version.Substring(pos));
                    }
                    else
                    {
                        throw new SystemException();
                    }
                }
                catch
                {
                    Version = CurrentVersion;
                }


                if (CurrentVersion.Compare(Version) == -1)
                {
                    NotifyData notify = new NotifyData();
                    notify.Version = Version;
                    notify.CurrentVersion = CurrentVersion;

                    foreach (IntPtr handle in Listeners)
                    {

                        int size = Marshal.SizeOf(notify);
                        IntPtr ptr = Marshal.AllocHGlobal(size);
                        Marshal.StructureToPtr(notify, ptr, false);
                        Win32API.PostMessage(handle, UM_NEW_VERSION_AVAILABLE, (uint)ptr.ToInt32(), 0);
                    }
                }

                System.Threading.Thread.Sleep(60000*60*12);
            }
        }
        #endregion

        #region
        private delegate void DownloadDelegate(IntPtr[] Listeners);
        #endregion

        #region Version struct
        public struct Version
        {
            #region Public Constructor
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <param name="p3"></param>
            /// <param name="p4"></param>
            public Version(int p1, int p2, int p3, int p4)
            {
                part1 = p1;
                part2 = p2;
                part3 = p3;
                part4 = p4;
            }
            #endregion

            #region Public Data
            public int part1;
            public int part2;
            public int part3;
            public int part4;
            #endregion

            #region Public Functions
            /// <summary>
            /// Compare specified version with this values
            /// </summary>
            /// <param name="ver"></param>
            /// <returns>0 if equal, 1 if this greater, -1 if this lesser</returns>
            public int Compare(Version ver)
            {              
                if (ver.part1 > part1)
                {
                    return -1;
                }
                else if (ver.part1 < part1)
                {
                    return 1;
                }
                else if (ver.part2 > part2)
                {
                    return -1;
                }
                else if (ver.part2 < part2)
                {
                    return 1;
                }
                else if (ver.part3 > part3)
                {
                    return -1;
                }
                else if (ver.part3 < part3)
                {
                    return 1;
                }
                else if (ver.part4 > part4)
                {
                    return -1;
                }
                else if (ver.part4 < part4)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            /// <summary>
            /// Return string representation of version
            /// </summary>
            /// <returns></returns>
            public new string ToString()
            {
                return part1.ToString() + "." + part2.ToString() + "." + part3.ToString() + "." + part4.ToString();
            }
            #endregion
        }
        #endregion

        #region NotifyData struct
        public struct NotifyData
        {
            public Version Version;
            public Version CurrentVersion;
        }
        #endregion
    }
    #endregion
}
