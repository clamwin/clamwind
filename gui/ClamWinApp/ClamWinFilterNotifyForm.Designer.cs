// Name:        ClamwinNotifyForm.Designer.cs
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
    partial class ClamWinFilterNotifyForm
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
            this.buttonApply = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelFileName = new System.Windows.Forms.Label();
            this.groupBoxAction = new System.Windows.Forms.GroupBox();
            this.radioButtonDeleteFile = new System.Windows.Forms.RadioButton();
            this.radioButtonQuarantine = new System.Windows.Forms.RadioButton();
            this.radioButtonIgnore = new System.Windows.Forms.RadioButton();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.panelExMainPanel.SuspendLayout();
            this.groupBoxAction.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExMainPanel
            // 
            this.panelExMainPanel.BackColor = System.Drawing.Color.SandyBrown;
            this.panelExMainPanel.ChangeCursor = false;
            this.panelExMainPanel.Controls.Add(this.buttonApply);
            this.panelExMainPanel.Controls.Add(this.buttonClose);
            this.panelExMainPanel.Controls.Add(this.groupBoxInfo);
            this.panelExMainPanel.Controls.Add(this.groupBoxAction);
            this.panelExMainPanel.DrawCollapseExpandIcons = false;
            this.panelExMainPanel.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExMainPanel.Image = null;
            this.panelExMainPanel.Location = new System.Drawing.Point(0, 0);
            this.panelExMainPanel.MouseSensitive = false;
            this.panelExMainPanel.Name = "panelExMainPanel";
            this.panelExMainPanel.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExMainPanel.Size = new System.Drawing.Size(279, 389);
            this.panelExMainPanel.StartColour = System.Drawing.Color.White;
            this.panelExMainPanel.TabIndex = 8;
            this.panelExMainPanel.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExMainPanel.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExMainPanel.TitleText = "On-Access Scaner notify:";
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(12, 351);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 5;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.Location = new System.Drawing.Point(6, 108);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(243, 84);
            this.labelMessage.TabIndex = 4;
            this.labelMessage.Text = "Message - ";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(188, 351);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Hide";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelFileName
            // 
            this.labelFileName.Location = new System.Drawing.Point(6, 16);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(243, 92);
            this.labelFileName.TabIndex = 3;
            this.labelFileName.Text = "File - ";
            // 
            // groupBoxAction
            // 
            this.groupBoxAction.Controls.Add(this.radioButtonIgnore);
            this.groupBoxAction.Controls.Add(this.radioButtonDeleteFile);
            this.groupBoxAction.Controls.Add(this.radioButtonQuarantine);
            this.groupBoxAction.Location = new System.Drawing.Point(12, 238);
            this.groupBoxAction.Name = "groupBoxAction";
            this.groupBoxAction.Size = new System.Drawing.Size(255, 96);
            this.groupBoxAction.TabIndex = 1;
            this.groupBoxAction.TabStop = false;
            this.groupBoxAction.Text = "Perform one of the following actions";
            // 
            // radioButtonDeleteFile
            // 
            this.radioButtonDeleteFile.AutoSize = true;
            this.radioButtonDeleteFile.Location = new System.Drawing.Point(15, 68);
            this.radioButtonDeleteFile.Name = "radioButtonDeleteFile";
            this.radioButtonDeleteFile.Size = new System.Drawing.Size(72, 17);
            this.radioButtonDeleteFile.TabIndex = 1;
            this.radioButtonDeleteFile.Text = "Delete file";
            this.radioButtonDeleteFile.UseVisualStyleBackColor = true;
            // 
            // radioButtonQuarantine
            // 
            this.radioButtonQuarantine.AutoSize = true;
            this.radioButtonQuarantine.Location = new System.Drawing.Point(15, 46);
            this.radioButtonQuarantine.Name = "radioButtonQuarantine";
            this.radioButtonQuarantine.Size = new System.Drawing.Size(93, 17);
            this.radioButtonQuarantine.TabIndex = 0;
            this.radioButtonQuarantine.Text = "Quarantine file";
            this.radioButtonQuarantine.UseVisualStyleBackColor = true;
            // 
            // radioButtonIgnore
            // 
            this.radioButtonIgnore.AutoSize = true;
            this.radioButtonIgnore.Checked = true;
            this.radioButtonIgnore.Location = new System.Drawing.Point(15, 23);
            this.radioButtonIgnore.Name = "radioButtonIgnore";
            this.radioButtonIgnore.Size = new System.Drawing.Size(55, 17);
            this.radioButtonIgnore.TabIndex = 2;
            this.radioButtonIgnore.Text = "Ignore";
            this.radioButtonIgnore.UseVisualStyleBackColor = true;
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelMessage);
            this.groupBoxInfo.Controls.Add(this.labelFileName);
            this.groupBoxInfo.Location = new System.Drawing.Point(12, 37);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(255, 195);
            this.groupBoxInfo.TabIndex = 3;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Information";
            // 
            // ClamWinFilterNotifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 389);
            this.Controls.Add(this.panelExMainPanel);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ClamWinFilterNotifyForm";
            this.Text = "ClamWinFilterNotifyForm";
            this.panelExMainPanel.ResumeLayout(false);
            this.groupBoxAction.ResumeLayout(false);
            this.groupBoxAction.PerformLayout();
            this.groupBoxInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelsEx.PanelEx panelExMainPanel;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBoxAction;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.RadioButton radioButtonDeleteFile;
        private System.Windows.Forms.RadioButton radioButtonQuarantine;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.RadioButton radioButtonIgnore;
    }
}