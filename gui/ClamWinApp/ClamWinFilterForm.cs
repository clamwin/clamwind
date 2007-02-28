// Name:        ClamwinFilterForm.cs
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
    #region ClamWinFilterForm class
    public partial class ClamWinFilterForm : Form
    {
        #region Private Data
        #endregion              

        #region Public Constructor
        public ClamWinFilterForm()
        {
            InitializeComponent();

            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonOK.DialogResult = DialogResult.OK;

            AcceptButton = buttonOK;
            CancelButton = buttonCancel;
        }  
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Manage buttons Enabled states
        /// </summary>
        void ManageButtons()
        {
            buttonExcludeEdit.Enabled = true;
            buttonExcludeMoveUp.Enabled = true;
            buttonExcludeMoveDown.Enabled = true;
            buttonExcludeRemove.Enabled = true;
            buttonExcludeNew.Enabled = true;
            buttonIncludeEdit.Enabled = true;
            buttonIncludeMoveUp.Enabled = true;
            buttonIncludeMoveDown.Enabled = true;
            buttonIncludeRemove.Enabled = true;
            buttonIncludeNew.Enabled = true;

            if (listViewExclude.SelectedIndices.Count != 0)
            {
                int count = listViewExclude.SelectedIndices[0];
                if (listViewExclude.Items[count].Text == "")
                {
                    buttonExcludeEdit.Enabled = false;
                    buttonExcludeMoveUp.Enabled = false;
                    buttonExcludeMoveDown.Enabled = false;
                    buttonExcludeRemove.Enabled = false;
                }
            }

            if (listViewInclude.SelectedIndices.Count != 0)
            {
                int count = listViewInclude.SelectedIndices[0];
                if (listViewInclude.Items[count].Text == "")
                {
                    buttonIncludeEdit.Enabled = false;
                    buttonIncludeMoveUp.Enabled = false;
                    buttonIncludeMoveDown.Enabled = false;
                    buttonIncludeRemove.Enabled = false;
                }
            }

            if (listViewExclude.Items.Count == 0)
            {
                buttonExcludeMoveUp.Enabled = false;
                buttonExcludeMoveDown.Enabled = false;
                buttonExcludeEdit.Enabled = false;
                buttonExcludeRemove.Enabled = false;
            }

            if (listViewExclude.SelectedIndices.Count != 0)
            {

                if (listViewExclude.SelectedIndices[0] == 0)
                {
                    buttonExcludeMoveUp.Enabled = false;
                }

                if (listViewExclude.SelectedIndices[0] == listViewExclude.Items.Count - 2)
                {
                    buttonExcludeMoveDown.Enabled = false;
                }
            }
            else
            {
                buttonExcludeEdit.Enabled = false;
                buttonExcludeMoveUp.Enabled = false;
                buttonExcludeMoveDown.Enabled = false;
                buttonExcludeRemove.Enabled = false;
            }            

            if (listViewInclude.Items.Count == 0)
            {
                buttonIncludeMoveUp.Enabled = false;
                buttonIncludeMoveDown.Enabled = false;
                buttonIncludeEdit.Enabled = false;
                buttonIncludeRemove.Enabled = false;
            }

            if (listViewInclude.SelectedIndices.Count != 0)
            {

                if (listViewInclude.SelectedIndices[0] == 0)
                {
                    buttonIncludeMoveUp.Enabled = false;
                }

                if (listViewInclude.SelectedIndices[0] == listViewInclude.Items.Count - 2)
                {
                    buttonIncludeMoveDown.Enabled = false;
                }
            }
            else
            {
                buttonIncludeMoveUp.Enabled = false;
                buttonIncludeMoveDown.Enabled = false;
                buttonIncludeEdit.Enabled = false;
                buttonIncludeRemove.Enabled = false;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Show filter form modally and add caller string to form caption
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public DialogResult FilterDoModal(string caller, ref ClamWinFilterData data)
        {
            this.Text = "Filter: " + caller;

            listViewExclude.Items.Clear();
            listViewInclude.Items.Clear();            

            foreach(string pattern in data.ExcludePatterns)
            {
                listViewExclude.Items.Add(pattern);
            }

            foreach (string pattern in data.IncludePatterns)
            {
                listViewInclude.Items.Add(pattern);
            }

            listViewExclude.Items.Add("");
            listViewInclude.Items.Add("");

            ManageButtons();

            DialogResult result =  ShowDialog();

            if (result != DialogResult.OK)
            {
                return result;
            }

            data.ExcludePatterns.Clear();
            foreach (ListViewItem item in listViewExclude.Items)
            {
                if (item.Text != "")
                {
                    data.ExcludePatterns.Add(item.Text);
                }
            }

            data.IncludePatterns.Clear();
            foreach (ListViewItem item in listViewInclude.Items)
            {
                if (item.Text != "")
                {
                    data.IncludePatterns.Add(item.Text);
                }
            }

            return result;
        }
        #endregion        

        #region Event Handlers
        
        #region Button Event Handlers
        /// <summary>
        /// buttonExcludeEdit on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExcludeEdit_Click(object sender, EventArgs e)
        {
            if (listViewExclude.SelectedItems.Count != 1)
            {
                return;
            }

            listViewExclude.SelectedItems[0].BeginEdit();
        }
        /// <summary>
        /// buttonExcludeNew on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExcludeNew_Click(object sender, EventArgs e)
        {
            int index = listViewExclude.Items.Count-1;
            ListViewItem item = listViewExclude.Items[index];
            item.BeginEdit();                        
        }
        /// <summary>
        /// buttonExcludeRemove on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExcludeRemove_Click(object sender, EventArgs e)
        {
            if (listViewExclude.SelectedItems.Count != 1)
            {
                return;
            }
            
            ListViewItem item = listViewExclude.SelectedItems[0];
            
            listViewExclude.Items.Remove(item);
        }
        /// <summary>
        /// buttonExcludeMoveUp on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExcludeMoveUp_Click(object sender, EventArgs e)
        {
            if (listViewExclude.SelectedItems.Count != 1)
            {
                return;
            }

            ListViewItem item = listViewExclude.SelectedItems[0];

            string text = item.Text;
            int position = item.Index;

            listViewExclude.Items.Remove(item);

            position--;
            
            item = listViewExclude.Items.Insert(position, text);

            item.Selected = true;
        }
        /// <summary>
        /// buttonExcludeMoveDown on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExcludeMoveDown_Click(object sender, EventArgs e)
        {
            if (listViewExclude.SelectedItems.Count != 1)
            {
                return;
            }

            ListViewItem item = listViewExclude.SelectedItems[0];           

            string text = item.Text;
            int position = item.Index;

            listViewExclude.Items.Remove(item);

            position++;

            item = listViewExclude.Items.Insert(position, text);

            item.Selected = true;
        }
        /// <summary>
        /// buttonIncludeEdit on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonIncludeEdit_Click(object sender, EventArgs e)
        {
            if (listViewInclude.SelectedItems.Count != 1)
            {
                return;
            }

            listViewInclude.SelectedItems[0].BeginEdit();
        }
        /// <summary>
        /// buttonIncludeNew on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonIncludeNew_Click(object sender, EventArgs e)
        {
            int index = listViewInclude.Items.Count - 1;
            ListViewItem item = listViewInclude.Items[index];
            item.BeginEdit();                                    
        }
        /// <summary>
        /// buttonIncludeRemove on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonIncludeRemove_Click(object sender, EventArgs e)
        {
            if (listViewInclude.SelectedItems.Count != 1)
            {
                return;
            }

            ListViewItem item = listViewInclude.SelectedItems[0];            

            listViewInclude.Items.Remove(item);
        }
        /// <summary>
        /// buttonIncludeMoveUp on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonIncludeMoveUp_Click(object sender, EventArgs e)
        {
            if (listViewInclude.SelectedItems.Count != 1)
            {
                return;
            }

            ListViewItem item = listViewInclude.SelectedItems[0];

            string text = item.Text;
            int position = item.Index;

            listViewInclude.Items.Remove(item);

            position--;

            item = listViewInclude.Items.Insert(position, text);

            item.Selected = true;
        }
        /// <summary>
        /// buttonIncludeMoveDown on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonIncludeMoveDown_Click(object sender, EventArgs e)
        {
            if (listViewInclude.SelectedItems.Count != 1)
            {
                return;
            }

            ListViewItem item = listViewInclude.SelectedItems[0];

            string text = item.Text;
            int position = item.Index;

            listViewInclude.Items.Remove(item);

            position++;

            item = listViewInclude.Items.Insert(position, text);

            item.Selected = true;
        }
        #endregion

        #region ListView Event Handlers
        /// <summary>
        /// Common ListView SelectedIndexChanged Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManageButtons();
        }
        /// <summary>
        /// listViewExclude AfterLabelEdit Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewExclude_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label != "")
                {
                    listViewExclude.Items.Add("");
                }
            }
        }
        /// <summary>
        /// listViewInclude AfterLabelEdit Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewInclude_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label != "")
                {
                    listViewInclude.Items.Add("");
                }
            }
        }
        #endregion

        #endregion        
    }
    #endregion 
}