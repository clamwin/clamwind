// Name:        ClamWinScanRequest.cs
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
using System.Text;

namespace ClamWinApp
{
    #region ClamWinScanRequest class
    /// <summary>
    /// ClamWinScanRequest class provide basic functionality to handle 
    /// service requests
    /// </summary>
    public class ClamWinScanRequest
    {
        #region Private Data
        /// <summary>
        /// Action identifier
        /// </summary>
        private ClamWinScanner.ActionID Action = ClamWinScanner.ActionID.NotDefined;
        /// <summary>
        /// Data base path, used as ActionReloadDB parameter
        /// </summary>
        private string DbPath = "";
        /// <summary>
        /// File path to scan, used as ActionScan parameter
        /// </summary>
        private string FilePath = "";
        /// <summary>
        /// Determine if fs monitor will be enabled or disabled,
        /// used as ActionFsFilterControl parameter
        /// </summary>
        private bool FsFilterEnabled = false;
        /// <summary>
        /// Request text
        /// </summary>
        private string RequestText = "";
        /// <summary>
        /// JobID for AsyncResult request
        /// </summary>
        private int JobID = 0;
        /// <summary>
        /// Value for tag "Value"
        /// </summary>
        private int Value = 0;
        #endregion

        #region Public Constructors
        public ClamWinScanRequest(ClamWinScanner.ActionID id)
        {
            Action = id;

            if (Action == ClamWinScanner.ActionID.GetInfo)
            {
                UpdateRequestText();
            }
        }
        #endregion        

        #region Private Helper Functions      
        private string EscapeFilePath(string filePath)
        {
            int position;
            string replacewith = "";
            // escape "'" and "&" characters as they are invalid in XML
            position = filePath.IndexOf("&");
            if (position > 0)
            {
                replacewith = "&amp;";
            }
            else
            {
                position = filePath.IndexOf("'");
                if (position > 0)
                {
                    replacewith = "&apos;";
                }
            }
            if (position > 0)
                return filePath.Substring(0, position) + replacewith + filePath.Substring(position + 1);
            else
                return filePath;
        }

        /// <summary>
        /// Updates RequestText memeber corresponding to current parameters
        /// </summary>
        private void UpdateRequestText()
        {
            if (Action == ClamWinScanner.ActionID.NotDefined)
            {
                return;
            }

            RequestText = "<?xml version=\"1.0\" ?><clamwinrequest>";
            RequestText += "<action>" + ClamWinScanner.GetActionName(Action) + "</action>";

            if (Action == ClamWinScanner.ActionID.ReloadDB)
            {               
                RequestText += "<dbpath>";
                RequestText += DbPath;
                RequestText += "</dbpath>";
            }
            else if (Action == ClamWinScanner.ActionID.Scan)
            {
                RequestText += EscapeFilePath(FilePath);
                RequestText += "</filename>";
            }
            else if (Action == ClamWinScanner.ActionID.FsFilterControl)
            {
                RequestText += "<value>";
                RequestText += FsFilterEnabled ? "1" : "0";
                RequestText += "</value>";
            }
            else if (Action == ClamWinScanner.ActionID.AsyncScan)
            {
                RequestText += "<filename>";
                RequestText += EscapeFilePath(FilePath);
                RequestText += "</filename>";            
            }
            else if (Action == ClamWinScanner.ActionID.AsyncResult)
            {
                RequestText += "<jobid>";
                RequestText += JobID.ToString();
                RequestText += "</jobid>";            
            }
            else if (Action == ClamWinScanner.ActionID.AsyncAbortScan)
            {
                RequestText += "<jobid>";
                RequestText += JobID.ToString();
                RequestText += "</jobid>";
            }
            else if (Action == ClamWinScanner.ActionID.RegisterGui ||
                     Action == ClamWinScanner.ActionID.UnregisterGui )
            {
                RequestText += "<value>";
                RequestText += Value.ToString();
                RequestText += "</value>";
            }           
            RequestText += "</clamwinrequest>";
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Sets path to database, used as ActionReloadDB parameter
        /// </summary>
        /// <param name="path">Path to database</param>
        public void SetDBPath(string path)
        {
            if (Action != ClamWinScanner.ActionID.ReloadDB)
            {
                return;
            }
            DbPath = path;
            UpdateRequestText();
        }
        /// <summary>
        /// Sets path to file, used as ActionScan parameter
        /// </summary>
        /// <param name="path"></param>
        public void SetFilePath(string path)
        {            
            FilePath = path;
            UpdateRequestText();
        }
        /// <summary>
        /// Set fs filter enabled, used as ActionFsFilterControl parameter
        /// </summary>
        /// <param name="enabled"></param>
        public void SetFsEnabled(bool enabled)
        {
            if (Action != ClamWinScanner.ActionID.FsFilterControl)
            {
                return;
            }
            FsFilterEnabled = enabled;
            UpdateRequestText();
        }        
        /// <summary>
        /// RequestText accessor
        /// </summary>
        /// <returns>RequestText</returns>
        public string GetRequestText()
        {
            return RequestText;
        }
        /// <summary>
        /// FilePath accessor
        /// </summary>
        /// <returns></returns>
        public string GetFilePath()
        {
            return FilePath;
        }
        /// <summary>
        /// Set JobID
        /// </summary>
        /// <param name="id"></param>
        public void SetJobID(int id)
        {
            JobID = id;

            UpdateRequestText();
        }
        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(int value)
        {
            Value = value;

            UpdateRequestText();
        }
        #endregion
    }
    #endregion
}
