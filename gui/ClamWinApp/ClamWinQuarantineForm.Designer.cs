// Name:        ClamWinQuarantineForm.Designer.cs
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
    partial class ClamWinQuarantineForm
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
            this.listViewQuarantineItems = new System.Windows.Forms.ListView();
            this.columnHeaderItem = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPath = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderTime = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.contextMenuQuarantined = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemQuarantine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUnquarantine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemQuaSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBarCompressingProgress = new System.Windows.Forms.ProgressBar();
            this.labelFileName = new System.Windows.Forms.Label();
            this.tabControlQuarantine = new System.Windows.Forms.TabControl();
            this.tabPageQuarantineItems = new System.Windows.Forms.TabPage();
            this.buttonQuarantinedActions = new System.Windows.Forms.Button();
            this.tabPageQuarantineQueue = new System.Windows.Forms.TabPage();
            this.buttonQueuedActions = new System.Windows.Forms.Button();
            this.listViewQueuedItems = new System.Windows.Forms.ListView();
            this.columnHeaderQueuedFile = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderQueuedPath = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderQueuedSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderQueuedJob = new System.Windows.Forms.ColumnHeader();
            this.contextMenuQueued = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemQueueCancelProcessing = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemQueueSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.toolStripMenuItemQuaRescan = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuQuarantined.SuspendLayout();
            this.tabControlQuarantine.SuspendLayout();
            this.tabPageQuarantineItems.SuspendLayout();
            this.tabPageQuarantineQueue.SuspendLayout();
            this.contextMenuQueued.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewQuarantineItems
            // 
            this.listViewQuarantineItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderItem,
            this.columnHeaderPath,
            this.columnHeaderTime,
            this.columnHeaderSize});
            this.listViewQuarantineItems.ContextMenuStrip = this.contextMenuQuarantined;
            this.listViewQuarantineItems.FullRowSelect = true;
            this.listViewQuarantineItems.HideSelection = false;
            this.listViewQuarantineItems.Location = new System.Drawing.Point(6, 6);
            this.listViewQuarantineItems.Name = "listViewQuarantineItems";
            this.listViewQuarantineItems.ShowItemToolTips = true;
            this.listViewQuarantineItems.Size = new System.Drawing.Size(610, 187);
            this.listViewQuarantineItems.TabIndex = 0;
            this.listViewQuarantineItems.UseCompatibleStateImageBehavior = false;
            this.listViewQuarantineItems.View = System.Windows.Forms.View.Details;
            this.listViewQuarantineItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewQuarantineItems_ColumnClick);
            // 
            // columnHeaderItem
            // 
            this.columnHeaderItem.Text = "File name";
            this.columnHeaderItem.Width = 214;
            // 
            // columnHeaderPath
            // 
            this.columnHeaderPath.Text = "Initial location";
            this.columnHeaderPath.Width = 164;
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "Time ";
            this.columnHeaderTime.Width = 103;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Initial size";
            this.columnHeaderSize.Width = 82;
            // 
            // contextMenuQuarantined
            // 
            this.contextMenuQuarantined.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.contextMenuQuarantined.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemQuarantine,
            this.toolStripMenuItemUnquarantine,
            this.toolStripMenuItemQuaSelectAll,
            this.toolStripMenuItemQuaRescan});
            this.contextMenuQuarantined.Name = "contextMenuQuarantined";
            this.contextMenuQuarantined.ShowImageMargin = false;
            this.contextMenuQuarantined.Size = new System.Drawing.Size(151, 92);
            // 
            // toolStripMenuItemQuarantine
            // 
            this.toolStripMenuItemQuarantine.Name = "toolStripMenuItemQuarantine";
            this.toolStripMenuItemQuarantine.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItemQuarantine.Text = "Quarantine file...";
            this.toolStripMenuItemQuarantine.Click += new System.EventHandler(this.toolStripMenuItemQuarantine_Click);
            // 
            // toolStripMenuItemUnquarantine
            // 
            this.toolStripMenuItemUnquarantine.Name = "toolStripMenuItemUnquarantine";
            this.toolStripMenuItemUnquarantine.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItemUnquarantine.Text = "Unquarantine";
            this.toolStripMenuItemUnquarantine.Click += new System.EventHandler(this.toolStripMenuItemUnquarantine_Click);
            // 
            // toolStripMenuItemQuaSelectAll
            // 
            this.toolStripMenuItemQuaSelectAll.Name = "toolStripMenuItemQuaSelectAll";
            this.toolStripMenuItemQuaSelectAll.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItemQuaSelectAll.Text = "Select All...";
            this.toolStripMenuItemQuaSelectAll.Click += new System.EventHandler(this.toolStripMenuItemQuaSelectAll_Click);
            // 
            // progressBarCompressingProgress
            // 
            this.progressBarCompressingProgress.Location = new System.Drawing.Point(9, 65);
            this.progressBarCompressingProgress.Name = "progressBarCompressingProgress";
            this.progressBarCompressingProgress.Size = new System.Drawing.Size(580, 18);
            this.progressBarCompressingProgress.TabIndex = 2;
            this.progressBarCompressingProgress.Visible = false;
            // 
            // labelFileName
            // 
            this.labelFileName.AutoEllipsis = true;
            this.labelFileName.Location = new System.Drawing.Point(9, 32);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(580, 17);
            this.labelFileName.TabIndex = 3;
            this.labelFileName.Text = "FileName";
            this.labelFileName.Visible = false;
            // 
            // tabControlQuarantine
            // 
            this.tabControlQuarantine.Controls.Add(this.tabPageQuarantineItems);
            this.tabControlQuarantine.Controls.Add(this.tabPageQuarantineQueue);
            this.tabControlQuarantine.Location = new System.Drawing.Point(12, 101);
            this.tabControlQuarantine.Name = "tabControlQuarantine";
            this.tabControlQuarantine.SelectedIndex = 0;
            this.tabControlQuarantine.Size = new System.Drawing.Size(630, 251);
            this.tabControlQuarantine.TabIndex = 4;
            this.tabControlQuarantine.SelectedIndexChanged += new System.EventHandler(this.tabControlQuarantine_SelectedIndexChanged);
            // 
            // tabPageQuarantineItems
            // 
            this.tabPageQuarantineItems.Controls.Add(this.buttonQuarantinedActions);
            this.tabPageQuarantineItems.Controls.Add(this.listViewQuarantineItems);
            this.tabPageQuarantineItems.Location = new System.Drawing.Point(4, 22);
            this.tabPageQuarantineItems.Name = "tabPageQuarantineItems";
            this.tabPageQuarantineItems.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQuarantineItems.Size = new System.Drawing.Size(622, 225);
            this.tabPageQuarantineItems.TabIndex = 0;
            this.tabPageQuarantineItems.Text = "Quarantined files";
            this.tabPageQuarantineItems.UseVisualStyleBackColor = true;
            // 
            // buttonQuarantinedActions
            // 
            this.buttonQuarantinedActions.Location = new System.Drawing.Point(541, 196);
            this.buttonQuarantinedActions.Name = "buttonQuarantinedActions";
            this.buttonQuarantinedActions.Size = new System.Drawing.Size(75, 23);
            this.buttonQuarantinedActions.TabIndex = 3;
            this.buttonQuarantinedActions.Text = "Actions";
            this.buttonQuarantinedActions.UseVisualStyleBackColor = true;
            this.buttonQuarantinedActions.Click += new System.EventHandler(this.buttonQuarantinedActions_Click);
            // 
            // tabPageQuarantineQueue
            // 
            this.tabPageQuarantineQueue.Controls.Add(this.buttonQueuedActions);
            this.tabPageQuarantineQueue.Controls.Add(this.listViewQueuedItems);
            this.tabPageQuarantineQueue.Location = new System.Drawing.Point(4, 22);
            this.tabPageQuarantineQueue.Name = "tabPageQuarantineQueue";
            this.tabPageQuarantineQueue.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQuarantineQueue.Size = new System.Drawing.Size(622, 225);
            this.tabPageQuarantineQueue.TabIndex = 1;
            this.tabPageQuarantineQueue.Text = "Quarantine queue";
            this.tabPageQuarantineQueue.UseVisualStyleBackColor = true;
            // 
            // buttonQueuedActions
            // 
            this.buttonQueuedActions.Location = new System.Drawing.Point(541, 196);
            this.buttonQueuedActions.Name = "buttonQueuedActions";
            this.buttonQueuedActions.Size = new System.Drawing.Size(75, 23);
            this.buttonQueuedActions.TabIndex = 6;
            this.buttonQueuedActions.Text = "Actions";
            this.buttonQueuedActions.UseVisualStyleBackColor = true;
            this.buttonQueuedActions.Click += new System.EventHandler(this.buttonQueuedActions_Click);
            // 
            // listViewQueuedItems
            // 
            this.listViewQueuedItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderQueuedFile,
            this.columnHeaderQueuedPath,
            this.columnHeaderQueuedSize,
            this.columnHeaderQueuedJob});
            this.listViewQueuedItems.ContextMenuStrip = this.contextMenuQueued;
            this.listViewQueuedItems.FullRowSelect = true;
            this.listViewQueuedItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewQueuedItems.HideSelection = false;
            this.listViewQueuedItems.Location = new System.Drawing.Point(6, 6);
            this.listViewQueuedItems.Name = "listViewQueuedItems";
            this.listViewQueuedItems.ShowItemToolTips = true;
            this.listViewQueuedItems.Size = new System.Drawing.Size(610, 187);
            this.listViewQueuedItems.TabIndex = 5;
            this.listViewQueuedItems.UseCompatibleStateImageBehavior = false;
            this.listViewQueuedItems.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderQueuedFile
            // 
            this.columnHeaderQueuedFile.Text = "File";
            this.columnHeaderQueuedFile.Width = 136;
            // 
            // columnHeaderQueuedPath
            // 
            this.columnHeaderQueuedPath.Text = "Initial location";
            this.columnHeaderQueuedPath.Width = 120;
            // 
            // columnHeaderQueuedSize
            // 
            this.columnHeaderQueuedSize.Text = "Initial size";
            this.columnHeaderQueuedSize.Width = 103;
            // 
            // columnHeaderQueuedJob
            // 
            this.columnHeaderQueuedJob.Text = "Type";
            this.columnHeaderQueuedJob.Width = 88;
            // 
            // contextMenuQueued
            // 
            this.contextMenuQueued.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemQueueCancelProcessing,
            this.toolStripMenuItemQueueSelectAll});
            this.contextMenuQueued.Name = "contextMenuQueued";
            this.contextMenuQueued.Size = new System.Drawing.Size(172, 48);
            // 
            // toolStripMenuItemQueueCancelProcessing
            // 
            this.toolStripMenuItemQueueCancelProcessing.Name = "toolStripMenuItemQueueCancelProcessing";
            this.toolStripMenuItemQueueCancelProcessing.Size = new System.Drawing.Size(171, 22);
            this.toolStripMenuItemQueueCancelProcessing.Text = "Cancel processing";
            this.toolStripMenuItemQueueCancelProcessing.Click += new System.EventHandler(this.toolStripMenuItemQueueCancelProcessing_Click);
            // 
            // toolStripMenuItemQueueSelectAll
            // 
            this.toolStripMenuItemQueueSelectAll.Name = "toolStripMenuItemQueueSelectAll";
            this.toolStripMenuItemQueueSelectAll.Size = new System.Drawing.Size(171, 22);
            this.toolStripMenuItemQueueSelectAll.Text = "Select all...";
            this.toolStripMenuItemQueueSelectAll.Click += new System.EventHandler(this.toolStripMenuItemQueueSelectAll_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File";
            this.columnHeader1.Width = 214;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Path";
            this.columnHeader2.Width = 164;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Time";
            this.columnHeader3.Width = 72;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Size";
            // 
            // toolStripMenuItemQuaRescan
            // 
            this.toolStripMenuItemQuaRescan.Name = "toolStripMenuItemQuaRescan";
            this.toolStripMenuItemQuaRescan.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItemQuaRescan.Text = "Rescan selected...";
            this.toolStripMenuItemQuaRescan.Click += new System.EventHandler(this.toolStripMenuItemQuaRescan_Click);
            // 
            // ClamWinQuarantineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(654, 364);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.progressBarCompressingProgress);
            this.Controls.Add(this.tabControlQuarantine);
            this.Name = "ClamWinQuarantineForm";
            this.Text = "Quarantine";
            this.Resize += new System.EventHandler(this.ClamWinQuarantineForm_Resize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClamWinQuarantineForm_FormClosing);
            this.Load += new System.EventHandler(this.ClamWinQuarantineForm_Load);
            this.contextMenuQuarantined.ResumeLayout(false);
            this.tabControlQuarantine.ResumeLayout(false);
            this.tabPageQuarantineItems.ResumeLayout(false);
            this.tabPageQuarantineQueue.ResumeLayout(false);
            this.contextMenuQueued.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewQuarantineItems;
        private System.Windows.Forms.ColumnHeader columnHeaderItem;
        private System.Windows.Forms.ColumnHeader columnHeaderPath;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ProgressBar progressBarCompressingProgress;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.TabControl tabControlQuarantine;
        private System.Windows.Forms.TabPage tabPageQuarantineItems;
        private System.Windows.Forms.TabPage tabPageQuarantineQueue;
        private System.Windows.Forms.ListView listViewQueuedItems;
        private System.Windows.Forms.ColumnHeader columnHeaderQueuedFile;
        private System.Windows.Forms.ColumnHeader columnHeaderQueuedPath;
        private System.Windows.Forms.ColumnHeader columnHeaderQueuedSize;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeaderQueuedJob;
        private System.Windows.Forms.ContextMenuStrip contextMenuQueued;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemQueueCancelProcessing;
        private System.Windows.Forms.ContextMenuStrip contextMenuQuarantined;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUnquarantine;
        private System.Windows.Forms.Button buttonQuarantinedActions;
        private System.Windows.Forms.Button buttonQueuedActions;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemQuarantine;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemQuaSelectAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemQueueSelectAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemQuaRescan;
    }
}