// Name:        ClamWinScanForm.Designer.cs
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
    partial class ClamWinScanForm
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
            this.panelExScanStatus = new PanelsEx.PanelEx();
            this.buttonPauseScan = new System.Windows.Forms.Button();
            this.buttonStartScan = new System.Windows.Forms.Button();
            this.buttonStopScan = new System.Windows.Forms.Button();
            this.tabScanning = new System.Windows.Forms.TabControl();
            this.tabPageDetected = new System.Windows.Forms.TabPage();
            this.buttonDetectedActions = new System.Windows.Forms.Button();
            this.listViewDetected = new System.Windows.Forms.ListView();
            this.columnDetectedStatus = new System.Windows.Forms.ColumnHeader();
            this.columnDetectedObject = new System.Windows.Forms.ColumnHeader();
            this.tabPageEvents = new System.Windows.Forms.TabPage();
            this.checkBoxShowNonCritical = new System.Windows.Forms.CheckBox();
            this.buttonEventsActions = new System.Windows.Forms.Button();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStatus = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderReason = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderTime = new System.Windows.Forms.ColumnHeader();
            this.contextMenuEventsActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EventsActionsGoToFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.EventsActionsCleanAllItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.EventsActionsSearchItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventsActionsSelectAllItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventsActionsCopyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewEventsCritical = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.tabPageStatistics = new System.Windows.Forms.TabPage();
            this.listViewStatistics = new System.Windows.Forms.ListView();
            this.columnObject = new System.Windows.Forms.ColumnHeader();
            this.columnScanned = new System.Windows.Forms.ColumnHeader();
            this.columnInfected = new System.Windows.Forms.ColumnHeader();
            this.columnErrors = new System.Windows.Forms.ColumnHeader();
            this.columnDeleted = new System.Windows.Forms.ColumnHeader();
            this.columnMovedToQuarantine = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStatisticsActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.StatisticsActionsSearchItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatisticsActionsSelectAllItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatisticsActionsCopyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelNowScanning = new System.Windows.Forms.Label();
            this.progressBarScanning = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.linkLabelBack = new System.Windows.Forms.LinkLabel();
            this.linkLabelNext = new System.Windows.Forms.LinkLabel();
            this.labelStartTime = new System.Windows.Forms.Label();
            this.labelEndTime = new System.Windows.Forms.Label();
            this.labelDuration = new System.Windows.Forms.Label();
            this.panelExScanStatus.SuspendLayout();
            this.tabScanning.SuspendLayout();
            this.tabPageDetected.SuspendLayout();
            this.tabPageEvents.SuspendLayout();
            this.contextMenuEventsActions.SuspendLayout();
            this.tabPageStatistics.SuspendLayout();
            this.contextMenuStatisticsActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExScanStatus
            // 
            this.panelExScanStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelExScanStatus.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelExScanStatus.ChangeCursor = false;
            this.panelExScanStatus.Controls.Add(this.buttonPauseScan);
            this.panelExScanStatus.Controls.Add(this.buttonStartScan);
            this.panelExScanStatus.Controls.Add(this.buttonStopScan);
            this.panelExScanStatus.DrawCollapseExpandIcons = false;
            this.panelExScanStatus.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExScanStatus.Image = global::ClamWinApp.Properties.Resources.dw;
            this.panelExScanStatus.Location = new System.Drawing.Point(12, 11);
            this.panelExScanStatus.MouseSensitive = false;
            this.panelExScanStatus.Name = "panelExScanStatus";
            this.panelExScanStatus.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExScanStatus.Size = new System.Drawing.Size(630, 37);
            this.panelExScanStatus.StartColour = System.Drawing.Color.White;
            this.panelExScanStatus.TabIndex = 1;
            this.panelExScanStatus.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExScanStatus.TitleFontColour = System.Drawing.Color.DarkGreen;
            this.panelExScanStatus.TitleText = "Scan:";
            // 
            // buttonPauseScan
            // 
            this.buttonPauseScan.Enabled = false;
            this.buttonPauseScan.Location = new System.Drawing.Point(495, 11);
            this.buttonPauseScan.Name = "buttonPauseScan";
            this.buttonPauseScan.Size = new System.Drawing.Size(45, 23);
            this.buttonPauseScan.TabIndex = 4;
            this.buttonPauseScan.Text = "Pause";
            this.buttonPauseScan.UseVisualStyleBackColor = true;
            this.buttonPauseScan.Click += new System.EventHandler(this.buttonPauseScan_Click);
            // 
            // buttonStartScan
            // 
            this.buttonStartScan.Location = new System.Drawing.Point(450, 11);
            this.buttonStartScan.Name = "buttonStartScan";
            this.buttonStartScan.Size = new System.Drawing.Size(39, 23);
            this.buttonStartScan.TabIndex = 3;
            this.buttonStartScan.Text = "Start";
            this.buttonStartScan.UseVisualStyleBackColor = true;
            this.buttonStartScan.Click += new System.EventHandler(this.buttonStartScan_Click);
            // 
            // buttonStopScan
            // 
            this.buttonStopScan.Enabled = false;
            this.buttonStopScan.Location = new System.Drawing.Point(407, 11);
            this.buttonStopScan.Name = "buttonStopScan";
            this.buttonStopScan.Size = new System.Drawing.Size(37, 23);
            this.buttonStopScan.TabIndex = 2;
            this.buttonStopScan.Text = "Stop";
            this.buttonStopScan.UseVisualStyleBackColor = true;
            this.buttonStopScan.Click += new System.EventHandler(this.buttonStopScan_Click);
            // 
            // tabScanning
            // 
            this.tabScanning.Controls.Add(this.tabPageDetected);
            this.tabScanning.Controls.Add(this.tabPageEvents);
            this.tabScanning.Controls.Add(this.tabPageStatistics);
            this.tabScanning.Location = new System.Drawing.Point(12, 180);
            this.tabScanning.Name = "tabScanning";
            this.tabScanning.SelectedIndex = 0;
            this.tabScanning.Size = new System.Drawing.Size(630, 251);
            this.tabScanning.TabIndex = 2;
            this.tabScanning.SelectedIndexChanged += new System.EventHandler(this.tabScanning_SelectedIndexChanged);
            // 
            // tabPageDetected
            // 
            this.tabPageDetected.Controls.Add(this.buttonDetectedActions);
            this.tabPageDetected.Controls.Add(this.listViewDetected);
            this.tabPageDetected.Location = new System.Drawing.Point(4, 22);
            this.tabPageDetected.Name = "tabPageDetected";
            this.tabPageDetected.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetected.Size = new System.Drawing.Size(622, 225);
            this.tabPageDetected.TabIndex = 0;
            this.tabPageDetected.Text = "Detected";
            this.tabPageDetected.UseVisualStyleBackColor = true;
            // 
            // buttonDetectedActions
            // 
            this.buttonDetectedActions.Location = new System.Drawing.Point(541, 196);
            this.buttonDetectedActions.Name = "buttonDetectedActions";
            this.buttonDetectedActions.Size = new System.Drawing.Size(75, 23);
            this.buttonDetectedActions.TabIndex = 2;
            this.buttonDetectedActions.Text = "Actions";
            this.buttonDetectedActions.UseVisualStyleBackColor = true;
            this.buttonDetectedActions.Click += new System.EventHandler(this.buttonDetectedActions_Click);
            // 
            // listViewDetected
            // 
            this.listViewDetected.AllowColumnReorder = true;
            this.listViewDetected.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDetectedStatus,
            this.columnDetectedObject});
            this.listViewDetected.FullRowSelect = true;
            this.listViewDetected.HideSelection = false;
            this.listViewDetected.Location = new System.Drawing.Point(6, 6);
            this.listViewDetected.Name = "listViewDetected";
            this.listViewDetected.ShowItemToolTips = true;
            this.listViewDetected.Size = new System.Drawing.Size(610, 187);
            this.listViewDetected.TabIndex = 1;
            this.listViewDetected.UseCompatibleStateImageBehavior = false;
            this.listViewDetected.View = System.Windows.Forms.View.Details;
            // 
            // columnDetectedStatus
            // 
            this.columnDetectedStatus.Text = "Status";
            this.columnDetectedStatus.Width = 245;
            // 
            // columnDetectedObject
            // 
            this.columnDetectedObject.Text = "Object";
            this.columnDetectedObject.Width = 299;
            // 
            // tabPageEvents
            // 
            this.tabPageEvents.Controls.Add(this.checkBoxShowNonCritical);
            this.tabPageEvents.Controls.Add(this.buttonEventsActions);
            this.tabPageEvents.Controls.Add(this.listViewEvents);
            this.tabPageEvents.Controls.Add(this.listViewEventsCritical);
            this.tabPageEvents.Location = new System.Drawing.Point(4, 22);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvents.Size = new System.Drawing.Size(622, 225);
            this.tabPageEvents.TabIndex = 1;
            this.tabPageEvents.Text = "Events";
            this.tabPageEvents.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowNonCritical
            // 
            this.checkBoxShowNonCritical.AutoSize = true;
            this.checkBoxShowNonCritical.Location = new System.Drawing.Point(6, 199);
            this.checkBoxShowNonCritical.Name = "checkBoxShowNonCritical";
            this.checkBoxShowNonCritical.Size = new System.Drawing.Size(142, 17);
            this.checkBoxShowNonCritical.TabIndex = 4;
            this.checkBoxShowNonCritical.Text = "Show non critical events";
            this.checkBoxShowNonCritical.UseVisualStyleBackColor = true;
            this.checkBoxShowNonCritical.CheckedChanged += new System.EventHandler(this.checkBoxShowNonCritical_CheckedChanged);
            // 
            // buttonEventsActions
            // 
            this.buttonEventsActions.Location = new System.Drawing.Point(541, 196);
            this.buttonEventsActions.Name = "buttonEventsActions";
            this.buttonEventsActions.Size = new System.Drawing.Size(75, 23);
            this.buttonEventsActions.TabIndex = 3;
            this.buttonEventsActions.Text = "Actions";
            this.buttonEventsActions.UseVisualStyleBackColor = true;
            this.buttonEventsActions.Click += new System.EventHandler(this.buttonEventsActions_Click);
            // 
            // listViewEvents
            // 
            this.listViewEvents.AllowColumnReorder = true;
            this.listViewEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderStatus,
            this.columnHeaderReason,
            this.columnHeaderTime});
            this.listViewEvents.ContextMenuStrip = this.contextMenuEventsActions;
            this.listViewEvents.FullRowSelect = true;
            this.listViewEvents.HideSelection = false;
            this.listViewEvents.Location = new System.Drawing.Point(6, 6);
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.ShowItemToolTips = true;
            this.listViewEvents.Size = new System.Drawing.Size(610, 187);
            this.listViewEvents.TabIndex = 0;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.View = System.Windows.Forms.View.Details;
            this.listViewEvents.Visible = false;
            this.listViewEvents.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewEvents_ColumnClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 205;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Status";
            this.columnHeaderStatus.Width = 106;
            // 
            // columnHeaderReason
            // 
            this.columnHeaderReason.Text = "Reason";
            this.columnHeaderReason.Width = 104;
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "Time";
            this.columnHeaderTime.Width = 139;
            // 
            // contextMenuEventsActions
            // 
            this.contextMenuEventsActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EventsActionsGoToFileItem,
            this.toolStripSeparator1,
            this.EventsActionsCleanAllItem,
            this.toolStripSeparator2,
            this.EventsActionsSearchItem,
            this.EventsActionsSelectAllItem,
            this.EventsActionsCopyItem});
            this.contextMenuEventsActions.Name = "contextMenuEventsActions";
            this.contextMenuEventsActions.Size = new System.Drawing.Size(131, 126);
            // 
            // EventsActionsGoToFileItem
            // 
            this.EventsActionsGoToFileItem.Name = "EventsActionsGoToFileItem";
            this.EventsActionsGoToFileItem.Size = new System.Drawing.Size(130, 22);
            this.EventsActionsGoToFileItem.Text = "Go to file";
            this.EventsActionsGoToFileItem.Click += new System.EventHandler(this.EventsActionsGoToFileItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(127, 6);
            // 
            // EventsActionsCleanAllItem
            // 
            this.EventsActionsCleanAllItem.Name = "EventsActionsCleanAllItem";
            this.EventsActionsCleanAllItem.Size = new System.Drawing.Size(130, 22);
            this.EventsActionsCleanAllItem.Text = "Clean All";
            this.EventsActionsCleanAllItem.Click += new System.EventHandler(this.EventsActionsCleanAllItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(127, 6);
            // 
            // EventsActionsSearchItem
            // 
            this.EventsActionsSearchItem.Name = "EventsActionsSearchItem";
            this.EventsActionsSearchItem.Size = new System.Drawing.Size(130, 22);
            this.EventsActionsSearchItem.Text = "Search...";
            this.EventsActionsSearchItem.Click += new System.EventHandler(this.EventsActionsSearchItem_Click);
            // 
            // EventsActionsSelectAllItem
            // 
            this.EventsActionsSelectAllItem.Name = "EventsActionsSelectAllItem";
            this.EventsActionsSelectAllItem.Size = new System.Drawing.Size(130, 22);
            this.EventsActionsSelectAllItem.Text = "Select All";
            this.EventsActionsSelectAllItem.Click += new System.EventHandler(this.EventsActionsSelectAllItem_Click);
            // 
            // EventsActionsCopyItem
            // 
            this.EventsActionsCopyItem.Name = "EventsActionsCopyItem";
            this.EventsActionsCopyItem.Size = new System.Drawing.Size(130, 22);
            this.EventsActionsCopyItem.Text = "Copy";
            this.EventsActionsCopyItem.Click += new System.EventHandler(this.EventsActionsCopyItem_Click);
            // 
            // listViewEventsCritical
            // 
            this.listViewEventsCritical.AllowColumnReorder = true;
            this.listViewEventsCritical.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listViewEventsCritical.ContextMenuStrip = this.contextMenuEventsActions;
            this.listViewEventsCritical.FullRowSelect = true;
            this.listViewEventsCritical.HideSelection = false;
            this.listViewEventsCritical.Location = new System.Drawing.Point(6, 6);
            this.listViewEventsCritical.Name = "listViewEventsCritical";
            this.listViewEventsCritical.ShowItemToolTips = true;
            this.listViewEventsCritical.Size = new System.Drawing.Size(610, 187);
            this.listViewEventsCritical.TabIndex = 5;
            this.listViewEventsCritical.UseCompatibleStateImageBehavior = false;
            this.listViewEventsCritical.View = System.Windows.Forms.View.Details;
            this.listViewEventsCritical.Visible = false;
            this.listViewEventsCritical.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewEvents_ColumnClick);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 205;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Status";
            this.columnHeader6.Width = 106;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Reason";
            this.columnHeader7.Width = 104;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Time";
            this.columnHeader8.Width = 139;
            // 
            // tabPageStatistics
            // 
            this.tabPageStatistics.Controls.Add(this.listViewStatistics);
            this.tabPageStatistics.Location = new System.Drawing.Point(4, 22);
            this.tabPageStatistics.Name = "tabPageStatistics";
            this.tabPageStatistics.Size = new System.Drawing.Size(622, 225);
            this.tabPageStatistics.TabIndex = 2;
            this.tabPageStatistics.Text = "Statistics";
            this.tabPageStatistics.UseVisualStyleBackColor = true;
            // 
            // listViewStatistics
            // 
            this.listViewStatistics.AllowColumnReorder = true;
            this.listViewStatistics.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnObject,
            this.columnScanned,
            this.columnInfected,
            this.columnErrors,
            this.columnDeleted,
            this.columnMovedToQuarantine});
            this.listViewStatistics.ContextMenuStrip = this.contextMenuStatisticsActions;
            this.listViewStatistics.FullRowSelect = true;
            this.listViewStatistics.HideSelection = false;
            this.listViewStatistics.Location = new System.Drawing.Point(6, 6);
            this.listViewStatistics.Name = "listViewStatistics";
            this.listViewStatistics.ShowItemToolTips = true;
            this.listViewStatistics.Size = new System.Drawing.Size(610, 216);
            this.listViewStatistics.TabIndex = 1;
            this.listViewStatistics.UseCompatibleStateImageBehavior = false;
            this.listViewStatistics.View = System.Windows.Forms.View.Details;
            // 
            // columnObject
            // 
            this.columnObject.Text = "Object";
            this.columnObject.Width = 86;
            // 
            // columnScanned
            // 
            this.columnScanned.Text = "Scanned";
            this.columnScanned.Width = 52;
            // 
            // columnInfected
            // 
            this.columnInfected.Text = "Infected";
            this.columnInfected.Width = 42;
            // 
            // columnErrors
            // 
            this.columnErrors.Text = "Errors";
            // 
            // columnDeleted
            // 
            this.columnDeleted.Text = "Deleted";
            this.columnDeleted.Width = 81;
            // 
            // columnMovedToQuarantine
            // 
            this.columnMovedToQuarantine.Text = "Moved to Quarantine";
            // 
            // contextMenuStatisticsActions
            // 
            this.contextMenuStatisticsActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatisticsActionsSearchItem,
            this.StatisticsActionsSelectAllItem,
            this.StatisticsActionsCopyItem});
            this.contextMenuStatisticsActions.Name = "contextMenuStatisticsActions";
            this.contextMenuStatisticsActions.Size = new System.Drawing.Size(131, 70);
            // 
            // StatisticsActionsSearchItem
            // 
            this.StatisticsActionsSearchItem.Name = "StatisticsActionsSearchItem";
            this.StatisticsActionsSearchItem.Size = new System.Drawing.Size(130, 22);
            this.StatisticsActionsSearchItem.Text = "Search...";
            this.StatisticsActionsSearchItem.Click += new System.EventHandler(this.StatisticsActionsSearchItem_Click);
            // 
            // StatisticsActionsSelectAllItem
            // 
            this.StatisticsActionsSelectAllItem.Name = "StatisticsActionsSelectAllItem";
            this.StatisticsActionsSelectAllItem.Size = new System.Drawing.Size(130, 22);
            this.StatisticsActionsSelectAllItem.Text = "Select All";
            this.StatisticsActionsSelectAllItem.Click += new System.EventHandler(this.StatisticsActionsSelectAllItem_Click);
            // 
            // StatisticsActionsCopyItem
            // 
            this.StatisticsActionsCopyItem.Name = "StatisticsActionsCopyItem";
            this.StatisticsActionsCopyItem.Size = new System.Drawing.Size(130, 22);
            this.StatisticsActionsCopyItem.Text = "Copy";
            this.StatisticsActionsCopyItem.Click += new System.EventHandler(this.StatisticsActionsCopyItem_Click);
            // 
            // labelNowScanning
            // 
            this.labelNowScanning.AutoEllipsis = true;
            this.labelNowScanning.Location = new System.Drawing.Point(12, 52);
            this.labelNowScanning.Name = "labelNowScanning";
            this.labelNowScanning.Size = new System.Drawing.Size(630, 13);
            this.labelNowScanning.TabIndex = 3;
            // 
            // progressBarScanning
            // 
            this.progressBarScanning.Location = new System.Drawing.Point(12, 146);
            this.progressBarScanning.Name = "progressBarScanning";
            this.progressBarScanning.Size = new System.Drawing.Size(630, 20);
            this.progressBarScanning.Step = 1;
            this.progressBarScanning.TabIndex = 4;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelProgress.Location = new System.Drawing.Point(9, 121);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(52, 13);
            this.labelProgress.TabIndex = 5;
            this.labelProgress.Text = "Progess";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 245;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 106;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Reason";
            this.columnHeader3.Width = 104;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Time";
            this.columnHeader4.Width = 81;
            // 
            // linkLabelBack
            // 
            this.linkLabelBack.AutoSize = true;
            this.linkLabelBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabelBack.Location = new System.Drawing.Point(90, 443);
            this.linkLabelBack.Name = "linkLabelBack";
            this.linkLabelBack.Size = new System.Drawing.Size(43, 13);
            this.linkLabelBack.TabIndex = 6;
            this.linkLabelBack.TabStop = true;
            this.linkLabelBack.Text = "<Back";
            this.linkLabelBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBack_LinkClicked);
            // 
            // linkLabelNext
            // 
            this.linkLabelNext.AutoSize = true;
            this.linkLabelNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabelNext.Location = new System.Drawing.Point(134, 443);
            this.linkLabelNext.Name = "linkLabelNext";
            this.linkLabelNext.Size = new System.Drawing.Size(40, 13);
            this.linkLabelNext.TabIndex = 7;
            this.linkLabelNext.TabStop = true;
            this.linkLabelNext.Text = "Next>";
            this.linkLabelNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelNext_LinkClicked);
            // 
            // labelStartTime
            // 
            this.labelStartTime.Location = new System.Drawing.Point(416, 73);
            this.labelStartTime.Name = "labelStartTime";
            this.labelStartTime.Size = new System.Drawing.Size(226, 18);
            this.labelStartTime.TabIndex = 8;
            this.labelStartTime.Text = "Start time:";
            // 
            // labelEndTime
            // 
            this.labelEndTime.Location = new System.Drawing.Point(416, 109);
            this.labelEndTime.Name = "labelEndTime";
            this.labelEndTime.Size = new System.Drawing.Size(222, 18);
            this.labelEndTime.TabIndex = 9;
            this.labelEndTime.Text = "Finish time:";
            // 
            // labelDuration
            // 
            this.labelDuration.Location = new System.Drawing.Point(416, 91);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(226, 18);
            this.labelDuration.TabIndex = 10;
            this.labelDuration.Text = "Durration:";
            // 
            // ClamWinScanForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(654, 465);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.labelEndTime);
            this.Controls.Add(this.labelStartTime);
            this.Controls.Add(this.linkLabelNext);
            this.Controls.Add(this.linkLabelBack);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.progressBarScanning);
            this.Controls.Add(this.labelNowScanning);
            this.Controls.Add(this.tabScanning);
            this.Controls.Add(this.panelExScanStatus);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(662, 499);
            this.Name = "ClamWinScanForm";
            this.ShowIcon = false;
            this.Text = "Scanning";
            this.Resize += new System.EventHandler(this.ClamWinScanForm_Resize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClamWinScanForm_FormClosing);
            this.Load += new System.EventHandler(this.ClamWinScanForm_Load);
            this.panelExScanStatus.ResumeLayout(false);
            this.tabScanning.ResumeLayout(false);
            this.tabPageDetected.ResumeLayout(false);
            this.tabPageEvents.ResumeLayout(false);
            this.tabPageEvents.PerformLayout();
            this.contextMenuEventsActions.ResumeLayout(false);
            this.tabPageStatistics.ResumeLayout(false);
            this.contextMenuStatisticsActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelsEx.PanelEx panelExScanStatus;
        private System.Windows.Forms.Button buttonPauseScan;
        private System.Windows.Forms.Button buttonStartScan;
        private System.Windows.Forms.Button buttonStopScan;
        private System.Windows.Forms.TabControl tabScanning;
        private System.Windows.Forms.TabPage tabPageDetected;
        private System.Windows.Forms.TabPage tabPageEvents;
        private System.Windows.Forms.TabPage tabPageStatistics;
        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderReason;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.Label labelNowScanning;
        private System.Windows.Forms.ProgressBar progressBarScanning;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ListView listViewStatistics;
        private System.Windows.Forms.ColumnHeader columnObject;
        private System.Windows.Forms.ColumnHeader columnScanned;
        private System.Windows.Forms.ColumnHeader columnInfected;
        private System.Windows.Forms.ColumnHeader columnDeleted;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnMovedToQuarantine;
        private System.Windows.Forms.ColumnHeader columnErrors;
        private System.Windows.Forms.ListView listViewDetected;
        private System.Windows.Forms.ColumnHeader columnDetectedStatus;
        private System.Windows.Forms.ColumnHeader columnDetectedObject;
        private System.Windows.Forms.LinkLabel linkLabelBack;
        private System.Windows.Forms.LinkLabel linkLabelNext;
        private System.Windows.Forms.Label labelStartTime;
        private System.Windows.Forms.Label labelEndTime;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.Button buttonDetectedActions;
        private System.Windows.Forms.Button buttonEventsActions;
        private System.Windows.Forms.ContextMenuStrip contextMenuEventsActions;
        private System.Windows.Forms.ToolStripMenuItem EventsActionsGoToFileItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem EventsActionsCleanAllItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem EventsActionsSearchItem;
        private System.Windows.Forms.ToolStripMenuItem EventsActionsSelectAllItem;
        private System.Windows.Forms.ToolStripMenuItem EventsActionsCopyItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStatisticsActions;
        private System.Windows.Forms.ToolStripMenuItem StatisticsActionsSearchItem;
        private System.Windows.Forms.ToolStripMenuItem StatisticsActionsSelectAllItem;
        private System.Windows.Forms.ToolStripMenuItem StatisticsActionsCopyItem;
        private System.Windows.Forms.CheckBox checkBoxShowNonCritical;
        private System.Windows.Forms.ListView listViewEventsCritical;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
    }
}