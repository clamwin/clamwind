// Name:        ClamWinScheduleForm.Designer.cs
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
    partial class ClamWinScheduleForm
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
            this.groupBoxSchedule = new System.Windows.Forms.GroupBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.panelWeekly = new System.Windows.Forms.Panel();
            this.labelWeeklyMo = new System.Windows.Forms.Label();
            this.labelWeeklyTu = new System.Windows.Forms.Label();
            this.labelWeeklyWe = new System.Windows.Forms.Label();
            this.labelWeeklyTh = new System.Windows.Forms.Label();
            this.labelWeeklyFr = new System.Windows.Forms.Label();
            this.labelWeeklySa = new System.Windows.Forms.Label();
            this.labelWeeklySu = new System.Windows.Forms.Label();
            this.checkBoxWeeklyTh = new System.Windows.Forms.CheckBox();
            this.checkBoxWeeklyFr = new System.Windows.Forms.CheckBox();
            this.checkBoxWeeklySa = new System.Windows.Forms.CheckBox();
            this.checkBoxWeeklyWe = new System.Windows.Forms.CheckBox();
            this.checkBoxWeeklyTu = new System.Windows.Forms.CheckBox();
            this.checkBoxWeeklyMo = new System.Windows.Forms.CheckBox();
            this.checkBoxWeeklySu = new System.Windows.Forms.CheckBox();
            this.dateTimePickerWeekly = new System.Windows.Forms.DateTimePicker();
            this.checkBoxWeeklyUseTime = new System.Windows.Forms.CheckBox();
            this.panelDaily = new System.Windows.Forms.Panel();
            this.dateTimePickerDaily = new System.Windows.Forms.DateTimePicker();
            this.checkBoxDailyUseTime = new System.Windows.Forms.CheckBox();
            this.labelDailyDay = new System.Windows.Forms.Label();
            this.numericUpDownDailyEveryN = new System.Windows.Forms.NumericUpDown();
            this.radioButtonDailyEveryWeekend = new System.Windows.Forms.RadioButton();
            this.radioButtonDailyEveryWeekday = new System.Windows.Forms.RadioButton();
            this.radioButtonDailyEveryN = new System.Windows.Forms.RadioButton();
            this.panelHourly = new System.Windows.Forms.Panel();
            this.labelHourlyHours = new System.Windows.Forms.Label();
            this.numericUpDownHourly = new System.Windows.Forms.NumericUpDown();
            this.labelHourlyEvery = new System.Windows.Forms.Label();
            this.panelMinutely = new System.Windows.Forms.Panel();
            this.labelMinutelyMin = new System.Windows.Forms.Label();
            this.numericUpDownMinutely = new System.Windows.Forms.NumericUpDown();
            this.labelMinutelyEvery = new System.Windows.Forms.Label();
            this.panelOnce = new System.Windows.Forms.Panel();
            this.dateTimePickerOnce = new System.Windows.Forms.DateTimePicker();
            this.labelOnceDate = new System.Windows.Forms.Label();
            this.panelMonthly = new System.Windows.Forms.Panel();
            this.labelMonthlyDay = new System.Windows.Forms.Label();
            this.numericUpDownMonthlyN = new System.Windows.Forms.NumericUpDown();
            this.labelMonthlyEvery = new System.Windows.Forms.Label();
            this.dateTimePickerMonthly = new System.Windows.Forms.DateTimePicker();
            this.checkBoxMonthlyUseTime = new System.Windows.Forms.CheckBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxSchedule.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            this.panelWeekly.SuspendLayout();
            this.panelDaily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDailyEveryN)).BeginInit();
            this.panelHourly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHourly)).BeginInit();
            this.panelMinutely.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinutely)).BeginInit();
            this.panelOnce.SuspendLayout();
            this.panelMonthly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMonthlyN)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxSchedule
            // 
            this.groupBoxSchedule.Controls.Add(this.comboBoxType);
            this.groupBoxSchedule.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSchedule.Name = "groupBoxSchedule";
            this.groupBoxSchedule.Size = new System.Drawing.Size(268, 52);
            this.groupBoxSchedule.TabIndex = 0;
            this.groupBoxSchedule.TabStop = false;
            this.groupBoxSchedule.Text = "Schedule";
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Once",
            "Minutely",
            "Hourly",
            "Daily",
            "Weekly",
            "Monthly"});
            this.comboBoxType.Location = new System.Drawing.Point(6, 19);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(256, 21);
            this.comboBoxType.TabIndex = 0;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.panelWeekly);
            this.groupBoxSettings.Controls.Add(this.panelDaily);
            this.groupBoxSettings.Controls.Add(this.panelHourly);
            this.groupBoxSettings.Controls.Add(this.panelMinutely);
            this.groupBoxSettings.Controls.Add(this.panelOnce);
            this.groupBoxSettings.Controls.Add(this.panelMonthly);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 70);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(268, 155);
            this.groupBoxSettings.TabIndex = 1;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Schedule settings";
            // 
            // panelWeekly
            // 
            this.panelWeekly.Controls.Add(this.labelWeeklyMo);
            this.panelWeekly.Controls.Add(this.labelWeeklyTu);
            this.panelWeekly.Controls.Add(this.labelWeeklyWe);
            this.panelWeekly.Controls.Add(this.labelWeeklyTh);
            this.panelWeekly.Controls.Add(this.labelWeeklyFr);
            this.panelWeekly.Controls.Add(this.labelWeeklySa);
            this.panelWeekly.Controls.Add(this.labelWeeklySu);
            this.panelWeekly.Controls.Add(this.checkBoxWeeklyTh);
            this.panelWeekly.Controls.Add(this.checkBoxWeeklyFr);
            this.panelWeekly.Controls.Add(this.checkBoxWeeklySa);
            this.panelWeekly.Controls.Add(this.checkBoxWeeklyWe);
            this.panelWeekly.Controls.Add(this.checkBoxWeeklyTu);
            this.panelWeekly.Controls.Add(this.checkBoxWeeklyMo);
            this.panelWeekly.Controls.Add(this.checkBoxWeeklySu);
            this.panelWeekly.Controls.Add(this.dateTimePickerWeekly);
            this.panelWeekly.Controls.Add(this.checkBoxWeeklyUseTime);
            this.panelWeekly.Location = new System.Drawing.Point(6, 19);
            this.panelWeekly.Name = "panelWeekly";
            this.panelWeekly.Size = new System.Drawing.Size(256, 130);
            this.panelWeekly.TabIndex = 7;
            // 
            // labelWeeklyMo
            // 
            this.labelWeeklyMo.AutoSize = true;
            this.labelWeeklyMo.Location = new System.Drawing.Point(41, 14);
            this.labelWeeklyMo.Name = "labelWeeklyMo";
            this.labelWeeklyMo.Size = new System.Drawing.Size(22, 13);
            this.labelWeeklyMo.TabIndex = 20;
            this.labelWeeklyMo.Text = "Mo";
            // 
            // labelWeeklyTu
            // 
            this.labelWeeklyTu.AutoSize = true;
            this.labelWeeklyTu.Location = new System.Drawing.Point(77, 14);
            this.labelWeeklyTu.Name = "labelWeeklyTu";
            this.labelWeeklyTu.Size = new System.Drawing.Size(20, 13);
            this.labelWeeklyTu.TabIndex = 19;
            this.labelWeeklyTu.Text = "Tu";
            // 
            // labelWeeklyWe
            // 
            this.labelWeeklyWe.AutoSize = true;
            this.labelWeeklyWe.Location = new System.Drawing.Point(113, 14);
            this.labelWeeklyWe.Name = "labelWeeklyWe";
            this.labelWeeklyWe.Size = new System.Drawing.Size(24, 13);
            this.labelWeeklyWe.TabIndex = 18;
            this.labelWeeklyWe.Text = "We";
            // 
            // labelWeeklyTh
            // 
            this.labelWeeklyTh.AutoSize = true;
            this.labelWeeklyTh.Location = new System.Drawing.Point(149, 14);
            this.labelWeeklyTh.Name = "labelWeeklyTh";
            this.labelWeeklyTh.Size = new System.Drawing.Size(20, 13);
            this.labelWeeklyTh.TabIndex = 17;
            this.labelWeeklyTh.Text = "Th";
            // 
            // labelWeeklyFr
            // 
            this.labelWeeklyFr.AutoSize = true;
            this.labelWeeklyFr.Location = new System.Drawing.Point(184, 14);
            this.labelWeeklyFr.Name = "labelWeeklyFr";
            this.labelWeeklyFr.Size = new System.Drawing.Size(16, 13);
            this.labelWeeklyFr.TabIndex = 16;
            this.labelWeeklyFr.Text = "Fr";
            // 
            // labelWeeklySa
            // 
            this.labelWeeklySa.AutoSize = true;
            this.labelWeeklySa.Location = new System.Drawing.Point(221, 14);
            this.labelWeeklySa.Name = "labelWeeklySa";
            this.labelWeeklySa.Size = new System.Drawing.Size(20, 13);
            this.labelWeeklySa.TabIndex = 15;
            this.labelWeeklySa.Text = "Sa";
            // 
            // labelWeeklySu
            // 
            this.labelWeeklySu.AutoSize = true;
            this.labelWeeklySu.Location = new System.Drawing.Point(5, 14);
            this.labelWeeklySu.Name = "labelWeeklySu";
            this.labelWeeklySu.Size = new System.Drawing.Size(20, 13);
            this.labelWeeklySu.TabIndex = 14;
            this.labelWeeklySu.Text = "Su";
            // 
            // checkBoxWeeklyTh
            // 
            this.checkBoxWeeklyTh.AutoSize = true;
            this.checkBoxWeeklyTh.Location = new System.Drawing.Point(152, 34);
            this.checkBoxWeeklyTh.Name = "checkBoxWeeklyTh";
            this.checkBoxWeeklyTh.Size = new System.Drawing.Size(15, 14);
            this.checkBoxWeeklyTh.TabIndex = 13;
            this.checkBoxWeeklyTh.UseVisualStyleBackColor = true;
            this.checkBoxWeeklyTh.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxWeeklyFr
            // 
            this.checkBoxWeeklyFr.AutoSize = true;
            this.checkBoxWeeklyFr.Location = new System.Drawing.Point(188, 34);
            this.checkBoxWeeklyFr.Name = "checkBoxWeeklyFr";
            this.checkBoxWeeklyFr.Size = new System.Drawing.Size(15, 14);
            this.checkBoxWeeklyFr.TabIndex = 12;
            this.checkBoxWeeklyFr.UseVisualStyleBackColor = true;
            this.checkBoxWeeklyFr.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxWeeklySa
            // 
            this.checkBoxWeeklySa.AutoSize = true;
            this.checkBoxWeeklySa.Location = new System.Drawing.Point(224, 34);
            this.checkBoxWeeklySa.Name = "checkBoxWeeklySa";
            this.checkBoxWeeklySa.Size = new System.Drawing.Size(15, 14);
            this.checkBoxWeeklySa.TabIndex = 11;
            this.checkBoxWeeklySa.UseVisualStyleBackColor = true;
            this.checkBoxWeeklySa.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxWeeklyWe
            // 
            this.checkBoxWeeklyWe.AutoSize = true;
            this.checkBoxWeeklyWe.Location = new System.Drawing.Point(116, 34);
            this.checkBoxWeeklyWe.Name = "checkBoxWeeklyWe";
            this.checkBoxWeeklyWe.Size = new System.Drawing.Size(15, 14);
            this.checkBoxWeeklyWe.TabIndex = 10;
            this.checkBoxWeeklyWe.UseVisualStyleBackColor = true;
            this.checkBoxWeeklyWe.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxWeeklyTu
            // 
            this.checkBoxWeeklyTu.AutoSize = true;
            this.checkBoxWeeklyTu.Location = new System.Drawing.Point(80, 34);
            this.checkBoxWeeklyTu.Name = "checkBoxWeeklyTu";
            this.checkBoxWeeklyTu.Size = new System.Drawing.Size(15, 14);
            this.checkBoxWeeklyTu.TabIndex = 9;
            this.checkBoxWeeklyTu.UseVisualStyleBackColor = true;
            this.checkBoxWeeklyTu.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxWeeklyMo
            // 
            this.checkBoxWeeklyMo.AutoSize = true;
            this.checkBoxWeeklyMo.Location = new System.Drawing.Point(44, 34);
            this.checkBoxWeeklyMo.Name = "checkBoxWeeklyMo";
            this.checkBoxWeeklyMo.Size = new System.Drawing.Size(15, 14);
            this.checkBoxWeeklyMo.TabIndex = 8;
            this.checkBoxWeeklyMo.UseVisualStyleBackColor = true;
            this.checkBoxWeeklyMo.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxWeeklySu
            // 
            this.checkBoxWeeklySu.AutoSize = true;
            this.checkBoxWeeklySu.Location = new System.Drawing.Point(8, 34);
            this.checkBoxWeeklySu.Name = "checkBoxWeeklySu";
            this.checkBoxWeeklySu.Size = new System.Drawing.Size(15, 14);
            this.checkBoxWeeklySu.TabIndex = 7;
            this.checkBoxWeeklySu.UseVisualStyleBackColor = true;
            this.checkBoxWeeklySu.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // dateTimePickerWeekly
            // 
            this.dateTimePickerWeekly.CustomFormat = "HH:mm";
            this.dateTimePickerWeekly.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerWeekly.Location = new System.Drawing.Point(63, 80);
            this.dateTimePickerWeekly.Name = "dateTimePickerWeekly";
            this.dateTimePickerWeekly.ShowUpDown = true;
            this.dateTimePickerWeekly.Size = new System.Drawing.Size(82, 20);
            this.dateTimePickerWeekly.TabIndex = 6;
            // 
            // checkBoxWeeklyUseTime
            // 
            this.checkBoxWeeklyUseTime.AutoSize = true;
            this.checkBoxWeeklyUseTime.Location = new System.Drawing.Point(8, 83);
            this.checkBoxWeeklyUseTime.Name = "checkBoxWeeklyUseTime";
            this.checkBoxWeeklyUseTime.Size = new System.Drawing.Size(49, 17);
            this.checkBoxWeeklyUseTime.TabIndex = 5;
            this.checkBoxWeeklyUseTime.Text = "Time";
            this.checkBoxWeeklyUseTime.UseVisualStyleBackColor = true;
            this.checkBoxWeeklyUseTime.CheckedChanged += new System.EventHandler(this.checkBoxWeeklyUseTime_CheckedChanged);
            // 
            // panelDaily
            // 
            this.panelDaily.Controls.Add(this.dateTimePickerDaily);
            this.panelDaily.Controls.Add(this.checkBoxDailyUseTime);
            this.panelDaily.Controls.Add(this.labelDailyDay);
            this.panelDaily.Controls.Add(this.numericUpDownDailyEveryN);
            this.panelDaily.Controls.Add(this.radioButtonDailyEveryWeekend);
            this.panelDaily.Controls.Add(this.radioButtonDailyEveryWeekday);
            this.panelDaily.Controls.Add(this.radioButtonDailyEveryN);
            this.panelDaily.Location = new System.Drawing.Point(6, 19);
            this.panelDaily.Name = "panelDaily";
            this.panelDaily.Size = new System.Drawing.Size(256, 130);
            this.panelDaily.TabIndex = 4;
            // 
            // dateTimePickerDaily
            // 
            this.dateTimePickerDaily.CustomFormat = "HH:mm";
            this.dateTimePickerDaily.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerDaily.Location = new System.Drawing.Point(63, 80);
            this.dateTimePickerDaily.Name = "dateTimePickerDaily";
            this.dateTimePickerDaily.ShowUpDown = true;
            this.dateTimePickerDaily.Size = new System.Drawing.Size(82, 20);
            this.dateTimePickerDaily.TabIndex = 6;
            // 
            // checkBoxDailyUseTime
            // 
            this.checkBoxDailyUseTime.AutoSize = true;
            this.checkBoxDailyUseTime.Location = new System.Drawing.Point(8, 83);
            this.checkBoxDailyUseTime.Name = "checkBoxDailyUseTime";
            this.checkBoxDailyUseTime.Size = new System.Drawing.Size(49, 17);
            this.checkBoxDailyUseTime.TabIndex = 5;
            this.checkBoxDailyUseTime.Text = "Time";
            this.checkBoxDailyUseTime.UseVisualStyleBackColor = true;
            this.checkBoxDailyUseTime.CheckedChanged += new System.EventHandler(this.checkBoxDailyUseTime_CheckedChanged);
            // 
            // labelDailyDay
            // 
            this.labelDailyDay.AutoSize = true;
            this.labelDailyDay.Location = new System.Drawing.Point(113, 16);
            this.labelDailyDay.Name = "labelDailyDay";
            this.labelDailyDay.Size = new System.Drawing.Size(35, 13);
            this.labelDailyDay.TabIndex = 4;
            this.labelDailyDay.Text = "day(s)";
            // 
            // numericUpDownDailyEveryN
            // 
            this.numericUpDownDailyEveryN.Location = new System.Drawing.Point(66, 14);
            this.numericUpDownDailyEveryN.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownDailyEveryN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDailyEveryN.Name = "numericUpDownDailyEveryN";
            this.numericUpDownDailyEveryN.Size = new System.Drawing.Size(44, 20);
            this.numericUpDownDailyEveryN.TabIndex = 3;
            this.numericUpDownDailyEveryN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButtonDailyEveryWeekend
            // 
            this.radioButtonDailyEveryWeekend.AutoSize = true;
            this.radioButtonDailyEveryWeekend.Location = new System.Drawing.Point(8, 56);
            this.radioButtonDailyEveryWeekend.Name = "radioButtonDailyEveryWeekend";
            this.radioButtonDailyEveryWeekend.Size = new System.Drawing.Size(99, 17);
            this.radioButtonDailyEveryWeekend.TabIndex = 2;
            this.radioButtonDailyEveryWeekend.TabStop = true;
            this.radioButtonDailyEveryWeekend.Text = "Every weekend";
            this.radioButtonDailyEveryWeekend.UseVisualStyleBackColor = true;
            this.radioButtonDailyEveryWeekend.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonDailyEveryWeekday
            // 
            this.radioButtonDailyEveryWeekday.AutoSize = true;
            this.radioButtonDailyEveryWeekday.Location = new System.Drawing.Point(8, 35);
            this.radioButtonDailyEveryWeekday.Name = "radioButtonDailyEveryWeekday";
            this.radioButtonDailyEveryWeekday.Size = new System.Drawing.Size(98, 17);
            this.radioButtonDailyEveryWeekday.TabIndex = 1;
            this.radioButtonDailyEveryWeekday.TabStop = true;
            this.radioButtonDailyEveryWeekday.Text = "Every weekday";
            this.radioButtonDailyEveryWeekday.UseVisualStyleBackColor = true;
            this.radioButtonDailyEveryWeekday.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonDailyEveryN
            // 
            this.radioButtonDailyEveryN.AutoSize = true;
            this.radioButtonDailyEveryN.Checked = true;
            this.radioButtonDailyEveryN.Location = new System.Drawing.Point(8, 14);
            this.radioButtonDailyEveryN.Name = "radioButtonDailyEveryN";
            this.radioButtonDailyEveryN.Size = new System.Drawing.Size(52, 17);
            this.radioButtonDailyEveryN.TabIndex = 0;
            this.radioButtonDailyEveryN.TabStop = true;
            this.radioButtonDailyEveryN.Text = "Every";
            this.radioButtonDailyEveryN.UseVisualStyleBackColor = true;
            this.radioButtonDailyEveryN.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // panelHourly
            // 
            this.panelHourly.Controls.Add(this.labelHourlyHours);
            this.panelHourly.Controls.Add(this.numericUpDownHourly);
            this.panelHourly.Controls.Add(this.labelHourlyEvery);
            this.panelHourly.Location = new System.Drawing.Point(6, 19);
            this.panelHourly.Name = "panelHourly";
            this.panelHourly.Size = new System.Drawing.Size(256, 130);
            this.panelHourly.TabIndex = 3;
            // 
            // labelHourlyHours
            // 
            this.labelHourlyHours.AutoSize = true;
            this.labelHourlyHours.Location = new System.Drawing.Point(103, 61);
            this.labelHourlyHours.Name = "labelHourlyHours";
            this.labelHourlyHours.Size = new System.Drawing.Size(39, 13);
            this.labelHourlyHours.TabIndex = 2;
            this.labelHourlyHours.Text = "hour(s)";
            // 
            // numericUpDownHourly
            // 
            this.numericUpDownHourly.Location = new System.Drawing.Point(51, 57);
            this.numericUpDownHourly.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericUpDownHourly.Name = "numericUpDownHourly";
            this.numericUpDownHourly.Size = new System.Drawing.Size(42, 20);
            this.numericUpDownHourly.TabIndex = 1;
            this.numericUpDownHourly.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelHourlyEvery
            // 
            this.labelHourlyEvery.AutoSize = true;
            this.labelHourlyEvery.Location = new System.Drawing.Point(12, 61);
            this.labelHourlyEvery.Name = "labelHourlyEvery";
            this.labelHourlyEvery.Size = new System.Drawing.Size(34, 13);
            this.labelHourlyEvery.TabIndex = 0;
            this.labelHourlyEvery.Text = "Every";
            // 
            // panelMinutely
            // 
            this.panelMinutely.Controls.Add(this.labelMinutelyMin);
            this.panelMinutely.Controls.Add(this.numericUpDownMinutely);
            this.panelMinutely.Controls.Add(this.labelMinutelyEvery);
            this.panelMinutely.Location = new System.Drawing.Point(6, 19);
            this.panelMinutely.Name = "panelMinutely";
            this.panelMinutely.Size = new System.Drawing.Size(256, 130);
            this.panelMinutely.TabIndex = 2;
            // 
            // labelMinutelyMin
            // 
            this.labelMinutelyMin.AutoSize = true;
            this.labelMinutelyMin.Location = new System.Drawing.Point(103, 61);
            this.labelMinutelyMin.Name = "labelMinutelyMin";
            this.labelMinutelyMin.Size = new System.Drawing.Size(49, 13);
            this.labelMinutelyMin.TabIndex = 2;
            this.labelMinutelyMin.Text = "minute(s)";
            // 
            // numericUpDownMinutely
            // 
            this.numericUpDownMinutely.Location = new System.Drawing.Point(51, 57);
            this.numericUpDownMinutely.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDownMinutely.Name = "numericUpDownMinutely";
            this.numericUpDownMinutely.Size = new System.Drawing.Size(42, 20);
            this.numericUpDownMinutely.TabIndex = 1;
            this.numericUpDownMinutely.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelMinutelyEvery
            // 
            this.labelMinutelyEvery.AutoSize = true;
            this.labelMinutelyEvery.Location = new System.Drawing.Point(12, 61);
            this.labelMinutelyEvery.Name = "labelMinutelyEvery";
            this.labelMinutelyEvery.Size = new System.Drawing.Size(34, 13);
            this.labelMinutelyEvery.TabIndex = 0;
            this.labelMinutelyEvery.Text = "Every";
            // 
            // panelOnce
            // 
            this.panelOnce.Controls.Add(this.dateTimePickerOnce);
            this.panelOnce.Controls.Add(this.labelOnceDate);
            this.panelOnce.Location = new System.Drawing.Point(6, 19);
            this.panelOnce.Name = "panelOnce";
            this.panelOnce.Size = new System.Drawing.Size(256, 130);
            this.panelOnce.TabIndex = 0;
            // 
            // dateTimePickerOnce
            // 
            this.dateTimePickerOnce.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerOnce.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerOnce.Location = new System.Drawing.Point(51, 57);
            this.dateTimePickerOnce.Name = "dateTimePickerOnce";
            this.dateTimePickerOnce.ShowUpDown = true;
            this.dateTimePickerOnce.Size = new System.Drawing.Size(169, 20);
            this.dateTimePickerOnce.TabIndex = 1;
            // 
            // labelOnceDate
            // 
            this.labelOnceDate.AutoSize = true;
            this.labelOnceDate.Location = new System.Drawing.Point(12, 61);
            this.labelOnceDate.Name = "labelOnceDate";
            this.labelOnceDate.Size = new System.Drawing.Size(33, 13);
            this.labelOnceDate.TabIndex = 0;
            this.labelOnceDate.Text = "Date:";
            // 
            // panelMonthly
            // 
            this.panelMonthly.Controls.Add(this.labelMonthlyDay);
            this.panelMonthly.Controls.Add(this.numericUpDownMonthlyN);
            this.panelMonthly.Controls.Add(this.labelMonthlyEvery);
            this.panelMonthly.Controls.Add(this.dateTimePickerMonthly);
            this.panelMonthly.Controls.Add(this.checkBoxMonthlyUseTime);
            this.panelMonthly.Location = new System.Drawing.Point(6, 19);
            this.panelMonthly.Name = "panelMonthly";
            this.panelMonthly.Size = new System.Drawing.Size(256, 130);
            this.panelMonthly.TabIndex = 21;
            // 
            // labelMonthlyDay
            // 
            this.labelMonthlyDay.AutoSize = true;
            this.labelMonthlyDay.Location = new System.Drawing.Point(90, 36);
            this.labelMonthlyDay.Name = "labelMonthlyDay";
            this.labelMonthlyDay.Size = new System.Drawing.Size(68, 13);
            this.labelMonthlyDay.TabIndex = 9;
            this.labelMonthlyDay.Text = "day of month";
            // 
            // numericUpDownMonthlyN
            // 
            this.numericUpDownMonthlyN.Location = new System.Drawing.Point(44, 34);
            this.numericUpDownMonthlyN.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDownMonthlyN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMonthlyN.Name = "numericUpDownMonthlyN";
            this.numericUpDownMonthlyN.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownMonthlyN.TabIndex = 8;
            this.numericUpDownMonthlyN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelMonthlyEvery
            // 
            this.labelMonthlyEvery.AutoSize = true;
            this.labelMonthlyEvery.Location = new System.Drawing.Point(5, 37);
            this.labelMonthlyEvery.Name = "labelMonthlyEvery";
            this.labelMonthlyEvery.Size = new System.Drawing.Size(34, 13);
            this.labelMonthlyEvery.TabIndex = 7;
            this.labelMonthlyEvery.Text = "Every";
            // 
            // dateTimePickerMonthly
            // 
            this.dateTimePickerMonthly.CustomFormat = "HH:mm";
            this.dateTimePickerMonthly.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerMonthly.Location = new System.Drawing.Point(63, 80);
            this.dateTimePickerMonthly.Name = "dateTimePickerMonthly";
            this.dateTimePickerMonthly.ShowUpDown = true;
            this.dateTimePickerMonthly.Size = new System.Drawing.Size(82, 20);
            this.dateTimePickerMonthly.TabIndex = 6;
            // 
            // checkBoxMonthlyUseTime
            // 
            this.checkBoxMonthlyUseTime.AutoSize = true;
            this.checkBoxMonthlyUseTime.Location = new System.Drawing.Point(8, 83);
            this.checkBoxMonthlyUseTime.Name = "checkBoxMonthlyUseTime";
            this.checkBoxMonthlyUseTime.Size = new System.Drawing.Size(49, 17);
            this.checkBoxMonthlyUseTime.TabIndex = 5;
            this.checkBoxMonthlyUseTime.Text = "Time";
            this.checkBoxMonthlyUseTime.UseVisualStyleBackColor = true;
            this.checkBoxMonthlyUseTime.CheckedChanged += new System.EventHandler(this.checkBoxMonthlyUseTime_CheckedChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(124, 231);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(205, 231);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // ClamWinScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.groupBoxSchedule);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClamWinScheduleForm";
            this.Text = "Schedule:";
            this.groupBoxSchedule.ResumeLayout(false);
            this.groupBoxSettings.ResumeLayout(false);
            this.panelWeekly.ResumeLayout(false);
            this.panelWeekly.PerformLayout();
            this.panelDaily.ResumeLayout(false);
            this.panelDaily.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDailyEveryN)).EndInit();
            this.panelHourly.ResumeLayout(false);
            this.panelHourly.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHourly)).EndInit();
            this.panelMinutely.ResumeLayout(false);
            this.panelMinutely.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinutely)).EndInit();
            this.panelOnce.ResumeLayout(false);
            this.panelOnce.PerformLayout();
            this.panelMonthly.ResumeLayout(false);
            this.panelMonthly.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMonthlyN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSchedule;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Panel panelOnce;
        private System.Windows.Forms.Label labelOnceDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerOnce;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelMinutely;
        private System.Windows.Forms.Label labelMinutelyMin;
        private System.Windows.Forms.NumericUpDown numericUpDownMinutely;
        private System.Windows.Forms.Label labelMinutelyEvery;
        private System.Windows.Forms.Panel panelHourly;
        private System.Windows.Forms.Label labelHourlyHours;
        private System.Windows.Forms.NumericUpDown numericUpDownHourly;
        private System.Windows.Forms.Label labelHourlyEvery;
        private System.Windows.Forms.Panel panelDaily;
        private System.Windows.Forms.RadioButton radioButtonDailyEveryWeekday;
        private System.Windows.Forms.RadioButton radioButtonDailyEveryN;
        private System.Windows.Forms.Label labelDailyDay;
        private System.Windows.Forms.NumericUpDown numericUpDownDailyEveryN;
        private System.Windows.Forms.RadioButton radioButtonDailyEveryWeekend;
        private System.Windows.Forms.DateTimePicker dateTimePickerDaily;
        private System.Windows.Forms.CheckBox checkBoxDailyUseTime;
        private System.Windows.Forms.Panel panelWeekly;
        private System.Windows.Forms.DateTimePicker dateTimePickerWeekly;
        private System.Windows.Forms.CheckBox checkBoxWeeklyUseTime;
        private System.Windows.Forms.CheckBox checkBoxWeeklyMo;
        private System.Windows.Forms.CheckBox checkBoxWeeklySu;
        private System.Windows.Forms.CheckBox checkBoxWeeklyTh;
        private System.Windows.Forms.CheckBox checkBoxWeeklyFr;
        private System.Windows.Forms.CheckBox checkBoxWeeklySa;
        private System.Windows.Forms.CheckBox checkBoxWeeklyWe;
        private System.Windows.Forms.CheckBox checkBoxWeeklyTu;
        private System.Windows.Forms.Label labelWeeklyMo;
        private System.Windows.Forms.Label labelWeeklyTu;
        private System.Windows.Forms.Label labelWeeklyWe;
        private System.Windows.Forms.Label labelWeeklyTh;
        private System.Windows.Forms.Label labelWeeklyFr;
        private System.Windows.Forms.Label labelWeeklySa;
        private System.Windows.Forms.Label labelWeeklySu;
        private System.Windows.Forms.Panel panelMonthly;
        private System.Windows.Forms.DateTimePicker dateTimePickerMonthly;
        private System.Windows.Forms.CheckBox checkBoxMonthlyUseTime;
        private System.Windows.Forms.Label labelMonthlyEvery;
        private System.Windows.Forms.Label labelMonthlyDay;
        private System.Windows.Forms.NumericUpDown numericUpDownMonthlyN;
    }
}