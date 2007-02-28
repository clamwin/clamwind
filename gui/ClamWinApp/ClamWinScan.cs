// Name:        ClamWinScan.cs
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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ClamWinApp
{    
    #region ClamWinScan
    /// <summary>
    /// Manage single scan
    /// </summary>
    public class ClamWinScan
    {
        #region NotifyData class
        /// <summary>
        /// Hold notification data
        /// </summary>
        public struct NotifyData
        {            
            /// <summary>
            /// Public constructor
            /// </summary>
            /// <param name="ID">Scan ID</param>
            public NotifyData(int ID)
            {
                ScanID = ID;
                Size = 0;
                Request = null;
                Response = null;
                Statistics = null;
                Item = "";
            }
            /// <summary>
            /// Public constructor
            /// </summary>
            /// <param name="ID">Scan ID</param>
            /// <param name="size">Item size</param>
            public NotifyData(int ID, ulong size)
            {
                ScanID = ID;
                Size = size;
                Request = null;
                Response = null;
                Statistics = null;
                Item = "";
            }
            /// <summary>
            /// Public constructor
            /// </summary>
            /// <param name="ID">Scan ID</param>
            /// <param name="size">Item size</param>
            /// /// <param name="Stat">Statistics</param>
            public NotifyData(int ID, ulong size, StatisticsData[] Stat)
            {
                ScanID = ID;
                Size = size;
                Request = null;
                Response = null;
                Statistics = new StatisticsData[Stat.Length];
                Stat.CopyTo(Statistics,0);
                Item = "";
            }
            /// <summary>
            /// Request
            /// </summary>
            public ClamWinScanRequest Request;
            /// <summary>
            /// Response
            /// </summary>
            public ClamWinScanResponse Response; 
            /// <summary>
            /// Item size
            /// </summary>
            public ulong Size;
            /// <summary>
            /// Scan idenitifier
            /// </summary>
            public int ScanID;
            /// <summary>
            /// Item name
            /// </summary>
            public string Item;
            /// <summary>
            /// Statistics
            /// </summary>
            public StatisticsData[] Statistics;
        }
        #endregion

        #region FilterNotifyData class
        /// <summary>
        /// Filter notifications data
        /// </summary>
        public class FilterNotifyData
        {
            public FilterNotifyData()
            {
                FileName = "";
                Message = "";
                Time = DateTime.Now;
                Processed = false;
            }

            public FilterNotifyData(FilterNotifyData obj)
            {
                this.FileName = (string)obj.FileName.Clone();
                this.Message = (string)obj.Message.Clone();
                this.Time = obj.Time;
                this.Processed = obj.Processed;
            }
            /// <summary>
            /// File name
            /// </summary>
            public string FileName;
            /// <summary>
            /// Message
            /// </summary>
            public string Message;
            /// <summary>
            /// Time
            /// </summary>
            public DateTime Time;
            /// <summary>
            /// Processed
            /// </summary>
            public bool Processed;
        }
        #endregion

        #region StatisticsData class
        /// <summary>
        /// Keep statistics for item
        /// </summary>
        public class StatisticsData
        {
            public StatisticsData()
            { 
            
            }
            public string Name;
            /// <summary>
            /// Number of scanned files
            /// </summary>
            public int Scanned = 0;
            /// <summary>
            /// Number of infected files
            /// </summary>
            public int Infected = 0;
            /// <summary>
            /// Number of errors 
            /// </summary>
            public int Errors = 0;
            /// <summary>
            /// Number of deleted files
            /// </summary>
            public int Deleted = 0;
            /// <summary>
            /// Number of files moved to quarantine
            /// </summary>
            public int MovedToQuarantine = 0;
        } 
        #endregion

        #region Private Data       
        /// <summary>
        /// Request/response locker
        /// </summary>
        private static object IOCommonLock = new object();
        /// <summary>
        /// Main resource locker
        /// </summary>
        private object MainLock = new object();
        /// <summary>
        /// If scan is running
        /// </summary>
        private bool Scanning = false;
        /// <summary>
        /// If scan need to be terminated
        /// </summary>
        private bool Terminate = false;
        /// <summary>
        /// If scan is suspended
        /// </summary>
        private bool Suspend = false;
        /// <summary>
        /// Scan delegate
        /// </summary>
        private ScanDelegate Scan = null;
        /// <summary>
        /// Filter worker instance
        /// </summary>
        private FilterWorker Filter = null;
        /// <summary>
        /// Keep result of Scan.BeginInvoke call
        /// </summary>
        private IAsyncResult ScanInvokeResult = null;
        /// <summary>
        /// Filter invoke result
        /// </summary>
        private IAsyncResult FilterInvokeResult = null;
        /// <summary>
        /// Statistics items array
        /// </summary>
        private StatisticsData[] Statistics = null;
        /// <summary>
        /// Index of current item 
        /// </summary>
        private int CurrentStatisticsItem = -1;
        /// <summary>
        /// Scan idintifier
        /// </summary>
        private int ScanID = -1;
        /// <summary>
        /// Current Items Size
        /// </summary>
        private ulong ItemsSize = 0;
        /// <summary>
        /// Scan events listeners
        /// </summary>
        private IntPtr[] Listeners;
        /// <summary>
        /// Structure for overlapped I/O
        /// </summary>
        private static Win32API.OVERLAPPED Overlapped = new Win32API.OVERLAPPED();
        /// <summary>
        /// Event 
        /// </summary>
        private static IntPtr OverlappedEvent = (IntPtr)Win32API.INVALID_HANDLE_VALUE;
        /// <summary>
        /// ReadFile static buffer (pipe reading works wrong if this buffer is not static)
        /// </summary>
        private static byte[] Buffer = new byte[1024];
        /// <summary>
        /// Pipe Handle
        /// </summary>
        private static IntPtr PipeHandle = (IntPtr)Win32API.INVALID_HANDLE_VALUE;
        /// <summary>
        /// Current job id , -1 if there is no current job
        /// </summary>
        private int CurrentJobID = -1;
        /// <summary>
        /// Scan is unbreakable
        /// </summary>
        private bool Unbreakable = false;
        /// <summary>
        /// If pipe ne to be reconected
        /// </summary>
        private static bool NeedToReconect = true;
        /// <summary>
        /// Size of items being scanned already
        /// </summary>
        private static ulong ScanedSoFarSize = 0;
        #endregion

        #region Public Constructor
        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="ID">Scan idintifier</param>
        public ClamWinScan(int ID)
        {
            if (ID == ClamWinScanner.FakeScanID)
            {
                Filter = new FilterWorker(FilterProc);
            }
            else
            {
                Scan = new ScanDelegate(ScanProc);
            }

            lock (MainLock)
            {
                ScanID = ID;                
            }
        }
        #endregion

        #region Constants
        /// <summary>
        /// Service pipe name to connect with
        /// </summary>
        public const string PipeName = "\\\\.\\pipe\\ClamWinD"; 
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Filter jobs worker
        /// </summary>
        private void FilterProc()
        {
            IntPtr[] Handles = new IntPtr[2];
            Handles[0] = ClamWinScanner.FilterJobsUpdatedEvent;
            Handles[1] = ClamWinScanner.StopFilterListening;

            ConnectToPipe();

            while (true)
            {
                int result = Win32API.WaitForMultipleObjects(2, Handles, false, -1);

                if (result == Win32API.WAIT_OBJECT_0)
                {                    
                    ClamWinScanRequest request = new ClamWinScanRequest(ClamWinScanner.ActionID.GetFilterJob);
                                        
                    ClamWinScanResponse response = null;
                    // Lock request/response pair
                    lock (ClamWinScan.IOCommonLock)
                    {
                        if (!SendRequest(ref request))
                        {
                            int error = Marshal.GetLastWin32Error();                            
                        }

                        if (!GetResponse(out response))
                        {
                            int error = Marshal.GetLastWin32Error();                         
                        }

                        if (response.GetActionStatus() == ClamWinScanner.ResponseStatus.Ok)
                        {
                            FilterNotifyData data = new FilterNotifyData();
                           
                            data.FileName = response.GetFileName();
                            data.Message = response.GetMessage();
                            data.Time = DateTime.Now;
                            data.Processed = false;

                            SendScanNotify(ClamWinScanner.UM_CATCH_FILTER_NOTIFY_DATA, data);

                            Win32API.SetEvent(Handles[0]);
                        }                        
                    }                                
                }
                else if (result == Win32API.WAIT_OBJECT_0+1)
                {
                    break;
                }            
            }
        }
        /// <summary>
        /// Scan functions
        /// </summary>
        /// <param name="Items"></param>
        private void ScanProc(ref ClamWinScanner.ScannerItem[] Items)
        {
            ConnectToPipe();

            lock (MainLock)
            {
                Scanning = true;
            }            

            ulong size = GetItemsSize(ref Items);

            lock (MainLock)
            {
                ScanedSoFarSize = 0;

                ItemsSize = size;
            }

            InitStatistics(ref Items);

            NotifyData notify;

            lock(MainLock)
            {
                notify = new NotifyData(ScanID,size,Statistics);
            }

            SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_START,notify);

            for (int i = 0; i < Items.Length; i++)
            {                
                if (TerminateScanSignaled())
                {                       
                    break;
                }

                // Processing items
                if (!Items[i].IsDrive && !Items[i].IsFolder)
                {
                    // Seems this is file, just scan it                    
                    if (!ScanFile(Items[i].Path))
                    {
                        AbortCurrentJob();
                        lock (MainLock)
                        {
                            if (Terminate)
                            {
                                break;
                            }
                        }
                        SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_FAILED, new NotifyData(ScanID));
                        break;
                    }
                }
                else
                {
                    string root;
                    root = Items[i].Path;

                    // Go recursively through folder
                    if (!ScanFolder(root))
                    {
                        lock (MainLock)
                        {
                            if (Terminate)
                            {
                                break;
                            }
                        }
                        SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_FAILED, new NotifyData(ScanID));
                        break;
                    }
                }
                
                lock (MainLock)
                {
                    CurrentStatisticsItem++;
                }
            }

            SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_COMPLETE, new NotifyData(ScanID));            

            lock (MainLock)
            {
                Terminate = false;

                Suspend = false;

                Scanning = false;
            }            
        }
        /// <summary>
        /// Returns total size 
        /// </summary>
        /// <param name="Items"></param>
        /// <returns></returns>
        private static ulong GetItemsSize(ref ClamWinScanner.ScannerItem[] Items)
        {
            ulong Size = 0;
            foreach (ClamWinScanner.ScannerItem item in Items)
            {
                if (!item.IsDrive && !item.IsFolder)
                {
                    //file
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(item.Path);
                        if (fi.Exists)
                        {
                            Size += (ulong)fi.Length;
                        }
                    }
                    catch
                    {
                    }
                }
                else if (item.IsFolder)
                {
                    //folder
                    try
                    {
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(item.Path);
                        if (di.Exists)
                        {
                            Size += GetDirectorySize(di);
                        }
                    }
                    catch
                    {
                    }
                }
                else if (item.IsDrive)
                {
                    //drive
                    try
                    {
                        System.IO.DriveInfo drvi = new System.IO.DriveInfo(item.Path);
                        Size += (ulong)(drvi.TotalSize - drvi.TotalFreeSpace);
                    }
                    catch
                    {
                    }
                }

            }
            return Size;
        }
        /// <summary>
        /// Figure out directory size
        /// </summary>
        /// <param name="di"></param>
        /// <returns></returns>
        private static ulong GetDirectorySize(System.IO.DirectoryInfo di)
        {
            ulong Size = 0;
            // Add file sizes.
            System.IO.FileInfo[] files = di.GetFiles();
            foreach (System.IO.FileInfo fi in files)
            {
                Size += (ulong)fi.Length;
            }
            // Add subdirectory sizes.
            System.IO.DirectoryInfo[] directories = di.GetDirectories();
            foreach (System.IO.DirectoryInfo directory in directories)
            {
                Size += GetDirectorySize(directory);
            }
            return Size;
        }             
        /// <summary>
        /// Send message to Listeners
        /// </summary>        
        private void SendScanNotify(int notification,NotifyData data)
        {           
            lock (MainLock)
            {
                if (Listeners == null)
                {
                    return;
                }

                foreach (IntPtr handle in Listeners)
                {
                    int size = Marshal.SizeOf(data);
                    IntPtr ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(data, ptr, false);
                    Win32API.PostMessage(handle, notification, (uint)ptr.ToInt32(), 0);
                }
            }
        }
        /// <summary>
        /// Send message to Listeners
        /// </summary>        
        private void SendScanNotify(int notification, FilterNotifyData data)
        {
            lock (MainLock)
            {
                if (Listeners == null)
                {
                    return;
                }

                foreach (IntPtr handle in Listeners)
                {
                    int size = Marshal.SizeOf(data);
                    IntPtr ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(data, ptr, false);
                    Win32API.PostMessage(handle, notification, (uint)ptr.ToInt32(), 0);
                }
            }
        }
        /// <summary>
        /// Return if scan need to be termianted, also send corresponding notification to callbacks
        /// </summary>             
        /// <returns>true - if signaled, false - otherwise</returns>
        private bool TerminateScanSignaled()
        {
            lock (MainLock)
            {
                if (Unbreakable)
                {
                    return false;
                }

                if (Terminate)
                {
                    SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_ABORTED_BY_USER, new NotifyData(ScanID));
                    return true;
                }
            }

            bool helper = false;
            while (true)
            {
                lock (MainLock)
                {
                    if (!Suspend)
                    {
                        if (helper)
                        {
                            SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_RESUMED, new NotifyData(ScanID));
                        }
                        break;
                    }
                    else
                    {
                        if (!helper)
                        {
                            SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_SUSPENDED, new NotifyData(ScanID));
                        }
                    }
                }
                lock (MainLock)
                {
                    if (Terminate)
                    {
                        SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_ABORTED_BY_USER, new NotifyData(ScanID));
                        return true;
                    }
                }
                System.Threading.Thread.Sleep(100);
                helper = true;
            }
            return false;
        }        
        /// <summary>
        /// Scan folder specified by path recursivelly
        /// </summary>
        /// <param name="path">Folder path</param>        
        private bool ScanFolder(string path)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            System.IO.FileInfo[] files;
            System.IO.DirectoryInfo[] folders;
            try
            {
                files = di.GetFiles("*.*");
                folders = di.GetDirectories("*.*");
            }
            catch
            {
                return true;
            }

            for (int i = 0; i < files.Length; i++)
            {

                if (TerminateScanSignaled())
                {
                    return false;
                }


                if (!ScanFile(files[i].FullName))
                {
                    AbortCurrentJob();                    
                    return false;
                }
            }

            for (int i = 0; i < folders.Length; i++)
            {

                if (TerminateScanSignaled())
                {
                    return false;
                }

                if (!ScanFolder(folders[i].FullName))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Check path against filter data
        /// </summary>
        /// <param name="patth"></param>
        /// <returns></returns>
        private bool CheckPath(string path)
        {
            bool UseFilter = false;
            ClamWinFilterData data = null;

            if (ScanID == (int)ClamWinMainForm.ScanPanelIDs.Scan)
            {
                UseFilter = ClamWinSettings.ScanUseFilter;
                data = ClamWinSettings.ScanFilterData;
            }
            else if (ScanID == (int)ClamWinMainForm.ScanPanelIDs.ScanMyPC)
            {
                UseFilter = ClamWinSettings.ScanMyPCUseFilter;
                data = ClamWinSettings.ScanMyPCFilterData;
            }
            else if (ScanID == (int)ClamWinMainForm.ScanPanelIDs.ScanCritical)
            {
                UseFilter = ClamWinSettings.ScanCriticalUseFilter;
                data = ClamWinSettings.ScanCriticalFilterData;
            }
            else
            {
                UseFilter = false;
            }

            if (!UseFilter)
            {
                return true;
            }

            if (data.IncludePatterns.Count != 0)
            {
                foreach (string pattern in data.IncludePatterns)
                {
                    //Regex expr = new Regex(pattern, RegexOptions.IgnoreCase);

                    if (Regex.IsMatch(path, ClamWinFilterData.WildcardToRegex(pattern)))                  
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                foreach (string pattern in data.ExcludePatterns)
                {
                    Regex expr = new Regex(pattern, RegexOptions.IgnoreCase);

                    Match match = expr.Match(path);
                    if (match.Success)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        /// <summary>
        /// Scan file specified by path
        /// </summary>
        /// <param name="path">Path to file</param>       
        private bool ScanFile(string path)
        {                                  
            ClamWinScanRequest request = new ClamWinScanRequest(ClamWinScanner.ActionID.AsyncScan);
            request.SetFilePath(path);

            ClamWinScanResponse response;

            lock (MainLock)
            {
                CurrentJobID = -1;
            }

            if (!CheckPath(path))
            {
                // Scan complete 
                ulong size = 0;
                try
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(path);
                    size = (ulong)fi.Length;
                }
                catch
                {

                }                

                NotifyData notify;
                lock (MainLock)
                {
                    notify = new NotifyData(ScanID, size, Statistics);
                }

                notify.Response = new ClamWinScanResponse("<clamwinreply><action>asynresult</action><status>CLEAN</status><jobid>-1</jobid><message>Skipped through filter</message><progress>100</progress></clamwinreply>"); 
                notify.Item = path;

                SendScanNotify(ClamWinScanner.UM_ITEM_SCAN_COMPLETE, notify);
                
                System.Threading.Thread.Sleep(25);
                return true;
            }
            // Lock request/response pair
            lock (ClamWinScan.IOCommonLock)
            {
                if (!SendRequest(ref request))
                {
                    int error = Marshal.GetLastWin32Error();
                    return false;
                }

                NotifyData data = new NotifyData(ScanID);
                data.Request = request;

                SendScanNotify(ClamWinScanner.UM_ITEM_SCAN_START, data);
                
                if (!GetResponse(out response))
                {
                    int error = Marshal.GetLastWin32Error();
                    return false;
                }
            }            

            if (response.GetActionStatus() != ClamWinScanner.ResponseStatus.Ok)
            { 
                // Service got problem with this file
                ulong size = 0;
                try
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(path);
                    size = (ulong)fi.Length;
                }
                catch
                {
                }                

                lock (MainLock)
                { 
                    //Statistics stuff 
                    Statistics[CurrentStatisticsItem].Errors++;
                    Statistics[0].Errors++;

                    ScanedSoFarSize += size;
                }

                NotifyData notify;
                lock (MainLock)
                {
                    notify = new NotifyData(ScanID, ScanedSoFarSize, Statistics);
                }

                notify.Response = new ClamWinScanResponse("<clamwinreply><action>asynresult</action><status>ERROR</status><jobid>-1</jobid></clamwinreply>");
                notify.Item = path;

                SendScanNotify(ClamWinScanner.UM_ITEM_SCAN_COMPLETE, notify);
                return true;
            }

            lock (MainLock)
            {
                CurrentJobID = response.GetJobID();
            }

            while (true)
            {
                // Check job status                 
                request = new ClamWinScanRequest(ClamWinScanner.ActionID.AsyncResult);

                lock (MainLock)
                {
                    request.SetJobID(CurrentJobID);
                }

                // Lock request/response pair
                lock (ClamWinScan.IOCommonLock)
                {
                    if (!SendRequest(ref request))
                    {
                        int error = Marshal.GetLastWin32Error();
                        return false;
                    }

                    if (!GetResponse(out response))
                    {
                        int error = Marshal.GetLastWin32Error();
                        return false;
                    }
                }

                if (response.GetActionStatus() == ClamWinScanner.ResponseStatus.Scanning)
                {         
                    // Still scanning
                    ulong size = 0;
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(path);
                        ulong fl = (ulong)fi.Length;
                        float progress = response.GetProgress() / 100.0f;
                        if (progress > 0)
                        {
                            size = (ulong)(fl * progress);
                        }
                        else
                        {
                            size = 0;
                        }
                    }
                    catch
                    {

                    }
                    
                    NotifyData notify;
                    lock (MainLock)
                    {
                        notify = new NotifyData(ScanID, ScanedSoFarSize + size);
                    }                   
                    notify.Item = path;

                    SendScanNotify(ClamWinScanner.UM_ITEM_SCAN_PROGRESS, notify);

                    System.Threading.Thread.Sleep(100);
                    continue;
                }
                else if (response.GetActionStatus() == ClamWinScanner.ResponseStatus.NoSuchJob)
                {
                    // Job gone, stop scanning now
                    return false;
                }
                else
                {
                    // Scan complete 
                    ulong size = 0;
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(path);
                        size = (ulong)fi.Length;
                    }
                    catch
                    {

                    }

                    lock (MainLock)
                    {
                        //Statistics stuff 
                        Statistics[CurrentStatisticsItem].Scanned++;
                        Statistics[0].Scanned++;

                        ScanedSoFarSize += size;
                    }

                    NotifyData notify;
                    lock (MainLock)
                    {
                        notify = new NotifyData(ScanID, ScanedSoFarSize, Statistics);
                    }

                    notify.Response = response;
                    notify.Item = path;

                    SendScanNotify(ClamWinScanner.UM_ITEM_SCAN_COMPLETE, notify);                    
                    break;
                }            
            }

            lock (MainLock)
            {
                CurrentJobID = -1;
            }
            return true;
        }        
        /// <summary>
        /// Create and intitalize statistics
        /// </summary>
        /// <param name="Items">Scan items</param>
        private void InitStatistics(ref ClamWinScanner.ScannerItem[] Items)
        {
            if (Items.Length == 0)
            {
                return;
            }

            lock (MainLock)
            {
                Statistics = new StatisticsData[Items.Length + 1];

                Statistics[0] = new StatisticsData();
                Statistics[0].Name = "Total";

                int i = 1;
                foreach (ClamWinScanner.ScannerItem item in Items)
                {
                    Statistics[i] = new StatisticsData();
                    Statistics[i].Name = item.Path;
                    i++;
                }

                CurrentStatisticsItem = 1;
            }
        }        
        /// <summary>
        /// Send request to service
        /// </summary>
        /// <returns>true if success, false if failed</returns>
        public bool SendRequest(ref ClamWinScanRequest request)
        {
            if (PipeHandle == (IntPtr)Win32API.INVALID_HANDLE_VALUE)
            {
                NeedToReconect = true;
                return false;
            }

            string text = request.GetRequestText();
            byte[] writen = new byte[4];
            int length = text.Length;
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(text);
            byte[] Buffer = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, unicodeBytes);

            const int PipeBufferSize = 1024;
            int pos = 0;
            bool Stop = false;
            while (!Stop)
            {
                byte[] helperBuffer;

                if (Buffer.Length - pos <= PipeBufferSize)
                {
                    helperBuffer = new byte[Buffer.Length - pos];
                    System.Array.Copy(Buffer, pos, helperBuffer, 0, helperBuffer.Length);
                    Stop = true;
                }
                else
                {
                    helperBuffer = new byte[PipeBufferSize];
                    System.Array.Copy(Buffer, pos, helperBuffer, 0, helperBuffer.Length);
                    pos += PipeBufferSize;
                }

                Overlapped.Offset = (IntPtr)0;
                Overlapped.OffsetHigh = (IntPtr)0;
                Overlapped.Internal = (IntPtr)0;
                Overlapped.InternalHigh = (IntPtr)0;
                
                Win32API.ResetEvent(Overlapped.hEvent);

                bool result = Win32API.WriteFile(PipeHandle, helperBuffer, helperBuffer.Length, writen, ref Overlapped);

                int lastError = Marshal.GetLastWin32Error();

                if (!result && lastError == Win32API.ERROR_IO_PENDING)
                {
                    while (true)
                    {
                        if (TerminateScanSignaled())
                        {
                            return false;
                        }

                        int res = Win32API.WaitForSingleObject(Overlapped.hEvent, 100);
                        if (res == Win32API.WAIT_OBJECT_0)
                        {
                            break;
                        }                        
                    }

                    if (Overlapped.InternalHigh.ToInt32() != helperBuffer.Length)
                    {
                        SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_FAILED, new NotifyData(ScanID));
                        return false;
                    }
                }
                else if (!result)
                {
                    SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_FAILED, new NotifyData(ScanID));
                    NeedToReconect = true;
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Get response from service
        /// </summary>
        /// <returns>true if success, false if failed</returns>
        public bool GetResponse(out ClamWinScanResponse response)
        {            
            if (PipeHandle == (IntPtr)Win32API.INVALID_HANDLE_VALUE)
            {
                response = new ClamWinScanResponse("");
                NeedToReconect = true;
                return false;
            }
            string text = "";            

            bool IsResponseValid = false;
            bool result = false;
            int lastError;

            while (true)
            {
                Overlapped.Offset = (IntPtr)0;
                Overlapped.OffsetHigh = (IntPtr)0;
                Overlapped.Internal = (IntPtr)0;
                Overlapped.InternalHigh = (IntPtr)0;

                Win32API.ResetEvent(Overlapped.hEvent);

                UInt32 read;
                result = Win32API.ReadFile(PipeHandle, Buffer, Buffer.Length - 1, out read, ref Overlapped);

                lastError = Marshal.GetLastWin32Error();

                if (!result && lastError == Win32API.ERROR_IO_PENDING)
                {
                    while (true)
                    {
                        if (TerminateScanSignaled())
                        {
                            response = new ClamWinScanResponse("");
                            return false;
                        }

                        int res = Win32API.WaitForSingleObject(Overlapped.hEvent, 100);
                        if (res == Win32API.WAIT_OBJECT_0)
                        {
                            break;
                        }

                        IntPtr bytes = new IntPtr();
                        bool res1 = Win32API.GetOverlappedResult(PipeHandle,
                                                                 ref Overlapped,
                                                                 ref bytes,
                                                                 false);                        
                    }
                }
                else if (!result)
                {
                    SendScanNotify(ClamWinScanner.UM_ITEMS_SCAN_FAILED, new NotifyData(ScanID));
                    response = new ClamWinScanResponse("");
                    NeedToReconect = true;
                    return false;
                }


                int helper = Overlapped.InternalHigh.ToInt32();// read[0] + read[1] * 256 + read[2] * 256 * 256 + read[3] * 256 * 256 * 256;
                if (helper == 0)
                {
                    break;
                }

                text += Encoding.UTF8.GetString(Buffer, 0, helper);

                if (text.IndexOf("</clamwinreply>") != -1)
                {
                    IsResponseValid = true;
                    break;
                }
            }

            response = new ClamWinScanResponse(text);

            if (!IsResponseValid)
            {
                return false;
            }

            return IsResponseValid;
        }
        /// <summary>
        /// Abort current job
        /// </summary>
        public void AbortCurrentJob()
        {
            ClamWinScanRequest request = new ClamWinScanRequest(ClamWinScanner.ActionID.AsyncAbortScan);
            
            lock (MainLock)
            {
                if (CurrentJobID == -1)
                {
                    return;
                }

                request.SetJobID(CurrentJobID);
            }

            ClamWinScanResponse response;

            lock (MainLock)
            {
                Unbreakable = true;
            }
            // Lock request/response pair
            lock (ClamWinScan.IOCommonLock)
            {
                if (!SendRequest(ref request))
                {
                    int error = Marshal.GetLastWin32Error();
                    return;
                }                

                if (!GetResponse(out response))
                {
                    int error = Marshal.GetLastWin32Error();
                    return;
                }
            }
            lock (MainLock)
            {
                Unbreakable = false;
            }
        }
        #endregion

        #region Enums
        enum ScanErrors : int { AlreadyRunning = -1, 
                                ItemsIsEmpty = -2,
                                ScanProcStillRunning = -3,
                                FailedToStartScanProc = -4,
                                ScanIDIsNotCorrect = -128
                              };
        #endregion

        #region Public Functions
        /// <summary>
        /// Connects to service pipe
        /// </summary>        
        public static void ConnectToPipe()
        {            
            lock (IOCommonLock)
            {
                if (!NeedToReconect)
                {
                    // already opened
                    return;
                }

                string name = "ClamWinClientOverlapped";
                OverlappedEvent = Win32API.CreateEvent(new IntPtr(), false, false, name);

                Overlapped.hEvent = OverlappedEvent;

                PipeHandle = Win32API.CreateFile(PipeName,
                                                 Win32API.GENERIC_READ | Win32API.GENERIC_WRITE,
                                                 0,
                                                 (IntPtr)0,
                                                 Win32API.OPEN_EXISTING,
                                                 Win32API.FILE_FLAG_OVERLAPPED,
                                                 0
                                                 );

                if (PipeHandle == (IntPtr)Win32API.INVALID_HANDLE_VALUE)
                {
                    System.Windows.Forms.MessageBox.Show("Failed to open scaning service handle!");
                    return;
                }

                NeedToReconect = false;
            }
        }
        /// <summary>
        /// Close pipe connection
        /// </summary>        
        private static void DisconnectPipe()
        {
            lock (IOCommonLock)
            {
                Win32API.CloseHandle(PipeHandle);

                PipeHandle = (IntPtr)Win32API.INVALID_HANDLE_VALUE;
            }
        }
        /// <summary>
        /// Start scan
        /// </summary>
        /// <param name="Items">Items for scan</param>
        /// <param name="Listeners">Listeners to be notified</param>       
        /// <returns>Scan ID on success , negative error code if failed</returns>
        public int StartScan(ref ClamWinScanner.ScannerItem[] Items, IntPtr[] Listeners)
        {
            lock (MainLock)
            {
                if (ScanID == ClamWinScanner.FakeScanID)
                {
                    return (int)ScanErrors.ScanIDIsNotCorrect;
                }
            }

            lock (MainLock)
            {
                if (Scanning)
                {
                    return (int)ScanErrors.AlreadyRunning;
                }
            }

            if (Items.Length < 1)
            {
                return (int)ScanErrors.ItemsIsEmpty;
            }

            if (ScanInvokeResult!=null)
            {
                if (!ScanInvokeResult.AsyncWaitHandle.WaitOne(new TimeSpan(10000), false))
                {
                    return (int)ScanErrors.ScanProcStillRunning;
                }
            }

            lock (MainLock)
            {
                Terminate = false;

                Suspend = false;

                this.Listeners = Listeners;
            }                       

            ScanInvokeResult = Scan.BeginInvoke(ref Items, null, null);

            if (ScanInvokeResult == null)
            {                
                return (int)ScanErrors.FailedToStartScanProc;
            }

            return 1;
        }
        /// <summary>
        /// Terminate scan
        /// </summary>
        public void TerminateScan()
        {
            lock (MainLock)
            {
                if (ScanID == ClamWinScanner.FakeScanID)
                {
                    // Fake scan is not allowed to call this function
                    return;
                }

                if (!Scanning)
                {
                    return;
                }

                Terminate = true;
            }

            /*if (ScanInvokeResult == null)
            {
                return;
            }

            if (ScanInvokeResult.AsyncWaitHandle.WaitOne(10000, false))
            {

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error waiting scan termination!",
                                                        "ClamWin",
                                                        System.Windows.Forms.MessageBoxButtons.OK,
                                                        System.Windows.Forms.MessageBoxIcon.Error
                                                     );
            }*/
        }
        /// <summary>
        /// Suspend scan
        /// </summary>
        public void SuspendScan()
        {
            lock (MainLock)
            {
                if (ScanID == ClamWinScanner.FakeScanID)
                {
                    // Fake scan is not allowed to call this function
                    return;
                }

                if (!Scanning)
                {
                    return;
                }

                Suspend = true;
            }
        }
        /// <summary>
        /// Resume scan
        /// </summary>
        public void ResumeScan()
        {
            lock (MainLock)
            {
                if (ScanID == ClamWinScanner.FakeScanID)
                {
                    // Fake scan is not allowed to call this function
                    return;
                }

                if (!Scanning)
                {
                    return;
                }

                Suspend = false;
            }
        }
        /// <summary>
        /// If scan is suspended
        /// </summary>
        /// <returns></returns>
        public bool IsSuspended()
        {
            lock (MainLock)
            {
                return Suspend;
            }
        }
        /// <summary>
        /// If scan is running
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            lock (MainLock)
            {
                return Scanning;
            }
        }
        /// <summary>
        /// ScanID accessor
        /// </summary>
        /// <returns>Scan ID</returns>
        public int GetScanID()
        {          
            return ScanID;            
        }
        /// <summary>
        /// Save statistics to database
        /// </summary>
        /// <param name="ScanID">Database scan id</param>
        public void FlushStatistics(int ScanID)
        {
            lock (MainLock)
            {
                if (ScanID == ClamWinScanner.FakeScanID)
                {
                    // Fake scan is not allowed to call this function
                    return;
                }

                string ID = ScanID.ToString();
                foreach (StatisticsData data in Statistics)
                {
                    // Specified record not found , try to insert new record
                    string command = "INSERT INTO Statistics(Object,Scanned,Infected,Errors,Deleted,MovedToQuarantine,ScanID) VALUES(";
                    string Object = data.Name.Replace("'", "''");
                    command += "'" + Object + "',";
                    command += "'" + data.Scanned.ToString() + "',";
                    command += "'" + data.Infected.ToString() + "',";
                    command += "'" + data.Errors.ToString() + "',";
                    command += "'" + data.Deleted.ToString() + "',";
                    command += "'" + data.MovedToQuarantine.ToString() + "',";                    
                    command += "'" + ID + "');";

                    int result;
                    ClamWinDatabase.ExecCommand(command, out result);                    
                }
            }                       
        }
        /// <summary>
        /// Returns database version
        /// </summary>
        /// <returns></returns>
        public string GetDBVersion()
        {
            string Version = "Unknown";

            lock(MainLock)
            {
                if (ScanID != ClamWinScanner.FakeScanID)
                {
                    // Common scans are not allowed to call this function
                    return Version;
                }
            }

            ConnectToPipe();
            

            ClamWinScanRequest request = new ClamWinScanRequest(ClamWinScanner.ActionID.GetInfo);
            
            ClamWinScanResponse response = null;
            // Lock request/response pair
            lock (ClamWinScan.IOCommonLock)
            {
                if (!SendRequest(ref request))
                {
                    int error = Marshal.GetLastWin32Error();
                    return Version;
                }                

                if (!GetResponse(out response))
                {
                    int error = Marshal.GetLastWin32Error();
                    return Version;
                }
            }        

            Version = response.GetDailyVersion()+":"+response.GetMainVersion();
            return Version;
        }
        /// <summary>
        /// Set OnAccess Scaner Status
        /// </summary>
        /// <param name="status"></param>
        public bool SetOnAccessScanerStatus(ClamWinSettings.OnAccessStatus status)
        {
            // TEST!!
            return true;

            lock (MainLock)
            {
                if (ScanID != ClamWinScanner.FakeScanID)
                {
                    // Common scans are not allowed to call this function
                    return false;
                }
            }

            ConnectToPipe();


            ClamWinScanRequest request = new ClamWinScanRequest(ClamWinScanner.ActionID.FsFilterControl);
            request.SetFsEnabled(status == ClamWinSettings.OnAccessStatus.Enabled);

            ClamWinScanResponse response = null;
            // Lock request/response pair
            lock (ClamWinScan.IOCommonLock)
            {
                if (!SendRequest(ref request))
                {
                    int error = Marshal.GetLastWin32Error();
                    return false;
                }

                if (!GetResponse(out response))
                {
                    int error = Marshal.GetLastWin32Error();
                    return false;
                }

                if (response.GetActionStatus() == ClamWinScanner.ResponseStatus.Error)
                {
                    return false;
                }

                return true;
            }
        }
        /// <summary>
        /// Register or unregister main window handle in service
        /// </summary>
        /// <param name="status"></param>
        public bool ManageGuiHandle(IntPtr handle, bool register)
        {          
            lock (MainLock)
            {
                if (ScanID != ClamWinScanner.FakeScanID)
                {
                    // Common scans are not allowed to call this function
                    return false;
                }
            }

            ConnectToPipe();


            ClamWinScanRequest request;

            if (register)
            {
                request = new ClamWinScanRequest(ClamWinScanner.ActionID.RegisterGui);
            }
            else
            {
                request = new ClamWinScanRequest(ClamWinScanner.ActionID.UnregisterGui);
            }

            request.SetValue(handle.ToInt32());

            ClamWinScanResponse response = null;
            // Lock request/response pair
            lock (ClamWinScan.IOCommonLock)
            {
                if (!SendRequest(ref request))
                {
                    int error = Marshal.GetLastWin32Error();
                    return false;
                }

                if (!GetResponse(out response))
                {
                    int error = Marshal.GetLastWin32Error();
                    return false;
                }

                if (response.GetActionStatus() == ClamWinScanner.ResponseStatus.Error)
                {
                    return false;
                }

                return true;
            }
        }
        /// <summary>
        /// Starts up worker thread to retrieve filter jobs
        /// </summary>
        /// <returns></returns>
        public int StartFilterListening(IntPtr[] Listeners)
        {
            lock (MainLock)
            {
                if (ScanID != ClamWinScanner.FakeScanID)
                {
                    return (int)ScanErrors.ScanIDIsNotCorrect;
                }                
            }

            if (FilterInvokeResult != null)
            {
                return (int)ScanErrors.AlreadyRunning;
            }

            lock (MainLock)
            {                
                this.Listeners = Listeners;
            }

            FilterInvokeResult = Filter.BeginInvoke(null, null);

            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int StopFilterListening()
        {
            lock (MainLock)
            {
                if (ScanID != ClamWinScanner.FakeScanID)
                {
                    return (int)ScanErrors.ScanIDIsNotCorrect;
                }
            }

            Win32API.SetEvent(ClamWinScanner.StopFilterListening);
            
            return 0;
        }       
        #endregion

        #region Delegates
        private delegate void ScanDelegate(ref ClamWinScanner.ScannerItem[] Items);
        private delegate void FilterWorker();
        #endregion

        #region
        #endregion

        #region
        #endregion
    }
    #endregion
}
