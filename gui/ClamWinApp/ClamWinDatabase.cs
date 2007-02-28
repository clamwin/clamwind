//-----------------------------------------------------------------------------
// Name:        ClamwinDatabase.cs
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
using System.Data.SQLite;
using System.Windows.Forms;
using System.Collections;

namespace ClamWinApp
{
    #region ClamWinDatabase class
    /// <summary>
    /// Database manager for ClamWin
    /// </summary>
    class ClamWinDatabase
    {
        #region Private Data
        /// <summary>
        /// Main SQLite connection
        /// </summary>
        private static SQLiteConnection SQLiteConnection = new SQLiteConnection();
        /// <summary>
        /// Main SQLite command
        /// </summary>
        private static SQLiteCommand SQLiteCommand;
        /// <summary>
        /// Main database lock
        /// </summary>
        private static object MainLock = new object();
        /// <summary>
        /// Database state flag
        /// </summary>
        private static bool Opened = false;
        /// <summary>
        /// ClamWin folder
        /// </summary>
        private static string ClamWinPath;
        /// <summary>
        /// Main form handle
        /// </summary>
        private static IntPtr MainFormHandle = (IntPtr)0;
        #endregion

        #region Public Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ClamWinDatabase()
        {            
        }
        #endregion

        #region Constants
        /// <summary>
        /// Fields per record in ScanItems table
        /// </summary>
        public const int ScanItemsFPR = 8;
        /// <summary>
        /// Fields per record in Events table
        /// </summary>
        public const int EventsFPR = 6;
        /// <summary>
        /// Fields per record in Scans table
        /// </summary>
        public const int ScansFPR = 5;
        /// <summary>
        /// Fields per record in Statistics table
        /// </summary>
        public const int StatisticsFPR = 8;
        /// <summary>
        /// Fields per record in Settings table
        /// </summary>
        public const int SettingsFPR = 3;
        /// <summary>
        /// Fields per record in QuarantineItems table
        /// </summary>
        public const int QuarantineItemsFPR = 5;
        /// <summary>
        /// Fields per record in MainFormNotifications table
        /// </summary>
        public const int MainFormNotificationsFPR = 5;
        /// <summary>
        /// Fields per record in FilterNotifications table
        /// </summary>
        public const int FilterNotificationsFPR = 5;
        /// <summary>
        /// Database changed
        /// </summary>
        public const int UM_DATABASE_CHANGED = Win32API.WM_USER + 777;
        #endregion

        #region Public Methods
        /// <summary>
        /// Open or create main ClamWin database
        /// , create tables also.
        /// </summary>
        /// <param name="path">Path to main clamwin folder</param>
        public static void Open(string path, IntPtr handle)
        {
            lock (MainLock)
            {
                if (Opened)
                {
                    return;
                }

                ClamWinPath = path;

                SQLiteConnection.ConnectionString = "Data Source=";
                SQLiteConnection.ConnectionString += path;
                SQLiteConnection.ConnectionString += "\\Data\\clamwin.db;Version=3;Compress=True;";
                try
                {
                    string source = path + "\\Data\\clamwin.db";
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(source);
                        if (!fi.Exists)
                        {
                            throw new SystemException();
                        }
                    }
                    catch(SystemException)
                    {
                        throw new SQLiteException();
                    }
                    SQLiteConnection.Open();

                    SQLiteCommand = SQLiteConnection.CreateCommand();

                    SQLiteCommand.CommandText = "PRAGMA synchronous = OFF;";
                    SQLiteCommand.ExecuteNonQuery();

                    Opened = true;

                }
                catch (SQLiteException)
                {
                    SQLiteConnection.Close();
                }

                if (!Opened)
                {
                    // it seems there is no existing database,
                    // proceed with creating new one
                    SQLiteConnection.ConnectionString += "New=True;";
                    try
                    {
                        SQLiteConnection.Open();

                        SQLiteCommand = SQLiteConnection.CreateCommand();

                        // Creating tables                    

                        // "Settings" Table: [id] [Name] [Value]
                        SQLiteCommand.CommandText = "CREATE TABLE Settings ( id integer primary key, name char(50), value char(50));";
                        SQLiteCommand.ExecuteNonQuery();

                        // "Events" Table: [id] [Name] [Status] [Reason] [Time] [ScanID]
                        SQLiteCommand.CommandText = "CREATE TABLE Events ( id integer primary key, Name char(384), Status char(20), Reason char(20), Time char(20), ScanID int);";
                        SQLiteCommand.ExecuteNonQuery();

                        // "Statistics" Table: [id] [Object] [Scanned] [Infected] [Errors] [Deleted] [MovedToQuarantine] [ScanID]
                        SQLiteCommand.CommandText = "CREATE TABLE Statistics ( id integer primary key, Object char(384), " +
                                                    "Scanned int unsigned, Infected int unsigned, Errors int unsigned, Deleted int unsigned, " +
                                                    "MovedToQuarantine int unsigned, ScanID int);";
                        SQLiteCommand.ExecuteNonQuery();

                        // "Scans" Table: [id] [ScanStart] [ScanEnd] [Status] [ScanPanelID]
                        SQLiteCommand.CommandText = "CREATE TABLE Scans ( id integer primary key, ScanStart datatime, ScanEnd datatime," +
                                                    "Status varchar(10), ScanPanelID int);";
                        SQLiteCommand.ExecuteNonQuery();

                        // "ScanItems" Table: [id] [Item] [Path] [IsChecked] [IsSystem] [IsFolder] [IsDrive] [ListID]
                        SQLiteCommand.CommandText = "CREATE TABLE ScanItems ( id integer primary key, Item char(384), Path char(384)," +
                                                    "IsChecked bit, IsSystem bit, IsFolder bit, IsDrive bit, ListID char[20]);";
                        SQLiteCommand.ExecuteNonQuery();

                        // "QuarantineItems" Table: [id] [InitialPath] [QuarantinePath] [QuarantineTime] [Size]
                        SQLiteCommand.CommandText = "CREATE TABLE QuarantineItems ( id integer primary key, InitialPath char(384), QuarantinePath char(384), QuarantineTime datatime, Size bigint);";
                        SQLiteCommand.ExecuteNonQuery();

                        // "MainFormNotifications" Table: [id] [Message] [Code] [Type] [Time]
                        SQLiteCommand.CommandText = "CREATE TABLE MainFormNotifications ( id integer primary key, Message char(384), Code integer, Type integer, Time integer);";
                        SQLiteCommand.ExecuteNonQuery();

                        // "FilterNotifications" [id] [Path] [Message] [Status] [Time]
                        SQLiteCommand.CommandText = "CREATE TABLE FilterNotifications ( id integer primary key, Path char(384), Message char(384), Status char(16), Time integer);";
                        SQLiteCommand.ExecuteNonQuery();

                        SQLiteCommand.CommandText = "PRAGMA synchronous = OFF;";
                        SQLiteCommand.ExecuteNonQuery();

                        Opened = true;
                    }
                    catch (SQLiteException ex)
                    {
                        SQLiteConnection.Close();
                        MessageBox.Show("Database intialization failed with message:" + ex.Message);
                    }
                }

                MainFormHandle = handle;
            }
        }                
        /// <summary>
        /// Close database
        /// </summary>
        public static void Close()
        {
            try
            {
                lock (MainLock)
                {
                    SQLiteConnection.Close();
                }
            }
            catch
            { 
            }

            lock (MainLock)
            {                
                Opened =  false;
            }
        }       
        /// <summary>
        /// Execute specified command, used for performing action if response is not expected
        /// </summary>
        /// <param name="text">Command text</param>
        /// <returns>true - on success, false - if failed</returns>
        public static bool ExecCommand(string text,out int result)
        {
            result = 0;
            
            lock (MainLock)
            {
                if (!Opened)
                {
                    return false;
                }

            }

            try
            {
                lock (MainLock)
                {
                    SQLiteCommand command = SQLiteConnection.CreateCommand();
                    command.CommandText = text;
                    result = command.ExecuteNonQuery();

                    Win32API.PostMessage(MainFormHandle, UM_DATABASE_CHANGED, 0, 0);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Failed to ExecComand with message:" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Execute specified command, used if some kind of reponse is expected
        /// </summary>
        /// <param name="text">Command text</param>
        /// <param name="items">Command response</param>
        /// <returns>true - on success, flase - if failed</returns>
        public static bool ExecReader(string text, out ArrayList items)
        {
            items = new ArrayList();

            lock (MainLock)
            {
                if (!Opened)
                {
                    return false;
                }
            }

            try
            {
                lock (MainLock)
                {
                    SQLiteCommand command = SQLiteConnection.CreateCommand();
                    command.CommandText = text;

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object obj = reader.GetValue(i);
                            string value = obj.ToString();
                            items.Add(value);
                        }
                    }
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Failed to ExecComand with message:" + ex.Message);
                return false;
            }            
        }
        #endregion
    }
    #endregion
}
