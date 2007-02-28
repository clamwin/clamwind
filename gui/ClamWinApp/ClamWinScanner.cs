// Name:        ClamWinScanner.cs
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
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

namespace ClamWinApp
{
    #region ClamWinScanner class
    /// <summary>
    /// Scanner class provide scanning services
    /// </summary>
    public class ClamWinScanner
    {
        #region Private Data        
        /// <summary>
        /// Main ClamWin path
        /// </summary>
        //private static string ClamWinPath;
        /// <summary>
        /// Current scans
        /// </summary>
        private static ArrayList Scans = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        private static IntPtr varFilterJobsUpdatedEvent = (IntPtr)0;
        /// <summary>
        /// 
        /// </summary>
        private static IntPtr varStopFilterListening = (IntPtr)0;
        /// <summary>
        /// 
        /// </summary>
        private static ClamWinScan FilterFakeScan = null;
        #endregion       

        #region Enums
        /// <summary>
        /// Defined actions 
        /// </summary>
        public enum ActionID { NotDefined = -1, 
                               GetInfo = 0, 
                               ReloadDB, 
                               Scan, 
                               FsFilterControl,
                               AsyncScan,
                               AsyncResult,
                               AsyncAbortScan,
                               RegisterGui,
                               UnregisterGui,
                               GetFilterJob                               
                             };
        public enum ResponseStatus { NotDefined = -1, Ok = 0, Clean, Infected, Error, Scanning, NoSuchJob };
        #endregion

        #region Constants
        public const int UM_ITEM_SCAN_START = Win32API.WM_USER + 13;
        public const int UM_ITEM_SCAN_COMPLETE = Win32API.WM_USER + 14;
        public const int UM_ITEMS_SCAN_START = Win32API.WM_USER + 15;
        public const int UM_ITEMS_SCAN_COMPLETE= Win32API.WM_USER + 16;
        public const int UM_ITEMS_SCAN_ABORTED_BY_USER = Win32API.WM_USER + 17;
        public const int UM_ITEMS_SCAN_FAILED = Win32API.WM_USER + 18;
        public const int UM_ITEMS_SCAN_SUSPENDED = Win32API.WM_USER + 19;
        public const int UM_ITEMS_SCAN_RESUMED = Win32API.WM_USER + 20;
        public const int UM_ITEM_SCAN_PROGRESS = Win32API.WM_USER + 21;
        public const int UM_CATCH_FILTER_NOTIFY_DATA = Win32API.WM_USER + 25;
        public const int FakeScanID = -1;
        #endregion

        #region Public Constructor
        public ClamWinScanner()
        {                       
        }
        #endregion

        #region Private Helper Functions        
        #endregion

        #region Public Functions        
        /// <summary>
        /// Figure out status from specified text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ResponseStatus StringToStatus(string text)
        {
            switch (text)
            { 
                case "OK":
                    return ResponseStatus.Ok;
                case "CLEAN":
                    return ResponseStatus.Clean;
                case "INFECTED":
                    return ResponseStatus.Infected;
                case "ERROR":
                    return ResponseStatus.Error;     
                case "SCANNING":
                    return ResponseStatus.Scanning;
                case "NOSUCHJOB":
                    return ResponseStatus.NoSuchJob;
                default:
                    return ResponseStatus.NotDefined;            
            }                    
        }
        /// <summary>
        /// Return text description of specified status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string StatusToString(ResponseStatus status)
        {
            switch (status)
            { 
                case ResponseStatus.Ok:
                    return "Ok";
                case ResponseStatus.Clean:
                    return "Clean";
                case ResponseStatus.Infected:
                    return "Infected";
                case ResponseStatus.Error:
                    return "Error";
                case ResponseStatus.Scanning:
                    return "Scanning";
                case ResponseStatus.NoSuchJob:
                    return "NoSuchJob";
                default:
                    return "Undefined";            
            }
        }
        /// <summary>
        /// Figure out action name by id
        /// </summary>        
        /// <param name="Action">Action id</param>
        /// <returns>Action name</returns>
        public static string GetActionName(ClamWinScanner.ActionID Action)
        {
            switch (Action)
            {
                case ActionID.GetInfo:
                    return "getinfo";
                case ActionID.ReloadDB:
                    return "reloaddb";
                case ActionID.Scan:
                    return "scan";
                case ActionID.FsFilterControl:
                    return "fsfiltercontrol";
                case ActionID.AsyncScan:
                    return "asynscan";
                case ActionID.AsyncResult:
                    return "asynresult";
                case ActionID.AsyncAbortScan:
                    return "asynabortscan";
                case ActionID.RegisterGui:
                    return "registergui";
                case ActionID.UnregisterGui:
                    return "unregistergui";   
                case ActionID.GetFilterJob:
                    return "getfilterjob";               
                default:
                    return "notdefined";
            }
        }
        /// <summary>
        /// Figure out action id by name
        /// </summary>
        /// <param name="Action">Action name</param>
        /// <returns>Action id</returns>
        public static ActionID GetActionID(string Action)
        {
            switch (Action)
            { 
                case "getinfo":
                    return ActionID.GetInfo;
                case "reloaddb":
                    return ActionID.ReloadDB;
                case "scan":
                    return ActionID.Scan;
                case "fsfiltercontrol":
                    return ActionID.FsFilterControl;
                case "asynscan":
                    return ActionID.AsyncScan;
                case "asynresult":
                    return ActionID.AsyncResult;
                case "asynabortscan":
                    return ActionID.AsyncAbortScan;
                case "registergui":
                    return ActionID.RegisterGui;
                case "unregistergui":
                    return ActionID.UnregisterGui;
                case "getfilterjob":
                    return ActionID.GetFilterJob;                
                default:
                    return ActionID.NotDefined;
            }
        }
        /// <summary>
        /// Opens service pipe 
        /// </summary>
        /// <returns>true if success, false if failed</returns>
        public static bool Open(IntPtr handle)
        {
            /*
            System.ServiceProcess.ServiceController controller = null;
            try
            {
                controller = new System.ServiceProcess.ServiceController("ClamWind");

                if (controller.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    controller.Start();
                    
                    controller.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running,
                                             new TimeSpan(0, 0, 0, 0, 35 * 1000)
                                            );
                    if (controller.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                    {
                        throw 0;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                controller.Close();
            }   */

            varFilterJobsUpdatedEvent = Win32API.CreateEvent( (IntPtr)0, false, false, "FilterListenerJobsUpdated");
            varStopFilterListening = Win32API.CreateEvent((IntPtr)0, false, false, "StopFilterListening");

            FilterFakeScan = new ClamWinScan(ClamWinScanner.FakeScanID);
            IntPtr[] listeners = new IntPtr[1];
            listeners[0] = handle;
            FilterFakeScan.StartFilterListening(listeners);
            return true;
        }
        /// <summary>
        /// Close scanner, free resources
        /// </summary>
        public static void Close()
        {
            FilterFakeScan.StopFilterListening();
        }
        /// <summary>
        /// Create new scan object or select existing one(by ID) and start scan
        /// </summary>
        /// <param name="Items">Items for scan</param>
        /// <param name="Listeners">Listeners to be notified</param>
        /// <param name="ID">Scan identifier</param>
        /// <returns>1 on success or error code if failed</returns>
        public static int StartScan(ref ScannerItem[] Items, ref IntPtr[] Listeners, int ID)
        {
            if (ID == FakeScanID)
            {
                return -128;
            }

            foreach (ClamWinScan scan in Scans)
            {
                if (scan.GetScanID() == ID)
                { 
                    // Scan already exist
                    return scan.StartScan(ref Items, Listeners);                    
                }                
            }

            // New scan
            ClamWinScan Scan = new ClamWinScan(ID);
            
            Scans.Add(Scan);

            return Scan.StartScan(ref Items, Listeners);            
        }
        /// <summary>
        /// Stop specified scan
        /// </summary>
        /// <param name="ID">Scan identifier</param>
        public static void StopScan(int ID)
        {
            foreach (ClamWinScan scan in Scans)
            {
                if (scan.GetScanID() == ID)
                {
                    scan.TerminateScan();
                    return;
                }
            }
        }
        /// <summary>
        /// Suspend specified scan
        /// </summary>
        /// <param name="ID">Scan identifier</param>
        public static void SuspendScan(int ID)
        {
            foreach (ClamWinScan scan in Scans)
            {
                if (scan.GetScanID() == ID)
                {
                    scan.SuspendScan();
                    return;
                }
            }
        }
        /// <summary>
        /// Resume specified scan
        /// </summary>
        /// <param name="ID">Scan identifier</param>
        public static void ResumeScan(int ID)
        {
            foreach (ClamWinScan scan in Scans)
            {
                if (scan.GetScanID() == ID)
                {
                    scan.ResumeScan();
                    return;
                }
            }
        }
        /// <summary>
        /// Is specified scan running?
        /// </summary>
        /// <param name="ID">Scan identifier</param>
        public static bool IsScanRunning(int ID)
        {
            foreach (ClamWinScan scan in Scans)
            {
                if (scan.GetScanID() == ID)
                {
                    return scan.IsRunning();                    
                }
            }
            return false;
        }
        /// <summary>
        /// Is specified scan suspended?
        /// </summary>
        /// <param name="ID">Scan identifier</param>
        public static bool IsScanSuspended(int ID)
        {
            foreach (ClamWinScan scan in Scans)
            {
                if (scan.GetScanID() == ID)
                {
                    return scan.IsSuspended();
                }
            }
            return false;
        }
        /// <summary>
        /// Is specified scan suspended?
        /// </summary>
        /// <param name="ID">Scan identifier</param>
        public static bool ScanExist(int ID)
        {
            foreach (ClamWinScan scan in Scans)
            {
                if (scan.GetScanID() == ID)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Flushes scan statistic to db
        /// </summary>
        /// <param name="ID"></param>
        public static void FlushStatistic(int ID, int ScanID)
        {
            foreach (ClamWinScan scan in Scans)
            {
                if (scan.GetScanID() == ID)
                {
                    scan.FlushStatistics(ScanID);
                }
            }
        }
        /// <summary>
        /// Return version of database service is curently using
        /// </summary>
        /// <returns></returns>
        public static string GetDBVersion()
        {
            // TEST!!
            return "Unknown";

            ClamWinScan Scan = new ClamWinScan(FakeScanID);

            return Scan.GetDBVersion();
        }
        /// <summary>
        /// Register or unregister gui handle in service
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        public static bool ManageGuiHandle(IntPtr handle, bool register)
        {
            ClamWinScan Scan = new ClamWinScan(FakeScanID);

            return Scan.ManageGuiHandle(handle, register);
        }
        /// <summary>
        /// Change OnAccess Scaner status
        /// </summary>
        /// <param name="status"></param>
        public static bool SetOnAccessScanerStatus(ClamWinSettings.OnAccessStatus status)
        {
            ClamWinScan Scan = new ClamWinScan(FakeScanID);
            return Scan.SetOnAccessScanerStatus(status);
        }
        /// <summary>
        /// Awakes filter worker
        /// </summary>
        public static void AwakeFilterWorker()
        {
            Win32API.SetEvent(varFilterJobsUpdatedEvent);
        }
        #endregion
        
        #region ScannerItem struct
        public struct ScannerItem
        {
            /// <summary>
            /// If item represents logical volume
            /// </summary>
            public bool IsDrive;
            /// <summary>
            /// If item represents folder
            /// </summary>
            public bool IsFolder;
            /// <summary>
            /// Item path
            /// </summary>
            public string Path;
        }
        #endregion       

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public static IntPtr StopFilterListening
        {
            get
            {
                return varStopFilterListening;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static IntPtr FilterJobsUpdatedEvent
        {
            get
            {
                return varFilterJobsUpdatedEvent;
            }
        }
        #endregion
    }
    #endregion
}
