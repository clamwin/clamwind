// Name:        ClamWinScanForm.cs
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
using System.Collections;
using System.Runtime.InteropServices;

namespace ClamWinApp
{
    #region ClamWinScanForm class
    public partial class ClamWinScanForm : Form
    {
        #region Scan class
        private class Scan
        {   
            #region Public Data
            /// <summary>
            /// Date and time scan started
            /// </summary>
            public DateTime ScanStart = new DateTime();
            /// <summary>
            /// Date and time scan stopped 
            /// </summary>
            public DateTime ScanEnd = new DateTime();
            /// <summary>
            /// Status of scan
            /// </summary>
            public ScanStatus Status = ScanStatus.NotDefined;
            /// <summary>
            /// Database scan id
            /// </summary>
            public int ScanID = 0;
            /// <summary>
            /// ScanPanel ID
            /// </summary>
            public ClamWinMainForm.ScanPanelIDs ScanPanelID;
            #endregion           

            #region Enums
            public enum ScanStatus { Complete = 0, Aborted, Failed, Running, NotDefined };
            #endregion

            #region Public Functions
            /// <summary>
            /// Return corresponding string for specified stauts
            /// </summary>
            /// <param name="status"></param>
            /// <returns></returns>
            public static string StatusToString(ScanStatus status)
            {
                switch (status)
                {
                    case ScanStatus.Aborted:
                        return "Aborted";
                    case ScanStatus.Complete:
                        return "Complete";
                    case ScanStatus.Failed:
                        return "Failed";
                    case ScanStatus.Running:
                        return "Running";
                    default:
                        return "NotDefined";
                }
            }
            /// <summary>
            /// Figur out scan status for specified string
            /// </summary>
            /// <param name="status"></param>
            /// <returns></returns>
            public static ScanStatus StringToStatus(string status)
            {
                switch (status)
                {
                    case "Aborted":
                        return ScanStatus.Aborted;
                    case "Complete":
                        return ScanStatus.Complete;
                    case "Failed":
                        return ScanStatus.Failed;
                    case "Running":
                        return ScanStatus.Running;
                    default:
                        return ScanStatus.NotDefined;
                }
            }
            #endregion
        }
        #endregion

        #region Private Data
        /// <summary>
        /// Current items size (in kb)
        /// </summary>
        ulong ItemsSize = 0;
        /// <summary>
        /// If form will be closed and disposed
        /// </summary>
        bool flagClose = false;
        /// <summary>
        /// Current scanning file
        /// </summary>
        string CurrentFile = "";
        /// <summary>
        /// Current scan info
        /// </summary>
        ArrayList Scans = new ArrayList();
        /// <summary>
        /// Current scan index
        /// </summary>
        int CurrentScan = -1;
        /// <summary>
        /// MainForm Handle
        /// </summary>
        IntPtr MainFormHandle;
        /// <summary>
        /// ScanPanelID
        /// </summary>
        ClamWinMainForm.ScanPanelIDs ScanPanelID;
        /// <summary>
        /// Last sorted column for Egvents
        /// </summary>
        private int EventsSortColumn = -1;
        /// <summary>
        /// Events search form
        /// </summary>
        private ClamWinListViewSearchForm formEventsSearch = new ClamWinListViewSearchForm();
        /// <summary>
        /// Critical events search form
        /// </summary>
        private ClamWinListViewSearchForm formCriticalEventsSearch = new ClamWinListViewSearchForm();
        #endregion         

        #region Public Constructor
        public ClamWinScanForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Save events to database
        /// </summary>
        private void SaveEvents()
        {
            if (listViewEvents.Items.Count == 0)
            {
                return;
            }

            string command;
            int result;           

            for (int i = 0; i < listViewEvents.Items.Count; i++)
            {
                string Name = listViewEvents.Items[i].Text;
                string Status = listViewEvents.Items[i].SubItems[1].Text;
                string Reason = listViewEvents.Items[i].SubItems[2].Text;
                string Time = listViewEvents.Items[i].SubItems[3].Text;

                int ID = 0;

                if (Scans.Count != 0)
                { 
                    Scan scan = (Scan)Scans[Scans.Count-1];
                    ID = scan.ScanID;
                }

                string ScanID = ID.ToString();

                command = "INSERT INTO Events(Name,Status,Reason,Time,ScanID,ScanPanelID) VALUES(";
                command += "'" + Name + "',";
                command += "'" + Status + "',";
                command += "'" + Reason + "',";
                command += "'" + Time + "',";
                command += "'" + ScanID + "');";
                ClamWinDatabase.ExecCommand(command, out result);
            }
        }
        /// <summary>
        /// Load events from database
        /// </summary>
        private void LoadEvents(int ScanID)
        {
            listViewEvents.Items.Clear();
            listViewEventsCritical.Items.Clear();

            string command;

            command = "SELECT * FROM Events WHERE ScanID='" + ScanID.ToString() + "';";            
             
            ArrayList list;
            if (ClamWinDatabase.ExecReader(command, out list))
            {                
                listViewEvents.Items.Clear();
                listViewEventsCritical.Items.Clear();
                

                const int FieldsPerRecord = ClamWinDatabase.EventsFPR;

                int RecordsCount = list.Count / FieldsPerRecord;

                for (int i = 0; i < RecordsCount; i++)
                {
                    // [Name]
                    string Name = (string)list[i * FieldsPerRecord+1];
                    // [Status] 
                    string Status = (string)list[i * FieldsPerRecord + 2];
                    // [Reason] 
                    string Reason = (string)list[i * FieldsPerRecord + 3];
                    // [Time] 
                    string Time = (string)list[i * FieldsPerRecord + 4];

                    ListViewItem item =  listViewEvents.Items.Add(Name);
                    item.SubItems.Add(Status);
                    item.SubItems.Add(Reason);
                    item.SubItems.Add(Time);

                    if (Status != ClamWinScanner.StatusToString(ClamWinScanner.ResponseStatus.Clean) )
                    {
                        ListViewItem itemCritical = listViewEventsCritical.Items.Add(Name);
                        itemCritical.SubItems.Add(Status);
                        itemCritical.SubItems.Add(Reason);
                        itemCritical.SubItems.Add(Time);
                    }

                    if (Status == ClamWinScanner.StatusToString(ClamWinScanner.ResponseStatus.Infected))
                    {
                        // Status 
                        ListViewItem itemDetected = listViewDetected.Items.Add(Reason);

                        // Object
                        itemDetected.SubItems.Add(Name);                        
                    }
                }                
            }
        }
        /// <summary>
        /// Load scans from database
        /// </summary>
        private void LoadScans()
        {
            Scans.Clear();

            ArrayList list;
            string command = "SELECT * FROM Scans WHERE ScanPanelID='"+((int)ScanPanelID).ToString()+"';";
            if (ClamWinDatabase.ExecReader(command, out list))
            {
                const int FieldsPerRecord = ClamWinDatabase.ScansFPR;

                int RecordsCount = list.Count / FieldsPerRecord;

                for (int i = 0; i < RecordsCount; i++)
                { 
                    // [id] 
                    string ID = (string)list[i * FieldsPerRecord];
                    // [ScanStart] 
                    string ScanStart = (string)list[i * FieldsPerRecord + 1];
                    // [ScanEnd] 
                    string ScanEnd = (string)list[i * FieldsPerRecord + 2];
                    // [Status]
                    string Status = (string)list[i * FieldsPerRecord + 3];
                    // [ScanPanelID]
                    string ScanPanel = (string)list[i * FieldsPerRecord + 4];

                    Scan scan = new Scan();
                    scan.ScanID = int.Parse(ID);
                    scan.ScanStart = DateTime.FromBinary(long.Parse(ScanStart));
                    scan.ScanEnd = DateTime.FromBinary(long.Parse(ScanEnd));
                    scan.Status = Scan.StringToStatus(Status);
                    scan.ScanPanelID = (ClamWinMainForm.ScanPanelIDs)int.Parse(ScanPanel);

                    Scans.Add(scan);
                }
            }

            if (Scans.Count != 0)
            {
                CurrentScan = Scans.Count - 1;

                SelectScan(CurrentScan, true);                
            }
            else
            { 
                linkLabelNext.Enabled = false;
                linkLabelBack.Enabled = false;
                listViewEvents.Items.Clear();
                listViewDetected.Items.Clear();
                listViewStatistics.Items.Clear();
                listViewEventsCritical.Items.Clear();
            }
        }
        /// <summary>
        /// Load statistics items from database
        /// </summary>
        /// <param name="ScanID"></param>
        private void LoadStatistics(int ScanID)
        {
            listViewStatistics.Items.Clear();

            string command = "SELECT * FROM Statistics WHERE ScanID='"+ScanID.ToString()+"';";
            ArrayList list;
            if (ClamWinDatabase.ExecReader(command, out list))
            {                 
                const int FieldsPerRecord = ClamWinDatabase.StatisticsFPR;

                int RecordsCount = list.Count / FieldsPerRecord;

                listViewStatistics.Items.Clear();

                for (int i = 0; i < RecordsCount; i++)
                {
                    if (RecordsCount == 2 && i == 0)
                    {
                        // Skeep total
                        continue;
                    }

                    string Helper;
                    // [Object]
                    Helper = (string)list[i * FieldsPerRecord + 1];
                    ListViewItem item = listViewStatistics.Items.Add(Helper);
                    // [Scanned]
                    Helper = (string)list[i * FieldsPerRecord + 2];
                    item.SubItems.Add(Helper);
                    // [Infected] 
                    Helper = (string)list[i * FieldsPerRecord + 3];
                    item.SubItems.Add(Helper);
                    // [Errors]
                    Helper = (string)list[i * FieldsPerRecord + 4];
                    item.SubItems.Add(Helper);
                    // [Deleted]
                    Helper = (string)list[i * FieldsPerRecord + 5];
                    item.SubItems.Add(Helper);
                    // [MovedToQuarantine]  
                    Helper = (string)list[i * FieldsPerRecord + 6];
                    item.SubItems.Add(Helper);
                }
            }                 
        }
        /// <summary>
        /// Save current scan to database, retrieve ScanId
        /// </summary>
        private void SaveScanOnStart()
        {
            if (Scans.Count == 0)
            {
                return;
            }
            
            Scan scan = (Scan)Scans[Scans.Count - 1];

            if (scan.Status == Scan.ScanStatus.NotDefined)
            {
                return;
            }

            string ScanStart = scan.ScanStart.ToBinary().ToString();
            string ScanEnd = ScanStart;
            string Status = Scan.StatusToString(scan.Status);
            string PanelID = ((int)ScanPanelID).ToString();

            string command = "INSERT INTO Scans(ScanStart,ScanEnd,Status,ScanPanelID) VALUES(";
            command += "'" + ScanStart + "',";
            command += "'" + ScanEnd + "',";
            command += "'" + Status + "',";
            command += "'" + PanelID + "');";            
            
            int result;
            ClamWinDatabase.ExecCommand(command, out result);

            command = "SELECT * FROM Scans WHERE ScanStart='" + ScanStart + "';";
            
            ArrayList list;
            if (ClamWinDatabase.ExecReader(command, out list))
            {
                const int FieldsPerRecord = ClamWinDatabase.ScansFPR;

                int RecordsCount = list.Count / FieldsPerRecord;

                int id = 0;
                for (int i = 0; i < RecordsCount; i++)
                {
                    int k = int.Parse((string)list[i * FieldsPerRecord]);
                    if (k > id)
                    {
                        id = k;
                    }
                }
                scan.ScanID = id;
            }  
          
            labelStartTime.Visible = true;
            labelStartTime.Text = "Start time: ";
            labelStartTime.Text += scan.ScanStart.ToShortDateString() + " " + scan.ScanStart.ToLongTimeString();

            labelEndTime.Text = "Finish time: ";
            labelDuration.Text = "Duration: ";
        }
        /// <summary>
        /// Update scan in database
        /// </summary>
        private void SaveScanOnStop()
        {
            if (Scans.Count == 0)
            {
                return;
            }

            Scan scan = (Scan)Scans[Scans.Count - 1];

            string ScanStart = scan.ScanStart.ToBinary().ToString();
            string ScanEnd = scan.ScanEnd.ToBinary().ToString();
            string Status = Scan.StatusToString(scan.Status);
            string ScanID = scan.ScanID.ToString();

            string command = "UPDATE Scans SET ScanStart = '" + ScanStart;
            command += "', ScanEnd = '" + ScanEnd;
            command += "', Status = '"+Status+"' ";
            command += "WHERE id = '" + ScanID + "';";           

            int result;
            ClamWinDatabase.ExecCommand(command, out result);

            ClamWinScanner.FlushStatistic((int)scan.ScanPanelID, scan.ScanID);
        }
        /// <summary>
        /// Select scan at specified index
        /// </summary>
        /// <param name="index"></param>
        private void SelectScan(int index, bool ReloadData)
        { 
            if( index >= Scans.Count )
            {
                return;
            }

            if (Scans.Count == 1)
            {
                linkLabelNext.Enabled = false;
                linkLabelBack.Enabled = false;
            }
            else if( index == Scans.Count-1)
            {
                linkLabelBack.Enabled = true;
                linkLabelNext.Enabled = false;
            }
            else if(index == 0 )
            {
                linkLabelBack.Enabled = false;
                linkLabelNext.Enabled = true;
            }
            else
            {
                linkLabelBack.Enabled = true;
                linkLabelNext.Enabled = true;
            }

            Scan scan = (Scan)Scans[index];

            if( ReloadData )
            {
                LoadEvents(scan.ScanID);

                LoadStatistics(scan.ScanID);
            }

            labelStartTime.Visible = true;
            labelStartTime.Text = "Start time: ";
            labelStartTime.Text += scan.ScanStart.ToShortDateString() + " " + scan.ScanStart.ToLongTimeString();

            labelEndTime.Visible = true;
            labelEndTime.Text = "Finsih time: ";
            labelEndTime.Text += scan.ScanEnd.ToShortDateString() + " " + scan.ScanEnd.ToLongTimeString();

            labelDuration.Visible = true;
            TimeSpan duration = scan.ScanEnd-scan.ScanStart;
            labelDuration.Text = "Duration: "+duration.ToString();
        }
        /// <summary>
        /// Adjust controls sizes according to scanning form size
        /// </summary>
        private void AdjustSizes()
        {
            panelExScanStatus.Size = new Size(this.Size.Width - 32, 37);

            buttonStartScan.Location = new Point(panelExScanStatus.Size.Width - 135,
                                                 13);

            buttonPauseScan.Location = new Point(panelExScanStatus.Size.Width - 95,
                                                 13);

            buttonStopScan.Location = new Point(panelExScanStatus.Size.Width - 50,
                                                13);

            progressBarScanning.Size = new Size(this.Size.Width - 32, 20);

            labelStartTime.Location = new Point(this.Size.Width - 246, 73);
            labelDuration.Location = new Point(this.Size.Width - 246, 91);
            labelEndTime.Location = new Point(this.Size.Width - 246, 109);

            tabScanning.Size = new Size(this.Size.Width - 32, this.Size.Height - 248);

            labelNowScanning.Size = new Size(this.Size.Width - 32, 13);

            linkLabelBack.Location = new Point(90, this.Size.Height - 56);
            linkLabelNext.Location = new Point(134, this.Size.Height - 56);

            buttonDetectedActions.Location = new Point(tabPageDetected.Size.Width - 81,
                                                       tabPageDetected.Size.Height - 29);

            buttonEventsActions.Location = new Point(tabPageEvents.Size.Width - 81,
                                                     tabPageEvents.Size.Height - 29);

            checkBoxShowNonCritical.Location = new Point(6, tabPageEvents.Size.Height - 26);

            listViewDetected.Size = new Size(tabPageDetected.Size.Width - 12,
                                             tabPageDetected.Size.Height - 38);            

            listViewEvents.Size = new Size(tabPageEvents.Size.Width - 12,
                                           tabPageEvents.Size.Height - 38);

            listViewEventsCritical.Size = new Size(tabPageEvents.Size.Width - 12,
                                           tabPageEvents.Size.Height - 38);

            listViewStatistics.Size = new Size(tabPageStatistics.Size.Width - 12,
                                             tabPageStatistics.Size.Height - 9);
            
        }
        /// <summary>
        /// Clean quarantine temp folder (if this is quarantine rescan form)
        /// </summary>
        private void OnCompleteQuarantineCleanUp()
        {
            if (this.ScanPanelID != ClamWinMainForm.ScanPanelIDs.Quarantine)
            {
                return;
            }

            try
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(ClamWinQuarantine.GetQuarantineTempFolder());

                if (!di.Exists)
                {
                    return;
                }

                foreach (System.IO.FileInfo fi in di.GetFiles())
                {
                    try
                    {
                        if (fi.Exists)
                        {
                            fi.Delete();
                        }
                    }
                    catch
                    {                     
                    }                
                }
                    
            }
            catch
            { 
            
            }        
        }
        #endregion

        #region Event Handlers

        #region LinkLabel Event Handlers
        /// <summary>
        /// linkLabelBack on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CurrentScan < 1 )
            {
                return;
            }

            CurrentScan--;          

            SelectScan(CurrentScan,true);
        }
        /// <summary>
        /// linkLabelNext on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CurrentScan >= Scans.Count - 1)
            {
                return;
            }

            CurrentScan++;            

            SelectScan(CurrentScan,true);
        }
        #endregion

        #region Button Event Handlers
        /// <summary>
        /// buttonStopScan on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStopScan_Click(object sender, EventArgs e)
        {
            ClamWinScanner.StopScan((int)ScanPanelID);
        }
        /// <summary>
        /// buttonStartScan on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartScan_Click(object sender, EventArgs e)
        {
            Win32API.PostMessage(MainFormHandle,
                                 ClamWinMainForm.UM_SCAN_BTN_PRESSED,
                                 (uint)this.ScanPanelID,
                                 0
                                 );            
        }
        /// <summary>
        /// buttonPauseScan on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPauseScan_Click(object sender, EventArgs e)
        {
            ClamWinScanner.SuspendScan((int)ScanPanelID);
        }
        /// <summary>
        /// buttonEventsActions on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEventsActions_Click(object sender, EventArgs e)
        {
            Point screen = buttonEventsActions.PointToScreen(new Point(0, 0));
            int x = screen.X;
            int y = screen.Y + buttonEventsActions.Height;
            contextMenuEventsActions.Show(new Point(x, y));
        }
        /// <summary>
        /// buttonDetectedActions on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDetectedActions_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region User Message Handlers
        /// <summary>
        /// Items scan starts
        /// </summary>
        /// <param name="size">Items size in Kb</param>
        private void OnScanStart(IntPtr data)
        {
            ClamWinScan.NotifyData Data = (ClamWinScan.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinScan.NotifyData));
            Marshal.FreeHGlobal(data);

            progressBarScanning.Maximum = (int)(Data.Size / 1024);
            progressBarScanning.Value = 0;
            
            listViewEvents.Items.Clear();
            listViewEventsCritical.Items.Clear();

            listViewStatistics.Items.Clear();
            ItemsSize = Data.Size;
            buttonStartScan.Enabled = false;
            buttonStopScan.Enabled = true;
            buttonPauseScan.Enabled = true;

            Scans.Add(new Scan());

            if (Scans.Count == 0)
            {
                return;
            }

            Scan scan = (Scan)Scans[Scans.Count - 1];

            scan.ScanStart = DateTime.Now;

            scan.Status = Scan.ScanStatus.Running;

            scan.ScanPanelID = ScanPanelID;

            SaveScanOnStart();
                                   
            listViewStatistics.Items.Clear();
            for( int i = 0; i<Data.Statistics.Length;i++)
            {                    
                if( Data.Statistics.Length == 2 && i == 0)
                {
                    //Skip total
                    continue;
                }

                ListViewItem item = listViewStatistics.Items.Add(Data.Statistics[i].Name);
                item.SubItems.Add("0");
                item.SubItems.Add("0");
                item.SubItems.Add("0");
                item.SubItems.Add("0");
                item.SubItems.Add("0");                                                  
            }

            linkLabelNext.Enabled = false;
            linkLabelBack.Enabled = false;

            if (ClamWinSettings.KeepOnlyRecentEvents)
            {
                int result;
                ClamWinDatabase.ExecCommand("DELETE FROM Events;",out result);                
            }
        }
        /// <summary>
        /// Items scan complete
        /// </summary>
        private void OnScanComlpete(IntPtr data)
        {
            buttonStartScan.Enabled = true;
            buttonStopScan.Enabled = false;
            buttonPauseScan.Enabled = false;
            progressBarScanning.Value = progressBarScanning.Maximum;

            OnCompleteQuarantineCleanUp();

            if (Scans.Count == 0)
            {
                return;
            }

            Scan scan = (Scan)Scans[Scans.Count - 1];

            if (scan.Status == Scan.ScanStatus.Running)
            {
                scan.ScanEnd = DateTime.Now;
                scan.Status = Scan.ScanStatus.Complete;
                
                SaveScanOnStop();

                //SaveEvents();

                ClamWinStatistics.FlushItems(scan.ScanID);

                CurrentScan = Scans.Count - 1;
                SelectScan(CurrentScan, false);

                labelNowScanning.Text = "Complete";                
            }                        
        }
        /// <summary>
        /// Items scan aborted by user (stop button)
        /// </summary>
        private void OnScanAbortedByUser(IntPtr data)
        {
            buttonStartScan.Enabled = true;
            buttonStopScan.Enabled = false;
            buttonPauseScan.Enabled = false;

            OnCompleteQuarantineCleanUp();

            if (Scans.Count == 0)
            {
                return;
            }

            Scan scan = (Scan)Scans[Scans.Count - 1];

            if (scan.Status == Scan.ScanStatus.Running)
            {
                scan.ScanEnd = DateTime.Now;
                scan.Status = Scan.ScanStatus.Aborted;
                
                SaveScanOnStop();

               // SaveEvents();

                ClamWinStatistics.FlushItems(scan.ScanID);

                CurrentScan = Scans.Count - 1;
                SelectScan(CurrentScan, false);

                labelNowScanning.Text = "Aborted";
            }            

            //SaveEvents();
        }
        /// <summary>
        /// Error occured during items scan
        /// </summary>
        private void OnScanFailed(IntPtr data)
        {
            buttonStartScan.Enabled = true;
            buttonStopScan.Enabled = false;
            buttonPauseScan.Enabled = false;

            if (Scans.Count == 0)
            {
                return;
            }

            Scan scan = (Scan)Scans[Scans.Count - 1];

            OnCompleteQuarantineCleanUp();

            if (scan.Status == Scan.ScanStatus.Running)
            {
                scan.ScanEnd = DateTime.Now;
                scan.Status = Scan.ScanStatus.Failed;

                SaveScanOnStop();

                //SaveEvents();

                ClamWinStatistics.FlushItems(scan.ScanID);

                CurrentScan = Scans.Count - 1;
                SelectScan(CurrentScan, false);

                labelNowScanning.Text = "Failed";
            }  

            //SaveEvents();

            System.Windows.Forms.MessageBox.Show("Scan failed!", "ClamWin", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Scan suspended by user (pause buttton)
        /// </summary>
        private void OnScanSuspended(IntPtr data)
        {
            buttonStartScan.Enabled = true;
            buttonStopScan.Enabled = true;
            buttonPauseScan.Enabled = false;
            labelNowScanning.Text = "Pause";
        }
        /// <summary>
        /// Scan resumed by user
        /// </summary>
        private void OnScanResumed(IntPtr data)
        {
            buttonStartScan.Enabled = false;
            buttonStopScan.Enabled = true;
            buttonPauseScan.Enabled = true;
            labelNowScanning.Text = "Now scanning: " + CurrentFile;
        }
        /// <summary>
        /// Single item(file) scan start
        /// </summary>
        private void OnItemScanStart(IntPtr data)
        {            
            ClamWinScan.NotifyData Data = (ClamWinScan.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinScan.NotifyData));
            Marshal.FreeHGlobal(data);

            string path = Data.Request.GetFilePath();
            labelNowScanning.Text = "Now scanning: " + path;
            CurrentFile = path;

        }
        /// <summary>
        /// Single item(file) scan complete
        /// </summary>
        private void OnItemScanComplete(IntPtr data)
        {
            labelNowScanning.Text = "Now scanning: ";
            ClamWinScan.NotifyData Data = (ClamWinScan.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinScan.NotifyData));
            Marshal.FreeHGlobal(data);

            string FileName = Data.Item;

            ClamWinScanner.ResponseStatus status = Data.Response.GetActionStatus();
            string Status = ClamWinScanner.StatusToString(status);
            string Reason = Data.Response.GetMessage();            
            string Time = System.DateTime.Today.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();

            // Save event to db
            string ScanID = "0";
            if (Scans.Count != 0)
            {
                Scan scan = (Scan)Scans[Scans.Count - 1];
                ScanID = scan.ScanID.ToString();
            }

            FileName = FileName.Replace("'","''");
            string command = "INSERT INTO Events(Name,Status,Reason,Time,ScanID) VALUES(";
            command += "'" + FileName + "',";
            command += "'" + Status + "',";
            command += "'" + Reason + "',";
            command += "'" + Time + "',";
            command += "'" + ScanID + "');";            

            int result;

            if (ClamWinSettings.LogNonCriticalEvents || (status != ClamWinScanner.ResponseStatus.Clean))
            {
                ClamWinDatabase.ExecCommand(command, out result);
            }
           
            // Name
            ListViewItem item = listViewEvents.Items.Add(FileName);            
            // Status sub-item
            item.SubItems.Add(Status);
            // Reason sub-item 
            // TODO: need reason field in response 
            item.SubItems.Add(Reason);
            // Date-time subitem
            item.SubItems.Add(Time);

            item.EnsureVisible();

            if (status != ClamWinScanner.ResponseStatus.Clean)
            {
                // Name
                ListViewItem itemCritical = listViewEventsCritical.Items.Add(FileName);
                // Status sub-item
                itemCritical.SubItems.Add(Status);
                // Reason sub-item 
                // TODO: need reason field in response 
                itemCritical.SubItems.Add(Reason);
                // Date-time subitem
                itemCritical.SubItems.Add(Time);
                
                itemCritical.EnsureVisible();
            }

            if (status == ClamWinScanner.ResponseStatus.Infected)
            {
                // Status
                ListViewItem itemDetected = listViewDetected.Items.Add(Reason);               
                // Object sub-item                 
                itemDetected.SubItems.Add(FileName);

                itemDetected.EnsureVisible();
            }

            progressBarScanning.Value = (int)(Data.Size / 1024);

            #region Update statistics for current object
            int k = 0;
            for (int i = 0; i < Data.Statistics.Length; i++)
            {
                if (Data.Statistics.Length == 2 && i == 0)
                {
                    //Skip total
                    continue;
                }

                listViewStatistics.Items[k].SubItems[1].Text = Data.Statistics[i].Scanned.ToString();
                listViewStatistics.Items[k].SubItems[2].Text = Data.Statistics[i].Infected.ToString();
                listViewStatistics.Items[k].SubItems[3].Text = Data.Statistics[i].Errors.ToString();
                listViewStatistics.Items[k].SubItems[4].Text = Data.Statistics[i].Deleted.ToString();
                listViewStatistics.Items[k].SubItems[5].Text = Data.Statistics[i].MovedToQuarantine.ToString();
                k++;
            }                        
            #endregion
        }
        /// <summary>
        /// Scanning progress
        /// </summary>
        /// <param name="data"></param>
        private void OnItemScanProgress(IntPtr data)
        {            
            ClamWinScan.NotifyData Data = (ClamWinScan.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinScan.NotifyData));
            Marshal.FreeHGlobal(data);            

            progressBarScanning.Value = (int)(Data.Size / 1024);           
        }
        /// <summary>
        /// OnSettingsChanged dispatcher
        /// </summary>
        /// <param name="id"></param>
        private void OnSettingsChanged(ClamWinSettings.SettingIDs id)
        {
            switch (id)
            {
                case ClamWinSettings.SettingIDs.LogNonCriticalEvents:
                    {
                        if (ClamWinSettings.LogNonCriticalEvents)
                        {
                            checkBoxShowNonCritical.Enabled = true;
                            listViewEvents.Visible = ClamWinSettings.ShowNonCriticalEvents;
                            listViewEventsCritical.Visible = !ClamWinSettings.ShowNonCriticalEvents;
                        }
                        else
                        {
                            checkBoxShowNonCritical.Enabled = false;
                            listViewEvents.Visible = false;
                            listViewEventsCritical.Visible = true;
                        }
                        break;
                    }

            }
        }
        #endregion        

        #region Context Menu Event Handlers
        /// <summary>
        /// EventsActionsGoToFileItem on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventsActionsGoToFileItem_Click(object sender, EventArgs e)
        {
            ListView listView;

            if (listViewEvents.Visible)
            {
                listView = listViewEvents;
            }
            else
            {
                listView = listViewEventsCritical;
            }

            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (listView.Items[i].Selected)
                {
                    string directory = System.IO.Path.GetDirectoryName(listView.Items[i].Text);
                    Win32API.Shell32.ShellExecute(new IntPtr(), "open", directory, "", "", Win32API.SW_SHOW);
                    break;
                }
            }
        }
        /// <summary>
        /// EventsActionsCleanAllItem on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventsActionsCleanAllItem_Click(object sender, EventArgs e)
        {           
            listViewEvents.Items.Clear();
            listViewEventsCritical.Items.Clear();

            if( CurrentScan >= 0 && CurrentScan < Scans.Count)
            {
                Scan scan = (Scan)Scans[CurrentScan];
                string command = "DELETE FROM Events WHERE ScanID='" + scan.ScanID.ToString() + "';";
                int result;
                ClamWinDatabase.ExecCommand(command, out result);
            }
        }
        /// <summary>
        /// EventsActionsSearchItem on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventsActionsSearchItem_Click(object sender, EventArgs e)
        {
            ClamWinListViewSearchForm form = new ClamWinListViewSearchForm();
            if (listViewEvents.Visible)
            {
                if (formEventsSearch.IsDisposed)
                {
                    formEventsSearch = new ClamWinListViewSearchForm();                    
                }

                if (!formEventsSearch.Visible)
                {
                    formEventsSearch.Show(listViewEvents, this.Handle);
                }                                
            }
            else if (listViewEventsCritical.Visible)
            {
                if (formCriticalEventsSearch.IsDisposed)
                {
                    formCriticalEventsSearch = new ClamWinListViewSearchForm();
                }

                if (!formCriticalEventsSearch.Visible)
                {
                    formCriticalEventsSearch.Show(listViewEventsCritical, this.Handle);
                }                               
            }
        }
        /// <summary>
        /// EventsActionsSelectAllItem on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventsActionsSelectAllItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewEvents.Items)
            {
                item.Selected = true;
            }

            foreach (ListViewItem item in listViewEventsCritical.Items)
            {
                item.Selected = true;
            }
        }
        /// <summary>
        /// EventsActionsCopyItem on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventsActionsCopyItem_Click(object sender, EventArgs e)
        {

            ListView listView;

            if (listViewEvents.Visible)
            {
                listView = listViewEvents;
            }
            else
            {
                listView = listViewEventsCritical;
            }

            string data = "";
            foreach (ListViewItem item in listView.Items)
            {
                if (!item.Selected)
                {
                    continue;
                }
                // [Name]
                data += item.Text;
                // [Status]
                data += " " + item.SubItems[1].Text;
                // [Reason]
                data += " " + item.SubItems[2].Text;
                // [Time]
                data += " " + item.SubItems[3].Text;
                data += "\r\n";
            }
            
            if (data.Length != 0)
            {
                Clipboard.SetDataObject(data);
            }
        }
        /// <summary>
        /// StatisticsActionsSearchItem on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsActionsSearchItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// StatisticsActionsSelectAllItem on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsActionsSelectAllItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewStatistics.Items)
            {
                item.Selected = true;
            }
        }
        /// <summary>
        /// StatisticsActionsCopyItem on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsActionsCopyItem_Click(object sender, EventArgs e)
        {
            string data = "";
            foreach (ListViewItem item in listViewStatistics.Items)
            {
                if (!item.Selected)
                {
                    continue;
                }
                // [Object] 
                data += item.Text;
                // [Scanned] 
                data += " " + item.SubItems[1].Text;
                // [Threats] 
                data += " " + item.SubItems[2].Text;
                // [Deleted] 
                data += " " + item.SubItems[3].Text;
                // [MovedToQuarantine] 
                data += " " + item.SubItems[4].Text;
                // [Archived] 
                data += " " + item.SubItems[5].Text;
                // [Packed] 
                data += " " + item.SubItems[6].Text;
                // [PasswordProtected] 
                data += " " + item.SubItems[7].Text;
                // [Corrupted]                                                                                               
                data += " " + item.SubItems[8].Text;
                data += "\r\n";
            }

            if (data.Length != 0)
            {
                Clipboard.SetDataObject(data);
            }
        }
        #endregion 

        #region Form Event Handlers
        /// <summary>
        /// Form load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClamWinScanForm_Load(object sender, EventArgs e)
        {
            AdjustSizes();    
        }
        /// <summary>
        /// Form closing event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClamWinScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!flagClose)
            {
                e.Cancel = true;
                Visible = false;
                return;
            }            
        }
        /// <summary>
        /// Form resize event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClamWinScanForm_Resize(object sender, EventArgs e)
        {
            AdjustSizes();
        }
        #endregion

        #region Checkbox Event Handlers
        /// <summary>
        /// checkBoxShowNonCritical check changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxShowNonCritical_CheckedChanged(object sender, EventArgs e)
        {
            ClamWinSettings.ShowNonCriticalEvents = checkBoxShowNonCritical.Checked;

            if (checkBoxShowNonCritical.Checked)
            {
                for (int i = 0; i < listViewEvents.Columns.Count; i++)
                {
                    listViewEvents.Columns[i].Width = listViewEventsCritical.Columns[i].Width;
                }                
            }
            else
            {
                for (int i = 0; i < listViewEvents.Columns.Count; i++)
                {
                    listViewEventsCritical.Columns[i].Width = listViewEvents.Columns[i].Width;
                }
            }

            listViewEvents.Visible = ClamWinSettings.ShowNonCriticalEvents;
            listViewEventsCritical.Visible = !ClamWinSettings.ShowNonCriticalEvents;
        }
        #endregion

        #region TabCtrl Event Handlers
        /// <summary>
        /// tabScanning SelectedIndexChanged event handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabScanning_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustSizes();
        }
        #endregion

        #region ListView Event Handlers
        /// <summary>
        /// listViewEvents On Column Click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewEvents_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != EventsSortColumn)
            {
                EventsSortColumn = e.Column;
                listViewEvents.Sorting = SortOrder.Ascending;
                listViewEventsCritical.Sorting = SortOrder.Ascending;
            }
            else
            {
                listViewEvents.Sorting = listViewEvents.Sorting == SortOrder.Ascending
                                                  ? SortOrder.Descending
                                                  : SortOrder.Ascending;

                listViewEventsCritical.Sorting = listViewEvents.Sorting;
            }

            listViewEvents.Sort();
            listViewEventsCritical.Sort();

            listViewEvents.ListViewItemSorter = new ListViewEventsComparer(e.Column,
                                                              listViewEvents.Sorting);

            listViewEventsCritical.ListViewItemSorter = new ListViewEventsComparer(e.Column,
                                                              listViewEvents.Sorting);
        }
        #endregion

        #endregion

        #region Public Functions
        /// <summary>
        /// Set Close flag to true
        /// </summary>
        public void EnableClose()
        {
            flagClose = true;
        }
        /// <summary>
        /// Show form using specified parameters
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="Handle"></param>
        public void ShowScanForm(ClamWinMainForm.ScanPanelIDs PanelID, IntPtr Handle)
        {                        
            try
            {
                ScanPanelID = PanelID;
                MainFormHandle = Handle;

                this.buttonPauseScan.BringToFront();
                this.buttonStartScan.BringToFront();
                this.buttonStopScan.BringToFront();

                checkBoxShowNonCritical.Checked = ClamWinSettings.ShowNonCriticalEvents;

                listViewEvents.Visible = ClamWinSettings.ShowNonCriticalEvents;
                listViewEventsCritical.Visible = !ClamWinSettings.ShowNonCriticalEvents;
                
                tabScanning.SelectedIndex = 1;

                LoadScans();

                Show();
                Win32API.SetWindowPos(  this.Handle,
                                        Win32API.HWND_TOP,
                                        0,
                                        0,
                                        0,
                                        0,
                                        Win32API.SWP_SHOWWINDOW | Win32API.SWP_NOSIZE | Win32API.SWP_NOMOVE
                                     );

                Win32API.SetForegroundWindow(this.Handle);
            }
            catch
            { 
            }
        }
        /// <summary>
        /// Show form 
        /// </summary>        
        public void ShowScanForm()
        {
            try
            {                               
                Show();
                Win32API.SetWindowPos(this.Handle,
                                        Win32API.HWND_TOP,
                                        0,
                                        0,
                                        0,
                                        0,
                                        Win32API.SWP_SHOWWINDOW | Win32API.SWP_NOSIZE | Win32API.SWP_NOMOVE
                                     );

                Win32API.SetForegroundWindow(this.Handle);
            }
            catch
            {
            }
        }
        /// <summary>
        /// ScanPanelID accessor
        /// </summary>
        /// <returns></returns>
        public ClamWinMainForm.ScanPanelIDs GetPanelID()
        {
            return ScanPanelID;
        }
        #endregion

        #region Windows Specific Stuff
        /// <summary>
        /// WndProc override to handle windows messages
        /// </summary>
        /// <param name="m">Message</param>        
        protected override void WndProc(ref Message m)
        {            
            switch (m.Msg)
            {
                case ClamWinScanner.UM_ITEMS_SCAN_START:
                {                    
                    OnScanStart(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEMS_SCAN_COMPLETE:
                {
                    OnScanComlpete(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEMS_SCAN_ABORTED_BY_USER:
                {
                    OnScanAbortedByUser(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEMS_SCAN_FAILED:
                {
                    OnScanFailed(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEMS_SCAN_SUSPENDED:
                {
                    OnScanSuspended(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEMS_SCAN_RESUMED:
                {
                    OnScanResumed(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEM_SCAN_START:
                {                    
                    OnItemScanStart(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEM_SCAN_COMPLETE:
                {                    
                    OnItemScanComplete(m.WParam);
                    break;
                }
                case ClamWinScanner.UM_ITEM_SCAN_PROGRESS:
                {
                    OnItemScanProgress(m.WParam);
                    break;
                }
                case ClamWinSettings.UM_SETTINGS_CHANGED:
                {
                    OnSettingsChanged((ClamWinSettings.SettingIDs)m.WParam.ToInt32());
                    break;
                }
                default:
                {
                    base.WndProc(ref m);
                    break;
                }
            }
        }
        #endregion                                                                      

        #region ListViewEventsComparer class
        private class ListViewEventsComparer : IComparer 
        { 
            #region Private Data
            /// <summary>
            /// Column
            /// </summary>
            private int Column;
            /// <summary>
            /// Sorting order
            /// </summary>
            private SortOrder Order;
            #endregion
            
            #region Public Constructor
            /// <summary>
            /// Public constructor
            /// </summary>
            public ListViewEventsComparer() 
            {
                Column=0;
                Order = SortOrder.Ascending;
            }
            /// <summary>
            /// Public constructor
            /// </summary>
            /// <param name="column"></param>
            /// <param name="order"></param>
            public ListViewEventsComparer(int column, SortOrder order) 
            {
                Column = column;
                Order = order;
            }
            #endregion
            
            #region Public Functions
            /// <summary>
            /// Comparison function
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(object x, object y) 
            {
                int returnVal = -1;
                string X = ((ListViewItem)x).SubItems[Column].Text;
                string Y = ((ListViewItem)y).SubItems[Column].Text;
                
                if (Column == 0 || Column == 1 || Column == 2)
                {
                    // Name, status, and reason
                    returnVal = String.Compare(X, Y);
                }
                else if (Column == 3)
                { 
                    // Time
                    DateTime timeX = DateTime.Parse(X);
                    DateTime timeY = DateTime.Parse(Y);

                    returnVal = DateTime.Compare(timeX, timeY);
                }              

                if (Order == SortOrder.Descending)
                {
                    returnVal *= -1;
                }
                return returnVal;
            }

            #endregion
        }
        #endregion        
        
    }
    #endregion
}