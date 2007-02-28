// Name:        ClamWinScheduleForm.cs
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
    #region ClamWinScheduleForm class
    /// <summary>
    /// Schedule form
    /// </summary>
    public partial class ClamWinScheduleForm : Form
    {
        #region Public Constructor
        public ClamWinScheduleForm()
        {
            InitializeComponent();

            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonOk.DialogResult = DialogResult.OK;

            AcceptButton = buttonOk;
            CancelButton = buttonCancel;

            comboBoxType.SelectedIndex = 0;
            dateTimePickerDaily.Enabled = checkBoxDailyUseTime.Checked;
            dateTimePickerWeekly.Enabled = checkBoxWeeklyUseTime.Checked;
            dateTimePickerMonthly.Enabled = checkBoxMonthlyUseTime.Checked;
            numericUpDownDailyEveryN.Enabled = radioButtonDailyEveryN.Checked;
        }
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Hide all panels
        /// </summary>
        void HidePanels()
        {
            foreach (Control control in this.groupBoxSettings.Controls)
            {
                if (control is Panel)
                {
                    control.Visible = false;
                }
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Show schedule form modally and add caller string to form caption
        /// </summary>
        /// <param name="caller"></param>
        public DialogResult ScheduleDoModal(string caller, ref ClamWinScheduleData data)
        {
            Text = "Schedule: " + caller;
            
            comboBoxType.SelectedIndex = (int)data.Type;
            
            #region Apply data values to controls
            switch (data.Type)
            {
                case ClamWinScheduleData.SchedulingTypes.Once:
                {
                    dateTimePickerOnce.Value = data.Date;
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Minutely:
                {
                    numericUpDownMinutely.Value = data.Frequency;
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Hourly:
                {
                    numericUpDownHourly.Value = data.Frequency;
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Daily:
                {
                    if (data.DailyType == ClamWinScheduleData.DailyTypes.EveryNDays)
                    {
                        radioButtonDailyEveryN.Checked = true;
                    }
                    else if (data.DailyType == ClamWinScheduleData.DailyTypes.EveryWeekday)
                    {
                        radioButtonDailyEveryWeekday.Checked = true;
                    }
                    else
                    {
                        radioButtonDailyEveryWeekend.Checked = true;
                    }

                    checkBoxDailyUseTime.Checked = data.UseDateTime;
                    
                    if (data.UseDateTime)
                    {
                        dateTimePickerDaily.Value = data.Date;
                    }
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Weekly:
                {
                    checkBoxWeeklySu.Checked = data.IsDayChecked(ClamWinScheduleData.DaysOfTheWeek.Su);
                    checkBoxWeeklyMo.Checked = data.IsDayChecked(ClamWinScheduleData.DaysOfTheWeek.Mo);
                    checkBoxWeeklyTu.Checked = data.IsDayChecked(ClamWinScheduleData.DaysOfTheWeek.Tu);
                    checkBoxWeeklyWe.Checked = data.IsDayChecked(ClamWinScheduleData.DaysOfTheWeek.We);
                    checkBoxWeeklyTh.Checked = data.IsDayChecked(ClamWinScheduleData.DaysOfTheWeek.Th);
                    checkBoxWeeklyFr.Checked = data.IsDayChecked(ClamWinScheduleData.DaysOfTheWeek.Fr);
                    checkBoxWeeklySa.Checked = data.IsDayChecked(ClamWinScheduleData.DaysOfTheWeek.Sa);

                    checkBoxWeeklyUseTime.Checked = data.UseDateTime;

                    if (data.UseDateTime)
                    {
                        try
                        {
                            dateTimePickerWeekly.Value = data.Date;
                        }
                        catch
                        { 
                        
                        }
                    }
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Monthly:
                {
                    numericUpDownMonthlyN.Value = data.Frequency;
                    checkBoxMonthlyUseTime.Checked = data.UseDateTime;
                    if (data.UseDateTime)
                    {
                        dateTimePickerMonthly.Value = data.Date;
                    }
                    break;
                }                        
            }
            #endregion

            DialogResult result = ShowDialog();
            
            if (result != DialogResult.OK)
            {
                return result;
            }

            #region Fill data with new values
            data.Type = (ClamWinScheduleData.SchedulingTypes)comboBoxType.SelectedIndex;
            switch (data.Type)
            {
                case ClamWinScheduleData.SchedulingTypes.Once:
                {
                    data.Date = dateTimePickerOnce.Value;
                    break;    
                }
                case ClamWinScheduleData.SchedulingTypes.Minutely:
                {
                    data.Frequency = (int)numericUpDownMinutely.Value;
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Hourly:
                {
                    data.Frequency = (int)numericUpDownHourly.Value;
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Daily:
                {
                    if (radioButtonDailyEveryN.Checked)
                    {
                        data.DailyType = ClamWinScheduleData.DailyTypes.EveryNDays;
                        data.Frequency = (int)numericUpDownDailyEveryN.Value;
                    }
                    else if (radioButtonDailyEveryWeekday.Checked)
                    {
                        data.DailyType = ClamWinScheduleData.DailyTypes.EveryWeekday;
                    }
                    else
                    {
                        data.DailyType = ClamWinScheduleData.DailyTypes.EveryWeekend;
                    }

                    data.UseDateTime = checkBoxDailyUseTime.Checked;

                    if (checkBoxDailyUseTime.Checked)
                    {
                        data.Date = dateTimePickerDaily.Value;
                    }
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Weekly:
                {
                    data.Days = ClamWinScheduleData.DaysOfTheWeek.Su |
                                ClamWinScheduleData.DaysOfTheWeek.Mo |
                                ClamWinScheduleData.DaysOfTheWeek.Tu |
                                ClamWinScheduleData.DaysOfTheWeek.We |
                                ClamWinScheduleData.DaysOfTheWeek.Th |
                                ClamWinScheduleData.DaysOfTheWeek.Fr |
                                ClamWinScheduleData.DaysOfTheWeek.Sa;

                    data.ChangeDay(ClamWinScheduleData.DaysOfTheWeek.Su, checkBoxWeeklySu.Checked);
                    data.ChangeDay(ClamWinScheduleData.DaysOfTheWeek.Mo, checkBoxWeeklyMo.Checked);
                    data.ChangeDay(ClamWinScheduleData.DaysOfTheWeek.Tu, checkBoxWeeklyTu.Checked);
                    data.ChangeDay(ClamWinScheduleData.DaysOfTheWeek.We, checkBoxWeeklyWe.Checked);
                    data.ChangeDay(ClamWinScheduleData.DaysOfTheWeek.Th, checkBoxWeeklyTh.Checked);
                    data.ChangeDay(ClamWinScheduleData.DaysOfTheWeek.Fr, checkBoxWeeklyFr.Checked);
                    data.ChangeDay(ClamWinScheduleData.DaysOfTheWeek.Sa, checkBoxWeeklySa.Checked);

                    data.UseDateTime = checkBoxWeeklyUseTime.Checked;

                    if (checkBoxWeeklyUseTime.Checked)
                    {
                        data.Date = dateTimePickerWeekly.Value;
                    }
                    break;
                }
                case ClamWinScheduleData.SchedulingTypes.Monthly:
                {
                    data.Frequency = (int)numericUpDownMonthlyN.Value;

                    data.UseDateTime = checkBoxMonthlyUseTime.Checked;

                    if (checkBoxMonthlyUseTime.Checked)
                    {
                        data.Date = dateTimePickerMonthly.Value;
                    }

                    break;
                }            
            }
            #endregion

            return result;
        }
        #endregion        

        #region Event Handlers

        #region Button Event Handlers
        /// <summary>
        /// buttonOk on-click event handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOk_Click(object sender, EventArgs e)
        {            
            
        }
        /// <summary>
        /// buttonCancel on-click event handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Combo Event Handlers
        /// <summary>
        /// comboBoxType SelectedIndexChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HidePanels();

            buttonOk.Enabled = true;
            switch( comboBoxType.SelectedIndex )
            {
                case 0:
                    panelOnce.Visible = true;
                    break;
                case 1:
                    panelMinutely.Visible = true;
                    break;
                case 2:
                    panelHourly.Visible = true;
                    break;
                case 3:
                    panelDaily.Visible = true;
                    break;
                case 4:
                    panelWeekly.Visible = true;
                    buttonOk.Enabled = checkBoxWeeklySu.Checked ||
                                       checkBoxWeeklyMo.Checked ||
                                       checkBoxWeeklyTu.Checked ||
                                       checkBoxWeeklyWe.Checked ||
                                       checkBoxWeeklyTh.Checked ||
                                       checkBoxWeeklyFr.Checked ||
                                       checkBoxWeeklySa.Checked;
                    break;
                case 5:
                    panelMonthly.Visible = true;
                    break;                            
            }

        }
        #endregion

        #region Checkbox Event Handlers
        /// <summary>
        /// checkBoxMonthlyUseTime CheckedChanged Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxMonthlyUseTime_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerMonthly.Enabled = checkBoxMonthlyUseTime.Checked;
        }
        /// <summary>
        /// checkBoxWeeklyUseTime CheckedChanged Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxWeeklyUseTime_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerWeekly.Enabled = checkBoxWeeklyUseTime.Checked;
        }
        /// <summary>
        /// checkBoxDailyUseTime CheckedChanged Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxDailyUseTime_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerDaily.Enabled = checkBoxDailyUseTime.Checked;
        }
        #endregion

        #region Radio button CheckedChanged event handler
        /// <summary>
        /// radioButton CheckedChanged Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownDailyEveryN.Enabled = radioButtonDailyEveryN.Checked;
        }        
        #endregion

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = checkBoxWeeklySu.Checked ||
                               checkBoxWeeklyMo.Checked ||
                               checkBoxWeeklyTu.Checked ||
                               checkBoxWeeklyWe.Checked ||
                               checkBoxWeeklyTh.Checked ||
                               checkBoxWeeklyFr.Checked ||
                               checkBoxWeeklySa.Checked;
        }

        #endregion        
    }
    #endregion
}