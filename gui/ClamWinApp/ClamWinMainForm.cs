// Name:        ClamWinMainForm.cs
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Management;
using System.Diagnostics;
using System.Timers;


using System.Data.SQLite;

using ClamWinApp.Properties;

namespace ClamWinApp
{   
    #region ClamWinMainForm class
    /// <summary>
    /// Main form for ClamWin configuration program
    /// </summary>
    public partial class ClamWinMainForm : Form
    {
        #region Private Data
        /// <summary>
        /// Flag to handle closing/exiting events 
        /// </summary>
        private bool flagExit = false;
        /// <summary>
        /// Determine if ClamWin was launched by Windows Scheduler
        /// </summary>
        private bool scheduledLaunch = false;
        /// <summary>
        /// Path to ClamWin folder
        /// </summary>
        private string clamWinPath = null;
        /// <summary>
        /// Path and file name to ClamWin config program
        /// </summary>
        private string clamWinPathName = null;
        /// <summary>
        /// Main database object
        /// </summary>
        private ClamWinDatabase dataBase = new ClamWinDatabase();
        /// <summary>
        /// Struct describes item type
        /// </summary>
        private class ItemData
        {
            public ItemData(bool system,bool drive,bool folder,string path)
            {
                System = system;
                Path = path;
                Drive = drive;
                Folder = folder;                
            }
            public bool System;
            public bool Drive;
            public bool Folder;
            public string Path;            
        };
        /// <summary>
        /// Drive lookup WMI helper
        /// </summary>
        ManagementObjectSearcher driveSearcher;    
        /// <summary>
        /// Scanning form
        /// </summary>
        static private ClamWinScanForm formScanning = new ClamWinScanForm();
        /// <summary>
        /// Settings form
        /// </summary>
        static private ClamWinSettingsForm formSettings = new ClamWinSettingsForm();
        /// <summary>
        /// Quarantine form
        /// </summary>
        static private ClamWinQuarantineForm formQuarantine = new ClamWinQuarantineForm();
        /// <summary>
        /// Current visible items list
        /// </summary>
        private ListView listViewCurrent;
        /// <summary>
        /// Currently visible scan panel id
        /// </summary>
        private ScanPanelIDs CurrentScanPanel;
        /// <summary>
        /// Scane forms array
        /// </summary>
        private ArrayList ScanForms = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        private ClamWinFilterNotifyForm FilterNotify = null;
        #endregion

        #region Public Constructor
        /// <summary>
        /// Constructor        
        /// </summary>      
        public ClamWinMainForm( string[] args)  
        {
            InitializeComponent();            

            // Getting ClamWin path
            clamWinPathName = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            clamWinPath = System.IO.Path.GetDirectoryName(clamWinPathName);

            // Create data directory
            System.IO.Directory.CreateDirectory(clamWinPath + "\\Data\\");            

            Resources.Culture = new CultureInfo("en");                        
            
            // Prepare driveSearcher
            const string Query = "SELECT * FROM Win32_LogicalDisk";
            driveSearcher = new ManagementObjectSearcher(Query);

            ClamWinScheduler.Open(clamWinPathName);

            ClamWinDatabase.Open(clamWinPath, this.Handle);

            FilterNotify = new ClamWinFilterNotifyForm(this);

            ClamWinMainFormNotifications.Open(this.Handle);
            ClamWinMainFormNotifications.AddNotification("Test", 
                                                         ClamWinMainFormNotifications.NotificationCodes.NeedDatabaseUpdate, 
                                                         ClamWinMainFormNotifications.NotificationType.Info);

            //ClamWinMainFormNotifications.NotificationDone(ClamWinMainFormNotifications.NotificationCodes.NewVersionAlert);
            //ClamWinMainFormNotifications.NotificationDone(ClamWinMainFormNotifications.NotificationCodes.NeedDatabaseUpdate);

            // Settings initialization
            ClamWinSettings.AddListener(this.Handle);
            ClamWinSettings.AddListener(formScanning.Handle);
            ClamWinSettings.Open(clamWinPathName);            

            // Scanner initialization            
            if (!ClamWinScanner.Open(this.Handle))
            { 
                System.Windows.Forms.MessageBox.Show("Unable to open main scanner service.",
                                                     "ClamWin",
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error);                
            }
            
            ClamWinStatistics.Open();   
         
            // Command line arguments processing
            Program.OnArguments(args, Handle);

            ClamWinQuarantine.Open(clamWinPath);

            IntPtr[] Listeners = new IntPtr[1];
            Listeners[0] = this.Handle;
            ClamWinVersion.CheckVersion(Listeners);                     
        }
        #endregion

        #region Constants
        public const int UM_FILTERJOBSUPDATED = Win32API.WM_USER + 2231;
        public const int UM_FILEALERT = Win32API.WM_USER + 2;
        public const int UM_SCAN_BTN_PRESSED = Win32API.WM_USER + 1001;        
        public const int UM_SCHEDULED_UPDATE = Win32API.WM_USER + 1002;
        public const int UM_SCHEDULED_SCAN = Win32API.WM_USER + 1003;
        public const int UM_SCHEDULED_SCAN_MY_PC = Win32API.WM_USER + 1004;
        public const int UM_SCHEDULED_SCAN_CRITICAL = Win32API.WM_USER + 1005;
        public const int UM_SCAN_QUARANTINE_RESCAN = Win32API.WM_USER + 1006;        
        #endregion

        #region Enums
        public enum ScanPanelIDs : int {Scan,ScanMyPC,ScanCritical,Quarantine};
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Initialize notify panel controls
        /// </summary>
        private void InitNotifyPanelControls()
        {
            linkLabelNotifyBack.BringToFront();
            linkLabelNotifyNext.BringToFront();
        }
        /// <summary>
        /// Inits home panel controls
        /// </summary>
        private void InitHomePanelControls()
        {
            // File Anti Virus status
            if (ClamWinSettings.OnAccessScannerStatus == ClamWinSettings.OnAccessStatus.Enabled)
            {
                labelResidentFileProtectionValue.Text = "Enabled";
                labelResidentFileProtectionValue.ForeColor = Color.Green;
            }
            else
            {
                labelResidentFileProtectionValue.Text = "Disabled";
                labelResidentFileProtectionValue.ForeColor = Color.Red;
            }

            // E-Mail Anti Virus status
            if (ClamWinSettings.EnableMailAntiVirus)
            {
                labelEmailProtectionValue.Text = "Enabled";
                labelEmailProtectionValue.ForeColor = Color.Green;

            }
            else
            {
                labelEmailProtectionValue.Text = "Disabled";
                labelEmailProtectionValue.ForeColor = Color.Red;
            }

            // Database last update
            labelLastUpdatedValue.Text = ClamWinSettings.LastDBUpdate;

            DateTime date;
            try
            {
                date = DateTime.Parse(labelLastUpdatedValue.Text);
            }
            catch
            {
                date = new DateTime();
            }

            DateTime now = DateTime.Now;

            TimeSpan span = now - date;

            if (span.Days < 2)
            {
                labelLastUpdatedValue.ForeColor = Color.Green;
            }
            else if (span.Days < 5)
            {
                labelLastUpdatedValue.ForeColor = Color.Brown;
            }
            else
            {
                labelLastUpdatedValue.ForeColor = Color.Red;
            }

            // Database Version
            labelDBVersionValue.Text = ClamWinSettings.DBVersion;
            if (labelDBVersionValue.Text == "Unknown")
            {
                labelDBVersionValue.ForeColor = Color.Red;
            }
            else
            {
                labelDBVersionValue.ForeColor = Color.Black;
            }
        }
        /// <summary>
        /// Initialize Protection panel controls
        /// </summary>
        private void InitProtectionPanelControls()
        {
            if (ClamWinSettings.OnAccessScannerStatus == ClamWinSettings.OnAccessStatus.Enabled)
            {
                radioButtonOnAccessOn.Checked = true;
            }
            else if (ClamWinSettings.OnAccessScannerStatus == ClamWinSettings.OnAccessStatus.Disabled)
            {
                radioButtonOnAccessOff.Checked = true;
            }
            else
            {
                radioButtonOnAccessSuspended.Checked = true;
            }
        }
        /// <summary>
        /// Load Scan statistics from database
        /// </summary>
        private void InitScanStatisticsGroupBox()
        {
            string command = "SELECT * FROM Scans WHERE ScanStart=(SELECT max(ScanStart) FROM Scans WHERE ScanPanelID='";
            command += ((int)ScanPanelIDs.Scan).ToString() + "');";

            ArrayList list;
            ClamWinDatabase.ExecReader(command, out list);

            if (list.Count != 0)
            {               

                long helper = long.Parse((string)list[1]);
                DateTime time = DateTime.FromBinary(helper);
                linkLabelScanLastScanValue.Text = time.ToShortDateString()+" "+time.ToLongTimeString();
                
                command = "SELECT * FROM Statistics WHERE ScanID='" + (string)list[0] + "' AND Object='Total';";
                ClamWinDatabase.ExecReader(command, out list);

                if (list.Count != 0)
                {
                    linkLabelScanScannedValue.Text = (string)list[2];
                    linkLabelScanThreatsValue.Text = (string)list[3];
                }
            }
            else
            {
                linkLabelScanThreatsValue.Text = "0";
                linkLabelScanScannedValue.Text = "0";
                linkLabelScanLastScanValue.Text = "Never";
            }
            
        }
        /// <summary>
        /// Load Scan My PC statistics from database
        /// </summary>
        private void InitScanMyPCStatisticsGroupBox()
        {
            string command = "SELECT * FROM Scans WHERE ScanStart=(SELECT max(ScanStart) FROM Scans WHERE ScanPanelID='";
            command += ((int)ScanPanelIDs.ScanMyPC).ToString() + "');";

            ArrayList list;
            ClamWinDatabase.ExecReader(command, out list);

            if (list.Count != 0)
            {

                long helper = long.Parse((string)list[1]);
                DateTime time = DateTime.FromBinary(helper);
                linkLabelScanMyPCLastScanValue.Text = time.ToShortDateString() + " " + time.ToLongTimeString();

                command = "SELECT * FROM Statistics WHERE ScanID='" + (string)list[0] + "' AND Object='Total';";
                ClamWinDatabase.ExecReader(command, out list);

                if (list.Count != 0)
                {
                    linkLabelScanMyPCScannedValue.Text = (string)list[2];
                    linkLabelScanMyPCThreatsValue.Text = (string)list[3];
                }
            }
            else
            {
                linkLabelScanMyPCThreatsValue.Text = "0";
                linkLabelScanMyPCScannedValue.Text = "0";
                linkLabelScanMyPCLastScanValue.Text = "Never";
            }

        }
        /// <summary>
        /// Load Scan Critical statistics from database
        /// </summary>
        private void InitScanCriticalStatisticsGroupBox()
        {
            string command = "SELECT * FROM Scans WHERE ScanStart=(SELECT max(ScanStart) FROM Scans WHERE ScanPanelID='";
            command += ((int)ScanPanelIDs.ScanCritical).ToString() + "');";

            ArrayList list;
            ClamWinDatabase.ExecReader(command, out list);

            if (list.Count != 0)
            {

                long helper = long.Parse((string)list[1]);
                DateTime time = DateTime.FromBinary(helper);
                linkLabelScanCriticalLastScanValue.Text = time.ToShortDateString() + " " + time.ToLongTimeString();

                command = "SELECT * FROM Statistics WHERE ScanID='" + (string)list[0] + "' AND Object='Total';";
                ClamWinDatabase.ExecReader(command, out list);

                if (list.Count != 0)
                {
                    linkLabelScanCriticalScannedValue.Text = (string)list[2];
                    linkLabelScanCriticalThreatsValue.Text = (string)list[3];
                }
            }
            else
            {
                linkLabelScanCriticalThreatsValue.Text = "0";
                linkLabelScanCriticalScannedValue.Text = "0";
                linkLabelScanCriticalLastScanValue.Text = "Never";
            }

        }
        /// <summary>
        /// Load quarantine Statistics from database
        /// </summary>
        private void InitQuarantineStatisticsGroupBox()
        {
            string command = "SELECT * FROM QuarantineItems;";            

            ArrayList list;
            ClamWinDatabase.ExecReader(command, out list);

            if (list.Count != 0)
            {
                int FilesQuarantined = list.Count / ClamWinDatabase.QuarantineItemsFPR;
                linkLabelFilesQuarantinedValue.Text = FilesQuarantined.ToString();


                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(ClamWinQuarantine.GetQuarantineFolder());
                System.IO.FileInfo[] files;

                try
                {
                    files = di.GetFiles("*.*");
                    long SizeTotal = 0;
                    
                    foreach( System.IO.FileInfo fi in files)
                    {
                        if (!fi.Exists)
                        {
                            continue;
                        }

                        SizeTotal += fi.Length;
                    }

                    if (SizeTotal < 1024 * 1024)
                    {
                        float helper = (float)(SizeTotal / 1024.0);
                        linkLabelQuarantineTotalSizeValue.Text = helper.ToString("F") + " Kb";
                    }
                    else
                    {
                        float helper = (float)(SizeTotal / (1024.0*1024.0));
                        linkLabelQuarantineTotalSizeValue.Text = helper.ToString("F") + " Mb";
                    }                                        

                }
                catch
                { 
                    linkLabelQuarantineTotalSizeValue.Text = "0 Mb";
                }

                command = "SELECT * FROM QuarantineItems WHERE QuarantineTime=(SELECT max(QuarantineTime) FROM QuarantineItems);";                
                ClamWinDatabase.ExecReader(command, out list);
                if (list.Count != 0)
                {
                    DateTime time = DateTime.FromBinary(long.Parse((string)list[3]));
                    linkLabelLastFileQuarantinedValue.Text = time.ToShortDateString() + " " + time.ToShortTimeString();
                }
                else
                {
                    linkLabelLastFileQuarantinedValue.Text = "Never";
                }
            }
            else
            {
                linkLabelFilesQuarantinedValue.Text = "0";
                linkLabelQuarantineTotalSizeValue.Text = "0 Mb";
                linkLabelLastFileQuarantinedValue.Text = "Never";
            }

        }
        /// <summary>
        /// Load quarantine Statistics from database
        /// </summary>
        private void InitDiskSpaceGroupBox()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(ClamWinQuarantine.GetQuarantineFolder());
            System.IO.FileInfo[] files;

            try
            {
                files = di.GetFiles("*.*");
                long SizeTotal = 0;

                foreach (System.IO.FileInfo fi in files)
                {
                    if (!fi.Exists)
                    {
                        continue;
                    }

                    SizeTotal += fi.Length;
                }
                
                if( SizeTotal < 1024*1024 )
                {
                    float helper = (float)(SizeTotal / 1024.0);
                    linkLabelQuarantineSizeValue.Text = helper.ToString("F") + " Kb";
                }
                else
                {
                    float helper = (float)(SizeTotal / (1024.0*1024.0));                    
                    linkLabelQuarantineSizeValue.Text = helper.ToString("F") + " Mb";
                }                
            }
            catch
            {
                linkLabelQuarantineSizeValue.Text = "0.0 Mb";
            }            

            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(clamWinPath + "\\Data\\clamwin.db");

                if (fi.Length < 1024 * 1024)
                {
                    float size = (float)fi.Length;
                    float helper = (float)(size / 1024.0);
                    linkLabelDataBaseSizeValue.Text = helper.ToString("F") + " Kb";
                }
                else
                {
                    float size = (float)fi.Length;
                    float helper = (float)(size / (1024.0 * 1024.0));
                    linkLabelDataBaseSizeValue.Text = helper.ToString("F") + " Mb";
                }                   
            }
            catch
            {
                linkLabelDataBaseSizeValue.Text = "0.0 Mb";
            }
        }
        /// <summary>
        /// Save scan items to database
        /// </summary>
        /// <summary>
        /// Save scan items to database
        /// </summary>
        private void SaveScanItemsToDatabase()
        {
            string command = "DELETE FROM ScanItems;";
            int result;
            ClamWinDatabase.ExecCommand(command, out result);

            for (int j = 0; j < 3; j++)
            {
                ListView listView;
                string ListID = "";
                if (j == 0)
                {
                    listView = listViewSCItems;
                    ListID = "Scan";
                }
                else if (j == 1)
                {
                    listView = listViewSCMyPCItems;
                    ListID = "ScanMyPC";
                }
                else
                {
                    listView = listViewSCCriticalItems;
                    ListID = "ScanCritical";
                }

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    string Item = listView.Items[i].Text;
                    ItemData data = (ItemData)listView.Items[i].Tag;

                    string Path;
                    if (data.System || data.Drive)
                    {
                        Path = data.Path;
                    }
                    else
                    {
                        Path = "0";
                    }
                    string IsChecked = listView.Items[i].Checked ? "1" : "0";
                    string IsSystem = data.System ? "1" : "0";
                    string IsFolder = data.Folder ? "1" : "0";
                    string IsDrive = data.Drive ? "1" : "0";

                    Item = Item.Replace("'", "''");
                    Path = Path.Replace("'", "''");

                    command = "INSERT INTO ScanItems(Item,Path,IsChecked,IsSystem,IsFolder,IsDrive,ListID) VALUES(";
                    command += "'" + Item + "',";
                    command += "'" + Path + "',";
                    command += "'" + IsChecked + "',";
                    command += "'" + IsSystem + "',";
                    command += "'" + IsFolder + "',";
                    command += "'" + IsDrive + "',";
                    command += "'" + ListID + "');";
                    ClamWinDatabase.ExecCommand(command, out result);
                }
            }
        }
        /// <summary>
        /// Initialize controls texts        
        /// </summary>
        private void InitControlsTexts()
        {
            // Non-control texts intialization 
            ResourceManager globalResources = new ResourceManager("ClamWinApp.ClamWinGlobal", typeof(ClamWinMainForm).Assembly);
            this.Text = globalResources.GetString("ClamWinMainForm.Text", Resources.Culture);

            // Control string intialization
            ResourceManager controlResources = new ResourceManager(typeof(ClamWinMainForm));
            /*this.menuItemFile.Text = controlResources.GetString("menuItemFile.Text", Resources.Culture);
            this.menuItemScan.Text = controlResources.GetString("menuItemScan.Text", Resources.Culture);
            this.menuItemExit.Text = controlResources.GetString("menuItemExit.Text", Resources.Culture);
            
            this.menuItemTools.Text = controlResources.GetString("menuItemTools.Text", Resources.Culture);
            this.menuItemPreferences.Text = controlResources.GetString("menuItemPreferences.Text", Resources.Culture);
            this.menuItemVDBUpdate.Text = controlResources.GetString("menuItemVDBUpdate.Text", Resources.Culture);
            this.menuItemDisplayReports.Text = controlResources.GetString("menuItemDisplayReports.Text", Resources.Culture);
            this.menuItemVDBUpdateReport.Text = controlResources.GetString("menuItemVDBUpdateReport.Text", Resources.Culture);
            this.menuItemScanReport.Text = controlResources.GetString("menuItemScanReport.Text", Resources.Culture);

            this.menuItemHelp.Text = controlResources.GetString("menuItemHelp.Text", Resources.Culture);
            this.menuItemIndex.Text = controlResources.GetString("menuItemIndex.Text", Resources.Culture);
            this.menuItemCheckVersion.Text = controlResources.GetString("menuItemCheckVersion.Text", Resources.Culture);
            this.menuItemWebsite.Text = controlResources.GetString("menuItemWebsite.Text", Resources.Culture);
            this.menuItemFAQ.Text = controlResources.GetString("menuItemFAQ.Text", Resources.Culture);
            this.menuItemAbout.Text = controlResources.GetString("menuItemAbout.Text", Resources.Culture);*/
        }
        /// <summary>
        /// Hides all right panel sub-panels         
        /// </summary>
        private void HideRightPanelSubPanels()
        {
            foreach (Control control in panelExRightPanel.Controls)
            {
                if (control is Panel &&
                     !(control is PanelsEx.PanelEx) &&
                     !(control is PanelsEx.PanelExGroup)
                   )
                {
                    control.Visible = false;
                }
            }
        }
        /// <summary>
        /// Fill given list with available logical volumes
        /// </summary>
        /// <param name="list">List to fill with</param>
        private void FillDrivesList(ref ListView list)
        {
            foreach (ListViewItem item in list.Items)
            {
                ItemData data = (ItemData)item.Tag;
                if (data.System)
                {
                    list.Items.Remove(item);
                }
            }
            list.Columns.Clear();
            list.View = View.Details;
            list.CheckBoxes = true;
            list.HeaderStyle = ColumnHeaderStyle.None;                        
            list.FullRowSelect = true;
            list.HideSelection = false;
            list.ShowItemToolTips = true;
            list.Columns.Add("Item", 170);
           
            ManagementObjectCollection collection = driveSearcher.Get();
            foreach (ManagementObject drive in collection)
            {
                string driveName = drive["Name"].ToString();                
                driveName += "\\";

                // Prepare flags:                
                Win32API.Shell32.SHGetFileInfoConstants dwFlags =
                    Win32API.Shell32.SHGetFileInfoConstants.SHGFI_ICON |
                    Win32API.Shell32.SHGetFileInfoConstants.SHGFI_SMALLICON;
                
                // Get image list
                Win32API.Shell32.SHFILEINFO shfi = new Win32API.Shell32.SHFILEINFO();
                uint shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

                Win32API.Shell32.SHGetFileInfo(driveName,
                                               0,
                                               ref shfi,
                                               shfiSize,
                                               (uint)dwFlags);

                ListViewItem item;
                if (shfi.hIcon != (IntPtr)0)
                {
                    Icon myIcon = Icon.FromHandle(shfi.hIcon);
                    imageFilesList.Images.Add(myIcon);

                    item = list.Items.Add(GetDriveString(drive), imageFilesList.Images.Count - 1);
                }
                else
                {
                    item = list.Items.Add(GetDriveString(drive));
                }

                item.Checked = true;
                item.Tag = new ItemData(true,true,false,driveName);
            }
            collection.Dispose();
        }
        /// <summary>
        /// Init ScanMyPC items
        /// </summary>
        private void FillScanMyPCItems()
        {
            listViewSCMyPCItems.Columns.Clear();
            listViewSCMyPCItems.View = View.Details;
            listViewSCMyPCItems.CheckBoxes = true;
            listViewSCMyPCItems.HeaderStyle = ColumnHeaderStyle.None;
            listViewSCMyPCItems.FullRowSelect = true;
            listViewSCMyPCItems.HideSelection = false;
            listViewSCMyPCItems.ShowItemToolTips = true;
            listViewSCMyPCItems.Columns.Add("Item", 170);

            ListViewItem item = listViewSCMyPCItems.Items.Add("All Hard Drives");
            item.Tag = new ItemData(true, false, false, "All Hard Drives");
            item.Checked = true;
            item = listViewSCMyPCItems.Items.Add("All Removable Drives");
            item.Tag = new ItemData(true, false, false, "All Removable Drives");
            item.Checked = true;
            item = listViewSCMyPCItems.Items.Add("All Network Drives");
            item.Tag = new ItemData(true, false, false, "All Network Drives");
            item.Checked = true;
        }
        /// <summary>
        /// Init ScanCritical items
        /// </summary>
        private void FillScanCriticalItems()
        {
            listViewSCCriticalItems.Columns.Clear();
            listViewSCCriticalItems.View = View.Details;
            listViewSCCriticalItems.CheckBoxes = true;
            listViewSCCriticalItems.HeaderStyle = ColumnHeaderStyle.None;
            listViewSCCriticalItems.FullRowSelect = true;
            listViewSCCriticalItems.HideSelection = false;
            listViewSCCriticalItems.ShowItemToolTips = true;
            listViewSCCriticalItems.Columns.Add("Item", 170);

            string Windows = Environment.GetEnvironmentVariable("WINDIR");

            // Prepare flags:                
            Win32API.Shell32.SHGetFileInfoConstants dwFlags =
                Win32API.Shell32.SHGetFileInfoConstants.SHGFI_ICON |
                Win32API.Shell32.SHGetFileInfoConstants.SHGFI_SMALLICON;

            // Get image list
            Win32API.Shell32.SHFILEINFO shfi = new Win32API.Shell32.SHFILEINFO();
            uint shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

            Win32API.Shell32.SHGetFileInfo(Windows,
                                           0,
                                           ref shfi,
                                           shfiSize,
                                           (uint)dwFlags);

            ListViewItem item;
            if (shfi.hIcon != (IntPtr)0)
            {
                Icon myIcon = Icon.FromHandle(shfi.hIcon);
                imageFilesList.Images.Add(myIcon);

                item = listViewSCCriticalItems.Items.Add(Windows, imageFilesList.Images.Count - 1);
            }
            else
            {
                item = listViewSCCriticalItems.Items.Add(Windows);
            }            
            item.Tag = new ItemData(true, false, true, Windows);
            item.Checked = true;

            string System = Environment.SystemDirectory;

            // Prepare flags:                
            dwFlags = Win32API.Shell32.SHGetFileInfoConstants.SHGFI_ICON |
                        Win32API.Shell32.SHGetFileInfoConstants.SHGFI_SMALLICON;

            // Get image list           
            shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

            Win32API.Shell32.SHGetFileInfo(System,
                                           0,
                                           ref shfi,
                                           shfiSize,
                                           (uint)dwFlags);
            
            if (shfi.hIcon != (IntPtr)0)
            {
                Icon myIcon = Icon.FromHandle(shfi.hIcon);
                imageFilesList.Images.Add(myIcon);

                item = listViewSCCriticalItems.Items.Add(System, imageFilesList.Images.Count - 1);
            }
            else
            {
                item = listViewSCCriticalItems.Items.Add(System);
            }                        
            item.Tag = new ItemData(true, false, true, System);
            item.Checked = true;
        }
        /// <summary>
        /// Load scan items from database
        /// </summary>
        private void LoadScanItems()
        {
            FillDrivesList(ref listViewSCItems);

            FillScanCriticalItems();

            FillScanMyPCItems();

            ArrayList list;
            if (ClamWinDatabase.ExecReader("SELECT * FROM ScanItems;", out list))
            {
                const int FieldsPerRecord = ClamWinDatabase.ScanItemsFPR;

                int RecordsCount = list.Count / FieldsPerRecord;

                for (int i = (RecordsCount-1); i >=0 ; i--)
                {
                    string Helper;
                    // [Item] 
                    string Item = (string)list[i * FieldsPerRecord + 1];
                    // [Path] 
                    string Path = (string)list[i * FieldsPerRecord + 2];
                    // [IsChecked] 
                    Helper = (string)list[i * FieldsPerRecord + 3];
                    bool IsChecked = bool.Parse(Helper);
                    // [IsSystem] 
                    Helper = (string)list[i * FieldsPerRecord + 4];
                    bool IsSystem = bool.Parse(Helper);
                    // [IsFolder] 
                    Helper = (string)list[i * FieldsPerRecord + 5];
                    bool IsFolder = bool.Parse(Helper);
                    // [IsDrive]
                    Helper = (string)list[i * FieldsPerRecord + 6];
                    bool IsDrive = bool.Parse(Helper);
                    // [ListID]
                    string ListID = (string)list[i * FieldsPerRecord + 7];

                    if (!IsSystem && !IsDrive)
                    {
                        Path = Item;
                    }

                    ListView listView;
                    switch (ListID)
                    { 
                        case "Scan":
                            listView = listViewSCItems;
                            break;
                        case "ScanMyPC":
                            listView = listViewSCMyPCItems;
                            break;
                        case "ScanCritical":
                            listView = listViewSCCriticalItems;
                            break;
                        default:
                            listView = listViewSCItems;
                            break;
                    }


                    bool Match = false;

                    for (int j = 0; j < listView.Items.Count; j++)
                    {
                        ItemData data = (ItemData)listView.Items[j].Tag;
                        string Text = listView.Items[j].Text;

                        if (Item == Text || data.Path == Path)
                        {
                            data.Drive = IsDrive;
                            data.Folder = IsFolder;
                            data.System = IsSystem;
                            listView.Items[j].Checked = IsChecked;
                            Match = true;
                            break;
                        }
                    }

                    if (!Match && !IsSystem && !IsDrive)
                    {    
                        // Insert saved item
                        Win32API.Shell32.SHGetFileInfoConstants dwFlags =
                        Win32API.Shell32.SHGetFileInfoConstants.SHGFI_ICON |
                        Win32API.Shell32.SHGetFileInfoConstants.SHGFI_SMALLICON;


                        Win32API.Shell32.SHFILEINFO shfi = new Win32API.Shell32.SHFILEINFO();
                        uint shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

                        if (!IsFolder)
                        {
                            System.IO.FileInfo fi = new System.IO.FileInfo(Path);

                            if (!fi.Exists)
                            {
                                // File does not exist, skip it
                                continue;
                            }
                        }
                        else
                        {
                            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Path);
                            if (!di.Exists)
                            { 
                                // Directory does not exist, skip it
                                continue;
                            }
                        }

                        Win32API.Shell32.SHGetFileInfo(Path, 0, ref shfi, shfiSize, (uint)dwFlags);

                        ListViewItem item;

                        if (shfi.hIcon != (IntPtr)0)
                        {
                            Icon myIcon = Icon.FromHandle(shfi.hIcon);
                            imageFilesList.Images.Add(myIcon);
                            item = listView.Items.Insert(0,Item, imageFilesList.Images.Count - 1);
                        }
                        else
                        {
                            item = listView.Items.Insert(0,Item);
                        }
                        

                        item.Checked = IsChecked;

                        item.Tag = new ItemData(IsSystem, IsDrive, IsFolder, Path);
                    }
                }
            }

        }
        /// <summary>
        /// Return description string for specified drive
        /// </summary>
        /// <param name="drive">Managment object which represent logical disk</param>
        /// <returns>Description string for drive</returns>
        private string GetDriveString(ManagementObject drive)
        {
            //const int Unknown = 0;
            //const int NoRootDirectory = 1;
            const int RemovableDisk = 2;
            const int LocalDisk = 3;
            const int NetworkDrive = 4;
            const int CompactDisc = 5;
            //const int RAMDisk = 6;

            int type = int.Parse(drive["DriveType"].ToString());
            string name = (string)drive["Name"];
            string volume = (string)drive["VolumeName"];
            string description = (string)drive["Description"];
            string provider = (string)drive["ProviderName"];
            string server = "";
            string share = "";
            if (provider != null)
            {                
                int pos = provider.IndexOf("\\\\", 0);
                if (pos != -1)
                {
                    pos += 2;
                }
                int pos1 = provider.IndexOf("\\", pos);

                if (pos != -1 && pos1 != -1)
                {
                    string upper;
                    server = provider.Substring(pos, pos1 - pos);
                    server = server.ToLower();
                    upper  = server.Substring(0, 1);
                    server = server.Remove(0, 1);
                    upper  = upper.ToUpper();
                    server = server.Insert(0, upper);

                    share = provider.Substring(pos1+1);
                    share = share.ToLower();
                    upper = share.Substring(0, 1);
                    share = share.Remove(0, 1);
                    upper = upper.ToUpper();
                    share = share.Insert(0, upper);
                }
            }
            
            string helper = "";
            switch (type)
            {
                case RemovableDisk:
                    int pos = description.IndexOf("3 1/2 Inch Floppy Drive",0);
                    if (pos != -1)
                    {
                        helper = "3.5 Floppy (" + name + ")";
                    }
                    else
                    {
                        helper = name;
                    }
                break;
                case LocalDisk:
                    if (volume != "")
                    {
                        helper = volume+" ("+name+")";
                    }
                    else
                    {
                        helper = "LocalDisk (" + name + ")";
                    }
                break;
                case NetworkDrive:
                    helper = share + " on '"+server+"' ("+name+")";
                break;
                case CompactDisc:
                    helper = helper = "CD-ROM (" + name + ")";
                break;                
                default:
                    helper = name;
                break;
            }
            return helper;
        }
        /// <summary>
        /// Return scan items count conteined in specified special item
        /// </summary>
        /// <param name="special"></param>
        /// <returns></returns>
        private int GetSpecialItemsCount(string special)
        {
            int count = 0;
            ManagementObjectCollection collection = driveSearcher.Get();
            foreach (ManagementObject drive in collection)
            {
                string type = drive["DriveType"].ToString();

                if ((type == "3") && special == "All Hard Drives")
                {
                    count++;
                }
                else if ((type == "2" || type == "5") && special == "All Removable Drives")
                {
                    count++;
                }
                else if (type == "4" && special == "All Network Drives")
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// If item is special
        /// </summary>
        /// <param name="special"></param>
        /// <returns></returns>
        private bool IsSpecialItem(string item)
        {
            switch (item)
            {
                case "All Hard Drives":
                    return true;
                case "All Removable Drives":
                    return true;
                case "All Network Drives":
                    return true;
                default:
                    return false;
            }           
        }
        /// <summary>
        /// Return string which contains valid types for item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsTypeValidForItem(string item, string type)
        {
            switch( item)
            {
                case "All Hard Drives":
                    if (type == "3")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "All Removable Drives":
                    if (type == "2" || type == "5")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }                    
                case "All Network Drives":
                    if (type == "4")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }           
        }
        /// <summary>
        /// Common start scan delegate function
        /// </summary>        
        private void OnStartScan(ScanPanelIDs ID)
        {
            if (ClamWinScanner.IsScanSuspended((int)ID))
            {
                ClamWinScanner.ResumeScan((int)ID);
                return;
            }

            if (ClamWinScanner.IsScanRunning((int)ID))
            {
                foreach (ClamWinScanForm form in ScanForms)
                {
                    if (form.GetPanelID() == ID)
                    {
                        form.ShowScanForm();
                        break;
                    }
                }
                return;
            }

            ClamWinScanner.ScannerItem[] items;

            if (ID != ScanPanelIDs.Quarantine)
            {
                //Regular scans
                int numItems = 0;
                foreach (ListViewItem item in listViewCurrent.Items)
                {
                    if (item.Checked)
                    {
                        ItemData data = (ItemData)item.Tag;

                        // Special items handling
                        if (IsSpecialItem(data.Path))
                        {
                            numItems += GetSpecialItemsCount(data.Path);
                        }
                        else
                        {
                            numItems++;
                        }
                    }
                }
                if (numItems == 0)
                {
                    return;
                }

                items = new ClamWinScanner.ScannerItem[numItems];
                int i = 0;
                foreach (ListViewItem li in listViewCurrent.Items)
                {
                    if (li.Checked)
                    {
                        ItemData data = (ItemData)li.Tag;

                        if (IsSpecialItem(data.Path))
                        {
                            ManagementObjectCollection collection = driveSearcher.Get();
                            foreach (ManagementObject drive in collection)
                            {
                                if (IsTypeValidForItem(data.Path, drive["DriveType"].ToString()))
                                {
                                    items[i].IsDrive = true;
                                    items[i].IsFolder = false;
                                    items[i].Path = drive["Name"].ToString() + "\\";
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            items[i].IsDrive = data.Drive;
                            items[i].IsFolder = data.Folder;
                            items[i].Path = data.Path;
                            i++;
                        }
                    }
                }
            }
            else
            { 
                //Quarantine rescan request
                items = new ClamWinScanner.ScannerItem[1];
                items[0].IsDrive = false;
                items[0].IsFolder = true;
                items[0].Path = ClamWinQuarantine.GetQuarantineTempFolder();
            }
           
            ClamWinScanForm Form = null;
            foreach( ClamWinScanForm form in ScanForms)
            {
                if (form.GetPanelID() == ID)
                {
                    Form = form;
                    break;
                }
            }
            if (Form == null)
            {
                Form = new ClamWinScanForm();
                ScanForms.Add(Form);
            }

            IntPtr[] Listeners = new IntPtr[2];

            Listeners[0] = Form.Handle;
            Listeners[1] = this.Handle;

            int Result = ClamWinScanner.StartScan(ref items, ref Listeners, (int)ID);

            if (Result == 1)
            {
                Form.ShowScanForm(ID, this.Handle);
            }
            else
            {
                Form.Close();
                ScanForms.Remove(Form);
            }
        }
        /// <summary>
        /// Add selected scan item to current scan list 
        /// </summary>
        private void AddScanItem()
        {
            FolderBrowser browser = new FolderBrowser();
            browser.SelectFiles = true;
            browser.ShowTextBox = true;
            browser.OnlySubfolders = true;

            if (browser.ShowDialog() == DialogResult.OK)
            {
                string name = browser.DirectoryPath;

                System.IO.FileInfo fi = new System.IO.FileInfo(name);
                if (!fi.Exists)
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(name);

                    if (di.Root.FullName != name )
                    {
                        name += "\\";
                    }
                }

                bool match = false;
                foreach (ListViewItem item in this.listViewCurrent.Items)
                {
                    ItemData data = (ItemData)item.Tag;
                    if (data.Path == name)
                    {
                        match = true;
                        item.Selected = true;
                        item.EnsureVisible();
                        break;
                    }
                }

                if (!match)
                {
                    // Prepare flags:                
                    Win32API.Shell32.SHGetFileInfoConstants dwFlags =
                        Win32API.Shell32.SHGetFileInfoConstants.SHGFI_ICON |
                        Win32API.Shell32.SHGetFileInfoConstants.SHGFI_SMALLICON;


                    Win32API.Shell32.SHFILEINFO shfi = new Win32API.Shell32.SHFILEINFO();
                    uint shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

                    Win32API.Shell32.SHGetFileInfo(name,
                                                   0,
                                                   ref shfi,
                                                   shfiSize,
                                                   (uint)dwFlags);

                    ListViewItem item;
                    if (shfi.hIcon != (IntPtr)0)
                    {
                        Icon myIcon = Icon.FromHandle(shfi.hIcon);
                        imageFilesList.Images.Add(myIcon);

                        item = listViewCurrent.Items.Insert(0, name, imageFilesList.Images.Count - 1);
                    }
                    else
                    {
                        item = listViewCurrent.Items.Insert(0, name);
                    }

                    item.Checked = true;
                    bool directory = false;
                    if (name[name.Length - 1] == '\\')
                    {
                        directory = true;
                    }
                    item.Tag = new ItemData(false, false, directory, name);
                    item.EnsureVisible();
                }
            }
        }
        /// <summary>
        /// Init labels and group boxe
        /// </summary>
        private void InitPanelsStuff()
        {
            linkLabelScanMyPCSettingsActionDescr.Text = ClamWinSettings.ActionIdToString(ClamWinSettings.ScanMyPCOnDetectAction);
            linkLabelScanSettingsActionDescr.Text = ClamWinSettings.ActionIdToString(ClamWinSettings.ScanOnDetectAction);
            linkLabelScanCriticalSettingsActionDescr.Text = ClamWinSettings.ActionIdToString(ClamWinSettings.ScanCriticalOnDetectAction);            
        }
        #endregion

        #region Event Handlers

        #region Form Event Handlers
        /// <summary>
        /// ClamWinMainForm on close event handler
        /// </summary>
        private void ClamWinMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            flagExit = true;
            if (!flagExit)
            {
                e.Cancel = true;
                formScanning.Visible = false;
                formSettings.Visible = false;
                this.Visible = false;
                return;
            }

            formQuarantine.EnableClose = true;

            if (ClamWinQuarantine.IsWorking())
            {
                DialogResult res =  MessageBox.Show( "Quarantine job is not finished yet!Do you want close ClamWin anyway?", 
                                                     "ClamWin", 
                                                     MessageBoxButtons.YesNo);

                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            SaveScanItemsToDatabase();

            // Alch 20070228 - temporarily disabled as service does not yet implement gui notify
            // ClamWinScanner.ManageGuiHandle(this.Handle, false);

            ClamWinQuarantine.Close();
            ClamWinScanner.Close();
            ClamWinStatistics.Close();
            ClamWinDatabase.Close();
        }
        /// <summary>
        /// Main application form initialization        
        /// </summary>        
        private void ClamWinMainForm_Load(object sender, EventArgs e)
        {
            // Alch 20070228 - temporarily disabled as service does not yet implement gui notify
            //ClamWinScanner.ManageGuiHandle(this.Handle, true);

            formQuarantine.MainFomrHandle = this.Handle;

            InitControlsTexts();           
            
            panelExScan.PanelState = PanelsEx.PanelState.Collapsed;
            panelExService.PanelState = PanelsEx.PanelState.Collapsed;
            panelExHome.PanelState = PanelsEx.PanelState.Collapsed;
            panelExQuarantine.PanelState = PanelsEx.PanelState.Collapsed;

            listViewCurrent = listViewSCItems;

            LoadScanItems();

            InitPanelsStuff();

            InitScanStatisticsGroupBox();

            InitScanMyPCStatisticsGroupBox();

            InitScanCriticalStatisticsGroupBox();

            InitQuarantineStatisticsGroupBox();

            InitHomePanelControls();

            InitNotifyPanelControls();

            InitDiskSpaceGroupBox();
        }
        #endregion 

        #region Menu Event Handlers
        /// <summary>
        /// Tray menu "Open" item event handler
        /// </summary>
        private void menuItemTrayOpen_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }
        /// <summary>
        /// Tray menu "Exit" item event handler
        /// </summary>
        private void menuItemTrayExit_Click(object sender, EventArgs e)
        {
            flagExit = true;
            
            formScanning.EnableClose();
            formScanning.Close();
          
            Close();
        }
        #endregion

        #region Tray Event Handlers 
        /// <summary>
        /// Tray mouse click event handler
        /// </summary>
        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {                
                this.BringToFront();
                this.WindowState = FormWindowState.Normal;
                this.Show();                
            }
        }
        #endregion

        #region PanelsEx Event Handlers 
        /// <summary>
        /// Hides all right panel sub-panels         
        /// </summary>
        private void panelExProtection_TitlePressed(object sender, PanelsEx.PanelEventArgs e)
        {            
            /*panelExProtection.PanelState = PanelsEx.PanelState.Expanded;
            panelExScan.PanelState = PanelsEx.PanelState.Collapsed;
            panelExService.PanelState = PanelsEx.PanelState.Collapsed;
            panelExHome.PanelState = PanelsEx.PanelState.Collapsed;
            panelExRightPanel.TitleText = panelExProtection.TitleText + ":";

            HideRightPanelSubPanels();

            this.panelProtection.Visible = true;*/
        }
        /// <summary>
        /// panelExScan on-title click event handler
        /// </summary>
        private void panelExScan_TitlePressed(object sender, PanelsEx.PanelEventArgs e)
        {
            panelExScan.PanelState = PanelsEx.PanelState.Expanded;            
            panelExService.PanelState = PanelsEx.PanelState.Collapsed;
            panelExHome.PanelState = PanelsEx.PanelState.Collapsed;
            panelExQuarantine.PanelState = PanelsEx.PanelState.Collapsed;
            panelExRightPanel.TitleText = panelExScan.TitleText + ":";

            HideRightPanelSubPanels();

            this.panelScan.Visible = true;            
        }
        /// <summary>
        /// panelExService on-title click event handler
        /// </summary>
        private void panelExService_TitlePressed(object sender, PanelsEx.PanelEventArgs e)
        {
            panelExService.PanelState = PanelsEx.PanelState.Expanded;
            panelExScan.PanelState = PanelsEx.PanelState.Collapsed;            
            panelExHome.PanelState = PanelsEx.PanelState.Collapsed;
            panelExQuarantine.PanelState = PanelsEx.PanelState.Collapsed;
            panelExRightPanel.TitleText = panelExService.TitleText+":";

            HideRightPanelSubPanels();

            panelService.Visible = true;
        }
        /// <summary>
        /// panelExHome on-title click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelExHome_TitlePressed(object sender, PanelsEx.PanelEventArgs e)
        {
            panelExHome.PanelState = PanelsEx.PanelState.Collapsed;
            panelExQuarantine.PanelState = PanelsEx.PanelState.Collapsed;
            panelExScan.PanelState = PanelsEx.PanelState.Collapsed;            
            panelExService.PanelState = PanelsEx.PanelState.Collapsed;
            panelExRightPanel.TitleText = panelExHome.TitleText + ":";

            HideRightPanelSubPanels();

            panelHome.Visible = true;
        }
        /// <summary>
        /// panelExQuarantine on-title click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelExQuarantine_TitlePressed(object sender, PanelsEx.PanelEventArgs e)
        {
            panelExHome.PanelState = PanelsEx.PanelState.Collapsed;
            panelExQuarantine.PanelState = PanelsEx.PanelState.Collapsed;
            panelExScan.PanelState = PanelsEx.PanelState.Collapsed;
            panelExService.PanelState = PanelsEx.PanelState.Collapsed;

            panelExRightPanel.TitleText = panelExQuarantine.TitleText + ":";

            HideRightPanelSubPanels();

            panelQuarantine.Visible = true;
        }
        /// <summary>
        /// panelScan VisibleChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelScan_VisibleChanged(object sender, EventArgs e)
        {
            if (panelScan.Visible)
            {
                CurrentScanPanel = ScanPanelIDs.Scan;
                listViewCurrent = listViewSCItems;                
            }
        }
        /// <summary>
        /// panelScanCritical VisibleChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelScanCritical_VisibleChanged(object sender, EventArgs e)
        {
            if (panelScanCritical.Visible)
            {
                CurrentScanPanel = ScanPanelIDs.ScanCritical;
                listViewCurrent = listViewSCCriticalItems;
            }
        }
        /// <summary>
        /// panelScanMyPC VisibleChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelScanMyPC_VisibleChanged(object sender, EventArgs e)
        {
            if (panelScanMyPC.Visible)
            {
                CurrentScanPanel = ScanPanelIDs.ScanMyPC;
                listViewCurrent = listViewSCMyPCItems;               
            }
        }
        #endregion

        #region Links Event Handlers        
        /// <summary>
        /// linkScanMyPC on-click event handler
        /// </summary>
        private void linkScanMyPC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelExRightPanel.TitleText = linkScanMyPC.Text + ":";

            HideRightPanelSubPanels();

            panelScanMyPC.Visible = true;            
        }
        /// <summary>
        /// linkUpdate on-click event handler
        /// </summary>
        private void linkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelExRightPanel.TitleText = linkUpdate.Text + ":";

            HideRightPanelSubPanels();

            panelUpdate.Visible = true;
        }
        /// <summary>
        /// linkDatafiles on-click event handler
        /// </summary>
        private void linkDatafiles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelExRightPanel.TitleText = linkDatafiles.Text + ":";

            HideRightPanelSubPanels();

            panelDataFiles.Visible = true;
        }               
        /// <summary>
        /// linkProtection on-click event handler
        /// </summary>
        private void linkProtection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelExRightPanel.TitleText = linkProtection.Text + ":";

            HideRightPanelSubPanels();

            panelProtection.Visible = true;
        }
        /// <summary>
        /// linkScanCritical on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkScanCritical_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelExRightPanel.TitleText = linkScanCritical.Text + ":";

            HideRightPanelSubPanels();

            panelScanCritical.Visible = true;           
        }
        /// <summary>
        /// linkSettings on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (formSettings.IsDisposed)
            {
                formSettings = new ClamWinSettingsForm();
            }

            formSettings.Show();
        }
        /// <summary>
        /// inkLabelProtectionSettings on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelProtectionSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (formSettings.IsDisposed)
            {
                formSettings = new ClamWinSettingsForm();
            }

            formSettings.Show();
            formSettings.SelectSection("NodeProtection");
        }
        /// <summary>
        /// linkLabelNotifyNext on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelNotifyNext_Click(object sender, EventArgs e)
        {
            ClamWinMainFormNotifications.Next();
        }
        /// <summary>
        /// linkLabelNotifyBack on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelNotifyBack_Click(object sender, EventArgs e)
        {
            ClamWinMainFormNotifications.Previous();
        }
        /// <summary>
        /// inkLabelNotify on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelNotify_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if( linkLabelNotify.Tag != null)
            {
                ClamWinMainFormNotifications.NotificationData Data = (ClamWinMainFormNotifications.NotificationData)linkLabelNotify.Tag;
            }
        }
        #endregion

        #region User Event Handlers
        /// <summary>
        /// Handle logical drives changes
        /// </summary>        
        private void OnLogicalDrivesChanged(object sender,EventArgs e)
        {            
            FillDrivesList(ref this.listViewSCItems);
        }
        /// <summary>
        /// Handle settings changes
        /// </summary>
        private void OnSettingChanged(ClamWinSettings.SettingIDs id)
        {
            switch (id)
            {
                case ClamWinSettings.SettingIDs.ScanMyPCOnDetectAction:
                {
                    linkLabelScanMyPCSettingsActionDescr.Text = ClamWinSettings.ActionIdToString(ClamWinSettings.ScanMyPCOnDetectAction);
                    break;
                }
                case ClamWinSettings.SettingIDs.ScanOnDetectAction:
                {
                    linkLabelScanSettingsActionDescr.Text = ClamWinSettings.ActionIdToString(ClamWinSettings.ScanOnDetectAction);
                    break;
                }
                case ClamWinSettings.SettingIDs.ScanCriticalOnDetectAction:
                {
                    linkLabelScanCriticalSettingsActionDescr.Text = ClamWinSettings.ActionIdToString(ClamWinSettings.ScanCriticalOnDetectAction);
                    break;
                }
                case ClamWinSettings.SettingIDs.OnAccessScannerStatus:
                {
                    InitProtectionPanelControls();
                    break;
                }
                case ClamWinSettings.SettingIDs.FileAntiVirusOnDetectAction:
                {                    
                    break;
                }
                case ClamWinSettings.SettingIDs.UpdateRunMode:
                {                   
                    if( ClamWinSettings.UpdateRunMode == ClamWinSettings.RunModes.Auto )
                    {
                        linkLabelUpdateMethodValue.Text = "Auto";   
                    }
                    else if (ClamWinSettings.UpdateRunMode == ClamWinSettings.RunModes.Manual)
                    {
                        linkLabelUpdateMethodValue.Text = "Manual";
                    }
                    else
                    {
                        linkLabelUpdateMethodValue.Text = ClamWinSettings.UpdateScheduleData.GetDescription();
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// Item scan complete
        /// </summary>
        private void OnItemScanComplete(IntPtr data)
        {
            ClamWinScan.NotifyData Data = (ClamWinScan.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinScan.NotifyData));
            Marshal.FreeHGlobal(data);
            switch ((ScanPanelIDs)Data.ScanID)
            { 
                case ScanPanelIDs.Scan:
                    linkLabelScanScannedValue.Text = Data.Statistics[0].Scanned.ToString();
                    linkLabelScanThreatsValue.Text = Data.Statistics[0].Infected.ToString();
                    progressBarScan.Value = (int)(Data.Size / 1024);
                    break;
                case ScanPanelIDs.ScanMyPC:
                    linkLabelScanMyPCScannedValue.Text = Data.Statistics[0].Scanned.ToString();
                    linkLabelScanMyPCThreatsValue.Text = Data.Statistics[0].Infected.ToString();
                    progressBarScanMyPC.Value = (int)(Data.Size / 1024);
                    break;
                case ScanPanelIDs.ScanCritical:
                    linkLabelScanCriticalScannedValue.Text = Data.Statistics[0].Scanned.ToString();
                    linkLabelScanCriticalThreatsValue.Text = Data.Statistics[0].Infected.ToString();
                    progressBarScanCritical.Value = (int)(Data.Size / 1024);
                    break;            
            }
        }
        /// <summary>
        /// Scan started
        /// </summary>
        /// <param name="size"></param>
        private void OnScanStart(IntPtr data)
        {
            ClamWinScan.NotifyData Data = (ClamWinScan.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinScan.NotifyData));
            Marshal.FreeHGlobal(data);

            switch ((ScanPanelIDs)Data.ScanID)
            {
                case ScanPanelIDs.Scan:
                    linkLabelScanLastScan.Visible = false;
                    linkLabelScanLastScanValue.Visible = false;
                    progressBarScan.Visible = true;
                    progressBarScan.Maximum = (int)(Data.Size / 1024);
                    progressBarScan.Value = 0;
                    break;
                case ScanPanelIDs.ScanMyPC:
                    linkLabelScanMyPCLastScan.Visible = false;
                    linkLabelScanMyPCLastScanValue.Visible = false;
                    progressBarScanMyPC.Visible = true;
                    progressBarScanMyPC.Maximum = (int)(Data.Size / 1024);
                    progressBarScanMyPC.Value = 0;
                    break;
                case ScanPanelIDs.ScanCritical:
                    linkLabelScanCriticalLastScan.Visible = false;
                    linkLabelScanCriticalLastScanValue.Visible = false;
                    progressBarScanCritical.Visible = true;
                    progressBarScanCritical.Maximum = (int)(Data.Size / 1024);
                    progressBarScanCritical.Value = 0;
                    break;
            }
        }
        /// <summary>
        /// Scan done
        /// </summary>
        private void OnScanDone(IntPtr data)
        {
            ClamWinScan.NotifyData Data = (ClamWinScan.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinScan.NotifyData));
            Marshal.FreeHGlobal(data);

            switch ((ScanPanelIDs)Data.ScanID)
            {
                case ScanPanelIDs.Scan:
                    linkLabelScanLastScan.Visible = true;
                    linkLabelScanLastScanValue.Visible = true;
                    progressBarScan.Visible = false;
                    InitScanStatisticsGroupBox();
                    break;
                case ScanPanelIDs.ScanMyPC:
                    linkLabelScanMyPCLastScan.Visible = true;
                    linkLabelScanMyPCLastScanValue.Visible = true;
                    progressBarScanMyPC.Visible = false;
                    InitScanMyPCStatisticsGroupBox();
                    break;
                case ScanPanelIDs.ScanCritical:
                    linkLabelScanCriticalLastScan.Visible = true;
                    linkLabelScanCriticalLastScanValue.Visible = true;
                    progressBarScanCritical.Visible = false;
                    InitScanCriticalStatisticsGroupBox();
                    break;
            }
        }
        /// <summary>
        /// Scheduled Update request handler
        /// </summary>
        private void OnScheduledUpdate()
        { 
        
        }
        /// <summary>
        /// Scheduled Scan request handler
        /// </summary>
        private void OnScheduledScan()
        {

        }
        /// <summary>
        /// Scheduled Scan Critical Areas request handler
        /// </summary>
        private void OnScheduledScanCritical()
        {

        }
        /// <summary>
        /// Scheduled Scan My Computer request handler
        /// </summary>
        private void OnScheduledScanMyPC()
        {

        }
        /// <summary>
        /// Quarantine changed
        /// </summary>
        /// <param name="?"></param>
        private void OnQuarantineChanged(IntPtr data)
        {
            ClamWinQuarantine.NotifyData Data = (ClamWinQuarantine.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinQuarantine.NotifyData));
            Marshal.FreeHGlobal(data);

            if (Data.Result == ClamWinQuarantine.QuarantineResults.Failed)
            {
                return;
            }

            InitQuarantineStatisticsGroupBox();

            InitDiskSpaceGroupBox();
        }
        /// <summary>
        /// Quarantine rescan request
        /// </summary>
        private void OnQuarantineRescan()
        {
            OnStartScan(ScanPanelIDs.Quarantine);
        }
        /// <summary>
        /// New Version Available
        /// </summary>
        /// <param name="data"></param>
        private void OnNewVersionAvailable(IntPtr data)
        {
            ClamWinVersion.NotifyData Data = (ClamWinVersion.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinVersion.NotifyData));
            Marshal.FreeHGlobal(data);

            ClamWinNewVersionNotifyForm form = new ClamWinNewVersionNotifyForm(Data.CurrentVersion,Data.Version);
            form.ShowNotify(ClamWinTrayNotifier.AppearingTimeInstant, 
                             ClamWinTrayNotifier.VisibleTimeInfinite,
                             ClamWinTrayNotifier.DisappearingTimeInstant);

            ClamWinMainFormNotifications.AddNotification("Test",
                                                         ClamWinMainFormNotifications.NotificationCodes.NewVersionAlert,
                                                         ClamWinMainFormNotifications.NotificationType.Info);

        }
        /// <summary>
        /// Current notification changed
        /// </summary>
        private void OnCurrentNotificationChanged()
        {
            ClamWinMainFormNotifications.NotificationPositions position = ClamWinMainFormNotifications.GetCurrentNotificationPos();
            
            linkLabelNotifyBack.Enabled = false;            
            linkLabelNotifyNext.Enabled = false;            

            if (position == ClamWinMainFormNotifications.NotificationPositions.Middle)
            {
                linkLabelNotifyBack.Enabled = true;
                linkLabelNotifyNext.Enabled = true;                
            }            
            else if (position == ClamWinMainFormNotifications.NotificationPositions.Left)
            {
                linkLabelNotifyBack.Enabled = false;
                linkLabelNotifyNext.Enabled = true;                
            }
            else if (position == ClamWinMainFormNotifications.NotificationPositions.Right)
            {
                linkLabelNotifyBack.Enabled = true;
                linkLabelNotifyNext.Enabled = false;                
            }
            else if (position == ClamWinMainFormNotifications.NotificationPositions.NotificationsEmpty)
            {
                linkLabelNotify.Text = "";
                linkLabelNotify.Tag = null;
                return;
            }
            


            ClamWinMainFormNotifications.NotificationData Data = new ClamWinMainFormNotifications.NotificationData();
            if (ClamWinMainFormNotifications.GetCurrentData(ref Data))
            {
                linkLabelNotify.Text = ClamWinMainFormNotifications.CodeToString(Data.Code);
                linkLabelNotify.Text += "\n";
                linkLabelNotify.Text += Data.Message;
                linkLabelNotify.Tag = Data;
            }
        }
        /// <summary>
        /// Filter jobs updated
        /// </summary>
        private void OnFilterJobsUpdated()
        {
            ClamWinScanner.AwakeFilterWorker();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void HandleFilterNotifyData(IntPtr data)
        {
            ClamWinScan.FilterNotifyData Data = (ClamWinScan.FilterNotifyData)Marshal.PtrToStructure(data, typeof(ClamWinScan.FilterNotifyData));
            Marshal.FreeHGlobal(data);

            FilterNotify.AddNotifyData(Data);
        }
        #endregion

        #region Buttons Event Handlers
        /// <summary>
        /// Remove button on-click event handler
        /// </summary>        
        private void buttonSCRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewCurrent.Items)
            {
                if (item.Selected)
                { 
                    ItemData data = (ItemData)item.Tag;
                    if (!data.System)
                    {
                        listViewSCItems.Items.Remove(item);
                    }
                }
            }
        }
        /// <summary>
        /// Add button on-click event handler
        /// </summary>        
        private void buttonSCAdd_Click(object sender, EventArgs e)
        {
            AddScanItem();   
        }
        /// <summary>
        /// Scan button on-click event handler
        /// </summary>        
        private void buttonSCScan_Click(object sender, EventArgs e)
        {            
            OnStartScan(CurrentScanPanel);
        }
        /// <summary>
        /// SCriticalAdd button on-click event handler
        /// </summary>        
        private void buttonSCriticalAdd_Click(object sender, EventArgs e)
        {
            AddScanItem();
        }
        /// <summary>
        /// SCMyPCAdd button on-click event handler
        /// </summary>
        private void buttonSCMyPCAdd_Click(object sender, EventArgs e)
        {
            AddScanItem();
        }
        /// <summary>
        /// SCMyPCRemove button on-click event handler
        /// </summary>
        private void buttonSCMyPCRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewCurrent.Items)
            {
                if (item.Selected)
                {
                    ItemData data = (ItemData)item.Tag;
                    if (!data.System)
                    {
                        listViewSCItems.Items.Remove(item);
                    }
                }
            }
        }
        /// <summary>
        /// SCriticalRemove button on-click event handler
        /// </summary>
        private void buttonSCriticalRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewCurrent.Items)
            {
                if (item.Selected)
                {
                    ItemData data = (ItemData)item.Tag;
                    if (!data.System)
                    {
                        listViewSCItems.Items.Remove(item);
                    }
                }
            }
        }
        /// <summary>
        /// SCriticalScan button on-click event handler
        /// </summary>
        private void buttonSCriticalScan_Click(object sender, EventArgs e)
        {
            OnStartScan(CurrentScanPanel);
        }
        /// <summary>
        /// SCMyPCScan button on-click event handler
        /// </summary>
        private void buttonSCMyPCScan_Click(object sender, EventArgs e)
        {
            OnStartScan(CurrentScanPanel);
        }
        /// <summary>
        /// buttonQuarantineManage button on-click event handler
        /// </summary>
        private void buttonQuarantineManage_Click(object sender, EventArgs e)
        {            
            formQuarantine.Show();
            Win32API.SetForegroundWindow(formQuarantine.Handle);
        }
        #endregion        

        #region GroupBox Events
        /// <summary>
        /// groupBoxSCMyPCSettings Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxSCMyPCSettings_Pressed(object sender)
        {
            if (formSettings.IsDisposed)
            {
                formSettings = new ClamWinSettingsForm();
            }

            formSettings.Show();
            formSettings.SelectSection("NodeScanMyComputer");
        }
        /// <summary>
        /// groupBoxSCSettings Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxSCSettings_Pressed(object sender)
        {
            if (formSettings.IsDisposed)
            {
                formSettings = new ClamWinSettingsForm();
            }

            formSettings.Show();
            formSettings.SelectSection("NodeScan");
        }
        /// <summary>
        /// groupBoxUpdateSettings Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxUpdateSettings_Pressed(object sender)
        {
            if (formSettings.IsDisposed)
            {
                formSettings = new ClamWinSettingsForm();
            }

            formSettings.Show();
            formSettings.SelectSection("NodeUpdate");
        }
        /// <summary>
        /// groupBoxSCCriticalSettings Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxSCCriticalSettings_Pressed(object sender)
        {
            if (formSettings.IsDisposed)
            {
                formSettings = new ClamWinSettingsForm();
            }

            formSettings.Show();
            formSettings.SelectSection("NodeScanCriticalAreas");
        }
        /// <summary>
        /// groupBoxFAStatus Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxFAStatus_Pressed(object sender)
        {

            if (formSettings.IsDisposed)
            {
                formSettings = new ClamWinSettingsForm();
            }

            formSettings.Show();
            formSettings.SelectSection("NodeFileAntivirus");
        }
        /// <summary>
        /// groupBoxSCMyPCStatistics Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxSCMyPCStatistics_Pressed(object sender)
        {
            formScanning.ShowScanForm(ScanPanelIDs.ScanMyPC, Handle);
        }
        /// <summary>
        /// groupBoxSCStatistics Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxSCStatistics_Pressed(object sender)
        {
            formScanning.ShowScanForm(ScanPanelIDs.Scan, Handle);
        }
        /// <summary>
        /// groupBoxSCCriticalStatistics Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxSCCriticalStatistics_Pressed(object sender)
        {
            formScanning.ShowScanForm(ScanPanelIDs.ScanCritical, Handle);
        }
        /// <summary>
        /// groupBoxQuarantineStatistics Pressed event handler
        /// </summary>
        /// <param name="sender"></param>
        private void groupBoxQuarantineStatistics_Pressed(object sender)
        {
            formQuarantine.Show();
            Win32API.SetForegroundWindow(formQuarantine.Handle);
        }
        #endregion

        #region Radio Button Event Handlers
        /// <summary>
        /// radioButtonOnAccessOn CheckedChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonOnAccessOn_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOnAccessOn.Checked)
            {
                ClamWinSettings.OnAccessScannerStatus = ClamWinSettings.OnAccessStatus.Enabled;
            }
            else if (radioButtonOnAccessOff.Checked)
            {
                ClamWinSettings.OnAccessScannerStatus = ClamWinSettings.OnAccessStatus.Disabled;
            }
            else
            {
                ClamWinSettings.OnAccessScannerStatus = ClamWinSettings.OnAccessStatus.Suspended;
            }
        }
        #endregion

        #endregion

        #region TEMP
        /// <summary>
        /// Saving code :) this should be removed to language selection form      
        /// </summary>
        private void LookForAvailableLanguages()
        {
            ArrayList listOfSupportedCultures = new ArrayList();
            Assembly asem = Assembly.GetExecutingAssembly();
            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                Assembly am = null;
                try
                {
                    am = asem.GetSatelliteAssembly(cultureInfo);
                    listOfSupportedCultures.Add(cultureInfo);
                }
                catch
                {
                }
            }
        }       
        #endregion               

        #region Windows Specific Stuff
        /// <summary>
        /// WndProc override to handle windows messages
        /// </summary>
        /// <param name="m">Message</param>        
        protected override void WndProc(ref Message m)
        {
            const int UM_HARDDRIVESCHANGE = Win32API.WM_USER + 1;

            switch( m.Msg )
            {
                case Win32API.WM_DEVICECHANGE:
                {
                    if ((int)m.WParam == Win32API.DBT_DEVICEARRIVAL ||
                        (int)m.WParam == Win32API.DBT_DEVICEREMOVECOMPLETE
                        )
                    {
                            // We got device change here                
                        Win32API._DEV_BROADCAST_HDR hdr = (Win32API._DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(Win32API._DEV_BROADCAST_HDR));
                        if (hdr.dbch_devicetype == Win32API.DBT_DEVTYP_VOLUME)
                        {
                            Win32API.PostMessage(this.Handle, UM_HARDDRIVESCHANGE, 0, 0);                        
                        }
                    }
                    break;
                }
                case UM_HARDDRIVESCHANGE:
                {
                    OnLogicalDrivesChanged(this, null);
                    break;
                }
                case UM_FILEALERT:
                {
                    break;
                }
                case UM_SCAN_BTN_PRESSED:
                {
                    OnStartScan((ScanPanelIDs)m.WParam);
                    break;
                }
                case UM_SCAN_QUARANTINE_RESCAN:
                {
                    OnQuarantineRescan();
                    break;
                }
                case ClamWinMainFormNotifications.UM_CURRENT_NOTIFICATION_CHANGED:
                {
                    OnCurrentNotificationChanged();
                    break;
                }
                case ClamWinDatabase.UM_DATABASE_CHANGED:
                {
                    InitDiskSpaceGroupBox();
                    break;
                }

                #region ClamWinScanner messages handling
                case ClamWinScanner.UM_ITEMS_SCAN_START:
                {                    
                    OnScanStart(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEMS_SCAN_COMPLETE:
                {
                    OnScanDone(m.WParam);
                    break;
                }
                 case ClamWinScanner.UM_ITEMS_SCAN_ABORTED_BY_USER:
                {
                    OnScanDone(m.WParam);
                    break;
                }
                 case ClamWinScanner.UM_ITEMS_SCAN_FAILED:
                {
                    OnScanDone(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEM_SCAN_COMPLETE:
                {                    
                    OnItemScanComplete(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_CATCH_FILTER_NOTIFY_DATA:
                {                    
                    HandleFilterNotifyData(m.WParam);
                    break;
                }
                #endregion

                case ClamWinSettings.UM_SETTINGS_CHANGED:
                {
                    OnSettingChanged((ClamWinSettings.SettingIDs)m.WParam.ToInt32());
                    break;
                }
                case ClamWinQuarantine.UM_QUARANTINE_ITEM_PROCESSING_COMPLETE:
                {
                    OnQuarantineChanged(m.WParam);
                    break;
                }
                case ClamWinVersion.UM_NEW_VERSION_AVAILABLE:
                {
                    OnNewVersionAvailable(m.WParam);
                    break;
                }
                case UM_FILTERJOBSUPDATED:
                {
                    OnFilterJobsUpdated();
                    break;
                }
                

                #region Schedule messages handling
                case UM_SCHEDULED_UPDATE:
                {
                    OnScheduledUpdate();
                    break;
                }
                case UM_SCHEDULED_SCAN:
                {
                    OnScheduledScan();
                    break;
                }
                case UM_SCHEDULED_SCAN_CRITICAL:
                {
                    OnScheduledScanCritical();
                    break;
                }
                case UM_SCHEDULED_SCAN_MY_PC:
                {
                    OnScheduledScanMyPC();
                    break;
                }
                #endregion
        }
            
            base.WndProc(ref m);     
        }                        		
        #endregion                                                                                                                                                                     

        #region Public Functions
        /// <summary>
        /// Add file to quarantine and show quarantine form
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="listeners"></param>
        public bool QuarantineFileAndShowProgress(string filename, IntPtr[] listeners)
        {
            IntPtr[] Listeners = null;
            if (listeners != null)
            {
                Listeners = new IntPtr[listeners.Length + 2];
            }
            else
            {
                Listeners = new IntPtr[2];
            }

            Listeners[0] = formQuarantine.Handle;
            Listeners[1] = this.Handle;

            if (listeners.Length != 0)
            {
                listeners.CopyTo(Listeners, 2);
            }

            formQuarantine.Show();
            Win32API.SetForegroundWindow(formQuarantine.Handle);
            ClamWinQuarantine.QuarantineResults result = ClamWinQuarantine.QuarantineFile(filename, Listeners);
            
            if (result != ClamWinQuarantine.QuarantineResults.Success)
            {                
                MessageBox.Show("Failed to quarantine file - \"" + filename + "\".\r\nResult - \"" + ClamWinQuarantine.QuarantineResultToString(result) + "\". ",
                                "ClamWin",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            return result == ClamWinQuarantine.QuarantineResults.Success;
        }
        #endregion

        #region TEST!!!
        private void linkLabelUpdateNow_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            /*ClamWinInfectedNotifyForm notify = new ClamWinInfectedNotifyForm();
            notify.ShowNotify(ClamWinTrayNotifier.AppearingTimeInstant, 
                              ClamWinTrayNotifier.VisibleTimeInfinite,
                              ClamWinTrayNotifier.DisappearingTimeInstant);*/

            ClamWinScan.FilterNotifyData data = new ClamWinScan.FilterNotifyData();
            data.FileName = "D:\\Copy of adult.xml";
            data.Message = "Infected!";
            FilterNotify.AddNotifyData(data);            
        }
        #endregion                                                

        private void Hoho()
        {
            int a = 0;
        }
    }
    #endregion
}