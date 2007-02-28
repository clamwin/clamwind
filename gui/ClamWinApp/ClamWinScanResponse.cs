// Name:        ClamWinScanResponse.cs
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
using System.Xml;

namespace ClamWinApp
{
    #region ClamWinScanResponse class 
    /// <summary>
    /// ClamWinScanResponse class provide basic functionality to handle 
    /// service response
    /// </summary>
    public class ClamWinScanResponse
    {
        #region Private Data
        /// <summary>
        /// Action identifier
        /// </summary>
        private ClamWinScanner.ActionID Action = ClamWinScanner.ActionID.NotDefined;
        /// <summary>
        /// Service version
        /// </summary>
        private string Core;
        /// <summary>
        /// Clamav engine version
        /// </summary>
        private string LibClamav;
        /// <summary>
        /// Main database version
        /// </summary>
        private string Main;
        /// <summary>
        /// Daily database version
        /// </summary>
        private string Daily;
        /// <summary>
        /// FS Filter status (on/off)
        /// </summary>
        private bool   FsFilter;
        /// <summary>
        /// Action status, actions share this member
        /// </summary>
        private ClamWinScanner.ResponseStatus Status = ClamWinScanner.ResponseStatus.NotDefined;
        /// <summary>
        /// Name of file on which action has been performed
        /// </summary>
        private string FileName;
        /// <summary>
        /// Code of error, this field is expected with status "ERROR"
        /// </summary>
        private Int32 Code = -1;
        /// <summary>
        /// Name of virus, this field is expected with status "INFECTED"
        /// </summary>
        private string VirusName;
        /// <summary>
        /// ID of job started
        /// </summary>
        private int JobID = -1;
        /// <summary>
        /// Scanning progress
        /// </summary>
        private int Progress = -1;
        /// <summary>
        /// Message string
        /// </summary>
        private string Message = "";
        #endregion

        #region Public Constructor
        public ClamWinScanResponse(string text)
        {

            XmlDocument document = new XmlDocument();
            XmlNode root;
            XmlNode action;

            try
            {
                document.LoadXml(text);

                root = document.SelectSingleNode("clamwinreply");

                action = root.SelectSingleNode("action");
            }
            catch
            {
                Action = ClamWinScanner.ActionID.NotDefined;
                return;
            }

            Action = ClamWinScanner.GetActionID(action.InnerText);

            switch (Action)
            {
                case ClamWinScanner.ActionID.GetInfo:
                {
                    try
                    {
                        XmlNode core = root.SelectSingleNode("core");
                        XmlNode libclamav = root.SelectSingleNode("libclamav");
                        XmlNode main = root.SelectSingleNode("main");
                        XmlNode daily = root.SelectSingleNode("daily");
                        XmlNode fsfilter = root.SelectSingleNode("fsfilter");

                        Core = core.InnerText;
                        LibClamav = libclamav.InnerText;
                        Main = main.InnerText;
                        Daily = daily.InnerText;
                        FsFilter = Convert.ToByte(fsfilter.InnerText) == 1 ? true : false;
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;
                        return;
                    }
                    break;
                }
                case ClamWinScanner.ActionID.ReloadDB:
                {
                    try
                    {
                        XmlNode status = root.SelectSingleNode("status");
                        Status = ClamWinScanner.StringToStatus(status.InnerText);
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;
                        return;
                    }
                    break;
                }
                case ClamWinScanner.ActionID.Scan:
                {
                    try
                    {
                        XmlNode status = root.SelectSingleNode("status");
                        Status = ClamWinScanner.StringToStatus(status.InnerText);
                        try
                        {
                            XmlNode code = status.SelectSingleNode("@code");
                            if (code != null)
                            {
                                Code = Convert.ToInt32(code.InnerText);
                            }
                        }
                        catch
                        {                         
                        }
                        try
                        {
                            XmlNode virusname = root.SelectSingleNode("virusname");
                            if (VirusName != null)
                            {
                                VirusName = virusname.InnerText;
                            }
                        }
                        catch
                        {                         
                        }

                        XmlNode filename = root.SelectSingleNode("filename");                       
                        FileName = filename.InnerText;
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;                        
                        return;
                    }
                    break;
                }
                case ClamWinScanner.ActionID.FsFilterControl:
                {
                    try
                    {
                        XmlNode status = root.SelectSingleNode("status");
                        Status = ClamWinScanner.StringToStatus(status.InnerText);
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;
                        return;
                    }
                    break;
                }
                case ClamWinScanner.ActionID.AsyncScan:
                { 
                    try
                    {
                        XmlNode filename = root.SelectSingleNode("filename");                        

                        FileName = filename.InnerText;
                        
                        try
                        {
                            XmlNode jobid = root.SelectSingleNode("jobid");
                            if (jobid != null)
                            {
                                JobID = int.Parse(jobid.InnerText);
                            }
                            else
                            {
                                JobID = -1;
                            }
                        }
                        catch
                        {
                            JobID = -1;
                        }

                        try
                        {
                            XmlNode status = root.SelectSingleNode("status");
                            if (status != null)
                            {
                                Status = ClamWinScanner.StringToStatus(status.InnerText);
                                try
                                {
                                    XmlNode code = status.SelectSingleNode("@code");
                                    if (code != null)
                                    {
                                        Code = Convert.ToInt32(code.InnerText);
                                    }
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                Status = ClamWinScanner.ResponseStatus.Ok;
                            }
                        }
                        catch
                        {
                            Status = ClamWinScanner.ResponseStatus.Ok;
                        }
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;
                        return;
                    }
                    break;
                }
                case ClamWinScanner.ActionID.AsyncResult:
                {
                    try
                    {
                        XmlNode status = root.SelectSingleNode("status");
                        Status = ClamWinScanner.StringToStatus(status.InnerText);
                        try
                        {
                            XmlNode code = status.SelectSingleNode("@code");
                            if (code != null)
                            {
                                Code = Convert.ToInt32(code.InnerText);
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            XmlNode progress = root.SelectSingleNode("progress");
                            if (progress != null)
                            {
                                Progress = Convert.ToInt32(progress.InnerText);
                            }
                        }
                        catch
                        {
                            Progress = -1;
                        }

                        try
                        {
                            XmlNode message = root.SelectSingleNode("message");
                            if (message != null)
                            {
                                Message = message.InnerText;
                            }
                        }
                        catch
                        {
                            Message = "";
                        }

                        try
                        {
                            XmlNode virusname = root.SelectSingleNode("virusname");
                            if (VirusName != null)
                            {
                                VirusName = virusname.InnerText;
                            }
                        }
                        catch
                        {
                        }
                        
                        XmlNode jobid = root.SelectSingleNode("jobid");                        
                        JobID = int.Parse(jobid.InnerText);
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;
                        return;
                    }
                    break;
                }
                case ClamWinScanner.ActionID.AsyncAbortScan:
                {
                    try
                    {                        
                        XmlNode jobid = root.SelectSingleNode("jobid");                        
                        JobID = int.Parse(jobid.InnerText);

                        XmlNode status = root.SelectSingleNode("status");
                        Status = ClamWinScanner.StringToStatus(status.InnerText);
                        try
                        {
                            XmlNode code = status.SelectSingleNode("@code");
                            if (code != null)
                            {
                                Code = Convert.ToInt32(code.InnerText);
                            }
                        }
                        catch
                        {
                        }
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;
                        return;
                    }
                    break;
                }
                case ClamWinScanner.ActionID.RegisterGui:
                case ClamWinScanner.ActionID.UnregisterGui:
                {
                    try
                    {
                        XmlNode status = root.SelectSingleNode("status");
                        Status = ClamWinScanner.StringToStatus(status.InnerText);
                        try
                        {
                            XmlNode code = status.SelectSingleNode("@code");
                            if (code != null)
                            {
                                Code = Convert.ToInt32(code.InnerText);
                            }
                        }
                        catch
                        {
                        }                       
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;
                        return;
                    }
                    break;                   
                }
                case ClamWinScanner.ActionID.GetFilterJob:
                {
                    #region
                    try
                    {
                        XmlNode status = root.SelectSingleNode("status");
                        Status = ClamWinScanner.StringToStatus(status.InnerText);
                        try
                        {
                            XmlNode code = status.SelectSingleNode("@code");
                            if (code != null)
                            {
                                Code = Convert.ToInt32(code.InnerText);
                            }
                        }
                        catch
                        {
                        }
                    }
                    catch
                    {
                        Action = ClamWinScanner.ActionID.NotDefined;
                        return;
                    }
                    
                    try
                    {
                        XmlNode message = root.SelectSingleNode("message");
                        if (message != null)
                        {
                            Message = message.InnerText;
                        }
                    }
                    catch
                    {
                        Message = "";
                    }

                    try
                    {
                        XmlNode filename = root.SelectSingleNode("filename");
                        if (filename != null)
                        {
                            FileName = filename.InnerText;
                        }
                    }
                    catch
                    {
                        FileName = "";
                    }
                    #endregion
                    break;
                }               
                default:
                { 
                    Core = "";
                    LibClamav = "";
                    Main = "";
                    Daily = "";
                    FsFilter = false;
                    Status = ClamWinScanner.ResponseStatus.NotDefined;
                    FileName = "";
                    Code = 0;
                    VirusName = "";
                    JobID = 0;
                    break;
                }
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Service version accessor
        /// </summary>
        /// <returns></returns>
        public string GetCoreVersion()
        {
            return Core;
        }
        /// <summary>
        /// Clamav engine version accessor
        /// </summary>
        public string GetLibClamavVErsion()
        {
            return LibClamav;
        }
        /// <summary>
        /// Main database version accessor
        /// </summary>
        public string GetMainVersion()
        {
            return Main;
        }
        /// <summary>
        /// Daily database version accessor
        /// </summary>
        public string GetDailyVersion()
        {
            return Daily;
        }
        /// <summary>
        /// FS Filter status (on/off) accessor
        /// </summary>
        public bool GetFsFilterStatus()
        {
            return FsFilter;
        }
        /// <summary>
        /// Action status accessor
        /// </summary>
        public ClamWinScanner.ResponseStatus GetActionStatus()
        {
            return Status;
        }
        /// <summary>
        /// Filename accessor 
        /// </summary>
        public string GetFileName()
        {
            return FileName;
        }
        /// <summary>
        /// Error code accessor
        /// </summary>
        public Int32 GetCode()
        {
            return Code;
        }
        /// <summary>
        /// Virus name accessor
        /// </summary>
        public string GetVirusName()
        {
            return VirusName;
        }
        /// <summary>
        /// Job ID accessor
        /// </summary>
        public int GetJobID()
        {
            return JobID;
        }        
        /// <summary>
        /// Returns scanning progress
        /// </summary>
        /// <returns></returns>
        public int GetProgress()
        {
            return Progress;
        }
        /// <summary>
        /// Returns message
        /// </summary>
        /// <returns></returns>
        public string GetMessage()
        {
            return Message;
        }
        #endregion
    }
    #endregion
}
