// Name:        ClamWinListViewSearchForm.cs
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

namespace ClamWinApp
{
    #region ClamWinListViewSearchForm class
    public partial class ClamWinListViewSearchForm : Form
    {
        #region Private Data
        /// <summary>
        /// List control for search
        /// </summary>
        private ListView ListCtrl = null;
        /// <summary>
        /// Current item index
        /// </summary>
        private int CurrentItemIndex = 0;
        /// <summary>
        /// Caller window handle
        /// </summary>
        private IntPtr CallerHandle = (IntPtr)0;
        #endregion

        #region Public Constructor
        public ClamWinListViewSearchForm()
        {
            InitializeComponent();
        }
        #endregion
         
        #region Public Functions
        /// <summary>
        /// Shows dialog
        /// </summary>
        /// <param name="listView"></param>
        public void Show(ListView listView, IntPtr Handle)
        {
            ListCtrl = listView;
            comboBoxColumns.Items.Clear();

            foreach (ColumnHeader header in ListCtrl.Columns)
            {
                comboBoxColumns.Items.Add(header.Text);
            }
            comboBoxColumns.SelectedIndex = 0;

            foreach (ListViewItem item in ListCtrl.Items)
            {
                item.Selected = false;
            }

            buttonFind.Enabled = false;

            CallerHandle = Handle;

            Win32API.SetWindowPos(this.Handle,Win32API.HWND_TOPMOST, 0, 0, 0, 0, Win32API.SWP_NOMOVE | Win32API.SWP_NOSIZE);

            base.Show();
        }
        #endregion
        
        #region Event Handlers
        /// <summary>
        /// buttonFind on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFind_Click(object sender, EventArgs e)
        {
            string Text = textBoxTextToSearch.Text;
            
            string Column = (string)comboBoxColumns.SelectedItem;

            int i = 0;
            foreach (ColumnHeader header in ListCtrl.Columns)
            {
                if (Column == header.Text)
                {
                    break;
                }
                i++;
            }

            bool Ok = false;
            for( int k = CurrentItemIndex; k < ListCtrl.Items.Count; k++)            
            {
                ListViewItem item = ListCtrl.Items[k];
                
                item.Selected = false;

                int pos = -1;

                if (checkBoxMatchCase.Checked)
                {
                    pos = item.SubItems[i].Text.IndexOf(Text);
                }
                else
                {
                    pos = item.SubItems[i].Text.IndexOf(Text, StringComparison.OrdinalIgnoreCase);
                }

                if( pos != -1)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    Ok = true;
                    CurrentItemIndex = k+1;
                    Win32API.SetForegroundWindow(CallerHandle);
                    break;
                }                
            }

            if (!Ok)
            {
                MessageBox.Show("The following specified text was not found: "+Text);
            }
        }
        /// <summary>
        /// textBoxTextToSearch TextChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxTextToSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTextToSearch.Text == "")
            {
                buttonFind.Enabled = false;
            }
            else
            {
                buttonFind.Enabled = true;
            }

            CurrentItemIndex = 0;
        }
        /// <summary>
        /// checkBoxMatchCase CheckedChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            CurrentItemIndex = 0;
        }
        #endregion        

        
    }
    #endregion
}