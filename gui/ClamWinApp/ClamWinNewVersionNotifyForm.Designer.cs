// Name:        ClamWinNewVersionNotifyForm.Designer.cs
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
    partial class ClamWinNewVersionNotifyForm
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
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.labelNewVerionValue = new System.Windows.Forms.Label();
            this.labelNewVerion = new System.Windows.Forms.Label();
            this.labelCurrentVersionValue = new System.Windows.Forms.Label();
            this.labelCurrentVersion = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelExMainPanel.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExMainPanel
            // 
            this.panelExMainPanel.BackColor = System.Drawing.Color.SandyBrown;
            this.panelExMainPanel.ChangeCursor = false;
            this.panelExMainPanel.Controls.Add(this.buttonUpdate);
            this.panelExMainPanel.Controls.Add(this.groupBoxInfo);
            this.panelExMainPanel.Controls.Add(this.buttonClose);
            this.panelExMainPanel.DrawCollapseExpandIcons = false;
            this.panelExMainPanel.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExMainPanel.Image = null;
            this.panelExMainPanel.Location = new System.Drawing.Point(0, 0);
            this.panelExMainPanel.MouseSensitive = false;
            this.panelExMainPanel.Name = "panelExMainPanel";
            this.panelExMainPanel.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExMainPanel.Size = new System.Drawing.Size(275, 200);
            this.panelExMainPanel.StartColour = System.Drawing.Color.White;
            this.panelExMainPanel.TabIndex = 8;
            this.panelExMainPanel.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExMainPanel.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExMainPanel.TitleText = "New version of ClamWin is available:";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(12, 152);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(99, 23);
            this.buttonUpdate.TabIndex = 3;
            this.buttonUpdate.Text = "Update now!";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelNewVerionValue);
            this.groupBoxInfo.Controls.Add(this.labelNewVerion);
            this.groupBoxInfo.Controls.Add(this.labelCurrentVersionValue);
            this.groupBoxInfo.Controls.Add(this.labelCurrentVersion);
            this.groupBoxInfo.Location = new System.Drawing.Point(12, 44);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(251, 78);
            this.groupBoxInfo.TabIndex = 1;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Information";
            // 
            // labelNewVerionValue
            // 
            this.labelNewVerionValue.Location = new System.Drawing.Point(116, 48);
            this.labelNewVerionValue.Name = "labelNewVerionValue";
            this.labelNewVerionValue.Size = new System.Drawing.Size(129, 13);
            this.labelNewVerionValue.TabIndex = 3;
            this.labelNewVerionValue.Text = "Current Version:";
            this.labelNewVerionValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelNewVerion
            // 
            this.labelNewVerion.AutoSize = true;
            this.labelNewVerion.Location = new System.Drawing.Point(6, 48);
            this.labelNewVerion.Name = "labelNewVerion";
            this.labelNewVerion.Size = new System.Drawing.Size(69, 13);
            this.labelNewVerion.TabIndex = 2;
            this.labelNewVerion.Text = "New version:";
            // 
            // labelCurrentVersionValue
            // 
            this.labelCurrentVersionValue.Location = new System.Drawing.Point(116, 25);
            this.labelCurrentVersionValue.Name = "labelCurrentVersionValue";
            this.labelCurrentVersionValue.Size = new System.Drawing.Size(129, 13);
            this.labelCurrentVersionValue.TabIndex = 1;
            this.labelCurrentVersionValue.Text = "Current Version:";
            this.labelCurrentVersionValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelCurrentVersion
            // 
            this.labelCurrentVersion.AutoSize = true;
            this.labelCurrentVersion.Location = new System.Drawing.Point(6, 25);
            this.labelCurrentVersion.Name = "labelCurrentVersion";
            this.labelCurrentVersion.Size = new System.Drawing.Size(82, 13);
            this.labelCurrentVersion.TabIndex = 0;
            this.labelCurrentVersion.Text = "Current Version:";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(164, 152);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(99, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ClamWinNewVersionNotifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 200);
            this.Controls.Add(this.panelExMainPanel);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ClamWinNewVersionNotifyForm";
            this.Text = "ClamWinNewVersionNotifyForm";
            this.panelExMainPanel.ResumeLayout(false);
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelsEx.PanelEx panelExMainPanel;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label labelNewVerionValue;
        private System.Windows.Forms.Label labelNewVerion;
        private System.Windows.Forms.Label labelCurrentVersionValue;
        private System.Windows.Forms.Label labelCurrentVersion;
    }
}