// Name:        ClamWinScanScheduler.cs
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
using TaskScheduler;

namespace ClamWinApp
{    
    #region ClamWinScanScheduler class
    class ClamWinScheduler
    {
        #region Private Data
        /// <summary>
        /// Main scheduler object
        /// </summary>
        private static ScheduledTasks tasks = new ScheduledTasks();
        /// <summary>
        /// Path and file name to ClamWin 
        /// </summary>
        private static string ClamWinPathName = "";
        private static object MainLock = new object();
        #endregion

        #region Public Constructor
        public ClamWinScheduler(string filename)
        {
            ClamWinPathName = filename;
        }
        #endregion        

        #region Private Helper Functions
        #endregion

        #region Public Methods         
        /// <summary>
        /// Add new task or update existing
        /// </summary>
        /// <param name="?"></param>
        public static void AddTask(ref ClamWinScheduleData data)
        {
            lock (MainLock)
            {
                Task task;

                try
                {
                    // Delete previous task if exists
                    tasks.DeleteTask(data.TaskName);
                    // Create new
                    task = tasks.CreateTask(data.TaskName);
                }
                catch (ArgumentException)
                {
                    return;
                }

                task.ApplicationName = ClamWinPathName;
                task.Parameters = "-" + data.CmdLineArguments;
                task.Comment = "ClamWin " + data.Name + ".";
                task.IdleWaitMinutes = 0;
                task.Priority = System.Diagnostics.ProcessPriorityClass.Normal;

                switch (data.Type)
                {
                    case ClamWinScheduleData.SchedulingTypes.Once:
                        {
                            task.Triggers.Add(new RunOnceTrigger(data.Date));
                            break;
                        }
                        case ClamWinScheduleData.SchedulingTypes.Minutely:
                        {
                            DateTime date = DateTime.Now;
                            date = date.Add(new TimeSpan(0, data.Frequency, 0));
                            Trigger trigger = new DailyTrigger((short)date.Hour, (short)date.Minute, 1);
                            trigger.DurationMinutes = 24 * 60;
                            trigger.IntervalMinutes = data.Frequency;                            
                            task.Triggers.Add(trigger);
                            //TODO: no such trigger available currently
                            break;
                        }
                        case ClamWinScheduleData.SchedulingTypes.Hourly:
                        {
                            DateTime date = DateTime.Now;
                            date = date.Add(new TimeSpan(data.Frequency, 0, 0));
                            Trigger trigger = new DailyTrigger((short)date.Hour, (short)date.Minute, 1);
                            trigger.DurationMinutes = 24 * 60;
                            trigger.IntervalMinutes = data.Frequency*60;                            
                            task.Triggers.Add(trigger);
                            //TODO: no such trigger available currently                            
                            break;
                        }
                        case ClamWinScheduleData.SchedulingTypes.Daily:
                        {
                            short hour = 0;
                            short minutes = 0;

                            if (data.UseDateTime)
                            {
                                hour = (short)data.Date.Hour;
                                minutes = (short)data.Date.Minute;
                            }

                            switch (data.DailyType)
                            {
                                case ClamWinScheduleData.DailyTypes.EveryNDays:
                                    {
                                        task.Triggers.Add(new DailyTrigger(hour, minutes, (short)data.Frequency));
                                        break;
                                    }
                                case ClamWinScheduleData.DailyTypes.EveryWeekday:
                                    {
                                        task.Triggers.Add(new DailyTrigger(hour, minutes));
                                        break;
                                    }
                                case ClamWinScheduleData.DailyTypes.EveryWeekend:
                                    {
                                        task.Triggers.Add(new WeeklyTrigger(hour, minutes, DaysOfTheWeek.Saturday | DaysOfTheWeek.Sunday));
                                        break;
                                    }
                            }

                            break;
                        }
                        case ClamWinScheduleData.SchedulingTypes.Weekly:
                        {
                            short hour = 0;
                            short minutes = 0;

                            if (data.UseDateTime)
                            {
                                hour = (short)data.Date.Hour;
                                minutes = (short)data.Date.Minute;
                            }

                            task.Triggers.Add(new WeeklyTrigger(hour, minutes, (DaysOfTheWeek)data.Days));
                            break;
                        }
                        case ClamWinScheduleData.SchedulingTypes.Monthly:
                        {
                            short hour = 0;
                            short minutes = 0;

                            if (data.UseDateTime)
                            {
                                hour = (short)data.Date.Hour;
                                minutes = (short)data.Date.Minute;
                            }

                            int[] day = new int[1];
                            day[0] = data.Frequency;
                            task.Triggers.Add(new MonthlyTrigger(hour, minutes, day));
                            break;
                        }
                }
                task.Save();
                task.Close();
            }
        }
        /// <summary>
        /// Remove specified task
        /// </summary>
        /// <param name="data"></param>
        public static void RemoveTask(ref ClamWinScheduleData data)
        {
            lock (MainLock)
            {
                try
                {
                    // Delete task 
                    tasks.DeleteTask(data.TaskName);
                }
                catch (ArgumentException)
                {

                }
            }
        }
        /// <summary>
        /// Open scheduler
        /// </summary>
        /// <param name="path"></param>
        public static void Open(string path)
        {
            lock (MainLock)
            {
                ClamWinPathName = path;
            }
        }
        #endregion
    }
    #endregion
}
