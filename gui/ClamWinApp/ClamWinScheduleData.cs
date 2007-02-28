// Name:        ClamWinScheduleData.cs
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
    #region ClamWinScheduleData class
    /// <summary>
    /// Schedule information holder class
    /// </summary>
    public class ClamWinScheduleData
    {
        #region Constants
        /// <summary>
        /// Database update command line argument
        /// </summary>
        public const string UpdateArg = "dbupdate";
        /// <summary>
        /// Scan critical command line argument
        /// </summary>
        public const string ScanCriticalArg = "sccrit";
        /// <summary>
        /// Scan My Computer command line argument
        /// </summary>
        public const string ScanMyPCArg = "scmypc";
        /// <summary>
        /// Scan command line argument
        /// </summary>
        public const string ScanArg = "scan";
        #endregion

        #region Public Data
        /// <summary>
        /// Name of the data
        /// </summary>
        public string Name;
        /// <summary>
        /// Scheduling type
        /// </summary>
        public SchedulingTypes Type = SchedulingTypes.Once;
        /// <summary>
        /// Date for Once, Daily, Weekly and Monthly scheduling types
        /// </summary>
        public DateTime Date = DateTime.Now;
        /// <summary>
        /// Frequency for Minutely, Hourly, Daily, Weekly and Mothly scheduling types
        /// </summary>
        public int Frequency = 1;
        /// <summary>
        /// If time will be used for Daily scheduling type
        /// </summary>
        public bool UseDateTime = false;
        /// <summary>
        /// Type of the Daily schedulling
        /// </summary>
        public DailyTypes DailyType = DailyTypes.EveryWeekday;
        /// <summary>
        /// Selected days for Weekly scheduling type
        /// </summary>
        public DaysOfTheWeek Days = (DaysOfTheWeek)0;
        /// <summary>
        /// ClamWind cmd line arguments
        /// </summary>
        public string CmdLineArguments = "";
        /// <summary>
        /// Task name
        /// </summary>
        public string TaskName = "";
        #endregion

        #region Public Constructor
        public ClamWinScheduleData(string name,string args)
        {
            Name = name;
            CmdLineArguments = args;

            int i = Name.IndexOf("ScheduleData");
            if (i != -1)
            {
                TaskName = "ClamWin"+Name.Substring(0,i)+"Task";
            }
            else
            {
                TaskName = "ClamWinTask";
            }
        }
        #endregion

        #region Enums
        /// <summary>
        /// Scheduling types
        /// </summary>
        public enum SchedulingTypes : int {Once = 0, Minutely, Hourly, Daily, Weekly, Monthly };
        /// <summary>
        /// Sub-types for Daily scheduling type
        /// </summary>
        public enum DailyTypes : int {EveryNDays = 0, EveryWeekday, EveryWeekend };
        /// <summary>
        /// Days of the week
        /// </summary>
        public enum DaysOfTheWeek : int {Su = 1, Mo = 2, Tu = 4, We = 8, Th = 16, Fr = 32, Sa = 64 };
        #endregion

        #region Private Helper Functions       
        #endregion

        #region Public Functions
        /// <summary>
        /// Return description string for current scheduling data
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string description = "";

            switch (Type)
            {
                case SchedulingTypes.Once:
                {
                    description = "Once at " + Date.ToShortDateString() + " " + Date.ToLongTimeString();
                    break;
                }
                case SchedulingTypes.Minutely:
                {
                    description = "Every " + Frequency.ToString() + " minute(s)";
                    break;
                }
                case SchedulingTypes.Hourly:
                {
                    description = "Every " + Frequency.ToString() + " hour(s)";
                    break;
                }
                case SchedulingTypes.Daily:
                {
                    switch (DailyType)
                    { 
                        case DailyTypes.EveryNDays:
                            description = "Every " + Frequency.ToString() + " day(s)";
                            break;
                        case DailyTypes.EveryWeekday:
                            description = "Every weekday";
                            break;
                        case DailyTypes.EveryWeekend:
                            description = "Every weekend";
                            break;                    
                    }

                    if (UseDateTime)
                    {
                        description += " at "+Date.ToLongTimeString();
                    }
                    break;
                }
                case SchedulingTypes.Weekly:
                {                    
                    if ( IsDayChecked(DaysOfTheWeek.Su) )
                    {
                        description += "Su";
                    }
                    if (IsDayChecked(DaysOfTheWeek.Mo))
                    {
                        description += "Mo";
                    }
                    if (IsDayChecked(DaysOfTheWeek.Tu))
                    {
                        description += "Tu";
                    }
                    if (IsDayChecked(DaysOfTheWeek.We))
                    {
                        description += "We";
                    }
                    if (IsDayChecked(DaysOfTheWeek.Th))
                    {
                        description += "Th";
                    }
                    if (IsDayChecked(DaysOfTheWeek.Fr))
                    {
                        description += "Fr";
                    }
                    if (IsDayChecked(DaysOfTheWeek.Sa))
                    {
                        description += "Sa";
                    }

                    if (UseDateTime)
                    {
                        description += " at " + Date.ToLongTimeString();
                    }                    
                    break;
                }
                case SchedulingTypes.Monthly:
                {
                    description = "Each "+Frequency.ToString()+" day of month";

                    if (UseDateTime)
                    {
                        description += " at " + Date.ToLongTimeString();
                    } 
                    break;
                }
                default:
                {
                    description = "Scheduling type is not defined";
                    break;
                }
            }

            return description;
        }
        /// <summary>
        /// Return true if day is checked 
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public bool IsDayChecked(DaysOfTheWeek day)
        {
            if ((((int)Days) & ((int)day)) != 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Switch specified day of or on
        /// </summary>
        /// <param name="day"></param>
        public void ChangeDay(DaysOfTheWeek day, bool check)
        {
            if (check)
            {
                Days |= day;
            }
            else
            { 
                Days ^= day; 
            }
        }
        #endregion
    }
    #endregion
}
