// Name:        ClamwinFilterForm.Designer.cs
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
    partial class ClamWinFilterForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelExclude = new System.Windows.Forms.Panel();
            this.panelInclude = new System.Windows.Forms.Panel();
            this.buttonIncludeMoveDown = new System.Windows.Forms.Button();
            this.buttonIncludeNew = new System.Windows.Forms.Button();
            this.buttonIncludeMoveUp = new System.Windows.Forms.Button();
            this.buttonIncludeEdit = new System.Windows.Forms.Button();
            this.buttonIncludeRemove = new System.Windows.Forms.Button();
            this.labelExclude = new System.Windows.Forms.Label();
            this.labelInclude = new System.Windows.Forms.Label();
            this.listViewExclude = new System.Windows.Forms.ListView();
            this.columnHeaderPattern = new System.Windows.Forms.ColumnHeader();
            this.listViewInclude = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.buttonExcludeMoveDown = new System.Windows.Forms.Button();
            this.buttonExcludeNew = new System.Windows.Forms.Button();
            this.buttonExcludeMoveUp = new System.Windows.Forms.Button();
            this.buttonExcludeEdit = new System.Windows.Forms.Button();
            this.buttonExcludeRemove = new System.Windows.Forms.Button();
            this.panelExclude.SuspendLayout();
            this.panelInclude.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(255, 240);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(74, 25);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(335, 240);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(74, 25);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // panelExclude
            // 
            this.panelExclude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelExclude.Controls.Add(this.buttonExcludeMoveDown);
            this.panelExclude.Controls.Add(this.buttonExcludeNew);
            this.panelExclude.Controls.Add(this.buttonExcludeMoveUp);
            this.panelExclude.Controls.Add(this.buttonExcludeEdit);
            this.panelExclude.Controls.Add(this.buttonExcludeRemove);
            this.panelExclude.Location = new System.Drawing.Point(12, 35);
            this.panelExclude.Name = "panelExclude";
            this.panelExclude.Size = new System.Drawing.Size(190, 31);
            this.panelExclude.TabIndex = 5;
            // 
            // panelInclude
            // 
            this.panelInclude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInclude.Controls.Add(this.buttonIncludeMoveDown);
            this.panelInclude.Controls.Add(this.buttonIncludeNew);
            this.panelInclude.Controls.Add(this.buttonIncludeMoveUp);
            this.panelInclude.Controls.Add(this.buttonIncludeEdit);
            this.panelInclude.Controls.Add(this.buttonIncludeRemove);
            this.panelInclude.Location = new System.Drawing.Point(219, 35);
            this.panelInclude.Name = "panelInclude";
            this.panelInclude.Size = new System.Drawing.Size(190, 31);
            this.panelInclude.TabIndex = 10;
            // 
            // buttonIncludeMoveDown
            // 
            this.buttonIncludeMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIncludeMoveDown.Image = global::ClamWinApp.Properties.Resources.down;
            this.buttonIncludeMoveDown.Location = new System.Drawing.Point(160, 3);
            this.buttonIncludeMoveDown.Name = "buttonIncludeMoveDown";
            this.buttonIncludeMoveDown.Size = new System.Drawing.Size(23, 23);
            this.buttonIncludeMoveDown.TabIndex = 9;
            this.buttonIncludeMoveDown.UseVisualStyleBackColor = true;
            this.buttonIncludeMoveDown.Click += new System.EventHandler(this.buttonIncludeMoveDown_Click);
            // 
            // buttonIncludeNew
            // 
            this.buttonIncludeNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIncludeNew.Image = global::ClamWinApp.Properties.Resources._new;
            this.buttonIncludeNew.Location = new System.Drawing.Point(88, 3);
            this.buttonIncludeNew.Name = "buttonIncludeNew";
            this.buttonIncludeNew.Size = new System.Drawing.Size(23, 23);
            this.buttonIncludeNew.TabIndex = 6;
            this.buttonIncludeNew.UseVisualStyleBackColor = true;
            this.buttonIncludeNew.Click += new System.EventHandler(this.buttonIncludeNew_Click);
            // 
            // buttonIncludeMoveUp
            // 
            this.buttonIncludeMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIncludeMoveUp.Image = global::ClamWinApp.Properties.Resources.up;
            this.buttonIncludeMoveUp.Location = new System.Drawing.Point(136, 3);
            this.buttonIncludeMoveUp.Name = "buttonIncludeMoveUp";
            this.buttonIncludeMoveUp.Size = new System.Drawing.Size(23, 23);
            this.buttonIncludeMoveUp.TabIndex = 8;
            this.buttonIncludeMoveUp.UseVisualStyleBackColor = true;
            this.buttonIncludeMoveUp.Click += new System.EventHandler(this.buttonIncludeMoveUp_Click);
            // 
            // buttonIncludeEdit
            // 
            this.buttonIncludeEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIncludeEdit.Image = global::ClamWinApp.Properties.Resources.edit;
            this.buttonIncludeEdit.Location = new System.Drawing.Point(64, 3);
            this.buttonIncludeEdit.Name = "buttonIncludeEdit";
            this.buttonIncludeEdit.Size = new System.Drawing.Size(23, 23);
            this.buttonIncludeEdit.TabIndex = 0;
            this.buttonIncludeEdit.UseVisualStyleBackColor = true;
            this.buttonIncludeEdit.Click += new System.EventHandler(this.buttonIncludeEdit_Click);
            // 
            // buttonIncludeRemove
            // 
            this.buttonIncludeRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIncludeRemove.Image = global::ClamWinApp.Properties.Resources.remove;
            this.buttonIncludeRemove.Location = new System.Drawing.Point(112, 3);
            this.buttonIncludeRemove.Name = "buttonIncludeRemove";
            this.buttonIncludeRemove.Size = new System.Drawing.Size(23, 23);
            this.buttonIncludeRemove.TabIndex = 7;
            this.buttonIncludeRemove.UseVisualStyleBackColor = true;
            this.buttonIncludeRemove.Click += new System.EventHandler(this.buttonIncludeRemove_Click);
            // 
            // labelExclude
            // 
            this.labelExclude.Location = new System.Drawing.Point(9, 17);
            this.labelExclude.Name = "labelExclude";
            this.labelExclude.Size = new System.Drawing.Size(190, 15);
            this.labelExclude.TabIndex = 11;
            this.labelExclude.Text = "Exclude Matching  Filenames";
            // 
            // labelInclude
            // 
            this.labelInclude.Location = new System.Drawing.Point(216, 17);
            this.labelInclude.Name = "labelInclude";
            this.labelInclude.Size = new System.Drawing.Size(190, 15);
            this.labelInclude.TabIndex = 12;
            this.labelInclude.Text = "Scan Only Matching Filenames";
            // 
            // listViewExclude
            // 
            this.listViewExclude.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPattern});
            this.listViewExclude.FullRowSelect = true;
            this.listViewExclude.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewExclude.HideSelection = false;
            this.listViewExclude.LabelEdit = true;
            this.listViewExclude.Location = new System.Drawing.Point(12, 72);
            this.listViewExclude.MultiSelect = false;
            this.listViewExclude.Name = "listViewExclude";
            this.listViewExclude.Size = new System.Drawing.Size(190, 162);
            this.listViewExclude.TabIndex = 13;
            this.listViewExclude.UseCompatibleStateImageBehavior = false;
            this.listViewExclude.View = System.Windows.Forms.View.Details;
            this.listViewExclude.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listViewExclude.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewExclude_AfterLabelEdit);
            // 
            // columnHeaderPattern
            // 
            this.columnHeaderPattern.Text = "Pattern";
            this.columnHeaderPattern.Width = 170;
            // 
            // listViewInclude
            // 
            this.listViewInclude.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewInclude.FullRowSelect = true;
            this.listViewInclude.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewInclude.HideSelection = false;
            this.listViewInclude.LabelEdit = true;
            this.listViewInclude.Location = new System.Drawing.Point(219, 72);
            this.listViewInclude.MultiSelect = false;
            this.listViewInclude.Name = "listViewInclude";
            this.listViewInclude.Size = new System.Drawing.Size(190, 162);
            this.listViewInclude.TabIndex = 14;
            this.listViewInclude.UseCompatibleStateImageBehavior = false;
            this.listViewInclude.View = System.Windows.Forms.View.Details;
            this.listViewInclude.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listViewInclude.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewInclude_AfterLabelEdit);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Pattern";
            this.columnHeader1.Width = 170;
            // 
            // buttonExcludeMoveDown
            // 
            this.buttonExcludeMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExcludeMoveDown.Image = global::ClamWinApp.Properties.Resources.down;
            this.buttonExcludeMoveDown.Location = new System.Drawing.Point(160, 3);
            this.buttonExcludeMoveDown.Name = "buttonExcludeMoveDown";
            this.buttonExcludeMoveDown.Size = new System.Drawing.Size(23, 23);
            this.buttonExcludeMoveDown.TabIndex = 9;
            this.buttonExcludeMoveDown.UseVisualStyleBackColor = true;
            this.buttonExcludeMoveDown.Click += new System.EventHandler(this.buttonExcludeMoveDown_Click);
            // 
            // buttonExcludeNew
            // 
            this.buttonExcludeNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExcludeNew.Image = global::ClamWinApp.Properties.Resources._new;
            this.buttonExcludeNew.Location = new System.Drawing.Point(88, 3);
            this.buttonExcludeNew.Name = "buttonExcludeNew";
            this.buttonExcludeNew.Size = new System.Drawing.Size(23, 23);
            this.buttonExcludeNew.TabIndex = 6;
            this.buttonExcludeNew.UseVisualStyleBackColor = true;
            this.buttonExcludeNew.Click += new System.EventHandler(this.buttonExcludeNew_Click);
            // 
            // buttonExcludeMoveUp
            // 
            this.buttonExcludeMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExcludeMoveUp.Image = global::ClamWinApp.Properties.Resources.up;
            this.buttonExcludeMoveUp.Location = new System.Drawing.Point(136, 3);
            this.buttonExcludeMoveUp.Name = "buttonExcludeMoveUp";
            this.buttonExcludeMoveUp.Size = new System.Drawing.Size(23, 23);
            this.buttonExcludeMoveUp.TabIndex = 8;
            this.buttonExcludeMoveUp.UseVisualStyleBackColor = true;
            this.buttonExcludeMoveUp.Click += new System.EventHandler(this.buttonExcludeMoveUp_Click);
            // 
            // buttonExcludeEdit
            // 
            this.buttonExcludeEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExcludeEdit.Image = global::ClamWinApp.Properties.Resources.edit;
            this.buttonExcludeEdit.Location = new System.Drawing.Point(64, 3);
            this.buttonExcludeEdit.Name = "buttonExcludeEdit";
            this.buttonExcludeEdit.Size = new System.Drawing.Size(23, 23);
            this.buttonExcludeEdit.TabIndex = 0;
            this.buttonExcludeEdit.UseVisualStyleBackColor = true;
            this.buttonExcludeEdit.Click += new System.EventHandler(this.buttonExcludeEdit_Click);
            // 
            // buttonExcludeRemove
            // 
            this.buttonExcludeRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExcludeRemove.Image = global::ClamWinApp.Properties.Resources.remove;
            this.buttonExcludeRemove.Location = new System.Drawing.Point(112, 3);
            this.buttonExcludeRemove.Name = "buttonExcludeRemove";
            this.buttonExcludeRemove.Size = new System.Drawing.Size(23, 23);
            this.buttonExcludeRemove.TabIndex = 7;
            this.buttonExcludeRemove.UseVisualStyleBackColor = true;
            this.buttonExcludeRemove.Click += new System.EventHandler(this.buttonExcludeRemove_Click);
            // 
            // ClamWinFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 276);
            this.Controls.Add(this.listViewInclude);
            this.Controls.Add(this.listViewExclude);
            this.Controls.Add(this.labelInclude);
            this.Controls.Add(this.labelExclude);
            this.Controls.Add(this.panelInclude);
            this.Controls.Add(this.panelExclude);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClamWinFilterForm";
            this.Text = "Filter:";
            this.panelExclude.ResumeLayout(false);
            this.panelInclude.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelExclude;
        private System.Windows.Forms.Button buttonExcludeMoveDown;
        private System.Windows.Forms.Button buttonExcludeNew;
        private System.Windows.Forms.Button buttonExcludeMoveUp;
        private System.Windows.Forms.Button buttonExcludeEdit;
        private System.Windows.Forms.Button buttonExcludeRemove;
        private System.Windows.Forms.Panel panelInclude;
        private System.Windows.Forms.Button buttonIncludeMoveDown;
        private System.Windows.Forms.Button buttonIncludeNew;
        private System.Windows.Forms.Button buttonIncludeMoveUp;
        private System.Windows.Forms.Button buttonIncludeEdit;
        private System.Windows.Forms.Button buttonIncludeRemove;
        private System.Windows.Forms.Label labelExclude;
        private System.Windows.Forms.Label labelInclude;
        private System.Windows.Forms.ListView listViewExclude;
        private System.Windows.Forms.ColumnHeader columnHeaderPattern;
        private System.Windows.Forms.ListView listViewInclude;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}