// Name:        ClamWinQuarantineForm.cs
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ClamWinApp
{
    #region ListViewQuarantineComparer class
    #endregion

    #region ClamWinQuarantineForm class
    public partial class ClamWinQuarantineForm : Form
    {
        #region Private Data
        /// <summary>
        /// If form waiting for rescan
        /// </summary>
        private bool WaitingForRescan = false;
        /// <summary>
        /// Last sorted column for Quarantine Items
        /// </summary>
        private int QuarantineSortColumn = -1;
        #endregion

        #region Public Constructor
        public ClamWinQuarantineForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Data 
        /// <summary>
        /// Enable close
        /// </summary>
        public bool EnableClose = false;
        /// <summary>
        /// MainForm handle
        /// </summary>
        public IntPtr MainFomrHandle = (IntPtr)0;
        #endregion

        #region Event Handlers

        #region Form Event Handlers
        /// <summary>
        /// ClamWinQuarantineForm Resize event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClamWinQuarantineForm_Resize(object sender, EventArgs e)
        {
            AdjustSizes();
        }
        /// <summary>
        /// ClamWinQuarantineForm Load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClamWinQuarantineForm_Load(object sender, EventArgs e)
        {
            AdjustSizes();

            UpdateQuarantineItems();
        }
        /// <summary>
        /// ClamWinQuarantineForm Closing event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClamWinQuarantineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (EnableClose)
            {
                return;
            }

            e.Cancel = true;
            Hide();
        }
        #endregion

        #region Button Event Handlers        
        /// <summary>
        /// buttonQuarantineFile on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonQuarantinedActions_Click(object sender, EventArgs e)
        {
            Point screen = buttonQuarantinedActions.PointToScreen(new Point(0, 0));
            int x = screen.X;
            int y = screen.Y + buttonQuarantinedActions.Height;
            contextMenuQuarantined.Show(new Point(x, y));
        }
        /// <summary>
        /// buttonQueuedActions on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonQueuedActions_Click(object sender, EventArgs e)
        {
            Point screen = buttonQueuedActions.PointToScreen(new Point(0, 0));
            int x = screen.X;
            int y = screen.Y + buttonQueuedActions.Height;
            contextMenuQueued.Show(new Point(x, y));
        }
        #endregion

        #region User Defined Event Handlers
        /// <summary>
        /// Compress start event handler
        /// </summary>
        /// <param name="data"></param>
        private void OnQuarantineProcessingStart(IntPtr data)
        {
            ClamWinQuarantine.NotifyData Data = (ClamWinQuarantine.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinQuarantine.NotifyData));
            Marshal.FreeHGlobal(data);

            progressBarCompressingProgress.Visible = true;
            progressBarCompressingProgress.Value = 0;

            labelFileName.Visible = true;
            labelFileName.Text = (Data.Quarantine ? "Compressing: " : "Exctracting: ") + Data.FileName;
        }
        /// <summary>
        /// Compress complete event handler
        /// </summary>        
        private void OnQuarantineProcessingComplete(IntPtr data)
        {
            ClamWinQuarantine.NotifyData Data = (ClamWinQuarantine.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinQuarantine.NotifyData));
            Marshal.FreeHGlobal(data);

            progressBarCompressingProgress.Visible = false;            
            labelFileName.Visible = false;

            foreach (ListViewItem item in listViewQueuedItems.Items)
            {
                if ((long)item.Tag == Data.ID)
                {
                    listViewQueuedItems.Items.Remove(item);
                    break;
                }
            }          

            UpdateQuarantineItems();
        }
        /// <summary>
        /// Compress progress event handler
        /// </summary>
        /// <param name="data"></param>
        private void OnQuarantineProcessingProgress(IntPtr data)
        {
            ClamWinQuarantine.NotifyData Data = (ClamWinQuarantine.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinQuarantine.NotifyData));
            Marshal.FreeHGlobal(data);

            progressBarCompressingProgress.Value = Data.Progress;            
        }
        /// <summary>
        /// Queue on new item event handler
        /// </summary>
        /// <param name="data"></param>
        private void OnQuarantineQueueNewItem(IntPtr data)
        {
            ClamWinQuarantine.NotifyData Data = (ClamWinQuarantine.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinQuarantine.NotifyData));
            Marshal.FreeHGlobal(data);

            ListViewItem item = listViewQueuedItems.Items.Add(System.IO.Path.GetFileName(Data.FileName));
            item.Tag = Data.ID;
            item.SubItems.Add(Data.FileName);
            item.SubItems.Add(Data.Size.ToString());
            item.SubItems.Add(Data.Quarantine ? "Quarantine" : "Unquarantine");            
        }
        /// <summary>
        /// On item processing canceled event handler
        /// </summary>
        /// <param name="data"></param>
        private void OnQuarantineProcessingCanceled(IntPtr data)
        {
            ClamWinQuarantine.NotifyData Data = (ClamWinQuarantine.NotifyData)Marshal.PtrToStructure(data, typeof(ClamWinQuarantine.NotifyData));
            Marshal.FreeHGlobal(data);

            foreach (ListViewItem item in listViewQueuedItems.Items)
            {
                if ((long)item.Tag == Data.ID)
                {
                    listViewQueuedItems.Items.Remove(item);
                    break;
                }
            }           
        }
        /// <summary>
        /// Quarantine quueu processing complete
        /// </summary>
        private void OnQuarantineQueueDone()
        {
            if (WaitingForRescan)
            {
                Win32API.PostMessage(MainFomrHandle, ClamWinMainForm.UM_SCAN_QUARANTINE_RESCAN,0,0);
                WaitingForRescan = false;
            }
        }
        #endregion        

        #region Menu Event Handlers
        /// <summary>
        /// toolStripMenuItemQueueCancelProcessing on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemQueueCancelProcessing_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewQueuedItems.Items)
            {
                if(!item.Selected)
                {
                    continue;
                }
                IntPtr[] Listeners = new IntPtr[1];
                Listeners[0] = this.Handle;
                ClamWinQuarantine.CancelItemProcessing((long)item.Tag, Listeners);
            }
        }
        /// <summary>
        /// toolStripMenuItemQuarantine on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemQuarantine_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (ClamWinQuarantine.IsFileInQuarantine(openFileDialog.FileName))
            {
                MessageBox.Show("File is already quarantined!");
                return;
            }

            IntPtr[] Listeners = new IntPtr[2];
            Listeners[0] = this.Handle;
            Listeners[1] = MainFomrHandle;
            ClamWinQuarantine.QuarantineResults result = ClamWinQuarantine.QuarantineFile(openFileDialog.FileName, Listeners);

            if (result != ClamWinQuarantine.QuarantineResults.Success)
            {
                MessageBox.Show("Operation failed with message: " + ClamWinQuarantine.QuarantineResultToString(result));
            }
        }
        /// <summary>
        /// toolStripMenuItemUnquarantine on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemUnquarantine_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewQuarantineItems.Items)
            {
                if (!item.Selected)
                {
                    continue;
                }
                IntPtr[] Listeners = new IntPtr[2];
                Listeners[0] = this.Handle;
                Listeners[1] = MainFomrHandle;
                ClamWinQuarantine.QuarantineResults result = ClamWinQuarantine.UnquarantineFile(item.SubItems[1].Text, Listeners, false);
                if (result != ClamWinQuarantine.QuarantineResults.Success)
                {
                    MessageBox.Show("Operation failed with message: " + ClamWinQuarantine.QuarantineResultToString(result));
                    break;
                }
            }     
        }
        /// <summary>
        /// toolStripMenuItemQuaSelectAll on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemQuaSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewQuarantineItems.Items)
            {
                item.Selected = true;                
            }     
        }
        /// <summary>
        /// toolStripMenuItemQueueSelectAll on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemQueueSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewQueuedItems.Items)
            {
                item.Selected = true;
            }  
        }
        /// <summary>
        /// toolStripMenuItemQuaRescan on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemQuaRescan_Click(object sender, EventArgs e)
        {
            if (ClamWinQuarantine.IsWorking())
            {
                MessageBox.Show("Quarantine queue contain unhandled items.Plese wait until queue is done.");
                return;
            }

            foreach (ListViewItem item in listViewQuarantineItems.Items)
            {
                if (!item.Selected)
                {
                    continue;
                }
                IntPtr[] Listeners = new IntPtr[2];
                Listeners[0] = this.Handle;
                Listeners[1] = MainFomrHandle;
                ClamWinQuarantine.UnquarantineFile(item.SubItems[1].Text, Listeners, true);
            }

            ClamWinQuarantine.LockQueue();

            WaitingForRescan = true;
        }
        #endregion

        #region Tab Event Handlers
        /// <summary>
        /// tabControlQuarantine SelectedIndexChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlQuarantine_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustSizes();
        }
        #endregion

        #region ListView Event Handlers
        /// <summary>
        /// listViewQuarantineItems On Column click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewQuarantineItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {             
            if (e.Column != QuarantineSortColumn)
            {                
                QuarantineSortColumn = e.Column;                
                listViewQuarantineItems.Sorting = SortOrder.Ascending;
            }
            else
            {
                listViewQuarantineItems.Sorting = listViewQuarantineItems.Sorting == SortOrder.Ascending 
                                                  ? SortOrder.Descending 
                                                  : SortOrder.Ascending;
            }

            listViewQuarantineItems.Sort();            
            
            listViewQuarantineItems.ListViewItemSorter = new ListViewQuarantineComparer(e.Column,
                                                              listViewQuarantineItems.Sorting);
        }
        #endregion

        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Fill quarantine items list with database records
        /// </summary>
        private void UpdateQuarantineItems()
        {
            listViewQuarantineItems.Items.Clear();

            //ListViewItem item = listViewQuarantineItems.Items

            string command = "SELECT * FROM QuarantineItems;";

            ArrayList list;
            ClamWinDatabase.ExecReader(command, out list);

            if (list.Count == 0)
            {
                return;
            }

            int FieldsPerRecord = ClamWinDatabase.QuarantineItemsFPR;
            int RecordsCount = list.Count / FieldsPerRecord;

            for (int i = 0; i < RecordsCount; i++)
            {
                string PathName = (string)list[i * FieldsPerRecord + 1];
                string QuarantinePathName = (string)list[i * FieldsPerRecord + 2];
                string time = (string)list[i * FieldsPerRecord + 3];
                DateTime Time = DateTime.FromBinary(long.Parse(time));
                long Size = long.Parse((string)list[i * FieldsPerRecord + 4]);

                if (!ClamWinQuarantine.CheckQuarantinedFile(QuarantinePathName, Size))
                {
                    continue;
                }
                

                string FileName = System.IO.Path.GetFileName(PathName);

                ListViewItem item = listViewQuarantineItems.Items.Add(FileName);
                item.SubItems.Add(PathName);
                item.SubItems.Add(Time.ToShortDateString() + " " + Time.ToShortTimeString());
                item.SubItems.Add(Size.ToString());
            }


        }
        /// <summary>
        /// Adjust controls
        /// </summary>
        private void AdjustSizes()
        {
            tabControlQuarantine.Size = new Size(this.Size.Width - 32, this.Size.Height - 148);

            labelFileName.Size = new Size(this.Size.Width - 32, 13);
            
            
            progressBarCompressingProgress.Size = new Size(this.Size.Width - 32, 20);

            buttonQuarantinedActions.Location = new Point(tabPageQuarantineItems.Size.Width - 81,
                                                       tabPageQuarantineItems.Size.Height - 29);

            buttonQueuedActions.Location = new Point(tabPageQuarantineQueue.Size.Width - 81,
                                                     tabPageQuarantineQueue.Size.Height - 29);

            
            listViewQuarantineItems.Size = new Size(tabPageQuarantineItems.Size.Width - 12,
                                             tabPageQuarantineItems.Size.Height - 38);

            listViewQueuedItems.Size = new Size(tabPageQuarantineQueue.Size.Width - 12,
                                           tabPageQuarantineQueue.Size.Height - 38);
            
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

                case ClamWinQuarantine.UM_QUARANTINE_ITEM_PROCESSING_START:
                {
                    OnQuarantineProcessingStart(m.WParam);
                    break;
                }
                case ClamWinQuarantine.UM_QUARANTINE_ITEM_PROCESSING_COMPLETE:
                {
                    OnQuarantineProcessingComplete(m.WParam);
                    break;
                }
                case ClamWinQuarantine.UM_QUARANTINE_ITEM_PROCESSING_PROGRESS:
                {
                    OnQuarantineProcessingProgress(m.WParam);
                    break;
                }
                case ClamWinQuarantine.UM_QUARANTINE_QUEUE_NEW_ITEM:
                {
                    OnQuarantineQueueNewItem(m.WParam);
                    break;
                }
                case ClamWinQuarantine.UM_QUARANTINE_ITEM_PROCESSING_CANCELED:
                {
                    OnQuarantineProcessingCanceled(m.WParam);
                    break;
                }
                case ClamWinQuarantine.UM_QUARANTINE_QUEUE_DONE:
                {
                    OnQuarantineQueueDone();
                    break;
                }
            }

            base.WndProc(ref m);
        }
        #endregion                                                                                    

        #region ListViewQuarantineComparer class
        private class ListViewQuarantineComparer : IComparer 
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
            public ListViewQuarantineComparer() 
            {
                Column=0;
                Order = SortOrder.Ascending;
            }
            /// <summary>
            /// Public constructor
            /// </summary>
            /// <param name="column"></param>
            /// <param name="order"></param>
            public ListViewQuarantineComparer(int column, SortOrder order) 
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
                
                if (Column == 0 || Column == 1)
                {
                    // Name and path
                    returnVal = String.Compare(X, Y);
                }
                else if (Column == 2)
                { 
                    // Time
                    DateTime timeX = DateTime.Parse(X);
                    DateTime timeY = DateTime.Parse(Y);

                    returnVal = DateTime.Compare(timeX, timeY);
                }
                else if (Column == 3)
                { 
                    // Size
                    long longX = long.Parse(X);
                    long longY = long.Parse(Y);
                    returnVal = longX == longY ? 0 : (longX < longY ? -1 : 1);
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