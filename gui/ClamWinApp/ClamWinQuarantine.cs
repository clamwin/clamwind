// Name:        ClamWinQuarantine.cs
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
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ClamWinApp
{
    #region ClamWinQuarantine class
    class ClamWinQuarantine
    {
        #region PrivateData
        /// <summary>
        /// Compress delegate instance
        /// </summary>
        private static QuarantineWorkerDelegate Compress = new QuarantineWorkerDelegate(QuarantineWorkerProc);
        /// <summary>
        /// Items which will be quarantined
        /// </summary>
        private static ArrayList ItemsQueue = new ArrayList();
        /// <summary>
        /// Main locker
        /// </summary>
        private static object MainLock = new object();
        /// <summary>
        /// Event signals whenever new quarantine item being added
        /// </summary>
        private static IntPtr NewItemAddedEvent = (IntPtr)Win32API.INVALID_HANDLE_VALUE;
        /// <summary>
        /// ClamWinQuarantine status
        /// </summary>
        private static bool Opened = false;
        /// <summary>
        /// Compressing thread result
        /// </summary>
        private static IAsyncResult CompressResult = null;
        /// <summary>
        /// Termination flag
        /// </summary>
        private static bool Terminate = false;
        /// <summary>
        /// If working thread active
        /// </summary>
        private static bool Working = false;
        /// <summary>
        /// If currently processing item should be skipped
        /// </summary>
        private static bool SkipCurrentItem = false;
        /// <summary>
        /// Quarantine folder path
        /// </summary>
        private static string QuarantinePath = "C:\\ClamWinQuarantine\\";
        /// <summary>
        /// File name currently processing
        /// </summary>
        private static long CurrentID = -1;
        /// <summary>
        /// Quarantine/unquarantine operations locked until current is done
        /// </summary>
        private static bool QueueLocked = false;
        /// <summary>
        /// Items counter
        /// </summary>
        private static long ItemsCounter = 0;
        #endregion

        #region Hidden Constructor
        private ClamWinQuarantine()
        { 
        
        }
        #endregion

        #region Enums
        /// <summary>
        /// Results of quarantine operation
        /// </summary>
        public enum QuarantineResults : int { Success = 0, 
                                              Failed, 
                                              FailedFileDoesNotExist, 
                                              FailedAlreadyQuarantined,
                                              FailedAlreadyQueued,
                                              FailedFileIsNotInQuarantine,
                                              FailedQueueLocked
                                          };
        #endregion

        #region Constants
        /// <summary>
        /// Compressing password
        /// </summary>
        private const string QuarantinePassword = "rBrZwEW@#fsvnf293kDFJ";
        private const string QuarantineExtension = "quarantine";
        public const int UM_QUARANTINE_ITEM_PROCESSING_START = Win32API.WM_USER + 201;
        public const int UM_QUARANTINE_ITEM_PROCESSING_COMPLETE = Win32API.WM_USER + 202;
        public const int UM_QUARANTINE_ITEM_PROCESSING_PROGRESS = Win32API.WM_USER + 203;
        public const int UM_QUARANTINE_QUEUE_NEW_ITEM = Win32API.WM_USER + 204;
        public const int UM_QUARANTINE_ITEM_PROCESSING_CANCELED = Win32API.WM_USER + 205;
        public const int UM_QUARANTINE_QUEUE_DONE = Win32API.WM_USER + 206;
        #endregion

        #region Public Functions
        /// <summary>
        /// Return corrsponding result description
        /// </summary>
        public static string QuarantineResultToString(QuarantineResults result)
        {
            switch (result)
            { 
                case QuarantineResults.Failed:
                    return "Failed.";
                case QuarantineResults.FailedAlreadyQuarantined:
                    return "File already quarantined.";
                case QuarantineResults.FailedAlreadyQueued:
                    return "File already queued.";
                case QuarantineResults.FailedFileDoesNotExist:
                    return "Specified file does not exist.";
                case QuarantineResults.FailedFileIsNotInQuarantine:
                    return "File is not in quarantined.";
                case QuarantineResults.FailedQueueLocked:
                    return "Current queue is locked. Please Wait until done.";
                case QuarantineResults.Success:
                    return "Success.";
                default:
                    return "ERROR: Not defined!";
            }        
        }
        /// <summary>
        /// Lock current queue until done
        /// </summary>
        /// <returns></returns>
        public static void LockQueue()
        {
            lock (MainLock)
            {
                QueueLocked = true;
            }
        }
        /// <summary>
        /// Check quarantined  file
        /// </summary>
        /// <param name="FilePathName"></param>
        /// <returns></returns>
        public static bool CheckQuarantinedFile(string QuarantinePathName,long SavedSize)
        {                        
            try
            {
                FileInfo fi = new FileInfo(QuarantinePathName);
                if (!fi.Exists)
                {
                    throw new SystemException();
                }

                return true;
            }
            catch
            {                
            }
            
            // Something is wrong, perform database and quarantined file clean up
           
            string command = "DELETE FROM QuarantineItems WHERE QuarantinePath='" + QuarantinePathName + "';";
            int result;
            ClamWinDatabase.ExecCommand(command, out result);

            try
            {
                FileInfo fi = new FileInfo(QuarantinePathName);
                
                if (fi.Exists)
                {
                    fi.Delete();
                }
            }
            catch
            { 
            }


            return false;
        }
        /// <summary>
        /// Quarantine specified file
        /// </summary>
        /// <param name="FileName">File to quarantine</param>
        /// <returns>Result</returns>
        public static QuarantineResults QuarantineFile(string FileName,IntPtr[] Listeners)
        {
            lock (MainLock)
            {
                if (QueueLocked)
                {
                    return QuarantineResults.FailedQueueLocked;
                }
            }

            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(FileName);
                if (!fi.Exists)
                {
                    return QuarantineResults.FailedFileDoesNotExist;
                }
            }
            catch
            {
                return QuarantineResults.Failed;
            }

            if (IsFileInQuarantine(FileName))
            {
                return QuarantineResults.FailedAlreadyQuarantined;    
            }

            long id = 0;
            lock (MainLock)
            {
                foreach (QuarantineItem queued in ItemsQueue)
                {
                    if (queued.FileName == FileName && queued.Quarantine == true)
                    {
                        return QuarantineResults.FailedAlreadyQueued;
                    }
                }

                id = ItemsCounter++;

                QuarantineItem item = new QuarantineItem();
                item.FileName = FileName;
                item.Listeners = Listeners;
                item.Quarantine = true;
                item.ID = id;
                ItemsQueue.Add(item);

                Win32API.SetEvent(NewItemAddedEvent);
            }

            NotifyData data = new NotifyData();
            data.FileName = FileName;
            data.Quarantine = true;
            data.Size = 0;
            data.ID = id;
            try
            {
                FileInfo fi = new FileInfo(FileName);
                data.Size = fi.Length;
            }
            catch
            { 
            
            }
            
            SendQuarantineNotify(UM_QUARANTINE_QUEUE_NEW_ITEM, data, Listeners);

            return QuarantineResults.Success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Listeners"></param>
        /// <returns></returns>
        public static QuarantineResults UnquarantineFile(string FileName, IntPtr[] Listeners, bool Temp)
        {
            lock (MainLock)
            {
                if (QueueLocked)
                {
                    return QuarantineResults.FailedQueueLocked;
                }
            }

            if (!IsFileInQuarantine(FileName))
            {
                return QuarantineResults.FailedFileIsNotInQuarantine;
            }

            long InitialSize = 0;
            string ComressedFileName = "";

            long id = 0;
            lock (MainLock)
            {
                foreach (QuarantineItem queued in ItemsQueue)
                {
                    if (queued.FileName == FileName && queued.Quarantine == false)
                    {
                        return QuarantineResults.FailedAlreadyQueued;
                    }
                }                

                string command = "SELECT * FROM QuarantineItems WHERE InitialPath='" + FileName + "';";
                ArrayList list;
                if (ClamWinDatabase.ExecReader(command, out list))
                {
                    if (list.Count != 0)
                    {
                        InitialSize = long.Parse((string)list[4]);
                        ComressedFileName = (string)list[2];
                    }
                    else
                    {
                        return QuarantineResults.Failed;
                    }
                }
                else
                {
                    return QuarantineResults.Failed;
                }

                id = ItemsCounter++;
                QuarantineItem item = new QuarantineItem();
                item.FileName = Temp ? GetQuarantineTempFolder() + Path.GetFileName(FileName) : FileName;
                item.Listeners = Listeners;
                item.Quarantine = false;
                item.InitialSize = InitialSize;
                item.CompressedFileName = ComressedFileName;
                item.Temp = Temp;
                item.ID = id;
            
                ItemsQueue.Add(item);

                Win32API.SetEvent(NewItemAddedEvent);
            }

            NotifyData data = new NotifyData();
            data.FileName = FileName;
            data.Quarantine = false;
            data.Size = InitialSize;
            data.ID = id;
            
            SendQuarantineNotify(UM_QUARANTINE_QUEUE_NEW_ITEM, data, Listeners);

            return QuarantineResults.Success;
        }
        /// <summary>
        /// Find out if file is quarantined
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static bool IsFileInQuarantine(string FileName)
        {
            string command = "SELECT * FROM QuarantineItems WHERE InitialPath='" + FileName + "';";
            ArrayList list;
            if (ClamWinDatabase.ExecReader(command, out list))
            {
                if (list.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


            string QuarantineName = (string)list[2];

            return FileExists(QuarantineName);

        }
        /// <summary>
        /// Initialize ClamWinQuarantine
        /// </summary>
        /// <returns></returns>
        public static bool Open(string ProgramFolder)
        {
            lock( MainLock )
            {
                if (Opened)
                {
                    // Already opened
                    return false;
                }

                NewItemAddedEvent = Win32API.CreateEvent((IntPtr)0 , false, false, "ClamWinQuarantineNewItemAddedEvent");

                if (NewItemAddedEvent == (IntPtr)Win32API.INVALID_HANDLE_VALUE)
                {
                    return false;
                }

                QuarantinePath = ProgramFolder + "\\Quarantine\\";
                
                try
                {
                    DirectoryInfo di = new DirectoryInfo(QuarantinePath);

                    if (!di.Exists)
                    {
                        di.Create();
                    }
                }
                catch
                { 
                
                }

                Terminate = false;

                SkipCurrentItem = false;

                Opened = true;
            }            

            CompressResult = Compress.BeginInvoke(null, null);

            return true;
        }
        /// <summary>
        /// Closes ClamWinQuarantine
        /// </summary>
        public static void Close()
        {            
            lock (MainLock)
            {
                if (!Opened)
                {
                    return;
                }

                Terminate = true;
            }

            if (!CompressResult.AsyncWaitHandle.WaitOne(10 * 1000, false))
            {
                throw new SystemException("ClmWinQuarantine: compressing thread termination waiting failed!");
            }

            lock (MainLock)
            {
                Win32API.CloseHandle(NewItemAddedEvent);
                NewItemAddedEvent = (IntPtr)Win32API.INVALID_HANDLE_VALUE;
                Opened = false;
            }            
        }
        /// <summary>
        /// If working thread is active
        /// </summary>
        /// <returns></returns>
        public static bool IsWorking()
        {
            lock (MainLock)
            {
                return Working;
            }
        }
        /// <summary>
        /// Removes specified file from quarantine queue,
        /// also terminate file processing if performed
        /// </summary>
        /// <param name="FileName"></param>
        public static void CancelItemProcessing(long id,IntPtr[] Listeners)
        {
            lock (MainLock)
            {
                if (QueueLocked)
                {
                    return;
                }

                foreach (QuarantineItem item in ItemsQueue)
                {
                    if (item.ID == id)
                    {
                        ItemsQueue.Remove(item);
                        NotifyData data = new NotifyData();
                        data.ID = id;
                        SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_CANCELED, data, Listeners);
                        return;
                    }
                }

                if (CurrentID == id)
                {
                    // File is processing right now
                    SkipCurrentItem = true;
                }
            }
        }
        /// <summary>
        /// QuarantinePath accessor
        /// </summary>
        /// <returns></returns>
        public static string GetQuarantineFolder()
        {
            return QuarantinePath;
        }
        /// <summary>
        /// Returns quarantine temporary folder
        /// </summary>
        /// <returns></returns>
        public static string GetQuarantineTempFolder()
        {
            return QuarantinePath + "\\Temp\\";            
        }
        #endregion

        #region Private Helper Functions        
        /// <summary>
        /// Compress specified file with given password applied
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        private static bool CompressEncrypted(string FileName,string CompressedFileName,string Password,IntPtr[] Listeners, long id)
        {                        
            const int MaxBufferSize = 1024 * 1024;// 1 mb max entry size
            Crc32 crc = new Crc32();

            ZipOutputStream ZipStream = null;
            FileStream CompressedFile = null;
            try
            {
                CompressedFile = File.Create(CompressedFileName);
                ZipStream = new ZipOutputStream(CompressedFile);

                if (ZipStream == null)
                {
                    throw new SystemException();
                }
            }
            catch
            {
                RemoveFile(CompressedFileName);
                return false;
            }

            ZipStream.SetLevel(0); // 0 - store only to 9 - means best compression
            
            ZipStream.Password = Password;

            FileStream fs = null;
            try
            {
                fs = File.OpenRead(FileName);
            }
            catch
            {
                ZipStream.Close();
                RemoveFile(CompressedFileName);
                return false;
            }

            byte[] Buffer = new byte[Math.Min(fs.Length,MaxBufferSize)];
                        
            ZipEntry Entry = new ZipEntry(Path.GetFileName(FileName));

            Entry.DateTime = DateTime.Now;

            Entry.Size = fs.Length;

            ZipStream.PutNextEntry(Entry);
            
            long Progress = 0;
            while (fs.Position < fs.Length)
            {
                int BytesRead = fs.Read(Buffer, 0, Buffer.Length);

                lock (MainLock)
                {
                    if (Terminate || SkipCurrentItem)
                    {
                        try
                        {
                            fs.Close();
                            CompressedFile.Close();
                            ZipStream.Close();                            
                        }
                        catch
                        {                         
                        }
                        RemoveFile(CompressedFileName);


                        if (SkipCurrentItem)
                        {
                            NotifyData data = new NotifyData();                            
                            data.ID = id;
                            SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_CANCELED, data, Listeners);

                            SkipCurrentItem = false;
                        }

                        return false;
                    }                    
                }

                ZipStream.Write(Buffer, 0, BytesRead); 
             
                Progress += BytesRead;

                NotifyData notify = new NotifyData();
                notify.FileName = FileName;
                notify.Progress = (int)((Progress / (double)fs.Length) * 100);

                SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_PROGRESS, notify, Listeners);
            }

            fs.Close();
            ZipStream.Finish();
            ZipStream.Close();
            ZipStream.Dispose();

            return true;
        }
        /// <summary>
        /// Exctract specified file 
        /// </summary>
        /// <param name="Compressed"></param>
        /// <returns></returns>
        private static bool Extract(string CompressedFileName, string FileName, long InitialSize, IntPtr[] Listeners, long id)
        {
            ZipInputStream ZipStream = new ZipInputStream(File.OpenRead(CompressedFileName));

            ZipStream.Password = QuarantinePassword;

            ZipEntry Entry = ZipStream.GetNextEntry();
            if (Entry == null)
            {
                ZipStream.Close();
                return false;
            }
            
            string DirectoryName = Path.GetDirectoryName(FileName);
                               
            Directory.CreateDirectory(DirectoryName);

            FileStream FileStream = File.Create(FileName);

            if (FileStream == null)
            {
                ZipStream.Close();
                return false;
            }

            int BytesRead = 0;
            byte[] buffer = new byte[1024*512];

            long Progress = 0;
            while (true)
            {
                BytesRead = ZipStream.Read(buffer, 0, buffer.Length);

                if (BytesRead > 0)
                {
                    FileStream.Write(buffer, 0, BytesRead);
                }
                else
                {
                    break;
                }

                lock (MainLock)
                {
                    if (Terminate || SkipCurrentItem)
                    {
                        try
                        {
                            FileStream.Close();
                            ZipStream.Close();                           
                        }
                        catch
                        {
                        }                        

                        if (SkipCurrentItem)
                        {
                            NotifyData data = new NotifyData();                            
                            data.ID = id;
                            SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_CANCELED, data, Listeners);

                            SkipCurrentItem = false;
                        }
                        return false;
                    }
                }

                Progress += BytesRead;

                NotifyData notify = new NotifyData();
                notify.FileName = FileName;
                notify.Progress = (int)((Progress / (double)InitialSize) * 100);

                SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_PROGRESS, notify, Listeners);
            }

            FileStream.Close();
            ZipStream.Close();

            return true;
        }
        /// <summary>
        /// Saves file info to database and removes file
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private static bool PostCompressProcessing(string FileName, string QuarantineName)
        {
            long Size = 0;
            try
            {
                FileInfo fi = new FileInfo(QuarantineName);
                if (!fi.Exists)
                {
                    return false;
                }

                fi = new FileInfo(FileName);

                if (!fi.Exists)
                {
                    return false;
                }

                Size = fi.Length;
            }
            catch
            {
                return false;
            }

            string command = "INSERT INTO QuarantineItems(InitialPath,QuarantinePath,QuarantineTime,Size) VALUES(";
            command += "'"+FileName+"',";
            command += "'"+QuarantineName+"',";
            command += "'" + DateTime.Now.ToBinary().ToString() + "',";
            command += "'" + Size.ToString() + "');";            
            
            int result;
            ClamWinDatabase.ExecCommand(command, out result);

            // Let we ensure record has been inserted successfully
            command = "SELECT * FROM QuarantineItems WHERE InitialPath='"+FileName+"';";
            ArrayList list;
            if (ClamWinDatabase.ExecReader(command, out list))
            {
                if( list.Count != 0 )
                {
                    RemoveFile(FileName);
                    return true;
                }
                else
                {
                    return false;
                }
            }            
            else
            {
                return false;
            }            
        }
        /// <summary>
        /// Check exctracted file size and removes database recod
        /// </summary>
        /// <returns></returns>
        private static bool PostExtractProcessing(string CompressedFileName, string FileName)
        {
            // Let we ensure record has been inserted successfully
            string command = "SELECT * FROM QuarantineItems WHERE InitialPath='" + FileName + "';";
            ArrayList list;
            if (ClamWinDatabase.ExecReader(command, out list))
            {
                if (list.Count != 0)
                {
                    long size = long.Parse((string)list[4]);
                    try
                    {
                        FileInfo fi = new FileInfo(FileName);
                        if (!fi.Exists)
                        {
                            return false;
                        }

                        if (fi.Length != size)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            command = "DELETE FROM QuarantineItems WHERE QuarantinePath='" + CompressedFileName + "';";
            int result;
            ClamWinDatabase.ExecCommand(command, out result);

            RemoveFile(CompressedFileName);

            return true;
        }
        /// <summary>
        /// Compress thread  procedure
        /// </summary>
        /// <param name="FileName">File to quarantine</param>
        /// <param name="Listeners">Listeners to be  notified about progress</param>
        private static void QuarantineWorkerProc()
        {
            while (true)
            {
                int Res = Win32API.WaitForSingleObject(NewItemAddedEvent, 250);

                lock (MainLock)
                {
                    if (Terminate)
                    {
                        break;
                    }
                }

                if (Res != Win32API.WAIT_OBJECT_0)
                {                    
                    continue;
                }

                while (true)
                {
                    QuarantineItem item;
                    lock (MainLock)
                    {
                        if (ItemsQueue.Count > 0)
                        {
                            item = (QuarantineItem)ItemsQueue[0];
                            ItemsQueue.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }

                        CurrentID = item.ID;
                        Working = true;
                    }

                    NotifyData notify = new NotifyData();
                    notify.FileName = item.FileName;
                    notify.Quarantine = item.Quarantine;
                    notify.ID = item.ID;

                    SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_START, notify, item.Listeners);

                    if (item.Quarantine)
                    {
                        // Perform compressing and quarantine file
                        string CompressedFileName = GetQuarantineFileName(item.FileName);

                        if (CompressEncrypted(item.FileName, CompressedFileName, QuarantinePassword, item.Listeners, item.ID))
                        {

                            if (PostCompressProcessing(item.FileName, CompressedFileName))
                            {
                                lock(MainLock)
                                {
                                    CurrentID = -1;
                                }
                                notify.Result = QuarantineResults.Success;
                                SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_COMPLETE, notify, item.Listeners);
                            }
                            else
                            {
                                lock(MainLock)
                                {
                                    CurrentID = -1;
                                }
                                notify.Result = QuarantineResults.Failed;
                                SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_COMPLETE, notify, item.Listeners);
                            }
                        }
                        else
                        {                            
                            notify.Result = QuarantineResults.Failed;
                            SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_COMPLETE, notify, item.Listeners);
                        }
                    }
                    else
                    { 
                        // Unquarantine file
                        if (Extract(item.CompressedFileName, item.FileName, item.InitialSize, item.Listeners, item.ID))
                        {
                            if (!item.Temp)
                            {
                                if (PostExtractProcessing(item.CompressedFileName, item.FileName))
                                {
                                    lock (MainLock)
                                    {
                                        CurrentID = -1;
                                    }
                                    notify.Result = QuarantineResults.Success;
                                    SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_COMPLETE, notify, item.Listeners);
                                }
                                else
                                {
                                    lock (MainLock)
                                    {
                                        CurrentID = -1;
                                    }
                                    notify.Result = QuarantineResults.Failed;
                                    SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_COMPLETE, notify, item.Listeners);
                                }
                            }
                            else
                            {
                                lock (MainLock)
                                {
                                    CurrentID = -1;
                                }
                                notify.Result = QuarantineResults.Failed;                                
                                SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_COMPLETE, notify, item.Listeners);
                            }
                        }
                        else
                        {
                            notify.Result = QuarantineResults.Failed;
                            SendQuarantineNotify(UM_QUARANTINE_ITEM_PROCESSING_COMPLETE, notify, item.Listeners);
                        }
                    }

                    lock (MainLock)
                    {
                        CurrentID = -1;

                        if (ItemsQueue.Count == 0)
                        {
                            QueueLocked = false;

                            notify = new NotifyData();                            
                            SendQuarantineNotify(UM_QUARANTINE_QUEUE_DONE, notify, item.Listeners);
                        }

                        if (Terminate)
                        {
                            Working = false;
                            break;
                        }
                    }
                }
                
                lock (MainLock)
                {                    
                    Working = false;
                }
            }
        }
        /// <summary>
        /// RemoveFile helper
        /// </summary>
        /// <param name="FileName"></param>
        private static void RemoveFile(string FileName)
        {           
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(FileName);
                fi.Delete();
            }
            catch
            {                
            }
        }
        /// <summary>
        /// FileExists helper
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private static bool FileExists(string FileName)
        {
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(FileName);
                return fi.Exists;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Send message to Listeners
        /// </summary>        
        private static void SendQuarantineNotify(int notification, NotifyData data, IntPtr[] Listeners)
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
        /// Find out first free quarantine file name
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private static string GetQuarantineFileName(string FileName)
        {            
            string QuarantineName = QuarantinePath + Path.GetFileName(FileName) + ".quarantine";
            if (!FileExists(QuarantineName))
            {
                return QuarantineName;
            }

            for (int i = 0; i < 1024 * 1024; i++)
            {
                QuarantineName = QuarantinePath + Path.GetFileName(FileName) + i.ToString() + ".quarantine";
                if (!FileExists(QuarantineName))
                {
                    return QuarantineName;
                }
            }
            throw new SystemException("No free file names available! :))");
        }
        #endregion

        #region Delegates
        /// <summary>
        /// QuarantineCompressProc Delegate
        /// </summary>
        /// <param name="FileName">File to quarantine</param>
        /// <param name="Listeners">Listeners to be  notified about progress</param>
        private delegate void QuarantineWorkerDelegate();
        #endregion

        #region QuarantineItem struct
        /// <summary>
        /// Quarantine Item
        /// </summary>
        private struct QuarantineItem
        {
            /// <summary>
            /// File name
            /// </summary>
            public string FileName;
            /// <summary>
            /// Compessed file to be extracted
            /// </summary>
            public string CompressedFileName;
            /// <summary>
            /// Initial (uncompressed) file size
            /// </summary>
            public long InitialSize;
            /// <summary>
            /// Listeners to be notified about this item compressing progress 
            /// </summary>
            public IntPtr[] Listeners;
            /// <summary>
            /// True - to quarantine item, false - to unquarantine item
            /// </summary>
            public bool Quarantine;
            /// <summary>
            /// If file being unquarantined temporaly
            /// </summary>
            public bool Temp;
            /// <summary>
            /// Item ID
            /// </summary>
            public long ID;
        }
        #endregion

        #region NotifyData struct
        /// <summary>
        /// NotifyData 
        /// </summary>
        public struct NotifyData
        {
            /// <summary>
            /// File name
            /// </summary>
            public string FileName;
            /// <summary>
            /// Result
            /// </summary>
            public QuarantineResults Result;
            /// <summary>
            /// Compressing progres(0..100%)
            /// </summary>
            public int Progress;
            /// <summary>
            /// True - to quarantine item, false - to unquarantine item
            /// </summary>
            public bool Quarantine;
            /// <summary>
            /// File size
            /// </summary>
            public long Size;
            /// <summary>
            /// Item ID
            /// </summary>
            public long ID;
        }
        #endregion
    }
    #endregion
}
