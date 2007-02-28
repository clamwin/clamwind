// Name:        ClamWinInfectedNotifyForm.Designer.cs
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

namespace ClamWinApp
{
    partial class ClamWinInfectedNotifyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelExMainPanel = new PanelsEx.PanelEx();
            this.groupBoxAction = new System.Windows.Forms.GroupBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelExMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExMainPanel
            // 
            this.panelExMainPanel.BackColor = System.Drawing.Color.SandyBrown;
            this.panelExMainPanel.ChangeCursor = false;
            this.panelExMainPanel.Controls.Add(this.groupBoxAction);
            this.panelExMainPanel.Controls.Add(this.buttonClose);
            this.panelExMainPanel.DrawCollapseExpandIcons = false;
            this.panelExMainPanel.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExMainPanel.Image = null;
            this.panelExMainPanel.Location = new System.Drawing.Point(0, 0);
            this.panelExMainPanel.MouseSensitive = false;
            this.panelExMainPanel.Name = "panelExMainPanel";
            this.panelExMainPanel.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExMainPanel.Size = new System.Drawing.Size(275, 386);
            this.panelExMainPanel.StartColour = System.Drawing.Color.White;
            this.panelExMainPanel.TabIndex = 7;
            this.panelExMainPanel.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExMainPanel.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExMainPanel.TitleText = "Warning:";
            // 
            // groupBoxAction
            // 
            this.groupBoxAction.Location = new System.Drawing.Point(12, 44);
            this.groupBoxAction.Name = "groupBoxAction";
            this.groupBoxAction.Size = new System.Drawing.Size(251, 130);
            this.groupBoxAction.TabIndex = 1;
            this.groupBoxAction.TabStop = false;
            this.groupBoxAction.Text = "Action";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(188, 351);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ClamWinInfectedNotifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 386);
            this.Controls.Add(this.panelExMainPanel);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ClamWinInfectedNotifyForm";
            this.Text = "ClamWinInfectedNotifyForm";
            this.panelExMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelsEx.PanelEx panelExMainPanel;
        private System.Windows.Forms.GroupBox groupBoxAction;
        private System.Windows.Forms.Button buttonClose;

    }
}