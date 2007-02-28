// Name:        Program.cs
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
using System.Windows.Forms;
using System.Threading;

namespace ClamWinApp
{
    #region Program class
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string [] args)
        {
            bool Created;
            Mutex mutex = new Mutex(false, "ClamWinStartupMutex", out Created);
            
            if (!Created)
            {
                // ClamWin instance already exist, let pop it up
                IntPtr Handle = Win32API.FindWindow("WindowsForms10.Window.8.app.0.378734a", "ClamWin Free Antivirus");

                if (0 != (int)Handle)
                {
                    Win32API.SetWindowPos(Handle,
                                          Win32API.HWND_TOP,
                                          0,
                                          0,
                                          0,
                                          0,
                                          Win32API.SWP_NOSIZE |
                                          Win32API.SWP_NOMOVE |
                                          Win32API.SWP_SHOWWINDOW
                                         );
                    Win32API.SetForegroundWindow(Handle);

                    OnArguments(args, Handle);

                    mutex.Close();
                    return;
                }
            }

            Application.EnableVisualStyles();            
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClamWinMainForm(args));

            /*try
            {
                Application.Run(new ClamWinMainForm(args));
            }
            catch (SystemException exc)
            { 
                MessageBox.Show( "Exception occured!\r\nWith message - \"" + exc.Message + "\"\r\n Source - \"" + exc.Source + "\"",
                                 "ClamWin",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }*/
            
            mutex.Close();
        }
        /// <summary>
        /// Check arguments and send corresponding mesages to existing instance of ClamWin
        /// </summary>
        /// <param name="args"></param>
        public static void OnArguments(string[] args, IntPtr Handle)
        {
            foreach (string arg in args)
            {
                if (arg == ClamWinScheduleData.UpdateArg)
                {
                    Win32API.PostMessage(Handle, ClamWinMainForm.UM_SCHEDULED_UPDATE, 0, 0);
                }
                else if (arg == ClamWinScheduleData.ScanArg)
                {
                    Win32API.PostMessage(Handle, ClamWinMainForm.UM_SCHEDULED_SCAN, 0, 0);
                }
                else if (arg == ClamWinScheduleData.ScanCriticalArg)
                {
                    Win32API.PostMessage(Handle, ClamWinMainForm.UM_SCHEDULED_SCAN_CRITICAL, 0, 0);
                }
                else if (arg == ClamWinScheduleData.ScanMyPCArg)
                {
                    Win32API.PostMessage(Handle, ClamWinMainForm.UM_SCHEDULED_SCAN_MY_PC, 0, 0);
                }            
            }        
        }
    }
    #endregion
}