// Name:        ClamWinNewVersionNotifyForm.cs
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
    #region ClamWinNewVersionNotifyForm class
    public partial class ClamWinNewVersionNotifyForm : ClamWinTrayNotifier
    {
        #region Public Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Current"></param>
        /// <param name="New"></param>
        public ClamWinNewVersionNotifyForm(ClamWinVersion.Version Current,ClamWinVersion.Version New)
        {
            InitializeComponent();

            labelCurrentVersionValue.Text = Current.ToString();
            labelNewVerionValue.Text = New.ToString();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        ///  buttonClose on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.StartDisappearing();
        }
        /// <summary>
        /// buttonUpdate on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdate_Click(object sender, EventArgs e)
        {

        }
        #endregion
        
    }
    #endregion
}