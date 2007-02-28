// Name:        ClamWinMainForm.Designer.cs
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
    partial class ClamWinMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClamWinMainForm));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemTrayOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemTrayExit = new System.Windows.Forms.ToolStripMenuItem();
            this.linkHelp = new System.Windows.Forms.LinkLabel();
            this.linkSettings = new System.Windows.Forms.LinkLabel();
            this.sqLiteDataAdapter = new System.Data.SQLite.SQLiteDataAdapter();
            this.imageFilesList = new System.Windows.Forms.ImageList(this.components);
            this.panelExRightPanel = new PanelsEx.PanelEx();
            this.panelScan = new System.Windows.Forms.Panel();
            this.buttonSCScan = new System.Windows.Forms.Button();
            this.buttonSCRemove = new System.Windows.Forms.Button();
            this.buttonSCAdd = new System.Windows.Forms.Button();
            this.listViewSCItems = new System.Windows.Forms.ListView();
            this.groupBoxSCStatistics = new ClamWinApp.GroupBoxEx();
            this.progressBarScan = new System.Windows.Forms.ProgressBar();
            this.linkLabelScanLastScanValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanScannedValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanThreatsValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanLastScan = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanScanned = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanThreats = new System.Windows.Forms.LinkLabel();
            this.groupBoxSCSettings = new ClamWinApp.GroupBoxEx();
            this.linkLabelScanSettingsActionDescr = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanSettings = new System.Windows.Forms.LinkLabel();
            this.panelScanCritical = new System.Windows.Forms.Panel();
            this.buttonSCriticalScan = new System.Windows.Forms.Button();
            this.groupBoxSCCriticalStatistics = new ClamWinApp.GroupBoxEx();
            this.progressBarScanCritical = new System.Windows.Forms.ProgressBar();
            this.linkLabelScanCriticalLastScanValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanCriticalScannedValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanCriticalThreatsValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanCriticalLastScan = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanCriticalScanned = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanCriticalThreats = new System.Windows.Forms.LinkLabel();
            this.buttonSCriticalRemove = new System.Windows.Forms.Button();
            this.groupBoxSCCriticalSettings = new ClamWinApp.GroupBoxEx();
            this.linkLabelScanCriticalSettingsActionDescr = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanCriticalAction = new System.Windows.Forms.LinkLabel();
            this.buttonSCriticalAdd = new System.Windows.Forms.Button();
            this.listViewSCCriticalItems = new System.Windows.Forms.ListView();
            this.panelProtection = new System.Windows.Forms.Panel();
            this.groupBoxProtectionOnAccess = new System.Windows.Forms.GroupBox();
            this.radioButtonOnAccessSuspended = new System.Windows.Forms.RadioButton();
            this.radioButtonOnAccessOff = new System.Windows.Forms.RadioButton();
            this.radioButtonOnAccessOn = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.panelDataFiles = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxDiskSpace = new ClamWinApp.GroupBoxEx();
            this.linkLabelDataBaseSizeValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelDataBaseSize = new System.Windows.Forms.LinkLabel();
            this.panelUpdate = new System.Windows.Forms.Panel();
            this.buttonUpdateNow = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxUpdateStatistics = new ClamWinApp.GroupBoxEx();
            this.linkLabelLastUpdateValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelLastUpdate = new System.Windows.Forms.LinkLabel();
            this.groupBoxUpdateSettings = new ClamWinApp.GroupBoxEx();
            this.linkLabelUpdateMethodValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelUpdateMethod = new System.Windows.Forms.LinkLabel();
            this.panelHome = new System.Windows.Forms.Panel();
            this.linkLabelStatistics = new System.Windows.Forms.LinkLabel();
            this.labelDBVersionValue = new System.Windows.Forms.Label();
            this.labelDBVersion = new System.Windows.Forms.Label();
            this.linkLabelUpdateNow = new System.Windows.Forms.LinkLabel();
            this.labelLastUpdatedValue = new System.Windows.Forms.Label();
            this.labelLastUpdated = new System.Windows.Forms.Label();
            this.linkLabelUpdates = new System.Windows.Forms.LinkLabel();
            this.linkLabelProtectionSettings = new System.Windows.Forms.LinkLabel();
            this.labelEmailProtectionValue = new System.Windows.Forms.Label();
            this.labelResidentFileProtectionValue = new System.Windows.Forms.Label();
            this.labelEmailProtection = new System.Windows.Forms.Label();
            this.labelResidentFileProtection = new System.Windows.Forms.Label();
            this.linkLabelProtectionStatus = new System.Windows.Forms.LinkLabel();
            this.labelRecomendations = new System.Windows.Forms.Label();
            this.pictureBoxProtectionLevel = new System.Windows.Forms.PictureBox();
            this.linkLabelProtectionLevel = new System.Windows.Forms.LinkLabel();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            this.labelClamWinStatus = new System.Windows.Forms.Label();
            this.panelQuarantine = new System.Windows.Forms.Panel();
            this.groupBoxQuarantineManage = new System.Windows.Forms.GroupBox();
            this.buttonQuarantineManage = new System.Windows.Forms.Button();
            this.groupBoxQuarantineStatistics = new ClamWinApp.GroupBoxEx();
            this.linkLabelLastFileQuarantinedValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelLastFileQuarantined = new System.Windows.Forms.LinkLabel();
            this.linkLabelQuarantineTotalSizeValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelQuarantineTotalSize = new System.Windows.Forms.LinkLabel();
            this.linkLabelFilesQuarantinedValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelFilesQuarantined = new System.Windows.Forms.LinkLabel();
            this.labelQuarantine = new System.Windows.Forms.Label();
            this.panelScanMyPC = new System.Windows.Forms.Panel();
            this.buttonSCMyPCScan = new System.Windows.Forms.Button();
            this.buttonSCMyPCRemove = new System.Windows.Forms.Button();
            this.buttonSCMyPCAdd = new System.Windows.Forms.Button();
            this.listViewSCMyPCItems = new System.Windows.Forms.ListView();
            this.groupBoxSCMyPCStatistics = new ClamWinApp.GroupBoxEx();
            this.progressBarScanMyPC = new System.Windows.Forms.ProgressBar();
            this.linkLabelScanMyPCLastScanValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanMyPCScannedValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanMyPCThreatsValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanMyPCLastScan = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanMyPCScanned = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanMyPCThreats = new System.Windows.Forms.LinkLabel();
            this.groupBoxSCMyPCSettings = new ClamWinApp.GroupBoxEx();
            this.linkLabelScanMyPCSettingsActionDescr = new System.Windows.Forms.LinkLabel();
            this.linkLabelScanMyPCSettings = new System.Windows.Forms.LinkLabel();
            this.panelService = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxServiceProductInfo = new ClamWinApp.GroupBoxEx();
            this.linkLabelServiceVersionValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelServiceVersion = new System.Windows.Forms.LinkLabel();
            this.panelExGroup = new PanelsEx.PanelExGroup();
            this.panelExHome = new PanelsEx.PanelEx();
            this.panelExScan = new PanelsEx.PanelEx();
            this.linkScanCritical = new System.Windows.Forms.LinkLabel();
            this.linkScanMyPC = new System.Windows.Forms.LinkLabel();
            this.panelExQuarantine = new PanelsEx.PanelEx();
            this.panelExService = new PanelsEx.PanelEx();
            this.linkProtection = new System.Windows.Forms.LinkLabel();
            this.linkDatafiles = new System.Windows.Forms.LinkLabel();
            this.linkUpdate = new System.Windows.Forms.LinkLabel();
            this.panelExNotification = new PanelsEx.PanelEx();
            this.linkLabelNotify = new System.Windows.Forms.LinkLabel();
            this.linkLabelNotifyNext = new System.Windows.Forms.LinkLabel();
            this.linkLabelNotifyBack = new System.Windows.Forms.LinkLabel();
            this.linkLabelQuarantineSizeValue = new System.Windows.Forms.LinkLabel();
            this.linkLabelQuarantineSize = new System.Windows.Forms.LinkLabel();
            this.menuTray.SuspendLayout();
            this.panelExRightPanel.SuspendLayout();
            this.panelScan.SuspendLayout();
            this.groupBoxSCStatistics.SuspendLayout();
            this.groupBoxSCSettings.SuspendLayout();
            this.panelScanCritical.SuspendLayout();
            this.groupBoxSCCriticalStatistics.SuspendLayout();
            this.groupBoxSCCriticalSettings.SuspendLayout();
            this.panelProtection.SuspendLayout();
            this.groupBoxProtectionOnAccess.SuspendLayout();
            this.panelDataFiles.SuspendLayout();
            this.groupBoxDiskSpace.SuspendLayout();
            this.panelUpdate.SuspendLayout();
            this.groupBoxUpdateStatistics.SuspendLayout();
            this.groupBoxUpdateSettings.SuspendLayout();
            this.panelHome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProtectionLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
            this.panelQuarantine.SuspendLayout();
            this.groupBoxQuarantineManage.SuspendLayout();
            this.groupBoxQuarantineStatistics.SuspendLayout();
            this.panelScanMyPC.SuspendLayout();
            this.groupBoxSCMyPCStatistics.SuspendLayout();
            this.groupBoxSCMyPCSettings.SuspendLayout();
            this.panelService.SuspendLayout();
            this.groupBoxServiceProductInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelExGroup)).BeginInit();
            this.panelExGroup.SuspendLayout();
            this.panelExScan.SuspendLayout();
            this.panelExService.SuspendLayout();
            this.panelExNotification.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.trayIcon.ContextMenuStrip = this.menuTray;
            resources.ApplyResources(this.trayIcon, "trayIcon");
            this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseClick);
            // 
            // menuTray
            // 
            this.menuTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemTrayOpen,
            this.toolStripSeparator1,
            this.menuItemTrayExit});
            this.menuTray.Name = "contextMenuStrip1";
            resources.ApplyResources(this.menuTray, "menuTray");
            // 
            // menuItemTrayOpen
            // 
            this.menuItemTrayOpen.Name = "menuItemTrayOpen";
            resources.ApplyResources(this.menuItemTrayOpen, "menuItemTrayOpen");
            this.menuItemTrayOpen.Click += new System.EventHandler(this.menuItemTrayOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // menuItemTrayExit
            // 
            this.menuItemTrayExit.Name = "menuItemTrayExit";
            resources.ApplyResources(this.menuItemTrayExit, "menuItemTrayExit");
            this.menuItemTrayExit.Click += new System.EventHandler(this.menuItemTrayExit_Click);
            // 
            // linkHelp
            // 
            resources.ApplyResources(this.linkHelp, "linkHelp");
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.TabStop = true;
            // 
            // linkSettings
            // 
            resources.ApplyResources(this.linkSettings, "linkSettings");
            this.linkSettings.Name = "linkSettings";
            this.linkSettings.TabStop = true;
            this.linkSettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSettings_LinkClicked);
            // 
            // sqLiteDataAdapter
            // 
            this.sqLiteDataAdapter.DeleteCommand = null;
            this.sqLiteDataAdapter.InsertCommand = null;
            this.sqLiteDataAdapter.SelectCommand = null;
            this.sqLiteDataAdapter.UpdateCommand = null;
            // 
            // imageFilesList
            // 
            this.imageFilesList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imageFilesList, "imageFilesList");
            this.imageFilesList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panelExRightPanel
            // 
            this.panelExRightPanel.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelExRightPanel.ChangeCursor = false;
            this.panelExRightPanel.Controls.Add(this.panelDataFiles);
            this.panelExRightPanel.Controls.Add(this.panelUpdate);
            this.panelExRightPanel.Controls.Add(this.panelHome);
            this.panelExRightPanel.Controls.Add(this.panelQuarantine);
            this.panelExRightPanel.Controls.Add(this.panelScanMyPC);
            this.panelExRightPanel.Controls.Add(this.panelService);
            this.panelExRightPanel.Controls.Add(this.panelScan);
            this.panelExRightPanel.Controls.Add(this.panelScanCritical);
            this.panelExRightPanel.Controls.Add(this.panelProtection);
            this.panelExRightPanel.DrawCollapseExpandIcons = false;
            this.panelExRightPanel.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExRightPanel.Image = null;
            resources.ApplyResources(this.panelExRightPanel, "panelExRightPanel");
            this.panelExRightPanel.MouseSensitive = false;
            this.panelExRightPanel.Name = "panelExRightPanel";
            this.panelExRightPanel.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExRightPanel.StartColour = System.Drawing.Color.White;
            this.panelExRightPanel.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExRightPanel.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExRightPanel.TitleText = "Select category:";
            // 
            // panelScan
            // 
            this.panelScan.Controls.Add(this.buttonSCScan);
            this.panelScan.Controls.Add(this.buttonSCRemove);
            this.panelScan.Controls.Add(this.buttonSCAdd);
            this.panelScan.Controls.Add(this.listViewSCItems);
            this.panelScan.Controls.Add(this.groupBoxSCStatistics);
            this.panelScan.Controls.Add(this.groupBoxSCSettings);
            resources.ApplyResources(this.panelScan, "panelScan");
            this.panelScan.Name = "panelScan";
            this.panelScan.VisibleChanged += new System.EventHandler(this.panelScan_VisibleChanged);
            // 
            // buttonSCScan
            // 
            resources.ApplyResources(this.buttonSCScan, "buttonSCScan");
            this.buttonSCScan.Name = "buttonSCScan";
            this.buttonSCScan.UseVisualStyleBackColor = true;
            this.buttonSCScan.Click += new System.EventHandler(this.buttonSCScan_Click);
            // 
            // buttonSCRemove
            // 
            resources.ApplyResources(this.buttonSCRemove, "buttonSCRemove");
            this.buttonSCRemove.Name = "buttonSCRemove";
            this.buttonSCRemove.UseVisualStyleBackColor = true;
            this.buttonSCRemove.Click += new System.EventHandler(this.buttonSCRemove_Click);
            // 
            // buttonSCAdd
            // 
            resources.ApplyResources(this.buttonSCAdd, "buttonSCAdd");
            this.buttonSCAdd.Name = "buttonSCAdd";
            this.buttonSCAdd.UseVisualStyleBackColor = true;
            this.buttonSCAdd.Click += new System.EventHandler(this.buttonSCAdd_Click);
            // 
            // listViewSCItems
            // 
            this.listViewSCItems.CheckBoxes = true;
            this.listViewSCItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            resources.ApplyResources(this.listViewSCItems, "listViewSCItems");
            this.listViewSCItems.Name = "listViewSCItems";
            this.listViewSCItems.SmallImageList = this.imageFilesList;
            this.listViewSCItems.UseCompatibleStateImageBehavior = false;
            this.listViewSCItems.View = System.Windows.Forms.View.Details;
            // 
            // groupBoxSCStatistics
            // 
            this.groupBoxSCStatistics.Controls.Add(this.progressBarScan);
            this.groupBoxSCStatistics.Controls.Add(this.linkLabelScanLastScanValue);
            this.groupBoxSCStatistics.Controls.Add(this.linkLabelScanScannedValue);
            this.groupBoxSCStatistics.Controls.Add(this.linkLabelScanThreatsValue);
            this.groupBoxSCStatistics.Controls.Add(this.linkLabelScanLastScan);
            this.groupBoxSCStatistics.Controls.Add(this.linkLabelScanScanned);
            this.groupBoxSCStatistics.Controls.Add(this.linkLabelScanThreats);
            this.groupBoxSCStatistics.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxSCStatistics, "groupBoxSCStatistics");
            this.groupBoxSCStatistics.Name = "groupBoxSCStatistics";
            this.groupBoxSCStatistics.TabStop = false;
            this.groupBoxSCStatistics.Pressed += new ClamWinApp.GroupBoxEx.OnPressed(this.groupBoxSCStatistics_Pressed);
            // 
            // progressBarScan
            // 
            resources.ApplyResources(this.progressBarScan, "progressBarScan");
            this.progressBarScan.Name = "progressBarScan";
            // 
            // linkLabelScanLastScanValue
            // 
            resources.ApplyResources(this.linkLabelScanLastScanValue, "linkLabelScanLastScanValue");
            this.linkLabelScanLastScanValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanLastScanValue.Name = "linkLabelScanLastScanValue";
            // 
            // linkLabelScanScannedValue
            // 
            resources.ApplyResources(this.linkLabelScanScannedValue, "linkLabelScanScannedValue");
            this.linkLabelScanScannedValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanScannedValue.Name = "linkLabelScanScannedValue";
            // 
            // linkLabelScanThreatsValue
            // 
            resources.ApplyResources(this.linkLabelScanThreatsValue, "linkLabelScanThreatsValue");
            this.linkLabelScanThreatsValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanThreatsValue.Name = "linkLabelScanThreatsValue";
            // 
            // linkLabelScanLastScan
            // 
            resources.ApplyResources(this.linkLabelScanLastScan, "linkLabelScanLastScan");
            this.linkLabelScanLastScan.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanLastScan.Name = "linkLabelScanLastScan";
            this.linkLabelScanLastScan.TabStop = true;
            // 
            // linkLabelScanScanned
            // 
            resources.ApplyResources(this.linkLabelScanScanned, "linkLabelScanScanned");
            this.linkLabelScanScanned.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanScanned.Name = "linkLabelScanScanned";
            this.linkLabelScanScanned.TabStop = true;
            // 
            // linkLabelScanThreats
            // 
            resources.ApplyResources(this.linkLabelScanThreats, "linkLabelScanThreats");
            this.linkLabelScanThreats.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanThreats.Name = "linkLabelScanThreats";
            this.linkLabelScanThreats.TabStop = true;
            // 
            // groupBoxSCSettings
            // 
            this.groupBoxSCSettings.Controls.Add(this.linkLabelScanSettingsActionDescr);
            this.groupBoxSCSettings.Controls.Add(this.linkLabelScanSettings);
            this.groupBoxSCSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxSCSettings, "groupBoxSCSettings");
            this.groupBoxSCSettings.Name = "groupBoxSCSettings";
            this.groupBoxSCSettings.TabStop = false;
            this.groupBoxSCSettings.Pressed += new ClamWinApp.GroupBoxEx.OnPressed(this.groupBoxSCSettings_Pressed);
            // 
            // linkLabelScanSettingsActionDescr
            // 
            resources.ApplyResources(this.linkLabelScanSettingsActionDescr, "linkLabelScanSettingsActionDescr");
            this.linkLabelScanSettingsActionDescr.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanSettingsActionDescr.Name = "linkLabelScanSettingsActionDescr";
            // 
            // linkLabelScanSettings
            // 
            resources.ApplyResources(this.linkLabelScanSettings, "linkLabelScanSettings");
            this.linkLabelScanSettings.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanSettings.Name = "linkLabelScanSettings";
            this.linkLabelScanSettings.TabStop = true;
            // 
            // panelScanCritical
            // 
            this.panelScanCritical.Controls.Add(this.buttonSCriticalScan);
            this.panelScanCritical.Controls.Add(this.groupBoxSCCriticalStatistics);
            this.panelScanCritical.Controls.Add(this.buttonSCriticalRemove);
            this.panelScanCritical.Controls.Add(this.groupBoxSCCriticalSettings);
            this.panelScanCritical.Controls.Add(this.buttonSCriticalAdd);
            this.panelScanCritical.Controls.Add(this.listViewSCCriticalItems);
            resources.ApplyResources(this.panelScanCritical, "panelScanCritical");
            this.panelScanCritical.Name = "panelScanCritical";
            this.panelScanCritical.VisibleChanged += new System.EventHandler(this.panelScanCritical_VisibleChanged);
            // 
            // buttonSCriticalScan
            // 
            resources.ApplyResources(this.buttonSCriticalScan, "buttonSCriticalScan");
            this.buttonSCriticalScan.Name = "buttonSCriticalScan";
            this.buttonSCriticalScan.UseVisualStyleBackColor = true;
            this.buttonSCriticalScan.Click += new System.EventHandler(this.buttonSCriticalScan_Click);
            // 
            // groupBoxSCCriticalStatistics
            // 
            this.groupBoxSCCriticalStatistics.Controls.Add(this.progressBarScanCritical);
            this.groupBoxSCCriticalStatistics.Controls.Add(this.linkLabelScanCriticalLastScanValue);
            this.groupBoxSCCriticalStatistics.Controls.Add(this.linkLabelScanCriticalScannedValue);
            this.groupBoxSCCriticalStatistics.Controls.Add(this.linkLabelScanCriticalThreatsValue);
            this.groupBoxSCCriticalStatistics.Controls.Add(this.linkLabelScanCriticalLastScan);
            this.groupBoxSCCriticalStatistics.Controls.Add(this.linkLabelScanCriticalScanned);
            this.groupBoxSCCriticalStatistics.Controls.Add(this.linkLabelScanCriticalThreats);
            this.groupBoxSCCriticalStatistics.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxSCCriticalStatistics, "groupBoxSCCriticalStatistics");
            this.groupBoxSCCriticalStatistics.Name = "groupBoxSCCriticalStatistics";
            this.groupBoxSCCriticalStatistics.TabStop = false;
            this.groupBoxSCCriticalStatistics.Pressed += new ClamWinApp.GroupBoxEx.OnPressed(this.groupBoxSCCriticalStatistics_Pressed);
            // 
            // progressBarScanCritical
            // 
            resources.ApplyResources(this.progressBarScanCritical, "progressBarScanCritical");
            this.progressBarScanCritical.Name = "progressBarScanCritical";
            // 
            // linkLabelScanCriticalLastScanValue
            // 
            resources.ApplyResources(this.linkLabelScanCriticalLastScanValue, "linkLabelScanCriticalLastScanValue");
            this.linkLabelScanCriticalLastScanValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanCriticalLastScanValue.Name = "linkLabelScanCriticalLastScanValue";
            // 
            // linkLabelScanCriticalScannedValue
            // 
            resources.ApplyResources(this.linkLabelScanCriticalScannedValue, "linkLabelScanCriticalScannedValue");
            this.linkLabelScanCriticalScannedValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanCriticalScannedValue.Name = "linkLabelScanCriticalScannedValue";
            // 
            // linkLabelScanCriticalThreatsValue
            // 
            resources.ApplyResources(this.linkLabelScanCriticalThreatsValue, "linkLabelScanCriticalThreatsValue");
            this.linkLabelScanCriticalThreatsValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanCriticalThreatsValue.Name = "linkLabelScanCriticalThreatsValue";
            // 
            // linkLabelScanCriticalLastScan
            // 
            resources.ApplyResources(this.linkLabelScanCriticalLastScan, "linkLabelScanCriticalLastScan");
            this.linkLabelScanCriticalLastScan.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanCriticalLastScan.Name = "linkLabelScanCriticalLastScan";
            this.linkLabelScanCriticalLastScan.TabStop = true;
            // 
            // linkLabelScanCriticalScanned
            // 
            resources.ApplyResources(this.linkLabelScanCriticalScanned, "linkLabelScanCriticalScanned");
            this.linkLabelScanCriticalScanned.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanCriticalScanned.Name = "linkLabelScanCriticalScanned";
            this.linkLabelScanCriticalScanned.TabStop = true;
            // 
            // linkLabelScanCriticalThreats
            // 
            resources.ApplyResources(this.linkLabelScanCriticalThreats, "linkLabelScanCriticalThreats");
            this.linkLabelScanCriticalThreats.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanCriticalThreats.Name = "linkLabelScanCriticalThreats";
            this.linkLabelScanCriticalThreats.TabStop = true;
            // 
            // buttonSCriticalRemove
            // 
            resources.ApplyResources(this.buttonSCriticalRemove, "buttonSCriticalRemove");
            this.buttonSCriticalRemove.Name = "buttonSCriticalRemove";
            this.buttonSCriticalRemove.UseVisualStyleBackColor = true;
            this.buttonSCriticalRemove.Click += new System.EventHandler(this.buttonSCriticalRemove_Click);
            // 
            // groupBoxSCCriticalSettings
            // 
            this.groupBoxSCCriticalSettings.Controls.Add(this.linkLabelScanCriticalSettingsActionDescr);
            this.groupBoxSCCriticalSettings.Controls.Add(this.linkLabelScanCriticalAction);
            this.groupBoxSCCriticalSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxSCCriticalSettings, "groupBoxSCCriticalSettings");
            this.groupBoxSCCriticalSettings.Name = "groupBoxSCCriticalSettings";
            this.groupBoxSCCriticalSettings.TabStop = false;
            this.groupBoxSCCriticalSettings.Pressed += new ClamWinApp.GroupBoxEx.OnPressed(this.groupBoxSCCriticalSettings_Pressed);
            // 
            // linkLabelScanCriticalSettingsActionDescr
            // 
            resources.ApplyResources(this.linkLabelScanCriticalSettingsActionDescr, "linkLabelScanCriticalSettingsActionDescr");
            this.linkLabelScanCriticalSettingsActionDescr.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanCriticalSettingsActionDescr.Name = "linkLabelScanCriticalSettingsActionDescr";
            // 
            // linkLabelScanCriticalAction
            // 
            resources.ApplyResources(this.linkLabelScanCriticalAction, "linkLabelScanCriticalAction");
            this.linkLabelScanCriticalAction.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanCriticalAction.Name = "linkLabelScanCriticalAction";
            this.linkLabelScanCriticalAction.TabStop = true;
            // 
            // buttonSCriticalAdd
            // 
            resources.ApplyResources(this.buttonSCriticalAdd, "buttonSCriticalAdd");
            this.buttonSCriticalAdd.Name = "buttonSCriticalAdd";
            this.buttonSCriticalAdd.UseVisualStyleBackColor = true;
            this.buttonSCriticalAdd.Click += new System.EventHandler(this.buttonSCriticalAdd_Click);
            // 
            // listViewSCCriticalItems
            // 
            this.listViewSCCriticalItems.CheckBoxes = true;
            this.listViewSCCriticalItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            resources.ApplyResources(this.listViewSCCriticalItems, "listViewSCCriticalItems");
            this.listViewSCCriticalItems.Name = "listViewSCCriticalItems";
            this.listViewSCCriticalItems.SmallImageList = this.imageFilesList;
            this.listViewSCCriticalItems.UseCompatibleStateImageBehavior = false;
            this.listViewSCCriticalItems.View = System.Windows.Forms.View.Details;
            // 
            // panelProtection
            // 
            this.panelProtection.Controls.Add(this.groupBoxProtectionOnAccess);
            this.panelProtection.Controls.Add(this.label5);
            resources.ApplyResources(this.panelProtection, "panelProtection");
            this.panelProtection.Name = "panelProtection";
            // 
            // groupBoxProtectionOnAccess
            // 
            this.groupBoxProtectionOnAccess.Controls.Add(this.radioButtonOnAccessSuspended);
            this.groupBoxProtectionOnAccess.Controls.Add(this.radioButtonOnAccessOff);
            this.groupBoxProtectionOnAccess.Controls.Add(this.radioButtonOnAccessOn);
            resources.ApplyResources(this.groupBoxProtectionOnAccess, "groupBoxProtectionOnAccess");
            this.groupBoxProtectionOnAccess.Name = "groupBoxProtectionOnAccess";
            this.groupBoxProtectionOnAccess.TabStop = false;
            // 
            // radioButtonOnAccessSuspended
            // 
            resources.ApplyResources(this.radioButtonOnAccessSuspended, "radioButtonOnAccessSuspended");
            this.radioButtonOnAccessSuspended.Name = "radioButtonOnAccessSuspended";
            this.radioButtonOnAccessSuspended.TabStop = true;
            this.radioButtonOnAccessSuspended.UseVisualStyleBackColor = true;
            this.radioButtonOnAccessSuspended.CheckedChanged += new System.EventHandler(this.radioButtonOnAccessOn_CheckedChanged);
            // 
            // radioButtonOnAccessOff
            // 
            resources.ApplyResources(this.radioButtonOnAccessOff, "radioButtonOnAccessOff");
            this.radioButtonOnAccessOff.Name = "radioButtonOnAccessOff";
            this.radioButtonOnAccessOff.TabStop = true;
            this.radioButtonOnAccessOff.UseVisualStyleBackColor = true;
            this.radioButtonOnAccessOff.CheckedChanged += new System.EventHandler(this.radioButtonOnAccessOn_CheckedChanged);
            // 
            // radioButtonOnAccessOn
            // 
            resources.ApplyResources(this.radioButtonOnAccessOn, "radioButtonOnAccessOn");
            this.radioButtonOnAccessOn.Name = "radioButtonOnAccessOn";
            this.radioButtonOnAccessOn.TabStop = true;
            this.radioButtonOnAccessOn.UseVisualStyleBackColor = true;
            this.radioButtonOnAccessOn.CheckedChanged += new System.EventHandler(this.radioButtonOnAccessOn_CheckedChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // panelDataFiles
            // 
            this.panelDataFiles.Controls.Add(this.label4);
            this.panelDataFiles.Controls.Add(this.groupBoxDiskSpace);
            resources.ApplyResources(this.panelDataFiles, "panelDataFiles");
            this.panelDataFiles.Name = "panelDataFiles";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // groupBoxDiskSpace
            // 
            this.groupBoxDiskSpace.Controls.Add(this.linkLabelQuarantineSizeValue);
            this.groupBoxDiskSpace.Controls.Add(this.linkLabelQuarantineSize);
            this.groupBoxDiskSpace.Controls.Add(this.linkLabelDataBaseSizeValue);
            this.groupBoxDiskSpace.Controls.Add(this.linkLabelDataBaseSize);
            this.groupBoxDiskSpace.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxDiskSpace, "groupBoxDiskSpace");
            this.groupBoxDiskSpace.Name = "groupBoxDiskSpace";
            this.groupBoxDiskSpace.TabStop = false;
            // 
            // linkLabelDataBaseSizeValue
            // 
            resources.ApplyResources(this.linkLabelDataBaseSizeValue, "linkLabelDataBaseSizeValue");
            this.linkLabelDataBaseSizeValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelDataBaseSizeValue.Name = "linkLabelDataBaseSizeValue";
            // 
            // linkLabelDataBaseSize
            // 
            resources.ApplyResources(this.linkLabelDataBaseSize, "linkLabelDataBaseSize");
            this.linkLabelDataBaseSize.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelDataBaseSize.Name = "linkLabelDataBaseSize";
            this.linkLabelDataBaseSize.TabStop = true;
            // 
            // panelUpdate
            // 
            this.panelUpdate.Controls.Add(this.buttonUpdateNow);
            this.panelUpdate.Controls.Add(this.label3);
            this.panelUpdate.Controls.Add(this.groupBoxUpdateStatistics);
            this.panelUpdate.Controls.Add(this.groupBoxUpdateSettings);
            resources.ApplyResources(this.panelUpdate, "panelUpdate");
            this.panelUpdate.Name = "panelUpdate";
            // 
            // buttonUpdateNow
            // 
            resources.ApplyResources(this.buttonUpdateNow, "buttonUpdateNow");
            this.buttonUpdateNow.Name = "buttonUpdateNow";
            this.buttonUpdateNow.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBoxUpdateStatistics
            // 
            this.groupBoxUpdateStatistics.Controls.Add(this.linkLabelLastUpdateValue);
            this.groupBoxUpdateStatistics.Controls.Add(this.linkLabelLastUpdate);
            this.groupBoxUpdateStatistics.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxUpdateStatistics, "groupBoxUpdateStatistics");
            this.groupBoxUpdateStatistics.Name = "groupBoxUpdateStatistics";
            this.groupBoxUpdateStatistics.TabStop = false;
            // 
            // linkLabelLastUpdateValue
            // 
            resources.ApplyResources(this.linkLabelLastUpdateValue, "linkLabelLastUpdateValue");
            this.linkLabelLastUpdateValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelLastUpdateValue.Name = "linkLabelLastUpdateValue";
            // 
            // linkLabelLastUpdate
            // 
            resources.ApplyResources(this.linkLabelLastUpdate, "linkLabelLastUpdate");
            this.linkLabelLastUpdate.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelLastUpdate.Name = "linkLabelLastUpdate";
            this.linkLabelLastUpdate.TabStop = true;
            // 
            // groupBoxUpdateSettings
            // 
            this.groupBoxUpdateSettings.Controls.Add(this.linkLabelUpdateMethodValue);
            this.groupBoxUpdateSettings.Controls.Add(this.linkLabelUpdateMethod);
            this.groupBoxUpdateSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxUpdateSettings, "groupBoxUpdateSettings");
            this.groupBoxUpdateSettings.Name = "groupBoxUpdateSettings";
            this.groupBoxUpdateSettings.TabStop = false;
            this.groupBoxUpdateSettings.Pressed += new ClamWinApp.GroupBoxEx.OnPressed(this.groupBoxUpdateSettings_Pressed);
            // 
            // linkLabelUpdateMethodValue
            // 
            resources.ApplyResources(this.linkLabelUpdateMethodValue, "linkLabelUpdateMethodValue");
            this.linkLabelUpdateMethodValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelUpdateMethodValue.Name = "linkLabelUpdateMethodValue";
            this.linkLabelUpdateMethodValue.TabStop = true;
            // 
            // linkLabelUpdateMethod
            // 
            resources.ApplyResources(this.linkLabelUpdateMethod, "linkLabelUpdateMethod");
            this.linkLabelUpdateMethod.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelUpdateMethod.Name = "linkLabelUpdateMethod";
            this.linkLabelUpdateMethod.TabStop = true;
            // 
            // panelHome
            // 
            this.panelHome.Controls.Add(this.linkLabelStatistics);
            this.panelHome.Controls.Add(this.labelDBVersionValue);
            this.panelHome.Controls.Add(this.labelDBVersion);
            this.panelHome.Controls.Add(this.linkLabelUpdateNow);
            this.panelHome.Controls.Add(this.labelLastUpdatedValue);
            this.panelHome.Controls.Add(this.labelLastUpdated);
            this.panelHome.Controls.Add(this.linkLabelUpdates);
            this.panelHome.Controls.Add(this.linkLabelProtectionSettings);
            this.panelHome.Controls.Add(this.labelEmailProtectionValue);
            this.panelHome.Controls.Add(this.labelResidentFileProtectionValue);
            this.panelHome.Controls.Add(this.labelEmailProtection);
            this.panelHome.Controls.Add(this.labelResidentFileProtection);
            this.panelHome.Controls.Add(this.linkLabelProtectionStatus);
            this.panelHome.Controls.Add(this.labelRecomendations);
            this.panelHome.Controls.Add(this.pictureBoxProtectionLevel);
            this.panelHome.Controls.Add(this.linkLabelProtectionLevel);
            this.panelHome.Controls.Add(this.pictureBoxStatus);
            this.panelHome.Controls.Add(this.labelClamWinStatus);
            resources.ApplyResources(this.panelHome, "panelHome");
            this.panelHome.Name = "panelHome";
            // 
            // linkLabelStatistics
            // 
            this.linkLabelStatistics.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabelStatistics.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelStatistics.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.linkLabelStatistics.DisabledLinkColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.linkLabelStatistics, "linkLabelStatistics");
            this.linkLabelStatistics.Image = global::ClamWinApp.Properties.Resources.red_ball;
            this.linkLabelStatistics.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelStatistics.LinkColor = System.Drawing.Color.Black;
            this.linkLabelStatistics.Name = "linkLabelStatistics";
            this.linkLabelStatistics.VisitedLinkColor = System.Drawing.Color.Black;
            // 
            // labelDBVersionValue
            // 
            resources.ApplyResources(this.labelDBVersionValue, "labelDBVersionValue");
            this.labelDBVersionValue.Name = "labelDBVersionValue";
            // 
            // labelDBVersion
            // 
            resources.ApplyResources(this.labelDBVersion, "labelDBVersion");
            this.labelDBVersion.Name = "labelDBVersion";
            // 
            // linkLabelUpdateNow
            // 
            resources.ApplyResources(this.linkLabelUpdateNow, "linkLabelUpdateNow");
            this.linkLabelUpdateNow.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelUpdateNow.Name = "linkLabelUpdateNow";
            this.linkLabelUpdateNow.TabStop = true;
            this.linkLabelUpdateNow.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabelUpdateNow.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUpdateNow_LinkClicked);
            // 
            // labelLastUpdatedValue
            // 
            resources.ApplyResources(this.labelLastUpdatedValue, "labelLastUpdatedValue");
            this.labelLastUpdatedValue.ForeColor = System.Drawing.Color.Red;
            this.labelLastUpdatedValue.Name = "labelLastUpdatedValue";
            // 
            // labelLastUpdated
            // 
            resources.ApplyResources(this.labelLastUpdated, "labelLastUpdated");
            this.labelLastUpdated.Name = "labelLastUpdated";
            // 
            // linkLabelUpdates
            // 
            this.linkLabelUpdates.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabelUpdates.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelUpdates.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.linkLabelUpdates.DisabledLinkColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.linkLabelUpdates, "linkLabelUpdates");
            this.linkLabelUpdates.Image = global::ClamWinApp.Properties.Resources.red_ball;
            this.linkLabelUpdates.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelUpdates.LinkColor = System.Drawing.Color.Black;
            this.linkLabelUpdates.Name = "linkLabelUpdates";
            this.linkLabelUpdates.VisitedLinkColor = System.Drawing.Color.Black;
            // 
            // linkLabelProtectionSettings
            // 
            resources.ApplyResources(this.linkLabelProtectionSettings, "linkLabelProtectionSettings");
            this.linkLabelProtectionSettings.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelProtectionSettings.Name = "linkLabelProtectionSettings";
            this.linkLabelProtectionSettings.TabStop = true;
            this.linkLabelProtectionSettings.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabelProtectionSettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelProtectionSettings_LinkClicked);
            // 
            // labelEmailProtectionValue
            // 
            resources.ApplyResources(this.labelEmailProtectionValue, "labelEmailProtectionValue");
            this.labelEmailProtectionValue.Name = "labelEmailProtectionValue";
            // 
            // labelResidentFileProtectionValue
            // 
            resources.ApplyResources(this.labelResidentFileProtectionValue, "labelResidentFileProtectionValue");
            this.labelResidentFileProtectionValue.Name = "labelResidentFileProtectionValue";
            // 
            // labelEmailProtection
            // 
            resources.ApplyResources(this.labelEmailProtection, "labelEmailProtection");
            this.labelEmailProtection.Name = "labelEmailProtection";
            // 
            // labelResidentFileProtection
            // 
            resources.ApplyResources(this.labelResidentFileProtection, "labelResidentFileProtection");
            this.labelResidentFileProtection.Name = "labelResidentFileProtection";
            // 
            // linkLabelProtectionStatus
            // 
            this.linkLabelProtectionStatus.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabelProtectionStatus.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelProtectionStatus.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.linkLabelProtectionStatus.DisabledLinkColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.linkLabelProtectionStatus, "linkLabelProtectionStatus");
            this.linkLabelProtectionStatus.Image = global::ClamWinApp.Properties.Resources.red_ball;
            this.linkLabelProtectionStatus.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelProtectionStatus.LinkColor = System.Drawing.Color.Black;
            this.linkLabelProtectionStatus.Name = "linkLabelProtectionStatus";
            this.linkLabelProtectionStatus.VisitedLinkColor = System.Drawing.Color.Black;
            // 
            // labelRecomendations
            // 
            resources.ApplyResources(this.labelRecomendations, "labelRecomendations");
            this.labelRecomendations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.labelRecomendations.Name = "labelRecomendations";
            // 
            // pictureBoxProtectionLevel
            // 
            this.pictureBoxProtectionLevel.Image = global::ClamWinApp.Properties.Resources.low_pr_level;
            resources.ApplyResources(this.pictureBoxProtectionLevel, "pictureBoxProtectionLevel");
            this.pictureBoxProtectionLevel.Name = "pictureBoxProtectionLevel";
            this.pictureBoxProtectionLevel.TabStop = false;
            // 
            // linkLabelProtectionLevel
            // 
            this.linkLabelProtectionLevel.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabelProtectionLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.linkLabelProtectionLevel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.linkLabelProtectionLevel.DisabledLinkColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.linkLabelProtectionLevel, "linkLabelProtectionLevel");
            this.linkLabelProtectionLevel.Image = global::ClamWinApp.Properties.Resources.red_ball;
            this.linkLabelProtectionLevel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelProtectionLevel.LinkColor = System.Drawing.Color.Black;
            this.linkLabelProtectionLevel.Name = "linkLabelProtectionLevel";
            this.linkLabelProtectionLevel.VisitedLinkColor = System.Drawing.Color.Black;
            // 
            // pictureBoxStatus
            // 
            this.pictureBoxStatus.Image = global::ClamWinApp.Properties.Resources.StatusSubPanel;
            resources.ApplyResources(this.pictureBoxStatus, "pictureBoxStatus");
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.TabStop = false;
            // 
            // labelClamWinStatus
            // 
            resources.ApplyResources(this.labelClamWinStatus, "labelClamWinStatus");
            this.labelClamWinStatus.ForeColor = System.Drawing.Color.Navy;
            this.labelClamWinStatus.Name = "labelClamWinStatus";
            // 
            // panelQuarantine
            // 
            this.panelQuarantine.Controls.Add(this.groupBoxQuarantineManage);
            this.panelQuarantine.Controls.Add(this.groupBoxQuarantineStatistics);
            this.panelQuarantine.Controls.Add(this.labelQuarantine);
            resources.ApplyResources(this.panelQuarantine, "panelQuarantine");
            this.panelQuarantine.Name = "panelQuarantine";
            // 
            // groupBoxQuarantineManage
            // 
            this.groupBoxQuarantineManage.Controls.Add(this.buttonQuarantineManage);
            resources.ApplyResources(this.groupBoxQuarantineManage, "groupBoxQuarantineManage");
            this.groupBoxQuarantineManage.Name = "groupBoxQuarantineManage";
            this.groupBoxQuarantineManage.TabStop = false;
            // 
            // buttonQuarantineManage
            // 
            resources.ApplyResources(this.buttonQuarantineManage, "buttonQuarantineManage");
            this.buttonQuarantineManage.Name = "buttonQuarantineManage";
            this.buttonQuarantineManage.UseVisualStyleBackColor = true;
            this.buttonQuarantineManage.Click += new System.EventHandler(this.buttonQuarantineManage_Click);
            // 
            // groupBoxQuarantineStatistics
            // 
            this.groupBoxQuarantineStatistics.Controls.Add(this.linkLabelLastFileQuarantinedValue);
            this.groupBoxQuarantineStatistics.Controls.Add(this.linkLabelLastFileQuarantined);
            this.groupBoxQuarantineStatistics.Controls.Add(this.linkLabelQuarantineTotalSizeValue);
            this.groupBoxQuarantineStatistics.Controls.Add(this.linkLabelQuarantineTotalSize);
            this.groupBoxQuarantineStatistics.Controls.Add(this.linkLabelFilesQuarantinedValue);
            this.groupBoxQuarantineStatistics.Controls.Add(this.linkLabelFilesQuarantined);
            this.groupBoxQuarantineStatistics.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxQuarantineStatistics, "groupBoxQuarantineStatistics");
            this.groupBoxQuarantineStatistics.Name = "groupBoxQuarantineStatistics";
            this.groupBoxQuarantineStatistics.TabStop = false;
            this.groupBoxQuarantineStatistics.Pressed += new ClamWinApp.GroupBoxEx.OnPressed(this.groupBoxQuarantineStatistics_Pressed);
            // 
            // linkLabelLastFileQuarantinedValue
            // 
            resources.ApplyResources(this.linkLabelLastFileQuarantinedValue, "linkLabelLastFileQuarantinedValue");
            this.linkLabelLastFileQuarantinedValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelLastFileQuarantinedValue.Name = "linkLabelLastFileQuarantinedValue";
            this.linkLabelLastFileQuarantinedValue.TabStop = true;
            // 
            // linkLabelLastFileQuarantined
            // 
            resources.ApplyResources(this.linkLabelLastFileQuarantined, "linkLabelLastFileQuarantined");
            this.linkLabelLastFileQuarantined.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelLastFileQuarantined.Name = "linkLabelLastFileQuarantined";
            this.linkLabelLastFileQuarantined.TabStop = true;
            // 
            // linkLabelQuarantineTotalSizeValue
            // 
            resources.ApplyResources(this.linkLabelQuarantineTotalSizeValue, "linkLabelQuarantineTotalSizeValue");
            this.linkLabelQuarantineTotalSizeValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelQuarantineTotalSizeValue.Name = "linkLabelQuarantineTotalSizeValue";
            this.linkLabelQuarantineTotalSizeValue.TabStop = true;
            // 
            // linkLabelQuarantineTotalSize
            // 
            resources.ApplyResources(this.linkLabelQuarantineTotalSize, "linkLabelQuarantineTotalSize");
            this.linkLabelQuarantineTotalSize.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelQuarantineTotalSize.Name = "linkLabelQuarantineTotalSize";
            this.linkLabelQuarantineTotalSize.TabStop = true;
            // 
            // linkLabelFilesQuarantinedValue
            // 
            resources.ApplyResources(this.linkLabelFilesQuarantinedValue, "linkLabelFilesQuarantinedValue");
            this.linkLabelFilesQuarantinedValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelFilesQuarantinedValue.Name = "linkLabelFilesQuarantinedValue";
            this.linkLabelFilesQuarantinedValue.TabStop = true;
            // 
            // linkLabelFilesQuarantined
            // 
            resources.ApplyResources(this.linkLabelFilesQuarantined, "linkLabelFilesQuarantined");
            this.linkLabelFilesQuarantined.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelFilesQuarantined.Name = "linkLabelFilesQuarantined";
            this.linkLabelFilesQuarantined.TabStop = true;
            // 
            // labelQuarantine
            // 
            resources.ApplyResources(this.labelQuarantine, "labelQuarantine");
            this.labelQuarantine.Name = "labelQuarantine";
            // 
            // panelScanMyPC
            // 
            this.panelScanMyPC.Controls.Add(this.buttonSCMyPCScan);
            this.panelScanMyPC.Controls.Add(this.buttonSCMyPCRemove);
            this.panelScanMyPC.Controls.Add(this.buttonSCMyPCAdd);
            this.panelScanMyPC.Controls.Add(this.listViewSCMyPCItems);
            this.panelScanMyPC.Controls.Add(this.groupBoxSCMyPCStatistics);
            this.panelScanMyPC.Controls.Add(this.groupBoxSCMyPCSettings);
            resources.ApplyResources(this.panelScanMyPC, "panelScanMyPC");
            this.panelScanMyPC.Name = "panelScanMyPC";
            this.panelScanMyPC.VisibleChanged += new System.EventHandler(this.panelScanMyPC_VisibleChanged);
            // 
            // buttonSCMyPCScan
            // 
            resources.ApplyResources(this.buttonSCMyPCScan, "buttonSCMyPCScan");
            this.buttonSCMyPCScan.Name = "buttonSCMyPCScan";
            this.buttonSCMyPCScan.UseVisualStyleBackColor = true;
            this.buttonSCMyPCScan.Click += new System.EventHandler(this.buttonSCMyPCScan_Click);
            // 
            // buttonSCMyPCRemove
            // 
            resources.ApplyResources(this.buttonSCMyPCRemove, "buttonSCMyPCRemove");
            this.buttonSCMyPCRemove.Name = "buttonSCMyPCRemove";
            this.buttonSCMyPCRemove.UseVisualStyleBackColor = true;
            this.buttonSCMyPCRemove.Click += new System.EventHandler(this.buttonSCMyPCRemove_Click);
            // 
            // buttonSCMyPCAdd
            // 
            resources.ApplyResources(this.buttonSCMyPCAdd, "buttonSCMyPCAdd");
            this.buttonSCMyPCAdd.Name = "buttonSCMyPCAdd";
            this.buttonSCMyPCAdd.UseVisualStyleBackColor = true;
            this.buttonSCMyPCAdd.Click += new System.EventHandler(this.buttonSCMyPCAdd_Click);
            // 
            // listViewSCMyPCItems
            // 
            this.listViewSCMyPCItems.CheckBoxes = true;
            this.listViewSCMyPCItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            resources.ApplyResources(this.listViewSCMyPCItems, "listViewSCMyPCItems");
            this.listViewSCMyPCItems.Name = "listViewSCMyPCItems";
            this.listViewSCMyPCItems.SmallImageList = this.imageFilesList;
            this.listViewSCMyPCItems.UseCompatibleStateImageBehavior = false;
            this.listViewSCMyPCItems.View = System.Windows.Forms.View.Details;
            // 
            // groupBoxSCMyPCStatistics
            // 
            this.groupBoxSCMyPCStatistics.Controls.Add(this.progressBarScanMyPC);
            this.groupBoxSCMyPCStatistics.Controls.Add(this.linkLabelScanMyPCLastScanValue);
            this.groupBoxSCMyPCStatistics.Controls.Add(this.linkLabelScanMyPCScannedValue);
            this.groupBoxSCMyPCStatistics.Controls.Add(this.linkLabelScanMyPCThreatsValue);
            this.groupBoxSCMyPCStatistics.Controls.Add(this.linkLabelScanMyPCLastScan);
            this.groupBoxSCMyPCStatistics.Controls.Add(this.linkLabelScanMyPCScanned);
            this.groupBoxSCMyPCStatistics.Controls.Add(this.linkLabelScanMyPCThreats);
            this.groupBoxSCMyPCStatistics.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxSCMyPCStatistics, "groupBoxSCMyPCStatistics");
            this.groupBoxSCMyPCStatistics.Name = "groupBoxSCMyPCStatistics";
            this.groupBoxSCMyPCStatistics.TabStop = false;
            this.groupBoxSCMyPCStatistics.Pressed += new ClamWinApp.GroupBoxEx.OnPressed(this.groupBoxSCMyPCStatistics_Pressed);
            // 
            // progressBarScanMyPC
            // 
            resources.ApplyResources(this.progressBarScanMyPC, "progressBarScanMyPC");
            this.progressBarScanMyPC.Name = "progressBarScanMyPC";
            // 
            // linkLabelScanMyPCLastScanValue
            // 
            resources.ApplyResources(this.linkLabelScanMyPCLastScanValue, "linkLabelScanMyPCLastScanValue");
            this.linkLabelScanMyPCLastScanValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanMyPCLastScanValue.Name = "linkLabelScanMyPCLastScanValue";
            // 
            // linkLabelScanMyPCScannedValue
            // 
            resources.ApplyResources(this.linkLabelScanMyPCScannedValue, "linkLabelScanMyPCScannedValue");
            this.linkLabelScanMyPCScannedValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanMyPCScannedValue.Name = "linkLabelScanMyPCScannedValue";
            // 
            // linkLabelScanMyPCThreatsValue
            // 
            resources.ApplyResources(this.linkLabelScanMyPCThreatsValue, "linkLabelScanMyPCThreatsValue");
            this.linkLabelScanMyPCThreatsValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanMyPCThreatsValue.Name = "linkLabelScanMyPCThreatsValue";
            // 
            // linkLabelScanMyPCLastScan
            // 
            resources.ApplyResources(this.linkLabelScanMyPCLastScan, "linkLabelScanMyPCLastScan");
            this.linkLabelScanMyPCLastScan.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanMyPCLastScan.Name = "linkLabelScanMyPCLastScan";
            this.linkLabelScanMyPCLastScan.TabStop = true;
            // 
            // linkLabelScanMyPCScanned
            // 
            resources.ApplyResources(this.linkLabelScanMyPCScanned, "linkLabelScanMyPCScanned");
            this.linkLabelScanMyPCScanned.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanMyPCScanned.Name = "linkLabelScanMyPCScanned";
            this.linkLabelScanMyPCScanned.TabStop = true;
            // 
            // linkLabelScanMyPCThreats
            // 
            resources.ApplyResources(this.linkLabelScanMyPCThreats, "linkLabelScanMyPCThreats");
            this.linkLabelScanMyPCThreats.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanMyPCThreats.Name = "linkLabelScanMyPCThreats";
            this.linkLabelScanMyPCThreats.TabStop = true;
            // 
            // groupBoxSCMyPCSettings
            // 
            this.groupBoxSCMyPCSettings.Controls.Add(this.linkLabelScanMyPCSettingsActionDescr);
            this.groupBoxSCMyPCSettings.Controls.Add(this.linkLabelScanMyPCSettings);
            this.groupBoxSCMyPCSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxSCMyPCSettings, "groupBoxSCMyPCSettings");
            this.groupBoxSCMyPCSettings.Name = "groupBoxSCMyPCSettings";
            this.groupBoxSCMyPCSettings.TabStop = false;
            this.groupBoxSCMyPCSettings.Pressed += new ClamWinApp.GroupBoxEx.OnPressed(this.groupBoxSCMyPCSettings_Pressed);
            // 
            // linkLabelScanMyPCSettingsActionDescr
            // 
            resources.ApplyResources(this.linkLabelScanMyPCSettingsActionDescr, "linkLabelScanMyPCSettingsActionDescr");
            this.linkLabelScanMyPCSettingsActionDescr.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanMyPCSettingsActionDescr.Name = "linkLabelScanMyPCSettingsActionDescr";
            // 
            // linkLabelScanMyPCSettings
            // 
            resources.ApplyResources(this.linkLabelScanMyPCSettings, "linkLabelScanMyPCSettings");
            this.linkLabelScanMyPCSettings.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelScanMyPCSettings.Name = "linkLabelScanMyPCSettings";
            this.linkLabelScanMyPCSettings.TabStop = true;
            // 
            // panelService
            // 
            this.panelService.Controls.Add(this.label2);
            this.panelService.Controls.Add(this.groupBoxServiceProductInfo);
            resources.ApplyResources(this.panelService, "panelService");
            this.panelService.Name = "panelService";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBoxServiceProductInfo
            // 
            this.groupBoxServiceProductInfo.Controls.Add(this.linkLabelServiceVersionValue);
            this.groupBoxServiceProductInfo.Controls.Add(this.linkLabelServiceVersion);
            this.groupBoxServiceProductInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.groupBoxServiceProductInfo, "groupBoxServiceProductInfo");
            this.groupBoxServiceProductInfo.Name = "groupBoxServiceProductInfo";
            this.groupBoxServiceProductInfo.TabStop = false;
            // 
            // linkLabelServiceVersionValue
            // 
            resources.ApplyResources(this.linkLabelServiceVersionValue, "linkLabelServiceVersionValue");
            this.linkLabelServiceVersionValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelServiceVersionValue.Name = "linkLabelServiceVersionValue";
            // 
            // linkLabelServiceVersion
            // 
            resources.ApplyResources(this.linkLabelServiceVersion, "linkLabelServiceVersion");
            this.linkLabelServiceVersion.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelServiceVersion.Name = "linkLabelServiceVersion";
            this.linkLabelServiceVersion.TabStop = true;
            // 
            // panelExGroup
            // 
            this.panelExGroup.Border = 8;
            this.panelExGroup.Controls.Add(this.panelExHome);
            this.panelExGroup.Controls.Add(this.panelExScan);
            this.panelExGroup.Controls.Add(this.panelExQuarantine);
            this.panelExGroup.Controls.Add(this.panelExService);
            resources.ApplyResources(this.panelExGroup, "panelExGroup");
            this.panelExGroup.Name = "panelExGroup";
            this.panelExGroup.Spacing = 8;
            // 
            // panelExHome
            // 
            resources.ApplyResources(this.panelExHome, "panelExHome");
            this.panelExHome.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelExHome.ChangeCursor = true;
            this.panelExHome.DrawCollapseExpandIcons = false;
            this.panelExHome.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExHome.Image = global::ClamWinApp.Properties.Resources.dw;
            this.panelExHome.MouseSensitive = false;
            this.panelExHome.Name = "panelExHome";
            this.panelExHome.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExHome.StartColour = System.Drawing.Color.White;
            this.panelExHome.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExHome.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExHome.TitleText = "Home";
            this.panelExHome.TitlePressed += new PanelsEx.TitlePressedEventHandler(this.panelExHome_TitlePressed);
            // 
            // panelExScan
            // 
            resources.ApplyResources(this.panelExScan, "panelExScan");
            this.panelExScan.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelExScan.ChangeCursor = true;
            this.panelExScan.Controls.Add(this.linkScanCritical);
            this.panelExScan.Controls.Add(this.linkScanMyPC);
            this.panelExScan.DrawCollapseExpandIcons = false;
            this.panelExScan.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExScan.Image = global::ClamWinApp.Properties.Resources.dw;
            this.panelExScan.MouseSensitive = false;
            this.panelExScan.Name = "panelExScan";
            this.panelExScan.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExScan.StartColour = System.Drawing.Color.White;
            this.panelExScan.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExScan.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExScan.TitleText = "Scan";
            this.panelExScan.TitlePressed += new PanelsEx.TitlePressedEventHandler(this.panelExScan_TitlePressed);
            // 
            // linkScanCritical
            // 
            resources.ApplyResources(this.linkScanCritical, "linkScanCritical");
            this.linkScanCritical.Name = "linkScanCritical";
            this.linkScanCritical.TabStop = true;
            this.linkScanCritical.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkScanCritical_LinkClicked);
            // 
            // linkScanMyPC
            // 
            resources.ApplyResources(this.linkScanMyPC, "linkScanMyPC");
            this.linkScanMyPC.Name = "linkScanMyPC";
            this.linkScanMyPC.TabStop = true;
            this.linkScanMyPC.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkScanMyPC_LinkClicked);
            // 
            // panelExQuarantine
            // 
            resources.ApplyResources(this.panelExQuarantine, "panelExQuarantine");
            this.panelExQuarantine.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelExQuarantine.ChangeCursor = true;
            this.panelExQuarantine.DrawCollapseExpandIcons = false;
            this.panelExQuarantine.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExQuarantine.Image = global::ClamWinApp.Properties.Resources.dw;
            this.panelExQuarantine.MouseSensitive = false;
            this.panelExQuarantine.Name = "panelExQuarantine";
            this.panelExQuarantine.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExQuarantine.StartColour = System.Drawing.Color.White;
            this.panelExQuarantine.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExQuarantine.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExQuarantine.TitleText = "Quarantine";
            this.panelExQuarantine.TitlePressed += new PanelsEx.TitlePressedEventHandler(this.panelExQuarantine_TitlePressed);
            // 
            // panelExService
            // 
            resources.ApplyResources(this.panelExService, "panelExService");
            this.panelExService.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelExService.ChangeCursor = true;
            this.panelExService.Controls.Add(this.linkProtection);
            this.panelExService.Controls.Add(this.linkDatafiles);
            this.panelExService.Controls.Add(this.linkUpdate);
            this.panelExService.DrawCollapseExpandIcons = false;
            this.panelExService.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExService.Image = global::ClamWinApp.Properties.Resources.dw;
            this.panelExService.MouseSensitive = false;
            this.panelExService.Name = "panelExService";
            this.panelExService.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExService.StartColour = System.Drawing.Color.White;
            this.panelExService.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExService.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExService.TitleText = "Services";
            this.panelExService.TitlePressed += new PanelsEx.TitlePressedEventHandler(this.panelExService_TitlePressed);
            // 
            // linkProtection
            // 
            resources.ApplyResources(this.linkProtection, "linkProtection");
            this.linkProtection.Name = "linkProtection";
            this.linkProtection.TabStop = true;
            this.linkProtection.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkProtection_LinkClicked);
            // 
            // linkDatafiles
            // 
            resources.ApplyResources(this.linkDatafiles, "linkDatafiles");
            this.linkDatafiles.Name = "linkDatafiles";
            this.linkDatafiles.TabStop = true;
            this.linkDatafiles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDatafiles_LinkClicked);
            // 
            // linkUpdate
            // 
            resources.ApplyResources(this.linkUpdate, "linkUpdate");
            this.linkUpdate.Name = "linkUpdate";
            this.linkUpdate.TabStop = true;
            this.linkUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkUpdate_LinkClicked);
            // 
            // panelExNotification
            // 
            resources.ApplyResources(this.panelExNotification, "panelExNotification");
            this.panelExNotification.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelExNotification.ChangeCursor = false;
            this.panelExNotification.Controls.Add(this.linkLabelNotify);
            this.panelExNotification.Controls.Add(this.linkLabelNotifyNext);
            this.panelExNotification.Controls.Add(this.linkLabelNotifyBack);
            this.panelExNotification.DrawCollapseExpandIcons = false;
            this.panelExNotification.EndColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExNotification.Image = global::ClamWinApp.Properties.Resources.dw;
            this.panelExNotification.MouseSensitive = false;
            this.panelExNotification.Name = "panelExNotification";
            this.panelExNotification.PanelState = PanelsEx.PanelState.Expanded;
            this.panelExNotification.StartColour = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.panelExNotification.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExNotification.TitleFontColour = System.Drawing.Color.Navy;
            this.panelExNotification.TitleText = "Notifications";
            // 
            // linkLabelNotify
            // 
            resources.ApplyResources(this.linkLabelNotify, "linkLabelNotify");
            this.linkLabelNotify.Name = "linkLabelNotify";
            this.linkLabelNotify.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelNotify_LinkClicked);
            // 
            // linkLabelNotifyNext
            // 
            resources.ApplyResources(this.linkLabelNotifyNext, "linkLabelNotifyNext");
            this.linkLabelNotifyNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.linkLabelNotifyNext.DisabledLinkColor = System.Drawing.Color.Navy;
            this.linkLabelNotifyNext.ForeColor = System.Drawing.Color.Navy;
            this.linkLabelNotifyNext.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelNotifyNext.LinkColor = System.Drawing.Color.DarkGoldenrod;
            this.linkLabelNotifyNext.Name = "linkLabelNotifyNext";
            this.linkLabelNotifyNext.TabStop = true;
            this.linkLabelNotifyNext.Click += new System.EventHandler(this.linkLabelNotifyNext_Click);
            // 
            // linkLabelNotifyBack
            // 
            resources.ApplyResources(this.linkLabelNotifyBack, "linkLabelNotifyBack");
            this.linkLabelNotifyBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(212)))), ((int)(((byte)(247)))));
            this.linkLabelNotifyBack.DisabledLinkColor = System.Drawing.Color.DarkGoldenrod;
            this.linkLabelNotifyBack.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelNotifyBack.LinkColor = System.Drawing.Color.DarkGoldenrod;
            this.linkLabelNotifyBack.Name = "linkLabelNotifyBack";
            this.linkLabelNotifyBack.TabStop = true;
            this.linkLabelNotifyBack.Click += new System.EventHandler(this.linkLabelNotifyBack_Click);
            // 
            // linkLabelQuarantineSizeValue
            // 
            resources.ApplyResources(this.linkLabelQuarantineSizeValue, "linkLabelQuarantineSizeValue");
            this.linkLabelQuarantineSizeValue.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelQuarantineSizeValue.Name = "linkLabelQuarantineSizeValue";
            // 
            // linkLabelQuarantineSize
            // 
            resources.ApplyResources(this.linkLabelQuarantineSize, "linkLabelQuarantineSize");
            this.linkLabelQuarantineSize.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelQuarantineSize.Name = "linkLabelQuarantineSize";
            this.linkLabelQuarantineSize.TabStop = true;
            // 
            // ClamWinMainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.panelExNotification);
            this.Controls.Add(this.panelExRightPanel);
            this.Controls.Add(this.linkSettings);
            this.Controls.Add(this.linkHelp);
            this.Controls.Add(this.panelExGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ClamWinMainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClamWinMainForm_FormClosing);
            this.Load += new System.EventHandler(this.ClamWinMainForm_Load);
            this.menuTray.ResumeLayout(false);
            this.panelExRightPanel.ResumeLayout(false);
            this.panelScan.ResumeLayout(false);
            this.groupBoxSCStatistics.ResumeLayout(false);
            this.groupBoxSCStatistics.PerformLayout();
            this.groupBoxSCSettings.ResumeLayout(false);
            this.panelScanCritical.ResumeLayout(false);
            this.groupBoxSCCriticalStatistics.ResumeLayout(false);
            this.groupBoxSCCriticalStatistics.PerformLayout();
            this.groupBoxSCCriticalSettings.ResumeLayout(false);
            this.panelProtection.ResumeLayout(false);
            this.groupBoxProtectionOnAccess.ResumeLayout(false);
            this.groupBoxProtectionOnAccess.PerformLayout();
            this.panelDataFiles.ResumeLayout(false);
            this.groupBoxDiskSpace.ResumeLayout(false);
            this.groupBoxDiskSpace.PerformLayout();
            this.panelUpdate.ResumeLayout(false);
            this.groupBoxUpdateStatistics.ResumeLayout(false);
            this.groupBoxUpdateStatistics.PerformLayout();
            this.groupBoxUpdateSettings.ResumeLayout(false);
            this.groupBoxUpdateSettings.PerformLayout();
            this.panelHome.ResumeLayout(false);
            this.panelHome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProtectionLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
            this.panelQuarantine.ResumeLayout(false);
            this.panelQuarantine.PerformLayout();
            this.groupBoxQuarantineManage.ResumeLayout(false);
            this.groupBoxQuarantineStatistics.ResumeLayout(false);
            this.groupBoxQuarantineStatistics.PerformLayout();
            this.panelScanMyPC.ResumeLayout(false);
            this.groupBoxSCMyPCStatistics.ResumeLayout(false);
            this.groupBoxSCMyPCStatistics.PerformLayout();
            this.groupBoxSCMyPCSettings.ResumeLayout(false);
            this.panelService.ResumeLayout(false);
            this.groupBoxServiceProductInfo.ResumeLayout(false);
            this.groupBoxServiceProductInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelExGroup)).EndInit();
            this.panelExGroup.ResumeLayout(false);
            this.panelExScan.ResumeLayout(false);
            this.panelExScan.PerformLayout();
            this.panelExService.ResumeLayout(false);
            this.panelExService.PerformLayout();
            this.panelExNotification.ResumeLayout(false);
            this.panelExNotification.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip menuTray;
        private System.Windows.Forms.ToolStripMenuItem menuItemTrayExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemTrayOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.LinkLabel linkHelp;
        private System.Windows.Forms.LinkLabel linkSettings;
        private PanelsEx.PanelExGroup panelExGroup;
        private PanelsEx.PanelEx panelExService;
        private PanelsEx.PanelEx panelExScan;
        private PanelsEx.PanelEx panelExRightPanel;
        private System.Windows.Forms.LinkLabel linkProtection;
        private System.Windows.Forms.LinkLabel linkDatafiles;
        private System.Windows.Forms.LinkLabel linkUpdate;
        private System.Windows.Forms.LinkLabel linkScanMyPC;
        private System.Windows.Forms.Panel panelHome;
        private System.Windows.Forms.Panel panelQuarantine;
        private System.Windows.Forms.Panel panelScan;
        private GroupBoxEx groupBoxSCStatistics;
        private GroupBoxEx groupBoxSCSettings;
        private System.Windows.Forms.Label labelClamWinStatus;
        private System.Windows.Forms.Label labelQuarantine;
        private System.Windows.Forms.Panel panelScanCritical;
        private GroupBoxEx groupBoxSCCriticalStatistics;
        GroupBoxEx groupBoxSCCriticalSettings;
        private System.Data.SQLite.SQLiteDataAdapter sqLiteDataAdapter;
        private System.Windows.Forms.ListView listViewSCItems;
        private System.Windows.Forms.Button buttonSCRemove;
        private System.Windows.Forms.Button buttonSCAdd;
        private System.Windows.Forms.Button buttonSCScan;
        private System.Windows.Forms.ImageList imageFilesList;
        private System.Windows.Forms.Button buttonSCriticalScan;
        private System.Windows.Forms.Button buttonSCriticalRemove;
        private System.Windows.Forms.Button buttonSCriticalAdd;
        private System.Windows.Forms.ListView listViewSCCriticalItems;
        private System.Windows.Forms.LinkLabel linkScanCritical;
        private System.Windows.Forms.Panel panelScanMyPC;
        private System.Windows.Forms.Button buttonSCMyPCScan;
        private System.Windows.Forms.Button buttonSCMyPCRemove;
        private System.Windows.Forms.Button buttonSCMyPCAdd;
        private System.Windows.Forms.ListView listViewSCMyPCItems;
        private GroupBoxEx groupBoxSCMyPCStatistics;
        private GroupBoxEx groupBoxSCMyPCSettings;
        private System.Windows.Forms.LinkLabel linkLabelScanMyPCSettings;
        private System.Windows.Forms.LinkLabel linkLabelScanMyPCSettingsActionDescr;
        private System.Windows.Forms.LinkLabel linkLabelScanSettingsActionDescr;
        private System.Windows.Forms.LinkLabel linkLabelScanSettings;
        private System.Windows.Forms.LinkLabel linkLabelScanCriticalSettingsActionDescr;
        private System.Windows.Forms.LinkLabel linkLabelScanCriticalAction;
        private System.Windows.Forms.LinkLabel linkLabelScanLastScanValue;
        private System.Windows.Forms.LinkLabel linkLabelScanScannedValue;
        private System.Windows.Forms.LinkLabel linkLabelScanThreatsValue;
        private System.Windows.Forms.LinkLabel linkLabelScanLastScan;
        private System.Windows.Forms.LinkLabel linkLabelScanScanned;
        private System.Windows.Forms.LinkLabel linkLabelScanThreats;
        private System.Windows.Forms.LinkLabel linkLabelScanMyPCLastScanValue;
        private System.Windows.Forms.LinkLabel linkLabelScanMyPCScannedValue;
        private System.Windows.Forms.LinkLabel linkLabelScanMyPCThreatsValue;
        private System.Windows.Forms.LinkLabel linkLabelScanMyPCLastScan;
        private System.Windows.Forms.LinkLabel linkLabelScanMyPCScanned;
        private System.Windows.Forms.LinkLabel linkLabelScanMyPCThreats;
        private System.Windows.Forms.LinkLabel linkLabelScanCriticalLastScanValue;
        private System.Windows.Forms.LinkLabel linkLabelScanCriticalScannedValue;
        private System.Windows.Forms.LinkLabel linkLabelScanCriticalThreatsValue;
        private System.Windows.Forms.LinkLabel linkLabelScanCriticalLastScan;
        private System.Windows.Forms.LinkLabel linkLabelScanCriticalScanned;
        private System.Windows.Forms.LinkLabel linkLabelScanCriticalThreats;
        private System.Windows.Forms.ProgressBar progressBarScanMyPC;
        private System.Windows.Forms.ProgressBar progressBarScan;
        private System.Windows.Forms.ProgressBar progressBarScanCritical;
        private System.Windows.Forms.Panel panelService;
        private System.Windows.Forms.Label label2;
        private GroupBoxEx groupBoxServiceProductInfo;
        private System.Windows.Forms.LinkLabel linkLabelServiceVersionValue;
        private System.Windows.Forms.LinkLabel linkLabelServiceVersion;
        private System.Windows.Forms.Panel panelUpdate;
        private System.Windows.Forms.Label label3;
        private GroupBoxEx groupBoxUpdateStatistics;
        private System.Windows.Forms.LinkLabel linkLabelLastUpdateValue;
        private System.Windows.Forms.LinkLabel linkLabelLastUpdate;
        private GroupBoxEx groupBoxUpdateSettings;
        private System.Windows.Forms.LinkLabel linkLabelUpdateMethodValue;
        private System.Windows.Forms.LinkLabel linkLabelUpdateMethod;
        private System.Windows.Forms.Panel panelDataFiles;
        private System.Windows.Forms.Label label4;
        private GroupBoxEx groupBoxDiskSpace;
        private System.Windows.Forms.LinkLabel linkLabelDataBaseSizeValue;
        private System.Windows.Forms.LinkLabel linkLabelDataBaseSize;
        private System.Windows.Forms.Panel panelProtection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonUpdateNow;
        private PanelsEx.PanelEx panelExHome;
        private PanelsEx.PanelEx panelExQuarantine;
        private System.Windows.Forms.PictureBox pictureBoxStatus;
        private System.Windows.Forms.LinkLabel linkLabelProtectionLevel;
        private System.Windows.Forms.PictureBox pictureBoxProtectionLevel;
        private System.Windows.Forms.Label labelRecomendations;
        private System.Windows.Forms.LinkLabel linkLabelProtectionStatus;
        private System.Windows.Forms.Label labelEmailProtectionValue;
        private System.Windows.Forms.Label labelResidentFileProtectionValue;
        private System.Windows.Forms.Label labelEmailProtection;
        private System.Windows.Forms.Label labelResidentFileProtection;
        private System.Windows.Forms.LinkLabel linkLabelProtectionSettings;
        private System.Windows.Forms.Label labelLastUpdatedValue;
        private System.Windows.Forms.Label labelLastUpdated;
        private System.Windows.Forms.LinkLabel linkLabelUpdates;
        private System.Windows.Forms.LinkLabel linkLabelUpdateNow;
        private System.Windows.Forms.Label labelDBVersion;
        private System.Windows.Forms.LinkLabel linkLabelStatistics;
        private System.Windows.Forms.Label labelDBVersionValue;
        private GroupBoxEx groupBoxQuarantineStatistics;
        private System.Windows.Forms.LinkLabel linkLabelQuarantineTotalSizeValue;
        private System.Windows.Forms.LinkLabel linkLabelQuarantineTotalSize;
        private System.Windows.Forms.LinkLabel linkLabelFilesQuarantinedValue;
        private System.Windows.Forms.LinkLabel linkLabelFilesQuarantined;
        private System.Windows.Forms.GroupBox groupBoxQuarantineManage;
        private System.Windows.Forms.Button buttonQuarantineManage;
        private System.Windows.Forms.LinkLabel linkLabelLastFileQuarantinedValue;
        private System.Windows.Forms.LinkLabel linkLabelLastFileQuarantined;
        private System.Windows.Forms.GroupBox groupBoxProtectionOnAccess;
        private System.Windows.Forms.RadioButton radioButtonOnAccessSuspended;
        private System.Windows.Forms.RadioButton radioButtonOnAccessOff;
        private System.Windows.Forms.RadioButton radioButtonOnAccessOn;
        private PanelsEx.PanelEx panelExNotification;
        private System.Windows.Forms.LinkLabel linkLabelNotify;
        private System.Windows.Forms.LinkLabel linkLabelNotifyNext;
        private System.Windows.Forms.LinkLabel linkLabelNotifyBack;
        private System.Windows.Forms.LinkLabel linkLabelQuarantineSizeValue;
        private System.Windows.Forms.LinkLabel linkLabelQuarantineSize;

    }
}

