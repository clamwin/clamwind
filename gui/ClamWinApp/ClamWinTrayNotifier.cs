// Name:        ClamWinTrayNotifier.cs
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
using System.Drawing.Drawing2D;

namespace ClamWinApp
{
    #region ClamWinTrayNotifier class
    public partial class ClamWinTrayNotifier : Form
    {
        #region Protected Data
        /// <summary>
        /// Current notifier state
        /// </summary>
        protected NotifierStates State = NotifierStates.Hidden;
        /// <summary>
        /// Animation timer
        /// </summary>
        protected Timer AnimationTimer = new Timer();
        /// <summary>
        /// Screen area rectangle
        /// </summary>
        protected Rectangle WorkAreaRectangle;
        /// <summary>
        /// Appearing animation increment (pixels)
        /// </summary>
        protected int AppearingIncrement = 0;
        /// <summary>
        /// Disappearing animation increment (pixels)
        /// </summary>
        protected int DisappearingIncrement = 0;
        /// <summary>
        /// 
        /// </summary>
        protected int DisappearingInterval = 100;
        /// <summary>
        /// Initial height of the notifier form
        /// </summary>
        protected int InitialHeight = -1;
        /// <summary>
        /// Visible time (ms)
        /// </summary>
        protected int VisibleTime = 5000;
        /// <summary>
        /// If notify will disappear instantly
        /// </summary>
        protected bool DisappearInstantly = false;
        #endregion

        #region Private Data
        private int Delta = 2;
        #endregion

        #region Public Constructor
        public ClamWinTrayNotifier()
        {
            // Adjust form styles
            WindowState = FormWindowState.Minimized;
            base.Show();
            base.Hide();
            WindowState = FormWindowState.Normal;

            FormBorderStyle = FormBorderStyle.None;            
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = false;
            TopMost = true;
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = false;

            AnimationTimer.Enabled = true;
            AnimationTimer.Tick += new EventHandler(OnTimer);

            //InitializeComponent();
        }
        #endregion

        #region Constants
        public const int VisibleTimeInfinite = -1;
        public const int AppearingTimeInstant = -2;
        public const int DisappearingTimeInstant = -3;
        #endregion

        #region Enums
        /// <summary>
        /// Notifier state enumeration
        /// </summary>
        protected enum NotifierStates : int {Hidden = 0, Visible, Appearing, Disappearing};
        #endregion

        #region Event Handlers
        /// <summary>
        /// Timer on-tick handler
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ea"></param>
        protected void OnTimer(Object obj, EventArgs ea)
        {
            switch (State)
            {
                case NotifierStates.Hidden:
                {                
                    break;
                }
                case NotifierStates.Visible:
                {
                    if (VisibleTime == VisibleTimeInfinite)
                    {
                        break;
                    }

                    AnimationTimer.Stop();
                    AnimationTimer.Interval = DisappearingInterval;
                    State = NotifierStates.Disappearing;
                                        
                    AnimationTimer.Start();
                    break;
                }
                case NotifierStates.Appearing:
                {                   
                    if (this.Height < InitialHeight)
                        SetBounds(Left, Top - AppearingIncrement, Width, Height + AppearingIncrement);
                    else
                    {                        
                        AnimationTimer.Stop();
                        Height = InitialHeight;
                        if (VisibleTime == VisibleTimeInfinite)
                        {
                            AnimationTimer.Interval = 1024*64;
                        }
                        else
                        {
                            AnimationTimer.Interval = VisibleTime;
                        }                        
                        State = NotifierStates.Visible;
                        AnimationTimer.Start();
                    }                   
                    break;
                }
                case NotifierStates.Disappearing:
                {
                    if (DisappearInstantly)
                    {
                        Hide();
                        break;
                    }

                    if (Top < WorkAreaRectangle.Bottom - Delta)
                    {
                        SetBounds(Left, Top + DisappearingIncrement, Width, Height - DisappearingIncrement);
                    }
                    else
                    {
                        Hide();
                    }                    
                    break;
                }
            }
        }       
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Create rectangle region with rounded corners
        /// </summary>       
        private Region CreateRoundedRectRegion(int radius)
        {
            int diameter = 2 * radius;

            GraphicsPath path = new GraphicsPath();            
            // Top line
            path.AddLine(Bounds.Left + radius, Bounds.Top, Bounds.Right - radius, Bounds.Top);
            // Right-upper
            path.AddArc(Bounds.Right - diameter, Bounds.Top, diameter, diameter, 270, 90);
            // Right line
            path.AddLine(Bounds.Right, Bounds.Top + radius, Bounds.Right, Bounds.Bottom - radius);
            // Right-lower
            path.AddArc(Bounds.Right - diameter, Bounds.Bottom - diameter, diameter, diameter, 0, 90);
            // Bottom line
            path.AddLine(Bounds.Right - radius, Bounds.Bottom, Bounds.Left + radius, Bounds.Bottom);
            //path.AddLine(Bounds.Right, Bounds.Bottom, Bounds.Left, Bounds.Bottom);
            // Left-lower
            path.AddArc(Bounds.Left, Bounds.Bottom - diameter, diameter, diameter, 90, 90);
            // Left line
            path.AddLine(Bounds.Left, Bounds.Bottom - radius, Bounds.Left, Bounds.Top + radius);
            // Left-upper
            path.AddArc(Bounds.Left, Bounds.Top, diameter, diameter, 180, 90);

            Region region = new Region(path);
            path.Dispose();
            return region;
        }
       
        #endregion

        #region Public Function
        /// <summary>
        /// Shows notify
        /// </summary>
        /// <param name="AppearingTime">Appearing animation time (ms)</param>
        /// <param name="VisibleTime">Visible time (ms)</param>
        /// <param name="DisappearingTime">Disappearing animation time (ms)</param>
        public void ShowNotify(int AppearingTime, int VisibleTime, int DisappearingTime)
        {           
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            
            if (InitialHeight == -1)
            {
                InitialHeight = this.Height;
                this.Region = CreateRoundedRectRegion(10);
            }
                        
            this.VisibleTime = VisibleTime;
            
            int AppearingInterval = 0;
            
            if (AppearingTime != AppearingTimeInstant)
            {
                AppearingInterval = Math.Max(6 * AppearingTime / this.InitialHeight, 1);
                AppearingIncrement = 6;
            }

            if (DisappearingTime != DisappearingTimeInstant)
            {
                DisappearingInterval = Math.Max(6 * DisappearingTime / this.InitialHeight, 1);
                DisappearingIncrement = 6;
            }
            else
            {
                DisappearInstantly = true;
            }
            

            switch (State)
            {
                case NotifierStates.Hidden:
                {
                    if (AppearingTime != AppearingTimeInstant)
                    {
                        State = NotifierStates.Appearing;
                        AnimationTimer.Interval = AppearingInterval;
                        AnimationTimer.Start();
                        Win32API.ShowWindow(this.Handle, Win32API.SW_SHOWNOACTIVATE);

                        SetBounds(WorkAreaRectangle.Right - this.Width - 1, WorkAreaRectangle.Bottom - Delta, this.Width, 0);
                    }
                    else
                    {
                        Win32API.ShowWindow(this.Handle, Win32API.SW_SHOWNOACTIVATE);

                        SetBounds(WorkAreaRectangle.Right - this.Width - 1, WorkAreaRectangle.Bottom - this.InitialHeight - Delta, this.Width, this.InitialHeight);

                        State = NotifierStates.Visible;
                        if (VisibleTime == VisibleTimeInfinite)
                        {
                            AnimationTimer.Interval = 1024 * 64;
                        }
                        else
                        {
                            AnimationTimer.Interval = VisibleTime;
                        }
                        AnimationTimer.Start();
                        Refresh();
                    }
                    break;
                }
                case NotifierStates.Appearing:
                {
                    Refresh();
                    break;
                }
                case NotifierStates.Visible:
                {
                    AnimationTimer.Stop();
                    if (VisibleTime == VisibleTimeInfinite)
                    {
                        AnimationTimer.Interval = 1024*64;
                    }
                    else
                    {
                        AnimationTimer.Interval = VisibleTime;
                    }
                    AnimationTimer.Start();
                    Refresh();
                    break;
                }
                case NotifierStates.Disappearing:
                {
                    AnimationTimer.Stop();

                    if (DisappearingTime != DisappearingTimeInstant)
                    {
                        State = NotifierStates.Appearing;
                        SetBounds(WorkAreaRectangle.Right - this.Width - 1, WorkAreaRectangle.Bottom - this.InitialHeight - Delta, this.Width, this.InitialHeight);

                        if (VisibleTime == VisibleTimeInfinite)
                        {
                            AnimationTimer.Interval = 1024 * 64;
                        }
                        else
                        {
                            AnimationTimer.Interval = VisibleTime;
                        }
                        AnimationTimer.Start();
                        Refresh();
                    }
                    else
                    {
                        Hide();
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// Hides the popup immediately
        /// </summary>        
        public new void Hide()
        {
            if (State != NotifierStates.Hidden)
            {
                AnimationTimer.Stop();
                State = NotifierStates.Hidden;
                base.Hide();
            }
        }
        /// <summary>
        /// Initiate disappearing animation
        /// </summary>
        public void StartDisappearing()
        {
            int Helper = VisibleTime;

            VisibleTime = 1;
            State = ClamWinTrayNotifier.NotifierStates.Visible;
            this.OnTimer(this, new EventArgs());

            VisibleTime = Helper;
        }        
        #endregion

    }
    #endregion
}