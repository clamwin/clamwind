// Name:        ClamWinSettings.cs
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
using Microsoft.Win32;

namespace ClamWinApp
{
    #region ClamWinSettings class
    public class ClamWinSettings
    {
        #region Private Data
        /// <summary>
        /// ClamWin application full path
        /// </summary>
        private static string ClamWinExePath = "";
        /// <summary>
        /// Windows Handles which will recieve notifications
        /// </summary>
        private static ArrayList Listeners = new ArrayList();
        /// <summary>
        /// Main locker
        /// </summary>
        private static object MainLock = new object();
        /// <summary>
        /// Settings state flag
        /// </summary>
        private static bool Opened = false;

        #region Properties Variables
        /// <summary>
        /// Variable for ShowNonCriticalEvents property
        /// </summary>
        private static bool varShowNonCriticalEvents = true;
        /// <summary>
        /// Variable for EnableProtection property
        /// </summary>
        private static bool varEnableProtection =  true;
        /// <summary>
        /// Variable for RunAtStartup property
        /// </summary>
        private static bool varRunAtStartup = true;
        /// <summary>
        /// Variable for EnableFileAntiVirus property
        /// </summary>
        private static OnAccessStatus varOnAccessScannerStatus = OnAccessStatus.Disabled;
        /// <summary>
        /// Variable for FileAntiVirusOnDetectAction property
        /// </summary>
        private static OnDetectActions varFileAntiVirusOnDetectAction = OnDetectActions.Prompt;
        /// <summary>
        /// Variable for EnableMailAntiVirus property
        /// </summary>
        private static bool varEnableMailAntiVirus = true;
        /// <summary>
        /// Variable for ScanOnDetectAction property
        /// </summary>
        private static OnDetectActions varScanOnDetectAction = OnDetectActions.Prompt;
        /// <summary>
        /// Variable for ScanMyPCOnDetectAction property
        /// </summary>
        private static OnDetectActions varScanMyPCOnDetectAction = OnDetectActions.Prompt;
        /// <summary>
        /// Variable for ScanCriticalOnDetectAction property
        /// </summary>
        private static OnDetectActions varScanCriticalOnDetectAction = OnDetectActions.Prompt;
        /// <summary>
        /// Variable for EnableNotifications property
        /// </summary>
        private static bool varEnableNotifications = true;
        /// <summary>
        /// Variable for UpdateRunMode property
        /// </summary>
        private static RunModes varUpdateRunMode = RunModes.Auto;
        /// <summary>
        /// Variable for RescanQuarantineAfterUpdate property
        /// </summary>
        private static bool varRescanQuarantineAfterUpdate = true;
        /// <summary>
        /// Variable for LogNonCriticalEvents property
        /// </summary>
        private static bool varLogNonCriticalEvents = true;
        /// <summary>
        /// Variable for KeepOnlyRecentEvents property
        /// </summary>
        private static bool varKeepOnlyRecentEvents = true;
        /// <summary>
        /// Variable for DeleteReportsAfterTime property
        /// </summary>
        private static bool varDeleteReportsAfterTime = true;
        /// <summary>
        /// Variable for ReportsLifeTime property
        /// </summary>
        private static int varReportsLifeTime = 30;
        /// <summary>
        /// Variable for DeleteQuarantineItemsAfterTime property
        /// </summary>
        private static bool varDeleteQuarantineItemsAfterTime = true;
        /// <summary>
        /// Variable for QuarantineItemsLifeTime property
        /// </summary>
        private static int varQuarantineItemsLifeTime = 30;
        /// <summary>
        /// Variable for UpdateScheduleData property
        /// </summary>
        private static ClamWinScheduleData varUpdateScheduleData = new ClamWinScheduleData(SettingIDToString(SettingIDs.UpdateScheduleData), ClamWinScheduleData.UpdateArg);
        /// <summary>
        /// Variable for ScanCriticalScheduleData property
        /// </summary>
        private static ClamWinScheduleData varScanCriticalScheduleData = new ClamWinScheduleData(SettingIDToString(SettingIDs.ScanCriticalScheduleData), ClamWinScheduleData.ScanCriticalArg);
        /// <summary>
        /// Variable for ScanCriticalSchedule property
        /// </summary>
        private static bool varScanCriticalSchedule = false;
        /// <summary>
        /// Variable for ScanMyPCScheduleData property
        /// </summary>
        private static ClamWinScheduleData varScanMyPCScheduleData = new ClamWinScheduleData(SettingIDToString(SettingIDs.ScanMyPCScheduleData), ClamWinScheduleData.ScanMyPCArg);
        /// <summary>
        /// Variable for ScanMyPCSchedule property
        /// </summary>
        private static bool varScanMyPCSchedule = false;
        /// <summary>
        /// Variable for ScanScheduleData property
        /// </summary>
        private static ClamWinScheduleData varScanScheduleData = new ClamWinScheduleData(SettingIDToString(SettingIDs.ScanScheduleData), ClamWinScheduleData.ScanArg);
        /// <summary>
        /// Variable for ScanSchedule property
        /// </summary>
        private static bool varScanSchedule = false;
        /// <summary>
        /// Variable for ScanUseFilter property
        /// </summary>
        private static bool varScanUseFilter = false;
        /// <summary>
        /// Variable for ScanFilterData property
        /// </summary>
        private static ClamWinFilterData varScanFilterData = new ClamWinFilterData(SettingIDToString(SettingIDs.ScanFilterData));
        /// <summary>
        /// Variable for ScanCriticalUseFilter property
        /// </summary>
        private static bool varScanCriticalUseFilter = false;
        /// <summary>
        /// Variable for ScanCriticalFilterData property
        /// </summary>
        private static ClamWinFilterData varScanCriticalFilterData = new ClamWinFilterData(SettingIDToString(SettingIDs.ScanCriticalFilterData));
        /// <summary>
        /// Variable for ScanMyPCUseFilter property
        /// </summary>
        private static bool varScanMyPCUseFilter = false;
        /// <summary>
        /// Variable for ScanMyPCFilterData property
        /// </summary>
        private static ClamWinFilterData varScanMyPCFilterData = new ClamWinFilterData(SettingIDToString(SettingIDs.ScanMyPCFilterData));
        /// <summary>
        /// Variable for LastDBUpdate
        /// </summary>
        private static string varLastDBUpdate = "Never";
        /// <summary>
        /// Variable for DBVersion
        /// </summary>
        private static string varDBVersion = "Unknown";
        #endregion

        #endregion

        #region Enums
        /// <summary>
        /// On Detect Threat Actions
        /// </summary>
        public enum OnDetectActions : int {  Prompt = 0,
                                            BlockAccess,
                                            Delete,
                                            MoveToQuarantine,
                                            PromptAfterScan,
                                            DoNothing
                                         };
        /// <summary>
        /// Setting ID number
        /// </summary>
        public enum SettingIDs : int {  EnableProtection = 0, 
                                        RunAtStartup, 
                                        ShowNonCriticalEvents,
                                        OnAccessScannerStatus,
                                        FileAntiVirusOnDetectAction,
                                        EnableMailAntiVirus,
                                        ScanOnDetectAction,
                                        ScanMyPCOnDetectAction,
                                        ScanCriticalOnDetectAction, 
                                        EnableNotifications,
                                        UpdateRunMode, 
                                        RescanQuarantineAfterUpdate,
                                        LogNonCriticalEvents,
                                        KeepOnlyRecentEvents,
                                        DeleteReportsAfterTime,
                                        ReportsLifeTime,
                                        DeleteQuarantineItemsAfterTime,
                                        QuarantineItemsLifeTime,
                                        UpdateScheduleData,
                                        ScanCriticalScheduleData,
                                        ScanCriticalSchedule,
                                        ScanMyPCScheduleData,
                                        ScanMyPCSchedule,
                                        ScanScheduleData,
                                        ScanSchedule,
                                        ScanUseFilter,
                                        ScanFilterData,
                                        ScanCriticalUseFilter,
                                        ScanCriticalFilterData,
                                        ScanMyPCUseFilter,
                                        ScanMyPCFilterData,
                                        LastDBUpdate,
                                        DBVersion
                                    };
        /// <summary>
        /// Update Run Modes
        /// </summary>
        public enum RunModes : int { Auto = 0, Scheduled, Manual};
        /// <summary>
        /// 
        /// </summary>
        public enum OnAccessStatus : int { Enabled = 0, Disabled, Suspended };
        #endregion

        #region Constants
        public const int UM_SETTINGS_CHANGED = Win32API.WM_USER + 230;
        #endregion

        #region Public Constructor
        public ClamWinSettings()
        {            
        }
        #endregion               

        #region Private Helper Functions
        /// <summary>
        /// Save Filter Data to database
        /// </summary>
        /// <param name="data"></param>
        private static void SaveFilterData(ref ClamWinFilterData data)
        {
            string MainName = data.Name;

            CleanupSetting(MainName + "Exclude");
            foreach (string pattern in data.ExcludePatterns)
            {
                SaveSettingNoUpdate(MainName+"Exclude", pattern);            
            }

            CleanupSetting(MainName + "Include");
            foreach (string pattern in data.IncludePatterns)
            {
                SaveSettingNoUpdate(MainName + "Include", pattern);
            }
        }
        /// <summary>
        /// Load Filter Data from database
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        private static void LoadFilterData(ref ClamWinFilterData data, string name)
        {
            lock (MainLock)
            {
                data = new ClamWinFilterData(name);

                string Name = name.Replace("'", "''");
                Name += "Exclude";

                string command = "SELECT * FROM Settings WHERE Name='" + Name + "';";

                ArrayList list;
                ClamWinDatabase.ExecReader(command, out list);

                if (list.Count != 0)
                {
                    data.ExcludePatterns.Clear();

                    int FiledsPerRecord = ClamWinDatabase.SettingsFPR;
                    int RecordsCount = list.Count / FiledsPerRecord;

                    for (int i = 0; i < RecordsCount; i++)
                    {
                        data.ExcludePatterns.Add((string)list[i * FiledsPerRecord + 2]);
                    }
                }

                Name = name.Replace("'", "''");
                Name += "Include";

                command = "SELECT * FROM Settings WHERE Name='" + Name + "';";
                
                ClamWinDatabase.ExecReader(command, out list);

                if (list.Count != 0)
                {
                    data.IncludePatterns.Clear();

                    int FiledsPerRecord = ClamWinDatabase.SettingsFPR;
                    int RecordsCount = list.Count / FiledsPerRecord;

                    for (int i = 0; i < RecordsCount; i++)
                    {
                        data.IncludePatterns.Add((string)list[i * FiledsPerRecord + 2]);
                    }
                }
            }
        }
        /// <summary>
        /// Save Schedule Data to database
        /// </summary>
        private static void SaveScheduleData(ref ClamWinScheduleData data)
        {
            string MainName = data.Name;            

            string Type = MainName+"Type";
            string Value = ((int)data.Type).ToString();
            SaveSetting(Type, Value);

            string Args = MainName + "Args";
            Value = data.CmdLineArguments;
            SaveSetting(Args, Value);

            string Frequency = MainName + "Frequency";
            Value = data.Frequency.ToString();
            SaveSetting(Frequency, Value);

            string Date = MainName + "Date";
            Value = data.Date.ToBinary().ToString();
            SaveSetting(Date, Value);

            string DailyType = MainName + "DailyType";
            Value = ((int)data.DailyType).ToString();
            SaveSetting(DailyType, Value);

            string Days = MainName + "Days";
            Value = ((int)data.Days).ToString();
            SaveSetting(Days, Value);

            string UseDateTime = MainName + "UseDateTime";
            Value = data.UseDateTime ? "true" : "false";
            SaveSetting(UseDateTime, Value);        
        }
        /// <summary>
        /// Load Schedule Data from database
        /// </summary>
        private static void LoadScheduleData(ref ClamWinScheduleData data, string name, string args)
        {                        
            lock (MainLock)
            {
                data = new ClamWinScheduleData(name, args);

                ArrayList Items;

                string MainName = data.Name;

                string Type = MainName + "Type";
                LoadSetting(Type, out Items);

                if (Items.Count == 3)
                {
                    data.Type = (ClamWinScheduleData.SchedulingTypes)int.Parse((string)Items[2]);
                }

                string Args = MainName + "Args";
                LoadSetting(Args, out Items);

                if (Items.Count == 3)
                {
                    data.CmdLineArguments = (string)Items[2];
                }

                string Frequency = MainName + "Frequency";
                LoadSetting(Frequency, out Items);

                if (Items.Count == 3)
                {
                    data.Frequency = int.Parse((string)Items[2]);
                }

                string Date = MainName + "Date";
                LoadSetting(Date, out Items);

                if (Items.Count == 3)
                {
                    data.Date = DateTime.FromBinary(long.Parse((string)Items[2]));
                }

                string DailyType = MainName + "DailyType";
                LoadSetting(DailyType, out Items);

                if (Items.Count == 3)
                {
                    data.DailyType = (ClamWinScheduleData.DailyTypes)int.Parse((string)Items[2]);
                }

                string Days = MainName + "Days";
                LoadSetting(Days, out Items);

                if (Items.Count == 3)
                {
                    data.Days = (ClamWinScheduleData.DaysOfTheWeek)int.Parse((string)Items[2]);
                }

                string UseDateTime = MainName + "UseDateTime";
                LoadSetting(UseDateTime, out Items);

                if (Items.Count == 3)
                {
                    data.UseDateTime = ((string)Items[2]) == "true" ? true : false;
                }
            }       
        }
        /// <summary>
        /// Update existing or insert new setting in database
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        private static void SaveSetting(string Name, string Value)
        {
            Name = Name.Replace("'", "''");
            Value = Value.Replace("'", "''");

            string command = "UPDATE Settings SET Value = '" + Value;            
            command += "' WHERE Name = '" + Name + "';";
            int result;
            ClamWinDatabase.ExecCommand(command, out result);

            if (result != 0)
            {
                return;
            }

            // Update failed
            command = "INSERT INTO Settings(Name,Value) VALUES(";
            command += "'" + Name + "',";            
            command += "'" + Value + "');";

            ClamWinDatabase.ExecCommand(command, out result);
        }
        /// <summary>
        /// Insert new setting in database
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        private static void SaveSettingNoUpdate(string Name, string Value)
        {
            Name = Name.Replace("'", "''");
            Value = Value.Replace("'", "''");            

            // Update failed
            string command = "INSERT INTO Settings(Name,Value) VALUES(";
            command += "'" + Name + "',";
            command += "'" + Value + "');";

            int result;
            ClamWinDatabase.ExecCommand(command, out result);
        }        
        /// <summary>
        /// Insert new setting into database
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        private static void NewSetting(string Name, string Value)
        {            
            string command = "INSERT INTO Settings(Name,Value) VALUES(";
            command += "'" + Name + "',";
            command += "'" + Value + "');";

            int result;
            ClamWinDatabase.ExecCommand(command, out result);
        }
        /// <summary>
        /// Delete specified setting(s) from database
        /// </summary>
        /// <param name="Name"></param>
        private static void CleanupSetting(string Name)
        {
            Name = Name.Replace("'","''");
            string command = "DELETE FROM Settings WHERE Name='" + Name + "';";
            int result;
            ClamWinDatabase.ExecCommand(command, out result);
        }
        /// <summary>
        /// Load specified setting from database
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="list"></param>
        private static void LoadSetting(string Name, out ArrayList list)
        {

            Name = Name.Replace("'", "''");
            string command = "SELECT * FROM Settings WHERE Name='" + Name + "';";    
           
            ClamWinDatabase.ExecReader(command, out list);
        }
        /// <summary>
        /// Set or clear run at startup
        /// </summary>
        /// <param name="run"></param>
        private static void SetRunAtStartup(bool run)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                if (key == null)
                {
                    return;
                }

                if (run)
                {
                    key.SetValue("ClamWin", ClamWinExePath, RegistryValueKind.String);
                }
                else
                {
                    key.DeleteValue("ClamWin");
                }
            }
            catch
            { 
            
            }
        }
        /// <summary>
        /// Send settings changed message to all listeners
        /// </summary>
        private static void SettingsChangedBroadcast()
        {
            lock (MainLock)
            {
                foreach (IntPtr item in Listeners)
                {
                    for (uint i = 0; i < 34; i++)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, i, 0);
                    }
                }
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// If non-critical ("Clean") events will be shown in events list
        /// </summary>
        public static bool ShowNonCriticalEvents
        {
            get
            {
                lock (MainLock)
                {
                    return varShowNonCriticalEvents;
                }
            }
            set
            {
                lock (MainLock)
                {
                    varShowNonCriticalEvents = value;

                    if (varShowNonCriticalEvents != value)
                    {
                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ShowNonCriticalEvents, 0);
                        }                        
                    }
                }
            }
        }
        /// <summary>
        /// If protection is enabled
        /// </summary>
        public static bool EnableProtection
        {
            get
            {
                lock (MainLock)
                {
                    return varEnableProtection;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varEnableProtection != value)
                    {
                        varEnableProtection = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.EnableProtection, 0);
                        }                        
                    }
                }               
            }
        }
        /// <summary>
        /// Run ClamWin at startup
        /// </summary>
        public static bool RunAtStartup
        {
            get
            {
                lock (MainLock)
                {
                    return varRunAtStartup;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varRunAtStartup != value)
                    {
                        varRunAtStartup = value;

                        SetRunAtStartup(varRunAtStartup);

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.RunAtStartup, 0);
                        }
                    }
                }                
            }
        }       
        /// <summary>
        /// Enable On-Access File Scaner
        /// </summary>
        public static OnAccessStatus OnAccessScannerStatus
        {
            get
            {
                lock (MainLock)
                {
                    return varOnAccessScannerStatus;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varOnAccessScannerStatus != value)
                    {
                        OnAccessStatus helper = varOnAccessScannerStatus;
                        varOnAccessScannerStatus = value;

                        if (ClamWinScanner.SetOnAccessScanerStatus(varOnAccessScannerStatus))
                        {
                            foreach (IntPtr item in Listeners)
                            {
                                Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.OnAccessScannerStatus, 0);
                            }
                        }
                        else
                        {
                            varOnAccessScannerStatus = helper;
                        }
                    }
                }
            }
        }                     
        /// <summary>
        /// What File Antivirus  should do if threat is detected
        /// </summary>
        public static OnDetectActions FileAntiVirusOnDetectAction
        {
            get
            {
                lock (MainLock)
                {
                    return varFileAntiVirusOnDetectAction;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varFileAntiVirusOnDetectAction != value)
                    {
                        varFileAntiVirusOnDetectAction = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.FileAntiVirusOnDetectAction, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Enable Mail Anti-Virus
        /// </summary>
        public static bool EnableMailAntiVirus
        {
            get
            {
                lock (MainLock)
                {
                    return varEnableMailAntiVirus;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varEnableMailAntiVirus != value)
                    {
                        varEnableMailAntiVirus = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.EnableMailAntiVirus, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// What Scan should do if threat is detected
        /// </summary>
        public static OnDetectActions ScanOnDetectAction
        {
            get
            {
                lock (MainLock)
                {
                    return varScanOnDetectAction;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanOnDetectAction != value)
                    {
                        varScanOnDetectAction = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanOnDetectAction, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// What Scan MyPC should do if threat is detected
        /// </summary>
        public static OnDetectActions ScanMyPCOnDetectAction
        {
            get
            {
                lock (MainLock)
                {
                    return varScanMyPCOnDetectAction;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanMyPCOnDetectAction != value)
                    {
                        varScanMyPCOnDetectAction = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanMyPCOnDetectAction, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// What Scan Critical should do if threat is detected
        /// </summary>
        public static OnDetectActions ScanCriticalOnDetectAction
        {
            get
            {
                lock (MainLock)
                {
                    return varScanCriticalOnDetectAction;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanCriticalOnDetectAction != value)
                    {
                        varScanCriticalOnDetectAction = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanCriticalOnDetectAction, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Enable Notifications
        /// </summary>
        public static bool EnableNotifications
        {
            get
            {
                lock (MainLock)
                {
                    return varEnableNotifications;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varEnableNotifications != value)
                    {
                        varEnableNotifications = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.EnableNotifications, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Update run mode
        /// </summary>
        public static RunModes UpdateRunMode
        {
            get
            {
                lock (MainLock)
                {
                    return varUpdateRunMode;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varUpdateRunMode != value)
                    {
                        varUpdateRunMode = value;

                        if (varUpdateRunMode != RunModes.Scheduled)
                        {
                            ClamWinScheduler.RemoveTask(ref varUpdateScheduleData);
                        }

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.UpdateRunMode, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Rescan quarantine afte update
        /// </summary>
        public static bool RescanQuarantineAfterUpdate
        {
            get
            {
                lock (MainLock)
                {
                    return varRescanQuarantineAfterUpdate;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varRescanQuarantineAfterUpdate != value)
                    {
                        varRescanQuarantineAfterUpdate = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.RescanQuarantineAfterUpdate, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Log non-critical events
        /// </summary>
        public static bool LogNonCriticalEvents
        {
            get
            {
                lock (MainLock)
                {
                    return varLogNonCriticalEvents;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varLogNonCriticalEvents != value)
                    {
                        varLogNonCriticalEvents = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.LogNonCriticalEvents, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Keep only recent events
        /// </summary>
        public static bool KeepOnlyRecentEvents
        {
            get
            {
                lock (MainLock)
                {
                    return varKeepOnlyRecentEvents;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varKeepOnlyRecentEvents != value)
                    {
                        varKeepOnlyRecentEvents = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.KeepOnlyRecentEvents, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Delete reports after time
        /// </summary>
        public static bool DeleteReportsAfterTime
        {
            get
            {
                lock (MainLock)
                {
                    return varDeleteReportsAfterTime;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varDeleteReportsAfterTime != value)
                    {
                        varDeleteReportsAfterTime = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.DeleteReportsAfterTime, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Reports life time
        /// </summary>
        public static int ReportsLifeTime
        {
            get
            {
                lock (MainLock)
                {
                    return varReportsLifeTime;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varReportsLifeTime != value)
                    {
                        varReportsLifeTime = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ReportsLifeTime, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Delete quarantine items after time
        /// </summary>
        public static bool DeleteQuarantineItemsAfterTime
        {
            get
            {
                lock (MainLock)
                {
                    return varDeleteQuarantineItemsAfterTime;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varDeleteQuarantineItemsAfterTime != value)
                    {
                        varDeleteQuarantineItemsAfterTime = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.DeleteQuarantineItemsAfterTime, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Quarantine items life time
        /// </summary>
        public static int QuarantineItemsLifeTime
        {
            get
            {
                lock (MainLock)
                {
                    return varQuarantineItemsLifeTime;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varQuarantineItemsLifeTime != value)
                    {
                        varQuarantineItemsLifeTime = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.QuarantineItemsLifeTime, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Update schedule data 
        /// </summary>
        public static ClamWinScheduleData UpdateScheduleData
        {
            get
            {
                lock (MainLock)
                {
                    return varUpdateScheduleData;
                }
            }
            set
            {
                lock (MainLock)
                {                    
                    varUpdateScheduleData = value;

                    if (varUpdateRunMode == RunModes.Scheduled)
                    {
                        ClamWinScheduler.AddTask(ref value);
                    }

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.UpdateScheduleData, 0);
                    }                    
                }
            }        
        }
        /// <summary>
        /// Scan Critical Areas Schedule Data
        /// </summary>
        public static ClamWinScheduleData ScanCriticalScheduleData
        {
            get
            {
                lock (MainLock)
                {
                    return varScanCriticalScheduleData;
                }
            }
            set
            {
                lock (MainLock)
                {                    
                    varScanCriticalScheduleData = value;

                    if (varScanCriticalSchedule)
                    {
                        ClamWinScheduler.AddTask(ref value);
                    }

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanCriticalScheduleData, 0);
                    }                    
                }
            }
        }
        /// <summary>
        /// If Scan Critical Ares is scheduled
        /// </summary>
        public static bool ScanCriticalSchedule
        {
            get
            {
                lock (MainLock)
                {
                    return varScanCriticalSchedule;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanCriticalSchedule != value)
                    {
                        varScanCriticalSchedule = value;

                        if (!varScanCriticalSchedule)
                        {
                            ClamWinScheduler.RemoveTask(ref varScanCriticalScheduleData);
                        }

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanCriticalSchedule, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Scan My Computer Schedule Data
        /// </summary>
        public static ClamWinScheduleData ScanMyPCScheduleData
        {
            get
            {
                lock (MainLock)
                {
                    return varScanMyPCScheduleData;
                }
            }
            set
            {
                lock (MainLock)
                {
                    varScanMyPCScheduleData = value;

                    if (varScanMyPCSchedule)
                    {
                        ClamWinScheduler.AddTask(ref value);
                    }

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanMyPCScheduleData, 0);
                    }
                }
            }
        }
        /// <summary>
        /// If Scan My Computer is scheduled
        /// </summary>
        public static bool ScanMyPCSchedule
        {
            get
            {
                lock (MainLock)
                {
                    return varScanMyPCSchedule;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanMyPCSchedule != value)
                    {
                        varScanMyPCSchedule = value;

                        if (!varScanMyPCSchedule)
                        {
                            ClamWinScheduler.RemoveTask(ref varScanMyPCScheduleData);
                        }

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanMyPCSchedule, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Scan Schedule Data
        /// </summary>
        public static ClamWinScheduleData ScanScheduleData
        {
            get
            {
                lock (MainLock)
                {
                    return varScanScheduleData;
                }
            }
            set
            {
                lock (MainLock)
                {
                    varScanScheduleData = value;

                    if (varScanSchedule)
                    {
                        ClamWinScheduler.AddTask(ref value);
                    }

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanScheduleData, 0);
                    }
                }
            }
        }
        /// <summary>
        /// If Scan My Computer is scheduled
        /// </summary>
        public static bool ScanSchedule
        {
            get
            {
                lock (MainLock)
                {
                    return varScanSchedule;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanSchedule != value)
                    {
                        varScanSchedule = value;

                        if (!varScanSchedule)
                        {
                            ClamWinScheduler.RemoveTask(ref varScanScheduleData);
                        }

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanSchedule, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// If Use Filter for Scan
        /// </summary>
        public static bool ScanUseFilter
        {
            get
            {
                lock (MainLock)
                {
                    return varScanUseFilter;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanUseFilter != value)
                    {
                        varScanUseFilter = value;                        

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanUseFilter, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Scan Filter Data
        /// </summary>
        public static ClamWinFilterData ScanFilterData
        {
            get
            {
                lock (MainLock)
                {
                    return varScanFilterData;
                }
            }
            set
            {
                lock (MainLock)
                {

                    varScanFilterData = value;

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanFilterData, 0);
                    }                    
                }
            }
        }
        /// <summary>
        /// If Use Filter for Scan Critical Areas
        /// </summary>
        public static bool ScanCriticalUseFilter
        {
            get
            {
                lock (MainLock)
                {
                    return varScanCriticalUseFilter;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanCriticalUseFilter != value)
                    {
                        varScanCriticalUseFilter = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanCriticalUseFilter, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Scan Critical Areas Filter Data
        /// </summary>
        public static ClamWinFilterData ScanCriticalFilterData
        {
            get
            {
                lock (MainLock)
                {
                    return varScanCriticalFilterData;
                }
            }
            set
            {
                lock (MainLock)
                {

                    varScanCriticalFilterData = value;

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanCriticalFilterData, 0);
                    }
                }
            }
        }
        /// <summary>
        /// If Use Filter for Scan My Computer
        /// </summary>
        public static bool ScanMyPCUseFilter
        {
            get
            {
                lock (MainLock)
                {
                    return varScanMyPCUseFilter;
                }
            }
            set
            {
                lock (MainLock)
                {
                    if (varScanMyPCUseFilter != value)
                    {
                        varScanMyPCUseFilter = value;

                        foreach (IntPtr item in Listeners)
                        {
                            Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanMyPCUseFilter, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Scan My Computer Filter Data
        /// </summary>
        public static ClamWinFilterData ScanMyPCFilterData
        {
            get
            {
                lock (MainLock)
                {
                    return varScanMyPCFilterData;
                }
            }
            set
            {
                lock (MainLock)
                {

                    varScanMyPCFilterData = value;

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.ScanMyPCFilterData, 0);
                    }
                }
            }
        }
        /// <summary>
        /// Last database update date-time string
        /// </summary>
        public static string LastDBUpdate
        {
            get
            {
                lock (MainLock)
                {
                    return varLastDBUpdate;
                }
            }
            set
            {
                lock (MainLock)
                {

                    varLastDBUpdate = value;

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.LastDBUpdate, 0);
                    }
                }
            }
        }
        /// <summary>
        /// Current database version
        /// </summary>
        public static string DBVersion
        {
            get
            {
                lock (MainLock)
                {
                    return varDBVersion;
                }
            }
            set
            {
                lock (MainLock)
                {

                    varDBVersion = value;

                    foreach (IntPtr item in Listeners)
                    {
                        Win32API.PostMessage(item, UM_SETTINGS_CHANGED, (int)SettingIDs.DBVersion, 0);
                    }
                }
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Add specified listener
        /// </summary>
        /// <param name="handle"></param>
        public static void AddListener(IntPtr handle)
        {            
            lock (MainLock)
            {
                foreach (IntPtr item in Listeners)
                {
                    if (handle == item)
                    {
                        return;
                    }
                }

                Listeners.Add(handle);
            }
        }
        /// <summary>
        /// Remove specified listener
        /// </summary>
        /// <param name="handle"></param>
        public static void RemoveListener(IntPtr handle)
        {
            lock (MainLock)
            {
                Listeners.Remove(handle);
            }
        }
        /// <summary>
        /// Return corresponding settings name for specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string SettingIDToString(SettingIDs id)
        {
            switch (id)
            {                     
                case SettingIDs.ShowNonCriticalEvents:
                    return "ShowNonCriticalEvents";
                case SettingIDs.EnableProtection:
                    return "EnableProtection";
                case SettingIDs.RunAtStartup:
                    return "RunAtStartup";
                case SettingIDs.OnAccessScannerStatus:
                    return "OnAccessScannerStatus";
                case SettingIDs.FileAntiVirusOnDetectAction:
                    return "FileAntiVirusOnDetectAction";
                case SettingIDs.EnableMailAntiVirus:
                    return "EnableMailAntiVirus";
                case SettingIDs.ScanOnDetectAction:
                    return "ScanOnDetectAction";
                case SettingIDs.ScanMyPCOnDetectAction:
                    return "ScanMyPCOnDetectAction";
                case SettingIDs.ScanCriticalOnDetectAction:
                    return "ScanCriticalOnDetectAction";
                case SettingIDs.EnableNotifications:
                    return "EnableNotifications";
                case SettingIDs.UpdateRunMode:
                    return "UpdateRunMode";
                case SettingIDs.RescanQuarantineAfterUpdate:
                    return "RescanQuarantineAfterUpdate";
                case SettingIDs.LogNonCriticalEvents:
                    return "LogNonCriticalEvents";
                case SettingIDs.KeepOnlyRecentEvents:
                    return "KeepOnlyRecentEvents";
                case SettingIDs.DeleteReportsAfterTime:
                    return "DeleteReportsAfterTime";
                case SettingIDs.ReportsLifeTime:
                    return "ReportsLifeTime";
                case SettingIDs.DeleteQuarantineItemsAfterTime:
                    return "DeleteQuarantineItemsAfterTime";
                case SettingIDs.QuarantineItemsLifeTime:
                    return "QuarantineItemsLifeTime";
                case SettingIDs.UpdateScheduleData:
                    return "UpdateScheduleData";
                case SettingIDs.ScanCriticalScheduleData:
                    return "ScanCriticalScheduleData";
                case SettingIDs.ScanCriticalSchedule:
                    return "ScanCriticalSchedule";
                case SettingIDs.ScanMyPCScheduleData:
                    return "ScanMyPCScheduleData";
                case SettingIDs.ScanMyPCSchedule:
                    return "ScanMyPCSchedule";
                case SettingIDs.ScanSchedule:
                    return "ScanSchedule";
                case SettingIDs.ScanScheduleData:
                    return "ScanScheduleData";
                case SettingIDs.ScanUseFilter:
                    return "ScanUseFilter";
                case SettingIDs.ScanFilterData:
                    return "ScanFilterData";
                case SettingIDs.ScanCriticalUseFilter:
                    return "ScanCriticalUseFilter";
                case SettingIDs.ScanCriticalFilterData:
                    return "ScanCriticalFilterData";
                case SettingIDs.ScanMyPCUseFilter:
                    return "ScanMyPCUseFilter";
                case SettingIDs.ScanMyPCFilterData:
                    return "ScanMyPCFilterData";
                case SettingIDs.LastDBUpdate:
                    return "LastDBUpdate";
                case SettingIDs.DBVersion:
                    return "DBVersion";
                default:
                    System.Windows.Forms.MessageBox.Show("ERROR:Setting is not defined");
                    return "ERROR:Setting is not defined";
                
            }
        }
        /// <summary>
        /// Return corresponding action description for specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ActionIdToString(OnDetectActions id)
        {
            switch (id)
            { 
                case OnDetectActions.BlockAccess:
                    return "Block access";
                case OnDetectActions.Delete:
                    return "Delete";
                case OnDetectActions.DoNothing:
                    return "Do not perform any actions";
                case OnDetectActions.MoveToQuarantine:
                    return "Move to Quarantine";
                case OnDetectActions.Prompt:
                    return "Prompt for action during scan";
                case OnDetectActions.PromptAfterScan:
                    return "Prompt for action when the scan is complete";
                default:
                    return "ERROR: NOT DEFINED!";
            }        
        }
        /// <summary>
        /// Save settings to database
        /// </summary>
        public static void SaveSettings()
        {
            lock (MainLock)
            {
                string Value = varShowNonCriticalEvents ? "true" : "false";
                string Name = SettingIDToString(SettingIDs.ShowNonCriticalEvents);
                SaveSetting(Name, Value);

                Value = varEnableProtection ? "true" : "false";
                Name = SettingIDToString(SettingIDs.EnableProtection);
                SaveSetting(Name, Value);

                Value = varRunAtStartup ? "true" : "false";
                Name = SettingIDToString(SettingIDs.RunAtStartup);
                SaveSetting(Name, Value);

                Value = ((int)varOnAccessScannerStatus).ToString();
                Name = SettingIDToString(SettingIDs.OnAccessScannerStatus);
                SaveSetting(Name, Value);

                Value = ((int)varFileAntiVirusOnDetectAction).ToString();
                Name = SettingIDToString(SettingIDs.FileAntiVirusOnDetectAction);
                SaveSetting(Name, Value);

                Value = varEnableMailAntiVirus ? "true" : "false";
                Name = SettingIDToString(SettingIDs.EnableMailAntiVirus);
                SaveSetting(Name, Value);

                Value = ((int)varScanOnDetectAction).ToString();
                Name = SettingIDToString(SettingIDs.ScanOnDetectAction);
                SaveSetting(Name, Value);

                Value = ((int)varScanMyPCOnDetectAction).ToString();
                Name = SettingIDToString(SettingIDs.ScanMyPCOnDetectAction);
                SaveSetting(Name, Value);

                Value = ((int)varScanCriticalOnDetectAction).ToString();
                Name = SettingIDToString(SettingIDs.ScanCriticalOnDetectAction);
                SaveSetting(Name, Value);

                Value = varEnableNotifications ? "true" : "false";
                Name = SettingIDToString(SettingIDs.EnableNotifications);
                SaveSetting(Name, Value);

                Value = ((int)varUpdateRunMode).ToString();
                Name = SettingIDToString(SettingIDs.UpdateRunMode);
                SaveSetting(Name, Value);

                Value = varRescanQuarantineAfterUpdate ? "true" : "false";
                Name = SettingIDToString(SettingIDs.RescanQuarantineAfterUpdate);
                SaveSetting(Name, Value);

                Value = varLogNonCriticalEvents ? "true" : "false";
                Name = SettingIDToString(SettingIDs.LogNonCriticalEvents);
                SaveSetting(Name, Value);

                Value = varKeepOnlyRecentEvents ? "true" : "false";
                Name = SettingIDToString(SettingIDs.KeepOnlyRecentEvents);
                SaveSetting(Name, Value);

                Value = varDeleteReportsAfterTime ? "true" : "false";
                Name = SettingIDToString(SettingIDs.DeleteReportsAfterTime);
                SaveSetting(Name, Value);

                Value = varReportsLifeTime.ToString();
                Name = SettingIDToString(SettingIDs.ReportsLifeTime);
                SaveSetting(Name, Value);

                Value = varDeleteQuarantineItemsAfterTime ? "true" : "false";
                Name = SettingIDToString(SettingIDs.DeleteQuarantineItemsAfterTime);
                SaveSetting(Name, Value);

                Value = varQuarantineItemsLifeTime.ToString();
                Name = SettingIDToString(SettingIDs.QuarantineItemsLifeTime);
                SaveSetting(Name, Value);

                Value = varScanCriticalSchedule ? "true" : "false";
                Name = SettingIDToString(SettingIDs.ScanCriticalSchedule);
                SaveSetting(Name, Value);

                Value = varScanMyPCSchedule ? "true" : "false";
                Name = SettingIDToString(SettingIDs.ScanMyPCSchedule);
                SaveSetting(Name, Value);

                Value = varScanSchedule ? "true" : "false";
                Name = SettingIDToString(SettingIDs.ScanSchedule);
                SaveSetting(Name, Value);

                Value = varScanUseFilter ? "true" : "false";
                Name = SettingIDToString(SettingIDs.ScanUseFilter);
                SaveSetting(Name, Value);

                Value = varScanCriticalUseFilter ? "true" : "false";
                Name = SettingIDToString(SettingIDs.ScanCriticalUseFilter);
                SaveSetting(Name, Value);

                Value = varScanMyPCUseFilter ? "true" : "false";
                Name = SettingIDToString(SettingIDs.ScanMyPCUseFilter);
                SaveSetting(Name, Value);

                Value = varLastDBUpdate;
                Name = SettingIDToString(SettingIDs.LastDBUpdate);
                SaveSetting(Name, Value);

                // Complex members saving
                SaveScheduleData(ref varUpdateScheduleData);

                SaveScheduleData(ref varScanCriticalScheduleData);

                SaveScheduleData(ref varScanMyPCScheduleData);

                SaveScheduleData(ref varScanScheduleData);

                SaveFilterData(ref varScanFilterData);

                SaveFilterData(ref varScanCriticalFilterData);

                SaveFilterData(ref varScanMyPCFilterData);

            }
        }
        /// <summary>
        /// Load settings from database
        /// </summary>
        public static void LoadSettings()
        {          
            string Name;
            ArrayList Items;

            // ShowNonCriticalEvents
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ShowNonCriticalEvents);                
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varShowNonCriticalEvents = ((string)Items[2]) == "true";
                }
            }
                        
            // EnableProtection
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.EnableProtection);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varEnableProtection = ((string)Items[2]) == "true";
                }
            }            

            // RunAtStartup
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.RunAtStartup);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varRunAtStartup = ((string)Items[2]) == "true";
                }
            }            

            // EnableFileAntiVirus
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.OnAccessScannerStatus);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varOnAccessScannerStatus = (OnAccessStatus) int.Parse(((string)Items[2]));
                }
            }            

            // FileAntiVirusOnDetectAction
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.FileAntiVirusOnDetectAction);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varFileAntiVirusOnDetectAction = (OnDetectActions)int.Parse((string)Items[2]);
                }
            }            

            // EnableMailAntiVirus
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.EnableMailAntiVirus);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varEnableMailAntiVirus = ((string)Items[2]) == "true";
                }
            }            

            // ScanOnDetectAction
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanOnDetectAction);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanOnDetectAction = (OnDetectActions)int.Parse((string)Items[2]);
                }
            }            

            // ScanMyPCOnDetectAction
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanMyPCOnDetectAction);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanMyPCOnDetectAction = (OnDetectActions)int.Parse((string)Items[2]);
                }
            }            

            // ScanCriticalOnDetectAction
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanCriticalOnDetectAction);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanCriticalOnDetectAction = (OnDetectActions)int.Parse((string)Items[2]);
                }
            }            
          
            // EnableNotifications
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.EnableNotifications);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varEnableNotifications = ((string)Items[2]) == "true";
                }
            }            

            // UpdateRunMode
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.UpdateRunMode);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varUpdateRunMode = (RunModes)int.Parse((string)Items[2]);
                } 
            }                   

            // RescanQuarantineAfterUpdate
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.RescanQuarantineAfterUpdate);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varRescanQuarantineAfterUpdate = ((string)Items[2]) == "true";
                }
            }            

            // LogNonCriticalEvents
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.LogNonCriticalEvents);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varLogNonCriticalEvents = ((string)Items[2]) == "true";
                }
            }            

            // KeepOnlyRecentEvents
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.KeepOnlyRecentEvents);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varKeepOnlyRecentEvents = ((string)Items[2]) == "true";
                }
            }            

            // DeleteReportsAfterTime
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.DeleteReportsAfterTime);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varDeleteReportsAfterTime = ((string)Items[2]) == "true";
                }
            }            

            // ReportsLifeTime
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ReportsLifeTime);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varReportsLifeTime = int.Parse((string)Items[2]);
                }
            }            

            // DeleteQuarantineItemsAfterTime
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.DeleteQuarantineItemsAfterTime);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varDeleteQuarantineItemsAfterTime = ((string)Items[2]) == "true";
                }
            }            

            // QuarantineItemsLifeTime
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.QuarantineItemsLifeTime);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varQuarantineItemsLifeTime = int.Parse((string)Items[2]);
                }
            }            

            // ScanCriticalSchedule
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanCriticalSchedule);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanCriticalSchedule = ((string)Items[2]) == "true";
                }
            }

            // ScanMyPCSchedule
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanMyPCSchedule);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanMyPCSchedule = ((string)Items[2]) == "true";
                }
            }

            // ScanSchedule
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanSchedule);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanSchedule = ((string)Items[2]) == "true";
                }
            }

            // ScanUseFilter
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanUseFilter);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanUseFilter = ((string)Items[2]) == "true";
                }
            }

            // ScanCriticalUseFilter
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanCriticalUseFilter);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanCriticalUseFilter = ((string)Items[2]) == "true";
                }
            }

            // ScanMyPCUseFilter
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.ScanMyPCUseFilter);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varScanMyPCUseFilter = ((string)Items[2]) == "true";
                }
            }

            // LastDBUpdate
            lock (MainLock)
            {
                Name = SettingIDToString(SettingIDs.LastDBUpdate);
                LoadSetting(Name, out Items);

                if (Items.Count == 3)
                {
                    varLastDBUpdate = (string)Items[2];
                }
            }

            // DBVersion ()
            string Value = ClamWinScanner.GetDBVersion();
            lock (MainLock)
            {
                varDBVersion = Value;
            }

            // UpdateScheduleData
            LoadScheduleData(ref varUpdateScheduleData, SettingIDToString(SettingIDs.UpdateScheduleData),ClamWinScheduleData.UpdateArg);

            // ScanCriticalScheduleData
            LoadScheduleData(ref varScanCriticalScheduleData, SettingIDToString(SettingIDs.ScanCriticalScheduleData), ClamWinScheduleData.ScanCriticalArg);

            // ScanMyPCScheduleData
            LoadScheduleData(ref varScanMyPCScheduleData, SettingIDToString(SettingIDs.ScanMyPCScheduleData), ClamWinScheduleData.ScanMyPCArg);

            // ScanScheduleData
            LoadScheduleData(ref varScanScheduleData, SettingIDToString(SettingIDs.ScanScheduleData), ClamWinScheduleData.ScanArg);

            // ScanFilterData
            LoadFilterData(ref varScanFilterData, SettingIDToString(SettingIDs.ScanFilterData));

            // ScanCriticalFilterData
            LoadFilterData(ref varScanCriticalFilterData, SettingIDToString(SettingIDs.ScanCriticalFilterData));

            // ScanMyPCFilterData
            LoadFilterData(ref varScanMyPCFilterData, SettingIDToString(SettingIDs.ScanMyPCFilterData));

            // Refresh all settings 
            SettingsChangedBroadcast();
        }
        /// <summary>
        /// Initialize settings 
        /// </summary>
        /// <param name="path"></param>
        public static void Open(string path)
        {
            lock(MainLock)
            {
                ClamWinExePath = path;
            }            

            LoadSettings();            

            lock (MainLock)
            {
                Opened = true;
            }
        }
        /// <summary>
        /// Close settings
        /// </summary>
        /// <param name="path"></param>
        public static void Close()
        {            
            lock (MainLock)
            {
                Opened = false;
            }
        }
        #endregion
    }
    #endregion
}
