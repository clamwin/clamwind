// Name:        GroupBoxEx.cs
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
using System.Windows.Forms;

namespace ClamWinApp
{
    #region GroupBoxEx class
    /// <summary>
    /// Extended group box fo statistics and settings information
    /// </summary>
    class GroupBoxEx : GroupBox
    {
        /// <summary>
        /// Pressed event delegat declaration
        /// </summary>
        /// <param name="sender"></param>
        public delegate void OnPressed(object sender);
        /// <summary>
        /// Event fires when group box or link label control on box is clicked
        /// </summary>
        public event OnPressed Pressed;

        /// <summary>
        /// Public constructor
        /// </summary>
        public GroupBoxEx()
        { 
            this.MouseEnter += new EventHandler(GroupBoxEx_MouseEnter);
            this.MouseLeave += new EventHandler(GroupBoxEx_MouseLeave);
            this.Click += new EventHandler(GroupBoxEx_Click);                 
        }
        
        /// <summary>
        /// ControlAdded event handler
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control is LinkLabel)
            {
                LinkLabel label = (LinkLabel)e.Control;
                label.MouseEnter += new EventHandler(GroupBoxEx_MouseEnter);
                label.MouseLeave += new EventHandler(GroupBoxEx_MouseLeave);
                label.Click += new EventHandler(GroupBoxEx_Click);
                label.LinkBehavior = LinkBehavior.NeverUnderline;
            }
            
            base.OnControlAdded(e);
        }

        /// <summary>
        /// MouseEnter common event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBoxEx_MouseEnter(object sender, EventArgs e)
        {
            if (sender is LinkLabel)
            {
                LinkLabel label = (LinkLabel)sender;

                if (label.Parent is GroupBox)
                {
                    GroupBox group = (GroupBox)label.Parent;
                    foreach (Control control in group.Controls)
                    {
                        if (control is LinkLabel)
                        {
                            label = (LinkLabel)control;
                            label.LinkBehavior = LinkBehavior.AlwaysUnderline;
                        }
                    }
                }
                return;
            }

            if (sender is GroupBox)
            {
                GroupBox group = (GroupBox)sender;
                foreach (Control control in group.Controls)
                {
                    if (control is LinkLabel)
                    {
                        LinkLabel label = (LinkLabel)control;
                        label.LinkBehavior = LinkBehavior.AlwaysUnderline;
                    }
                }
            }
        }
        /// <summary>
        /// MouseLeave common event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBoxEx_MouseLeave(object sender, EventArgs e)
        {
            if (sender is LinkLabel)
            {
                LinkLabel label = (LinkLabel)sender;

                if (label.Parent is GroupBox)
                {
                    GroupBox group = (GroupBox)label.Parent;
                    foreach (Control control in group.Controls)
                    {
                        if (control is LinkLabel)
                        {
                            label = (LinkLabel)control;
                            label.LinkBehavior = LinkBehavior.NeverUnderline;
                        }
                    }
                }
                return;
            }

            if (sender is GroupBox)
            {
                GroupBox group = (GroupBox)sender;
                foreach (Control control in group.Controls)
                {
                    if (control is LinkLabel)
                    {
                        LinkLabel label = (LinkLabel)control;
                        label.LinkBehavior = LinkBehavior.NeverUnderline;
                    }
                }
            }
        }
        /// <summary>
        /// Click common event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBoxEx_Click(object sender, EventArgs e)
        {
            if (Pressed != null)
            {
                Pressed(this);
            }
        }      
    }
    #endregion
}
