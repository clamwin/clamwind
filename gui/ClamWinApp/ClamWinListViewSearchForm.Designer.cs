// Name:        ClamWinListViewSearchForm.Designer.cs
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
    partial class ClamWinListViewSearchForm
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
            this.comboBoxColumns = new System.Windows.Forms.ComboBox();
            this.textBoxTextToSearch = new System.Windows.Forms.TextBox();
            this.labelFindWhat = new System.Windows.Forms.Label();
            this.labelLookIn = new System.Windows.Forms.Label();
            this.buttonFind = new System.Windows.Forms.Button();
            this.checkBoxMatchCase = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // comboBoxColumns
            // 
            this.comboBoxColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColumns.FormattingEnabled = true;
            this.comboBoxColumns.Location = new System.Drawing.Point(12, 64);
            this.comboBoxColumns.Name = "comboBoxColumns";
            this.comboBoxColumns.Size = new System.Drawing.Size(280, 21);
            this.comboBoxColumns.TabIndex = 2;
            // 
            // textBoxTextToSearch
            // 
            this.textBoxTextToSearch.Location = new System.Drawing.Point(12, 25);
            this.textBoxTextToSearch.Name = "textBoxTextToSearch";
            this.textBoxTextToSearch.Size = new System.Drawing.Size(280, 20);
            this.textBoxTextToSearch.TabIndex = 1;
            this.textBoxTextToSearch.TextChanged += new System.EventHandler(this.textBoxTextToSearch_TextChanged);
            // 
            // labelFindWhat
            // 
            this.labelFindWhat.AutoSize = true;
            this.labelFindWhat.Location = new System.Drawing.Point(12, 9);
            this.labelFindWhat.Name = "labelFindWhat";
            this.labelFindWhat.Size = new System.Drawing.Size(56, 13);
            this.labelFindWhat.TabIndex = 2;
            this.labelFindWhat.Text = "Find what:";
            // 
            // labelLookIn
            // 
            this.labelLookIn.AutoSize = true;
            this.labelLookIn.Location = new System.Drawing.Point(12, 48);
            this.labelLookIn.Name = "labelLookIn";
            this.labelLookIn.Size = new System.Drawing.Size(45, 13);
            this.labelLookIn.TabIndex = 3;
            this.labelLookIn.Text = "Look in:";
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(217, 91);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 4;
            this.buttonFind.Text = "Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // checkBoxMatchCase
            // 
            this.checkBoxMatchCase.AutoSize = true;
            this.checkBoxMatchCase.Location = new System.Drawing.Point(12, 95);
            this.checkBoxMatchCase.Name = "checkBoxMatchCase";
            this.checkBoxMatchCase.Size = new System.Drawing.Size(82, 17);
            this.checkBoxMatchCase.TabIndex = 5;
            this.checkBoxMatchCase.Text = "Match case";
            this.checkBoxMatchCase.UseVisualStyleBackColor = true;
            this.checkBoxMatchCase.CheckedChanged += new System.EventHandler(this.checkBoxMatchCase_CheckedChanged);
            // 
            // ClamWinListViewSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 121);
            this.Controls.Add(this.checkBoxMatchCase);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.labelLookIn);
            this.Controls.Add(this.labelFindWhat);
            this.Controls.Add(this.textBoxTextToSearch);
            this.Controls.Add(this.comboBoxColumns);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ClamWinListViewSearchForm";
            this.Text = "Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxColumns;
        private System.Windows.Forms.TextBox textBoxTextToSearch;
        private System.Windows.Forms.Label labelFindWhat;
        private System.Windows.Forms.Label labelLookIn;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.CheckBox checkBoxMatchCase;
    }
}