// Name:        ClamwinNotifyForm.cs
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
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace ClamWinApp
{
    public partial class ClamWinFilterNotifyForm : ClamWinTrayNotifier
    {
        #region Private Data
        /// <summary>
        /// Notify data list
        /// </summary>
        private ArrayList FilterNotifications = new ArrayList();
        /// <summary>
        /// Cureent notify data
        /// </summary>
        private ClamWinScan.FilterNotifyData CurrentNotify = null;
        /// <summary>
        /// Main form reference
        /// </summary>
        private ClamWinMainForm MainFormRef = null;
        #endregion

        #region Public Constructor
        /// <summary>
        /// Public constructor
        /// </summary>
        public ClamWinFilterNotifyForm(ClamWinMainForm form)
        {            
            MainFormRef = form;
            
            InitializeComponent();

            LoadNotifications();
        }
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Insert new or update existing row
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status"></param>
        private void UpdateDatabase(ClamWinScan.FilterNotifyData data, string status)
        {
            int result = 0;
            string command = "SELECT * FROM FilterNotifications WHERE Path = '"+data.FileName+"'";
            command += " AND Message = '" + data.Message + "' AND Time = '" + data.Time.ToBinary().ToString() + "';";

            ArrayList items;
            ClamWinDatabase.ExecReader(command, out items);

            if (items.Count == 0)
            {
                // Insert new item 
                command = "INSERT INTO FilterNotifications(Path,Message,Status,Time) VALUES(";
                command += "'" + data.FileName + "',";
                command += "'" + data.Message + "',";
                command += "'" + status + "',";
                command += "'" + data.Time.ToBinary().ToString() + "');";
                ClamWinDatabase.ExecCommand(command, out result);
            }
            else
            { 
                // Update existing
                command = "UPDATE FilterNotifications SET Status = '" + status;
                command += "' WHERE Path = '" + data.FileName + "' AND Message = '" + data.Message + "'";
                command += " AND Time = '" + data.Time.ToBinary().ToString() + "';";
                ClamWinDatabase.ExecCommand(command, out result);
            }            
        }
        /// <summary>
        /// Loads undefined notifications
        /// </summary>
        private void LoadNotifications()
        {
            int result = 0;
            string command = "SELECT * FROM FilterNotifications WHERE Status = 'Undefined';";            

            ArrayList list;
            ClamWinDatabase.ExecReader(command, out list);

            if (list.Count == 0)
            {
                return;
            }

            const int FieldsPerRecord = ClamWinDatabase.FilterNotificationsFPR;

            int RecordsCount = list.Count / FieldsPerRecord;

            FilterNotifications.Clear();
            
            for (int i = 0;i < RecordsCount; i++)
            {
                ClamWinScan.FilterNotifyData data = new ClamWinScan.FilterNotifyData();
                // [id]

                // [Path]
                data.FileName = list[i * FieldsPerRecord + 1].ToString();
                // [Message]
                data.Message = list[i * FieldsPerRecord + 2].ToString();
                // [Status]                

                // [Time]
                data.Time = DateTime.FromBinary(long.Parse(list[i * FieldsPerRecord + 4].ToString()));

                AddNotifyData(data);
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// buttonClose on-click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.StartDisappearing();
        }
        /// <summary>
        /// buttonApply on-click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonApply_Click(object sender, EventArgs e)
        {
            if( CurrentNotify == null )
            {
                throw new SystemException("Applying action with CurrentNotify == null!");
            }

            FileInfo fi = new FileInfo(CurrentNotify.FileName);

            bool Deleted = false;
            bool Quarantine = false;
            bool DoesNotExist = false;

            if (!fi.Exists)
            {
                DoesNotExist = true;                
            }

            if (radioButtonDeleteFile.Checked && !DoesNotExist)
            {
                
                fi.Delete();

                fi = new FileInfo(CurrentNotify.FileName);

                if (fi.Exists)
                {
                    MessageBox.Show("Unable to delete file - \"" + CurrentNotify.FileName + "\"!",
                                        "ClamWin",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                }
                else
                {
                    Deleted = true;
                }                
            }
            else if (radioButtonQuarantine.Checked)
            {
                IntPtr[] Listeners = new IntPtr[1];
                Listeners[0] = this.Handle;

                Quarantine = MainFormRef.QuarantineFileAndShowProgress(CurrentNotify.FileName, Listeners);                
            }            


            if (Deleted)
            {
                UpdateDatabase(CurrentNotify, "Deleted");
            }
            else if (Quarantine)
            {
                CurrentNotify.Processed = true;
                FilterNotifications.Add(CurrentNotify);
            }
            else if (DoesNotExist)
            {
                UpdateDatabase(CurrentNotify, "DoesNotExist");
            }
            else if (radioButtonIgnore.Checked)
            {
                UpdateDatabase(CurrentNotify, "Ignored");
            }
            else
            {
                FilterNotifications.Add(CurrentNotify);
            }
            

            if (FilterNotifications.Count != 0)
            {
                CurrentNotify = null;
                for (int i = 0; i < FilterNotifications.Count; i++)
                {
                    if (!((ClamWinScan.FilterNotifyData)FilterNotifications[i]).Processed)
                    {
                        CurrentNotify = (ClamWinScan.FilterNotifyData)FilterNotifications[i];
                        FilterNotifications.RemoveAt(i);
                        break;
                    }
                }

                if (CurrentNotify != null)
                {
                    labelFileName.Text = "File - " + CurrentNotify.FileName;
                    labelMessage.Text = "Message - " + CurrentNotify.Message;
                }
                else
                {
                    base.StartDisappearing();
                }
            }
            else
            {
                CurrentNotify = null;
                base.StartDisappearing();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void OnQuarantineItemComplete(IntPtr data)
        {
            ClamWinQuarantine.NotifyData Data = (ClamWinQuarantine.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinQuarantine.NotifyData));
            Marshal.FreeHGlobal(data);

            ClamWinScan.FilterNotifyData notify = null;
            int position = -1;
            for (int i = 0; i < FilterNotifications.Count; i++)
            {
                if (((ClamWinScan.FilterNotifyData)FilterNotifications[i]).Processed && 
                    ((ClamWinScan.FilterNotifyData)FilterNotifications[i]).FileName == Data.FileName)
                {
                    notify = (ClamWinScan.FilterNotifyData)FilterNotifications[i];
                    position = i;
                    break;
                }
            }

            if( notify == null )
            {
                return;
            }

            if (Data.Result == ClamWinQuarantine.QuarantineResults.Failed ||
                Data.Result == ClamWinQuarantine.QuarantineResults.FailedQueueLocked)
            {                
                notify.Processed = false;
            }
            else if (Data.Result == ClamWinQuarantine.QuarantineResults.Success)
            {
                UpdateDatabase(notify, "Quarantined");
                FilterNotifications.RemoveAt(position);
            }
            else if (Data.Result == ClamWinQuarantine.QuarantineResults.FailedFileDoesNotExist)
            {
                MessageBox.Show("File - " + notify.FileName + " does not exist.",
                                "ClamWin",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                UpdateDatabase(notify, "DoesNotExist");
                FilterNotifications.RemoveAt(position);
            }
            else if (Data.Result == ClamWinQuarantine.QuarantineResults.FailedAlreadyQuarantined)
            {
                MessageBox.Show("File - " + notify.FileName+ " already quarantined.",
                                "ClamWin",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                UpdateDatabase(notify, "AlreadyQua");
                notify.Processed = false;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Adds filter notify data to notifies list, and pop up notify form
        /// </summary>
        /// <param name="data"></param>
        public void AddNotifyData(ClamWinScan.FilterNotifyData data)
        {
            for (int i = 0; i < FilterNotifications.Count; i++)
            {
                if (((ClamWinScan.FilterNotifyData)FilterNotifications[i]).FileName == data.FileName &&
                    ((ClamWinScan.FilterNotifyData)FilterNotifications[i]).Message == data.Message)
                {
                    return;    
                }
            
            }

            if (CurrentNotify != null)
            {
                if (CurrentNotify.FileName == data.FileName &&
                    CurrentNotify.Message == data.Message)
                {
                    return;
                }
            }


            FilterNotifications.Add(data);
            UpdateDatabase(data, "Undefined");

            if (CurrentNotify == null)
            {                
                for (int i = 0; i < FilterNotifications.Count; i++)
                {
                    if (!((ClamWinScan.FilterNotifyData)FilterNotifications[i]).Processed)
                    {
                        CurrentNotify = (ClamWinScan.FilterNotifyData)FilterNotifications[i];                        
                        FilterNotifications.RemoveAt(i);
                        break;
                    }
                }

                if (CurrentNotify != null)
                {
                    labelFileName.Text = "File - " + CurrentNotify.FileName;
                    labelMessage.Text = "Message - " + CurrentNotify.Message;
                }
                else
                {
                    return;
                }
            }

            if (this.State == NotifierStates.Hidden)
            {
                base.ShowNotify(ClamWinTrayNotifier.AppearingTimeInstant,
                                ClamWinTrayNotifier.VisibleTimeInfinite,
                                ClamWinTrayNotifier.DisappearingTimeInstant);
            
            }
        }
        #endregion

        #region Windows Specific Stuff
        protected override void WndProc(ref Message m)
        {           
            switch (m.Msg)
            {              
                case ClamWinQuarantine.UM_QUARANTINE_ITEM_PROCESSING_COMPLETE:
                {
                    OnQuarantineItemComplete(m.WParam);
                    break;
                }             
            }

            base.WndProc(ref m);
        }                        		
        #endregion

    }
}