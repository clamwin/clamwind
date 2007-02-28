// Name:        ClamWinMainFormNotifications.cs
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
using System.Collections;
using System.Text;

namespace ClamWinApp
{
    #region ClamWinMainFormNotifications class
    public class ClamWinMainFormNotifications
    {
        #region Private Data
        /// <summary>
        /// Main form handle
        /// </summary>
        private static IntPtr MainFormHandle = (IntPtr)0;
        /// <summary>
        /// Notifications list
        /// </summary>
        private static ArrayList Notifications = new ArrayList();
        /// <summary>
        /// Current notification
        /// </summary>
        private static int CurrentNotification = -1;
        /// <summary>
        /// Main locker
        /// </summary>
        private static object MainLocker = new object();
        #endregion

        #region Hidden Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private ClamWinMainFormNotifications()
        {

        }
        #endregion

        #region Constants 
        public const int UM_CURRENT_NOTIFICATION_CHANGED = Win32API.WM_USER + 501;
        #endregion

        #region Public Functions
        /// <summary>
        /// Opens notifications
        /// </summary>
        public static bool Open(IntPtr Handle)
        {
            string command = "SELECT * FROM MainFormNotifications;";
            ArrayList Items;

            if (!ClamWinDatabase.ExecReader(command, out Items))
            {
                return false;
            }

            if (Items.Count > 0)
            { 
                const int FieldsPerRecord = ClamWinDatabase.MainFormNotificationsFPR;

                int RecordsCount = Items.Count / FieldsPerRecord;

                for (int i = 0; i < RecordsCount; i++)
                {
                    NotificationData Data = new NotificationData();
                    // [ID] 
                    Data.ID = int.Parse((string)Items[i * FieldsPerRecord + 0]);
                    // [Message] 
                    Data.Message = (string)Items[i * FieldsPerRecord + 1];
                    // [Code] 
                    Data.Code = (NotificationCodes)int.Parse((string)Items[i * FieldsPerRecord + 2]);
                    // [Type] 
                    Data.Type = (NotificationType)int.Parse((string)Items[i * FieldsPerRecord + 3]);
                    // [Time]                     
                    Data.Time = DateTime.FromBinary(long.Parse((string)Items[i * FieldsPerRecord + 4]));

                    lock(MainLocker)
                    {
                        Notifications.Add(Data);
                    }
                }
                lock (MainLocker)
                {
                    CurrentNotification = Notifications.Count - 1;                                       
                }                
            }

            lock (MainLocker)
            {
                MainFormHandle = Handle;
            }

            PostCurrentChanged();

            return true;        
        }
        /// <summary>
        /// Return current notification position
        /// </summary>
        /// <returns></returns>
        public static NotificationPositions GetCurrentNotificationPos()
        {
            lock (MainLocker)
            {
                if (Notifications.Count == 0)
                {
                    return NotificationPositions.NotificationsEmpty;
                }
                else if (Notifications.Count == 1)
                {
                    return NotificationPositions.Single;
                }
                else if (CurrentNotification == 0)
                {
                    return NotificationPositions.Left;
                }
                else if (CurrentNotification == Notifications.Count - 1)
                {
                    return NotificationPositions.Right;
                }
                else
                {
                    return NotificationPositions.Middle;
                }
            }
        }
        /// <summary>
        /// Close and save notifications
        /// </summary>
        public static void Close()
        { 
            lock(MainLocker)
            {
                string command = "DELETE FROM MainFormNotifications;";
                int result;
                ClamWinDatabase.ExecCommand(command, out result);

                while(true)
                {
                    NotificationData data;                
                    if( Notifications.Count == 0 )
                    {
                        CurrentNotification = -1;
                        return;
                    }

                    data = (NotificationData)Notifications[0];
                    Notifications.RemoveAt(0);
                    
                    
                    command = "INSERT INTO MainFormNotifications(Message,Code,Type,Time) VALUES(";
                    command += "'" + data.Message + "',";
                    command += "'" + ((int)data.Code).ToString() + "',";
                    command += "'" + ((int)data.Type).ToString() + "',";
                    command += "'" + data.Time.ToBinary().ToString() + "');";
                    
                    ClamWinDatabase.ExecCommand(command, out result);                            
                }                
            }
        }
        /// <summary>
        /// Add new notifcation
        /// </summary>
        public static int AddNotification(string message, NotificationCodes code, NotificationType type)
        {
            lock (MainLocker)
            {
                for (int i = 0; i < Notifications.Count; i++)
                {
                    NotificationData item = (NotificationData)Notifications[i];
                    if (code == item.Code)
                    {
                        //Already exist
                        CurrentNotification = i;                        
                        PostCurrentChanged();

                        return -1;
                    }
                }
                NotificationData data = new NotificationData();
                data.Message = message;
                data.Code = code;
                data.Type = type;
                data.Time = DateTime.Now;

                string command = "INSERT INTO MainFormNotifications(Message,Code,Type,Time) VALUES(";
                command += "'" + data.Message + "',";
                command += "'" + ((int)data.Code).ToString() + "',";
                command += "'" + ((int)data.Type).ToString() + "',";
                command += "'" + data.Time.ToBinary().ToString() + "');";
                
                int result;
                ClamWinDatabase.ExecCommand(command, out result); 

                command = "SELECT * FROM MainFormNotifications WHERE Time=(SELECT max(Time) FROM MainFormNotifications);"; 

                ArrayList Items;
                if (!ClamWinDatabase.ExecReader(command, out Items))
                {
                    return -1;
                }
                else if (Items.Count == 0)
                {
                    return -1;
                }

                data.ID = int.Parse((string)Items[0]);
                
                Notifications.Add(data);

                CurrentNotification = Notifications.Count - 1;

                PostCurrentChanged();

                return data.ID;
            }
        }
        /// <summary>
        /// Remove current notification
        /// </summary>
        public static void CurrentNotifcationDone()
        { 
            lock (MainLocker)
            {
                if (CurrentNotification < 0 || CurrentNotification >= Notifications.Count)
                {
                    return;
                }

                NotificationData data = (NotificationData)Notifications[CurrentNotification];

                string command = "DELETE FROM MainFormNotifications WHERE id = '"+data.ID.ToString()+"';";
                int result;
                ClamWinDatabase.ExecCommand(command, out result);

                Notifications.RemoveAt(CurrentNotification);

                if (Notifications.Count == 0)
                {
                    CurrentNotification = -1;
                }
                else if (CurrentNotification >= Notifications.Count)
                {
                    CurrentNotification = Notifications.Count-1;
                }

                PostCurrentChanged();
            }
        }
        /// <summary>
        /// Remove specified notification
        /// </summary>
        public static void NotificationDone(NotificationCodes code)
        {
            lock (MainLocker)
            {
                int CurrID = -1;
                if (CurrentNotification != -1)
                {
                    CurrID = ((NotificationData)Notifications[CurrentNotification]).ID;
                }

                NotificationData data = new NotificationData();
                data.ID = -1;

                bool ok = false;
                for (int i = 0; i < Notifications.Count; i++)
                {
                    data = (NotificationData)Notifications[i];

                    if (data.Code == code)
                    {
                        ok = true;
                        Notifications.RemoveAt(i);
                        break;
                    }
                }

                if (!ok)
                {
                    return;
                }

                string command = "DELETE FROM MainFormNotifications WHERE id = '" + data.ID.ToString() + "';";
                int result;
                ClamWinDatabase.ExecCommand(command, out result);

                CurrentNotification = -1;
                for (int i = 0; i < Notifications.Count; i++)
                {
                    data = (NotificationData)Notifications[i];

                    if (data.ID == CurrID)
                    {
                        CurrentNotification = i;
                        break;
                    }
                }

                if (CurrentNotification == -1 && Notifications.Count != 0)
                {
                    CurrentNotification = Notifications.Count - 1;
                }           

                PostCurrentChanged();
            }
        }
        /// <summary>
        /// Select next notification
        /// </summary>
        public static void Next()
        {
            lock (MainLocker)
            {
                if (CurrentNotification < Notifications.Count-1)
                {
                    CurrentNotification++;

                    PostCurrentChanged();
                }
            }
        }
        /// <summary>
        /// Select previous notification
        /// </summary>
        public static void Previous()
        {
            lock (MainLocker)
            {
                if (CurrentNotification > 0 )
                {
                    CurrentNotification--;

                    PostCurrentChanged();
                }
            }
        }
        /// <summary>
        /// Current notification data accessor
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool GetCurrentData(ref NotificationData data)
        {
            lock (MainLocker)
            {                
                if( CurrentNotification == -1)
                {
                    return false;
                }
                                
                data = (NotificationData)Notifications[CurrentNotification];
                return true;
            }
        }
        /// <summary>
        /// Return corresponding string
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CodeToString(NotificationCodes code)
        {
            switch (code)
            { 
                case NotificationCodes.NewVersionAlert:
                    return "New version available.";
                case NotificationCodes.NeedDatabaseUpdate:
                    return "Anti Virus Database is out of date!";
                default:
                    return "Not defined notification!";                
            }
        }
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Post user message to main form
        /// </summary>
        public static void PostCurrentChanged()
        { 
            Win32API.PostMessage(MainFormHandle,UM_CURRENT_NOTIFICATION_CHANGED,0,0);
        }
        #endregion

        #region Enum
        /// <summary>
        /// Types of notification
        /// </summary>
        public enum NotificationType : int {Info = 0, Warning, Error};
        /// <summary>
        /// Notification positions
        /// </summary>
        public enum NotificationPositions : int { Left = 0, Middle, Right, NotificationsEmpty, Single };
        public enum NotificationCodes : int {NewVersionAlert,NeedDatabaseUpdate};
        #endregion

        #region NotificationData struct
        public struct NotificationData
        {
            /// <summary>
            /// Notification message
            /// </summary>
            public string Message;
            /// <summary>
            /// Windows message code 
            /// </summary>
            public NotificationCodes Code;
            /// <summary>
            /// Notification type
            /// </summary>
            public NotificationType Type;
            /// <summary>
            /// Database identifier
            /// </summary>
            public int ID;
            /// <summary>
            /// Notification time
            /// </summary>
            public DateTime Time;
        }
        #endregion
    }
    #endregion
}
