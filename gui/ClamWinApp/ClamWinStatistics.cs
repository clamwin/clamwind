// Name:        ClamWinStatistics.cs
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
using System.Collections;

namespace ClamWinApp
{
    #region ClamWinStatistics class
    class ClamWinStatistics
    {
        #region Private Data
        /// <summary>
        /// Locker for all statistics items
        /// </summary>
        private static object MainLock = new object();
        /// <summary>
        /// Statistic items
        /// </summary>
        private static ArrayList Items = new ArrayList();
        /// <summary>
        /// Current monitored object
        /// </summary>
        private static StatisticItem Current;
        /// <summary>
        /// Statistics state 
        /// </summary>
        private static bool Opened = false;
        /// <summary>
        /// String for total
        /// </summary>
        private const string TotalString = "Total";
        /// <summary>
        /// File Anti-Virus object
        /// </summary>
        private static StatisticItem FileAntiVirus = new StatisticItem("FileAntiVirus");
        #endregion
         
        #region Public Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ClamWinStatistics()
        {         
        }
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Clean-up statistics table
        /// </summary>
        /// <returns></returns>
        private static bool StatisticsTableCleanUp()
        {
            string command = "DELETE FROM Statistics;";

            int result;
            return ClamWinDatabase.ExecCommand(command, out result);
        }
        /// <summary>
        /// Load FileAntiVirus statistics from database
        /// </summary>
        /// <returns></returns>
        private static bool LoadFileAntiVirusStatistics()
        {
            lock (MainLock)
            {
                ArrayList list;
                string command = "SELECT * FROM Statistics WHERE ScanID='-1' AND Object='FileAntiVirus';";
                ClamWinDatabase.ExecReader(command, out list);

                if (list.Count == ClamWinDatabase.StatisticsFPR)
                {                   
                    // [Object]
                    FileAntiVirus.Object = (string)list[1];
                    // [Scanned]
                    FileAntiVirus.Scanned = ulong.Parse((string)list[2]);
                    // [Threats] 
                    FileAntiVirus.Threats = ulong.Parse((string)list[3]);
                    // [Deleted]
                    FileAntiVirus.Deleted = ulong.Parse((string)list[4]);
                    // [MovedToQuarantine]
                    FileAntiVirus.MovedToQuarantine = ulong.Parse((string)list[5]);
                    // [Archived] 
                    FileAntiVirus.Archived = ulong.Parse((string)list[6]);
                    // [Packed] 
                    FileAntiVirus.Packed = ulong.Parse((string)list[7]);
                    // [PasswordProtected] 
                    FileAntiVirus.PasswordProtected = ulong.Parse((string)list[8]);
                    // [Corrupted]
                    FileAntiVirus.Corrupted = ulong.Parse((string)list[9]);
                    // [LastScanned]
                    FileAntiVirus.LastScanned = (string)list[10];

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Initialize statistics and load saved items from database
        /// </summary>
        /// <returns>true - on success, false - if failed</returns>
        public static bool Open()
        {
            lock (MainLock)
            {
                if (Opened)
                {
                    return false;
                }
            }

            LoadFileAntiVirusStatistics();
            
            lock(MainLock)
            {
                Opened = true;
                return Opened;           
            }
        }
        /// <summary>
        /// Close statistics, save all items to database
        /// </summary>
        public static void Close()
        {
            lock (MainLock)
            {
                if (!Opened)
                {
                    return;
                }                
            }           

            lock(MainLock)
            {
                Opened = false;            
            }
        }
        /// <summary>
        /// Cleanup current staitsitcs and delete records from "Statistics" table
        /// </summary>
        /*public static void Reset()
        {
            lock (MainLock)
            {
                Items.Clear();
                StatisticItem item = new StatisticItem(TotalString);
                Items.Insert(0, item);
            }

            StatisticsTableCleanUp();
        }*/
        /// <summary>
        /// Save items to data base, and clear Items
        /// </summary>
        /// <returns>true - on success, false - if failed</returns>
        public static bool FlushItems(int ScanID)
        {
            lock (MainLock)
            {
                if (!Opened)
                {
                    return false;
                }

                string ID = ScanID.ToString();
                foreach (StatisticItem item in Items)
                {                                                         
                    // Specified record not found , try to insert new record
                    string command = "INSERT INTO Statistics(Object,Scanned,Threats,Deleted,MovedToQuarantine,Archived,Packed,PasswordProtected,Corrupted,LastScanned,ScanID) VALUES(";
                    string Object = item.Object.Replace("'","''");
                    command += "'" + Object + "',";
                    command += "'" + item.Scanned.ToString() + "',";
                    command += "'" + item.Threats.ToString() + "',";
                    command += "'" + item.Deleted.ToString() + "',";
                    command += "'" + item.MovedToQuarantine.ToString() + "',";
                    command += "'" + item.Archived.ToString() + "',";
                    command += "'" + item.Packed.ToString() + "',";
                    command += "'" + item.PasswordProtected.ToString() + "',";
                    command += "'" + item.Corrupted.ToString() + "',";
                    command += "'" + item.LastScanned + "',";
                    command += "'" + ID + "');";                    

                    int result;
                    if (!ClamWinDatabase.ExecCommand(command, out result))
                    {
                        return false;
                    }
                }
            }

            Items.Clear();            

            return true;            
        }       
        /// <summary>
        /// Save FileAntiVirus statistics to data base
        /// </summary>
        /// <returns>true - on success, false - if failed</returns>
        public static bool FlushFileAntiVirus()
        {
            lock (MainLock)
            {
                //string command = "INSERT INTO Statistics(Object,Scanned,Threats,Deleted,MovedToQuarantine,Archived,Packed,PasswordProtected,Corrupted,LastScanned,ScanID) VALUES(";
                string command = "UPDATE Statistics SET Scanned = '" + FileAntiVirus.Scanned.ToString()+"', ";
                string LastScanned = FileAntiVirus.LastScanned.Replace("'","''");
                command += "Threats = '" + FileAntiVirus.Threats.ToString() + "', ";
                command += "Deleted = '" + FileAntiVirus.Deleted.ToString() + "', ";
                command += "MovedToQuarantine = '" + FileAntiVirus.MovedToQuarantine.ToString() + "', ";
                command += "Archived = '" + FileAntiVirus.Archived.ToString() + "', ";
                command += "Packed = '" + FileAntiVirus.Packed.ToString() + "', ";
                command += "PasswordProtected = '" + FileAntiVirus.PasswordProtected.ToString() + "', ";
                command += "Corrupted = '" + FileAntiVirus.Corrupted.ToString() + "', ";
                command += "LastScanned = '" + LastScanned + "', ";
                command += "ScanID = '-1'";
                command += " WHERE Object = '" + FileAntiVirus.Object + "';";

                int result;
                ClamWinDatabase.ExecCommand(command, out result);

                if (result == 0)
                { 
                    //UPDATE failed
                    command = "INSERT INTO Statistics(Object,Scanned,Threats,Deleted,MovedToQuarantine,Archived,Packed,PasswordProtected,Corrupted,LastScanned,ScanID) VALUES(";
                    LastScanned = FileAntiVirus.LastScanned.Replace("'", "''");
                    command += "'" + FileAntiVirus.Object + "',";
                    command += "'" + FileAntiVirus.Scanned.ToString() + "',";
                    command += "'" + FileAntiVirus.Threats.ToString() + "',";
                    command += "'" + FileAntiVirus.Deleted.ToString() + "',";
                    command += "'" + FileAntiVirus.MovedToQuarantine.ToString() + "',";
                    command += "'" + FileAntiVirus.Archived.ToString() + "',";
                    command += "'" + FileAntiVirus.Packed.ToString() + "',";
                    command += "'" + FileAntiVirus.PasswordProtected.ToString() + "',";
                    command += "'" + FileAntiVirus.Corrupted.ToString() + "',";
                    command += "'" + LastScanned + "',";
                    command += "'-1');";
                   
                    if (!ClamWinDatabase.ExecCommand(command, out result))
                    {
                        return false;
                    }                
                }

                return true;
            }
        }
        /// <summary>
        /// Add new object to items and make it current,
        /// also flush items to make sure new object is saved
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static bool StartNewObject(string Obj, ClamWinMainForm.ScanPanelIDs ScanID)
        {
            lock (MainLock)
            {
                if (!Opened)
                {
                    return false;
                }

                if (Items.Count == 0)
                {
                    // Add Total object
                    StatisticItem total = new StatisticItem(TotalString);
                    Items.Insert(0, total);                                        
                }

                if (Obj == TotalString)
                {
                    // Special handling for total
                    // Total is not for direct selection 
                    return false;
                }

                foreach (StatisticItem item in Items)
                {
                    if (item.Object == Obj)
                    {
                        Current = item;
                        return true;
                    }
                }
                Items.Add(new StatisticItem(Obj));
                Current = (StatisticItem)Items[Items.Count - 1];
            }
            return true;
        }
        /// <summary>
        /// Look for specified object and make it current if found
        /// </summary>
        /// <param name="Obj">Object to select</param>
        /// <returns>true - on success, false - if failed</returns>
        public static bool SelectObject(string Obj)
        {
            lock (MainLock)
            {
                if (!Opened)
                {
                    return false;
                }

                if (Obj == TotalString)
                {
                    // Special handling for total
                    // Total is not for direct selection 
                    return false;
                }

                foreach (StatisticItem item in Items)
                {
                    if (item.Object == Obj)
                    {
                        Current = item;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Fill objects array with current objects
        /// </summary>
        /// <param name="objects"></param>
        public static bool GetAvailableObjects(out string[] objects)
        {
            lock (MainLock)
            { 
                if( Items.Count < 2 )
                {
                    objects = new string[0];
                    return false;
                }

                int OutSize = Items.Count;
                int Start = 0;
                if( Items.Count == 2 )
                {
                    OutSize--;
                    Start = 1;
                }

                objects = new string[OutSize];
                                
                for (int i = Start,j = 0; i < Items.Count; i++,j++)
                { 
                    StatisticItem item = (StatisticItem)Items[i];
                    objects[j] = item.Object;
                }
                
                return true;
            }        
        }

        #region Current Object Accessors and Manipulators
        /// <summary>
        /// Scanned increment
        /// </summary>
        public static void ScannedInc()
        {
            lock (MainLock)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    Total.Scanned++;
                    Current.Scanned++;                    
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Threats increment
        /// </summary>
        public static void ThreatsInc()
        {
            lock (MainLock)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    Total.Threats++;
                    Current.Threats++;                   
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Deleted increment
        /// </summary>
        public static void DeletedInc()
        {
            lock (MainLock)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    Total.Deleted++;
                    Current.Deleted++;                    
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// MovedToQuarantine increment
        /// </summary>
        public static void MovedToQuarantineInc()
        {
            lock (MainLock)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    Total.MovedToQuarantine++;
                    Current.MovedToQuarantine++;                    
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Archived increment
        /// </summary>
        public static void ArchivedInc()
        {
            lock (MainLock)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    Total.Archived++;
                    Current.Archived++;                    
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Packed increment
        /// </summary>
        public static void PackedInc()
        {
            lock (MainLock)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    Total.Packed++;
                    Current.Packed++;                  
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// PasswordProtected increment
        /// </summary>
        public static void PasswordProtectedInc()
        {
            lock (MainLock)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    Total.PasswordProtected++;
                    Current.PasswordProtected++;                   
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Corrupted increment
        /// </summary>
        public static void CorruptedInc()
        {
            lock (MainLock)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    Total.Corrupted++;
                    Current.Corrupted++;                   
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Object accessor
        /// </summary>
        /// <returns></returns>
        public static string GetObject()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.Object;
                }
                catch
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// Scanned accessor
        /// </summary>
        public static ulong GetScanned()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.Scanned;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Threats accessor
        /// </summary>
        public static ulong GetThreats()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.Threats;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Deleted accessor
        /// </summary>
        public static ulong GetDeleted()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.Deleted;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// MovedToQuarantine accessor
        /// </summary>
        public static ulong GetMovedToQuarantine()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.MovedToQuarantine;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Archived accessor
        /// </summary>
        public static ulong GetArchived()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.Archived;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Packed accessor
        /// </summary>
        public static ulong GetPacked()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.Packed;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// PasswordProtected accessor
        /// </summary>
        public static ulong GetPasswordProtected()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.PasswordProtected;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Corrupted accessor
        /// </summary>
        public static ulong GetCorrupted()
        {
            lock (MainLock)
            {
                try
                {
                    return Current.Corrupted;
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion

        #region Total Object Accessors
        /// <summary>
        /// Total.Scanned accessor
        /// </summary>
        public static ulong GetTotalScanned()
        {
            lock (MainLock)
            {
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    return Total.Scanned;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Total.Threats accessor
        /// </summary>
        public static ulong GetTotalThreats()
        {
            lock (MainLock)
            {
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    return Total.Threats;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Total.Deleted accessor
        /// </summary>
        public static ulong GetTotalDeleted()
        {
            lock (MainLock)
            {
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    return Total.Deleted;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Total.MovedToQuarantine accessor
        /// </summary>
        public static ulong GetTotalMovedToQuarantine()
        {
            lock (MainLock)
            {
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    return Total.MovedToQuarantine;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Total.Archived accessor
        /// </summary>
        public static ulong GetTotalArchived()
        {
            lock (MainLock)
            {
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    return Total.Archived;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Total.Packed accessor
        /// </summary>
        public static ulong GetTotalPacked()
        {
            lock (MainLock)
            {
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    return Total.Packed;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Total.PasswordProtected accessor
        /// </summary>
        public static ulong GetTotalPasswordProtected()
        {
            lock (MainLock)
            {
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    return Total.PasswordProtected;
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Total.Corrupted accessor
        /// </summary>
        public static ulong GetTotalCorrupted()
        {
            lock (MainLock)
            {
                try
                {
                    StatisticItem Total = (StatisticItem)Items[0];
                    return Total.Corrupted;
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion

        #region FileAntiVirus Object Accessors and Manipulators
        /// <summary>
        /// FileAntiVirus.Scanned increment
        /// </summary>
        public static void FileAntiVirusScannedInc()
        {
            lock (MainLock)
            {
                FileAntiVirus.Scanned++;
            }
        }
        /// <summary>
        /// FileAntiVirus.Threats increment
        /// </summary>
        public static void FileAntiVirusThreatsInc()
        {
            lock (MainLock)
            {
                FileAntiVirus.Threats++;
            }
        }
        /// <summary>
        /// Set FileAntiVirus.LastScanned
        /// </summary>
        /// <param name="item"></param>
        public static void FileAntiVirusSetLastScanned(string item)
        {
            lock (MainLock)
            {
                FileAntiVirus.LastScanned = item;
            }
        }
        /// <summary>
        /// FileAntiVirus.Scanned accessor
        /// </summary>
        public static ulong GetFileAntiVirusScanned()
        {
            lock (MainLock)
            {                                 
                return FileAntiVirus.Scanned;                
            }
        }
        /// <summary>
        /// FileAntiVirus.Threats accessor
        /// </summary>
        public static ulong GetFileAntiVirusThreats()
        {
            lock (MainLock)
            {
                return FileAntiVirus.Threats;               
            }
        }
        /// <summary>
        /// FileAntiVirus.Deleted accessor
        /// </summary>
        public static ulong GetFileAntiVirusDeleted()
        {
            lock (MainLock)
            {
                return FileAntiVirus.Deleted;               
            }
        }
        /// <summary>
        /// FileAntiVirus.MovedToQuarantine accessor
        /// </summary>
        public static ulong GetFileAntiVirusMovedToQuarantine()
        {
            lock (MainLock)
            {
                return FileAntiVirus.MovedToQuarantine;               
            }
        }
        /// <summary>
        /// FileAntiVirus.Archived accessor
        /// </summary>
        public static ulong GetFileAntiVirusArchived()
        {
            lock (MainLock)
            {
                return FileAntiVirus.Archived;               
            }
        }
        /// <summary>
        /// FileAntiVirus.Packed accessor
        /// </summary>
        public static ulong GetFileAntiVirusPacked()
        {
            lock (MainLock)
            {
                return FileAntiVirus.Packed;                
            }
        }
        /// <summary>
        /// FileAntiVirus.PasswordProtected accessor
        /// </summary>
        public static ulong GetFileAntiVirusProtected()
        {
            lock (MainLock)
            {
                return FileAntiVirus.PasswordProtected;               
            }
        }
        /// <summary>
        /// FileAntiVirus.Corrupted accessor
        /// </summary>
        public static ulong GetFileAntiVirusCorrupted()
        {
            lock (MainLock)
            {
               return FileAntiVirus.Corrupted;               
            }
        }
        /// <summary>
        /// FileAntiVirus.LastScanned accessor
        /// </summary>
        public static string GetFileAntiVirusLastScanned()
        {
            lock (MainLock)
            {
                return FileAntiVirus.LastScanned;
            }
        }
        #endregion

        #endregion

        #region StatisticItem class
        /// <summary>
        /// Basic statistics item, 
        /// keep information like: total files scanned,
        /// threats detected, etc.
        /// </summary>
        private class StatisticItem
        {
            #region Public Constructor
            /// <summary>
            /// Default public constructor
            /// </summary>
            public StatisticItem(string Obj)
            {
                Object = Obj;
                Scanned = 0;                
                Threats = 0;
                Deleted = 0;
                MovedToQuarantine = 0;
                Archived = 0;
                Packed = 0;
                PasswordProtected = 0;
                Corrupted = 0;
                LastScanned = "none";
            }
            #endregion

            #region Public Data
            /// <summary>
            /// Object name (fe:""Total", "C:\")
            /// </summary>
            public string Object;
            /// <summary>
            /// Scanned items (files) amount
            /// </summary>
            public ulong Scanned;
            /// <summary>
            /// Number of threats detected
            /// </summary>
            public ulong Threats;
            /// <summary>
            /// Number of files deleted
            /// </summary>
            public ulong Deleted;
            /// <summary>
            /// Number of files moved to quarantine
            /// </summary>
            public ulong MovedToQuarantine;
            /// <summary>
            /// Numbers of archived files
            /// </summary>
            public ulong Archived;
            /// <summary>
            /// Number of packed files
            /// </summary>
            public ulong Packed;
            /// <summary>
            /// Number of pasword protected files
            /// </summary>
            public ulong PasswordProtected;
            /// <summary>
            /// Number of corrupted files
            /// </summary>
            public ulong Corrupted;
            /// <summary>
            /// Last scanned file (used for FA statistics)
            /// </summary>
            public string LastScanned;
            #endregion           
        }
        #endregion
    }
    #endregion
}
