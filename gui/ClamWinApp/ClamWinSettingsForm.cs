// Name:        ClamWinSettingsForm.cs
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
    #region ClamWinSettingsForm class
    public partial class ClamWinSettingsForm : Form
    {
        #region Private Data
        /// <summary>
        /// Keep update schedule data
        /// </summary>
        private ClamWinScheduleData UpdateScheduleData = new ClamWinScheduleData(ClamWinSettings.SettingIDToString(ClamWinSettings.SettingIDs.UpdateScheduleData), ClamWinScheduleData.UpdateArg);
        /// <summary>
        /// Keep ScanCritical schedule data
        /// </summary>
        private ClamWinScheduleData ScanCriticalScheduleData = new ClamWinScheduleData(ClamWinSettings.SettingIDToString(ClamWinSettings.SettingIDs.ScanCriticalScheduleData), ClamWinScheduleData.ScanCriticalArg);        
        /// <summary>
        /// Keep ScanMyPC schedule data
        /// </summary>
        private ClamWinScheduleData ScanMyPCScheduleData = new ClamWinScheduleData(ClamWinSettings.SettingIDToString(ClamWinSettings.SettingIDs.ScanMyPCScheduleData), ClamWinScheduleData.ScanMyPCArg);
        /// <summary>
        /// Keep Scan schedule data
        /// </summary>
        private ClamWinScheduleData ScanScheduleData = new ClamWinScheduleData(ClamWinSettings.SettingIDToString(ClamWinSettings.SettingIDs.ScanScheduleData), ClamWinScheduleData.ScanArg);
        /// <summary>
        /// Keep Scan Filter Data
        /// </summary>
        private ClamWinFilterData ScanFilterData = new ClamWinFilterData(ClamWinSettings.SettingIDToString(ClamWinSettings.SettingIDs.ScanFilterData));
        /// <summary>
        /// Keep Scan Critical Filter Data
        /// </summary>
        private ClamWinFilterData ScanCriticalFilterData = new ClamWinFilterData(ClamWinSettings.SettingIDToString(ClamWinSettings.SettingIDs.ScanCriticalFilterData));
        /// <summary>
        /// Keep Scan My Computer Filter Data
        /// </summary>
        private ClamWinFilterData ScanMyPCFilterData = new ClamWinFilterData(ClamWinSettings.SettingIDToString(ClamWinSettings.SettingIDs.ScanMyPCFilterData));
        #endregion

        #region Public Constructor
        public ClamWinSettingsForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Hide all right panels
        /// </summary>
        private void HideRightPannels()
        {
            foreach (Control control in this.Controls)
            {
                if (!(control is PanelsEx.PanelEx))
                {
                    if (control is Panel)
                    {
                        control.Visible = false;
                    }
                }
            }
        }
        /// <summary>
        /// Apply current settings and save them to database
        /// </summary>
        private void ApplySettings()
        {
            // Protection panel
            ClamWinSettings.EnableProtection = checkBoxEnableProtection.Checked;
            ClamWinSettings.RunAtStartup = checkBoxRunAtStartup.Checked;

            // File Antivirus Panel
            ClamWinSettings.OnAccessScannerStatus = checkBoxEnableFA.Checked ? 
                                                    ClamWinSettings.OnAccessStatus.Enabled :
                                                    ClamWinSettings.OnAccessStatus.Disabled;

            if( radioButtonFAPrompt.Checked)
            {
                ClamWinSettings.FileAntiVirusOnDetectAction = ClamWinSettings.OnDetectActions.Prompt;
            }
            else if(radioButtonFABlock.Checked)
            {
                ClamWinSettings.FileAntiVirusOnDetectAction = ClamWinSettings.OnDetectActions.BlockAccess;
            }
            else if(radioButtonFADelete.Checked)
            {
                ClamWinSettings.FileAntiVirusOnDetectAction = ClamWinSettings.OnDetectActions.Delete;
            }
            else if(radioButtonFAQuarantine.Checked)
            {
                ClamWinSettings.FileAntiVirusOnDetectAction = ClamWinSettings.OnDetectActions.MoveToQuarantine;
            }           

            // Mail Antivirus Panel
            ClamWinSettings.EnableMailAntiVirus = checkBoxMAEnable.Checked;

            // Scan
            if (radioButtonScanPromptDuringScan.Checked)
            {
                ClamWinSettings.ScanOnDetectAction = ClamWinSettings.OnDetectActions.Prompt;
            }
            else if (radioButtonScanPromptAfterScan.Checked)
            {
                ClamWinSettings.ScanOnDetectAction = ClamWinSettings.OnDetectActions.PromptAfterScan;
            }
            else if (radioButtonScanDelete.Checked)
            {
                ClamWinSettings.ScanOnDetectAction = ClamWinSettings.OnDetectActions.Delete;
            }
            else if (radioButtonScanDoNothing.Checked)
            {
                ClamWinSettings.ScanOnDetectAction = ClamWinSettings.OnDetectActions.DoNothing;
            }

            ClamWinSettings.ScanSchedule = checkBoxScanSchedule.Checked;
            if (checkBoxScanSchedule.Checked)
            {
                ClamWinSettings.ScanScheduleData = ScanScheduleData;
            }

            ClamWinSettings.ScanUseFilter = checkBoxScanUseFilter.Checked;
            ClamWinSettings.ScanFilterData = ScanFilterData;           

            // Scan Critical Panel
            if (radioButtonScanCriticalPromptDuringScan.Checked)
            {
                ClamWinSettings.ScanCriticalOnDetectAction = ClamWinSettings.OnDetectActions.Prompt;
            }
            else if (radioButtonScanCriticalPromptAfterScan.Checked)
            {
                ClamWinSettings.ScanCriticalOnDetectAction = ClamWinSettings.OnDetectActions.PromptAfterScan;
            }
            else if (radioButtonScanCriticalDelete.Checked)
            {
                ClamWinSettings.ScanCriticalOnDetectAction = ClamWinSettings.OnDetectActions.Delete;
            }
            else if (radioButtonScanCriticalDoNothing.Checked)
            {
                ClamWinSettings.ScanCriticalOnDetectAction = ClamWinSettings.OnDetectActions.DoNothing;
            }
            
            ClamWinSettings.ScanCriticalSchedule = checkBoxScanCriticalSchedule.Checked;
            if (checkBoxScanCriticalSchedule.Checked)
            {
                ClamWinSettings.ScanCriticalScheduleData = ScanCriticalScheduleData;
            }

            ClamWinSettings.ScanCriticalUseFilter = checkBoxScanCriticalUseFilter.Checked;
            ClamWinSettings.ScanCriticalFilterData = ScanCriticalFilterData;            

            // Scan My PC Panel
            if (radioButtonScanMyPCPromptDuringScan.Checked)
            {
                ClamWinSettings.ScanMyPCOnDetectAction = ClamWinSettings.OnDetectActions.Prompt;
            }
            else if (radioButtonScanMyPCPromptAfterScan.Checked)
            {
                ClamWinSettings.ScanMyPCOnDetectAction = ClamWinSettings.OnDetectActions.PromptAfterScan;
            }
            else if (radioButtonScanMyPCDelete.Checked)
            {
                ClamWinSettings.ScanMyPCOnDetectAction = ClamWinSettings.OnDetectActions.Delete;
            }
            else if (radioButtonScanMyPCDoNothing.Checked)
            {
                ClamWinSettings.ScanMyPCOnDetectAction = ClamWinSettings.OnDetectActions.DoNothing;
            }

            ClamWinSettings.ScanMyPCSchedule = checkBoxScanMyPCSchedule.Checked;
            if (checkBoxScanMyPCSchedule.Checked)
            {
                ClamWinSettings.ScanMyPCScheduleData = ScanMyPCScheduleData;
            }

            ClamWinSettings.ScanMyPCUseFilter = checkBoxScanMyPCUseFilter.Checked;
            ClamWinSettings.ScanMyPCFilterData = ScanMyPCFilterData;            

            // Service Panel
            ClamWinSettings.EnableNotifications = checkBoxEnableNotifications.Checked;

            // Update Panel            
            if (radioButtonUpdateAuto.Checked)
            {
                ClamWinSettings.UpdateRunMode = ClamWinSettings.RunModes.Auto;
            }
            else if (radioButtonUpdateManual.Checked)
            {
                ClamWinSettings.UpdateRunMode = ClamWinSettings.RunModes.Manual;
            }
            else if (radioButtonUpdateEvery.Checked)
            {
                ClamWinSettings.UpdateRunMode = ClamWinSettings.RunModes.Scheduled;
                ClamWinSettings.UpdateScheduleData = UpdateScheduleData;
                radioButtonUpdateEvery.Text = UpdateScheduleData.GetDescription();
            }

            ClamWinSettings.RescanQuarantineAfterUpdate = checkBoxRescanQuarantine.Checked;

            // Data Files Panel
            ClamWinSettings.KeepOnlyRecentEvents = checkBoxKeepOnlyRecent.Checked;
            ClamWinSettings.LogNonCriticalEvents = checkBoxLogNonCritical.Checked;
            ClamWinSettings.DeleteReportsAfterTime = checkBoxDeleteReports.Checked;
            ClamWinSettings.DeleteQuarantineItemsAfterTime = checkBoxDeleteFromQuarantine.Checked;

            ClamWinSettings.ReportsLifeTime = (int)numericUpDownDeleteReportsDays.Value;
            ClamWinSettings.DeleteReportsAfterTime = numericUpDownDeleteReportsDays.Enabled;
            ClamWinSettings.QuarantineItemsLifeTime = (int)numericUpDownDeleteFromQuarantineDays.Value;
            ClamWinSettings.DeleteQuarantineItemsAfterTime = numericUpDownDeleteFromQuarantineDays.Enabled;

            ClamWinSettings.SaveSettings();
        }
        /// <summary>
        /// Assign settings values to controls
        /// </summary>
        private void AssignSettingsToControls()
        { 
            // Protection panel
            checkBoxEnableProtection.Checked = ClamWinSettings.EnableProtection;
            checkBoxRunAtStartup.Checked = ClamWinSettings.RunAtStartup;

            // File Antivirus Panel
            checkBoxEnableFA.Checked = ClamWinSettings.OnAccessScannerStatus == ClamWinSettings.OnAccessStatus.Enabled ?
                                       true : false;

            switch (ClamWinSettings.FileAntiVirusOnDetectAction)
            {
                case ClamWinSettings.OnDetectActions.Prompt:
                    radioButtonFAPrompt.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.BlockAccess:
                    radioButtonFABlock.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.Delete:
                    radioButtonFADelete.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.MoveToQuarantine:
                    radioButtonFAQuarantine.Checked = true;
                    break;
            }

            // Mail Antivirus Panel
            checkBoxMAEnable.Checked = ClamWinSettings.EnableMailAntiVirus;

            // Scan
            switch (ClamWinSettings.ScanOnDetectAction)
            {
                case ClamWinSettings.OnDetectActions.Prompt:
                    radioButtonScanPromptDuringScan.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.PromptAfterScan:
                    radioButtonScanPromptAfterScan.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.Delete:
                    radioButtonScanDelete.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.DoNothing:
                    radioButtonScanDoNothing.Checked = true;
                    break;
            }
            ScanScheduleData = ClamWinSettings.ScanScheduleData;
            ScanFilterData = ClamWinSettings.ScanFilterData;
            checkBoxScanSchedule.Checked = ClamWinSettings.ScanSchedule;
            checkBoxScanSchedule.Text = ClamWinSettings.ScanScheduleData.GetDescription();
            checkBoxScanUseFilter.Checked = ClamWinSettings.ScanUseFilter;

            // Scan Critical Panel
            switch (ClamWinSettings.ScanCriticalOnDetectAction)
            {
                case ClamWinSettings.OnDetectActions.Prompt:
                    radioButtonScanCriticalPromptDuringScan.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.PromptAfterScan:
                    radioButtonScanCriticalPromptAfterScan.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.Delete:
                    radioButtonScanCriticalDelete.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.DoNothing:
                    radioButtonScanCriticalDoNothing.Checked = true;
                    break;
            }
            ScanCriticalScheduleData = ClamWinSettings.ScanCriticalScheduleData;
            checkBoxScanCriticalSchedule.Checked = ClamWinSettings.ScanCriticalSchedule;            
            checkBoxScanCriticalSchedule.Text = ClamWinSettings.ScanCriticalScheduleData.GetDescription();
            ScanCriticalFilterData = ClamWinSettings.ScanCriticalFilterData;
            checkBoxScanCriticalUseFilter.Checked = ClamWinSettings.ScanCriticalUseFilter;

            // Scan My PC Panel
            switch (ClamWinSettings.ScanMyPCOnDetectAction)
            {
                case ClamWinSettings.OnDetectActions.Prompt:
                    radioButtonScanMyPCPromptDuringScan.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.PromptAfterScan:
                    radioButtonScanMyPCPromptAfterScan.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.Delete:
                    radioButtonScanMyPCDelete.Checked = true;
                    break;
                case ClamWinSettings.OnDetectActions.DoNothing:
                    radioButtonScanMyPCDoNothing.Checked = true;
                    break;
            }

            ScanMyPCScheduleData = ClamWinSettings.ScanMyPCScheduleData;
            checkBoxScanMyPCSchedule.Checked = ClamWinSettings.ScanMyPCSchedule;
            checkBoxScanMyPCSchedule.Text = ClamWinSettings.ScanMyPCScheduleData.GetDescription();
            ScanMyPCFilterData = ClamWinSettings.ScanMyPCFilterData;
            checkBoxScanMyPCUseFilter.Checked = ClamWinSettings.ScanMyPCUseFilter;

            // Service Panel
            checkBoxEnableNotifications.Checked = ClamWinSettings.EnableNotifications;

            // Update Panel
            UpdateScheduleData = ClamWinSettings.UpdateScheduleData;

            switch(ClamWinSettings.UpdateRunMode)
            {
                case ClamWinSettings.RunModes.Auto:
                    radioButtonUpdateAuto.Checked = true;
                    break;
                case ClamWinSettings.RunModes.Manual:
                    radioButtonUpdateManual.Checked = true;
                    break;
                case ClamWinSettings.RunModes.Scheduled:
                    radioButtonUpdateEvery.Checked = true;                    
                    break;
            }
            radioButtonUpdateEvery.Text = UpdateScheduleData.GetDescription();
            checkBoxRescanQuarantine.Checked = ClamWinSettings.RescanQuarantineAfterUpdate;           

            // Data Files Panel
            checkBoxKeepOnlyRecent.Checked = ClamWinSettings.KeepOnlyRecentEvents; 
            checkBoxLogNonCritical.Checked = ClamWinSettings.LogNonCriticalEvents;
            checkBoxDeleteReports.Checked = ClamWinSettings.DeleteReportsAfterTime;
            checkBoxDeleteFromQuarantine.Checked = ClamWinSettings.DeleteQuarantineItemsAfterTime;

            numericUpDownDeleteReportsDays.Value = ClamWinSettings.ReportsLifeTime;
            numericUpDownDeleteReportsDays.Enabled = ClamWinSettings.DeleteReportsAfterTime;
            numericUpDownDeleteFromQuarantineDays.Value = ClamWinSettings.QuarantineItemsLifeTime;
            numericUpDownDeleteFromQuarantineDays.Enabled = ClamWinSettings.DeleteQuarantineItemsAfterTime;          

        }
        #endregion

        #region Event Handlers

        #region Buttons Event Handlers       
        /// <summary>
        /// buttonSettingsOK on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSettingsOK_Click(object sender, EventArgs e)
        {
            if (buttonSettingsApply.Enabled)
            {
                ApplySettings();
            }
            Close();
        }
        /// <summary>
        /// buttonSettingsClose on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSettingsClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// buttonSettingsApply on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSettingsApply_Click(object sender, EventArgs e)
        {
            ApplySettings();
            buttonSettingsApply.Enabled = false;
        }
        /// <summary>
        /// buttonUpdateEveryChange on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdateEveryChange_Click(object sender, EventArgs e)
        {
            ClamWinScheduleForm form = new ClamWinScheduleForm();
            DialogResult result = form.ScheduleDoModal("Update", ref UpdateScheduleData);

            if (result != DialogResult.OK)
            {
                return;
            }

            buttonSettingsApply.Enabled = true;

            radioButtonUpdateEvery.Text = UpdateScheduleData.GetDescription();
        }
        /// <summary>
        /// buttonScanCriticalScheduleChange on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScanCriticalScheduleChange_Click(object sender, EventArgs e)
        {
            ClamWinScheduleForm form = new ClamWinScheduleForm();
            DialogResult result = form.ScheduleDoModal("Scan Critical Areas", ref ScanCriticalScheduleData);

            if (result != DialogResult.OK)
            {
                return;
            }

            buttonSettingsApply.Enabled = true;

            checkBoxScanCriticalSchedule.Text = ScanCriticalScheduleData.GetDescription();
        }
        /// <summary>
        /// buttonScanMyPCSchedule on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScanMyPCSchedule_Click(object sender, EventArgs e)
        {
            ClamWinScheduleForm form = new ClamWinScheduleForm();
            DialogResult result = form.ScheduleDoModal("Scan My Computer", ref ScanMyPCScheduleData);

            if (result != DialogResult.OK)
            {
                return;
            }

            buttonSettingsApply.Enabled = true;

            checkBoxScanMyPCSchedule.Text = ScanMyPCScheduleData.GetDescription();
        }
        /// <summary>
        /// buttonScanScheduleChange on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScanScheduleChange_Click(object sender, EventArgs e)
        {
            ClamWinScheduleForm form = new ClamWinScheduleForm();
            DialogResult result = form.ScheduleDoModal("Scan", ref ScanScheduleData);

            if (result != DialogResult.OK)
            {
                return;
            }

            buttonSettingsApply.Enabled = true;

            checkBoxScanSchedule.Text = ScanScheduleData.GetDescription();
        }
        /// <summary>
        /// buttonScanEditFilter on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScanEditFilter_Click(object sender, EventArgs e)
        {
            ClamWinFilterForm form = new ClamWinFilterForm();            
            DialogResult result = form.FilterDoModal("Scan", ref ScanFilterData);

            if (result != DialogResult.OK)
            {
                return;
            }

            buttonSettingsApply.Enabled = true;
        }
        /// <summary>
        /// buttonScanCriticalEditFilter on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScanCriticalEditFilter_Click(object sender, EventArgs e)
        {
            ClamWinFilterForm form = new ClamWinFilterForm();
            DialogResult result = form.FilterDoModal("Scan Critical Areas", ref ScanCriticalFilterData);

            if (result != DialogResult.OK)
            {
                return;
            }

            buttonSettingsApply.Enabled = true;
        }
        /// <summary>
        /// buttonScanMyPCEditFilter on-click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScanMyPCEditFilter_Click(object sender, EventArgs e)
        {
            ClamWinFilterForm form = new ClamWinFilterForm();
            DialogResult result = form.FilterDoModal("Scan My Computer", ref ScanMyPCFilterData);

            if (result != DialogResult.OK)
            {
                return;
            }

            buttonSettingsApply.Enabled = true;
        }
        #endregion

        #region Tree View Event Handlers
        /// <summary>
        /// treeViewSettings AfterSelect event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            panelExCurrentPanel.TitleText = e.Node.Text + ":";
            switch (e.Node.Name)
            {
                case "NodeProtection":
                    OnNodeProtectionSelect();
                    return;
                case "NodeFileAntivirus":
                    OnNodeFileAntivirusSelect();
                    return;
                case "NodeMailAntivirus":
                    OnNodeMailAntivirusSelect();
                    return;
                case "NodeScan":
                    OnNodeScanSelect();
                    return;
                case "NodeScanMyComputer":
                    OnNodeScanMyComputerSelect();
                    return;
                case "NodeScanCriticalAreas":
                    OnNodeScanCriticalSelect();
                    return;
                case "NodeService":
                    OnNodeServiceSelect();
                    return;
                case "NodeUpdate":
                    OnNodeUpdateSelect();
                    return;
                case "NodeDataFiles":
                    OnNodeDataFilesSelect();
                    return;
                default:
                    return;

            }
        }   
        /// <summary>
        /// NodeProtection on-select event handler
        /// </summary>
        private void OnNodeProtectionSelect()                                   
        {
            HideRightPannels();
            panelProtection.Visible = true;            
        }
        /// <summary>
        /// NodeFileAntivirus on-select event handler
        /// </summary>
        private void OnNodeFileAntivirusSelect()
        {
            HideRightPannels();
            panelFileAntivirus.Visible = true;

        }
        /// <summary>
        /// NodeMailAntivirus on-select event handler
        /// </summary>
        private void OnNodeMailAntivirusSelect()
        {
            HideRightPannels();
            panelMailAntivirus.Visible = true;
        }
        /// <summary>
        /// NodeScan on-select event handler
        /// </summary>
        private void OnNodeScanSelect()
        {
            HideRightPannels();
            panelScan.Visible = true;
        }
        /// <summary>
        /// NodeScanMyComputer on-select event handler
        /// </summary>
        private void OnNodeScanMyComputerSelect()
        {
            HideRightPannels();
            panelScanMyPC.Visible = true;
        }
        /// <summary>
        /// NodeScanCritical on-select event handler
        /// </summary>
        private void OnNodeScanCriticalSelect()
        {
            HideRightPannels();
            panelScanCriticalAreas.Visible = true;
        }
        /// <summary>
        /// NodeService on-select event handler
        /// </summary>
        private void OnNodeServiceSelect()
        {
            HideRightPannels();
            panelService.Visible = true;
        }
        /// <summary>
        /// NodeUpdate on-select event handler
        /// </summary>
        private void OnNodeUpdateSelect()
        {
            HideRightPannels();
            panelUpdate.Visible = true;
        }
        /// <summary>
        /// NodeDataFiles on-select event handler
        /// </summary>
        private void OnNodeDataFilesSelect()
        {
            HideRightPannels();
            panelDataFiles.Visible = true;
        }
        #endregion       

        #region Checkboxes Event Handlers       
        /// <summary>
        /// Checkbox CheckedChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownDeleteReportsDays.Enabled = checkBoxDeleteReports.Checked;
            numericUpDownDeleteFromQuarantineDays.Enabled = checkBoxDeleteFromQuarantine.Checked;
            
            buttonScanCriticalScheduleChange.Enabled = checkBoxScanCriticalSchedule.Checked;

            buttonScanMyPCScheduleChange.Enabled = checkBoxScanMyPCSchedule.Checked;

            buttonScanScheduleChange.Enabled = checkBoxScanSchedule.Checked;

            buttonScanEditFilter.Enabled = checkBoxScanUseFilter.Checked;

            buttonScanCriticalEditFilter.Enabled = checkBoxScanCriticalUseFilter.Checked;

            buttonScanMyPCEditFilter.Enabled = checkBoxScanMyPCUseFilter.Checked;
            
            buttonSettingsApply.Enabled = true;
        }
        #endregion

        #region Numeric Event Handlers
        /// <summary>
        /// numericUpDownDeleteReportsDays ValueChanged Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownDeleteReportsDays_ValueChanged(object sender, EventArgs e)
        {
            buttonSettingsApply.Enabled = true;
        }
        /// <summary>
        /// numericUpDownDeleteFromQuarantineDays ValueChanged Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownDeleteFromQuarantineDays_ValueChanged(object sender, EventArgs e)
        {
            buttonSettingsApply.Enabled = true;
        }
        #endregion

        #region Radio Buttons Event Handlers       
        /// <summary>
        /// Radio Buttons CheckedChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {            
            buttonUpdateEveryChange.Enabled = radioButtonUpdateEvery.Checked;
            buttonSettingsApply.Enabled = true;
        }        
        #endregion

        #region Form Event Handlers
        private void ClamWinSettingsForm_Load(object sender, EventArgs e)
        {
            AssignSettingsToControls();

            buttonSettingsApply.Enabled = false;
        }
        #endregion              

        #endregion        

        #region Public Functions
        public void SelectSection(string name)
        {
            foreach (TreeNode node in treeViewSettings.Nodes)
            {
                if (node.Name == name)
                {
                    treeViewSettings.SelectedNode = node;
                    node.Expand();
                    break;
                }

                foreach (TreeNode nodeChild in node.Nodes)
                {
                    if (nodeChild.Name == name)
                    {
                        treeViewSettings.SelectedNode = nodeChild;
                        nodeChild.Expand();
                        break;
                    }

                }
            }
        }
        #endregion                                         
        
    }
    #endregion
}