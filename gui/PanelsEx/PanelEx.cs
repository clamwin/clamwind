// Name:        PanelEx.cs
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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PanelsEx
{
    #region PanelEx class
    /// <summary>
    /// An extended <see cref="System.Windows.Forms.Panel">Panel</see> that provides collapsible panels like those provided in Windows XP.
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
    public class PanelEx : System.Windows.Forms.Panel
    {
        #region Events
        /// <summary>
        /// A <see cref="PanelState">PanelState</see> changed event.
        /// </summary>
        [Category("State"),
        Description("Raised when panel state has changed.")]
        public event PanelStateChangedEventHandler PanelStateChanged;
        /// <summary>
        /// Title mouse-up event
        /// </summary>
        [Category("State"),
        Description("Raised when panel title pressed.")]
        public event TitlePressedEventHandler TitlePressed;
        #endregion

        #region Private class data
        private ColorMatrix     grayMatrix;
		private ImageAttributes grayAttributes;
        private PanelState      state = PanelState.Expanded;
        private IContainer      components = null;
        private int             panelHeight;
        private bool            mouseSensitive = true;
        private bool            changeCursor = true;
        private bool            drawCollapseExpandIcons = true;
        private const int       minTitleHeight = 24;
        private const int       iconBorder = 2;
        private const int       expandBorder = 4;
        private const int       diameter = 20;
        private Color           startColour = Color.White;
        private Color           endColour = Color.FromArgb(199, 212, 247);
        private Label           labelTitle;
        private Image           image;
        private ImageList       imageList;
        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelEx));
            this.labelTitle = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            this.SetStyle(  ControlStyles.UserPaint |
                            ControlStyles.AllPaintingInWmPaint |
                            ControlStyles.OptimizedDoubleBuffer, true);
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.labelTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Navy;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(200, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseMove);
            this.labelTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.labelTitle_Paint);
            this.labelTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseUp);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Collapse.gif");
            this.imageList.Images.SetKeyName(1, "Expand.gif");
            // 
            // PanelEx
            // 
            this.Controls.Add(this.labelTitle);
            this.BackColorChanged += new System.EventHandler(this.PanelEx_BackColorChanged);
            this.SizeChanged += new System.EventHandler(this.PanelEx_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        #region Public Constructors
        /// <summary>
        /// Initialises a new instance of <a cref="PanelEx">PanelEx</a>.
        /// </summary>
        public PanelEx()
        {
            InitializeComponent();                        

            // Store the current panelHeight
            this.panelHeight = this.Height;

            // Setup the ColorMatrix and ImageAttributes for grayscale images.
            this.grayMatrix = new ColorMatrix();
            this.grayMatrix.Matrix00 = 1 / 3f;
            this.grayMatrix.Matrix01 = 1 / 3f;
            this.grayMatrix.Matrix02 = 1 / 3f;
            this.grayMatrix.Matrix10 = 1 / 3f;
            this.grayMatrix.Matrix11 = 1 / 3f;
            this.grayMatrix.Matrix12 = 1 / 3f;
            this.grayMatrix.Matrix20 = 1 / 3f;
            this.grayMatrix.Matrix21 = 1 / 3f;
            this.grayMatrix.Matrix22 = 1 / 3f;
            this.grayAttributes = new ImageAttributes();
            this.grayAttributes.SetColorMatrix(this.grayMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
        }        
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets/sets the <see cref="PanelState">PanelState</see>.
        /// </summary>
        [Browsable(false)]
        public PanelState PanelState
        {
            get
            {
                return this.state;
            }
            set
            {
                PanelState oldState = this.state;
                this.state = value;
                if (oldState != this.state)
                {
                    // State has changed to update the display                  
                    UpdateDisplayedState();
                }
            }
        }      

        /// <summary>
        /// Gets/sets the text displayed as the panel title.
        /// </summary>
        [Category("Title"),
        Description("The text contained in the title bar.")]
        public string TitleText
        {
            get
            {
                return this.labelTitle.Text;
            }
            set
            {
                this.labelTitle.Text = value;
            }
        }


        /// <summary>
        /// Gets/sets if panel will expand\collapse reacting on mouse event.
        /// </summary>
        [Category("Title"),
        Description("If title bar will react on mouse events.")]
        public bool MouseSensitive
        {
            get
            {
                return this.mouseSensitive;
            }
            set
            {
                this.mouseSensitive = value;
            }
        }

        /// <summary>
        /// Gets/sets if panel will expand\collapse reacting on mouse event.
        /// </summary>
        [Category("Title"),
        Description("If title bar will change cursor on mouse events.")]
        public bool ChangeCursor
        {
            get
            {
                return this.changeCursor;
            }
            set
            {
                this.changeCursor = value;
            }
        }

        /// <summary>
        /// Gets/sets if panel will draw icons to reflect current state.
        /// </summary>
        [Category("Title"),
       Description("If panel will draw icons to reflect current state.")]
        public bool DrawCollapseExpandIcons
        {
            get
            {
                return this.drawCollapseExpandIcons;
            }
            set
            {
                this.drawCollapseExpandIcons = value;
            }
        }

        



        /// <summary>
        /// Gets/sets the foreground colour used for the title bar.
        /// </summary>
        [Category("Title"),
        Description("The foreground colour used to display the title text.")]
        public Color TitleFontColour
        {
            get
            {
                return this.labelTitle.ForeColor;
            }
            set
            {
                this.labelTitle.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets/sets the font used for the title bar text.
        /// </summary>
        [Category("Title"),
        Description("The font used to display the title text.")]
        public Font TitleFont
        {
            get
            {
                return this.labelTitle.Font;
            }
            set
            {
                this.labelTitle.Font = value;
            }
        }

        /// <summary>
        /// Gets/sets the image list used for the expand/collapse image.
        /// </summary>
        [Category("Title"),
        Description("The image list to get the images displayed for expanding/collapsing the panel.")]
        public ImageList ImageList
        {
            get
            {
                return this.imageList;
            }
            set
            {
                this.imageList = value;               
            }
        }

        /// <summary>
        /// Gets/sets the starting colour for the background gradient of the header.
        /// </summary>
        [Category("Title"),
        Description("The colour used at the start of the colour gradient displayed as the background of the title bar.")]
        public Color StartColour
        {
            get
            {
                return this.startColour;
            }
            set
            {
                this.startColour = value;
                this.labelTitle.Invalidate();
            }
        }

        /// <summary>
        /// Gets/sets the ending colour for the background gradient of the header.
        /// </summary>
        [Category("Title"),
        Description("The colour used at the end of the colour gradient displayed as the background of the title bar.")]
        public Color EndColour
        {
            get
            {
                return this.endColour;
            }
            set
            {
                this.endColour = value;
                this.labelTitle.Invalidate();
            }
        }

        /// <summary>
        /// Gets/sets the image displayed in the header of the title bar.
        /// </summary>
        [Category("Title"),
        Description("The image that will be displayed on the left hand side of the title bar.")]
        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                if (null != value)
                {
                    // Update the height of the title label
                    this.labelTitle.Height = this.image.Height + (2 * PanelEx.iconBorder);
                    if (this.labelTitle.Height < minTitleHeight)
                    {
                        this.labelTitle.Height = minTitleHeight;
                    }
                }
                this.labelTitle.Invalidate();
            }
        }
        #endregion                       

        #region Private Helper functions       
        /// <summary>
        /// Helper function to determine if the mouse is currently over the title bar.
        /// </summary>
        /// <param name="xPos">The x-coordinate of the mouse position.</param>
        /// <param name="yPos">The y-coordinate of the mouse position.</param>
        /// <returns></returns>
        private bool IsOverTitle(int xPos, int yPos)
        {
            // Get the dimensions of the title label
            Rectangle rectTitle = this.labelTitle.Bounds;
            // Check if the supplied coordinates are over the title label
            if (rectTitle.Contains(xPos, yPos))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper function to update the displayed state of the panel.
        /// </summary>
        private void UpdateDisplayedState()
        {
            switch (this.state)
            {
                case PanelState.Collapsed:
                    // Entering collapsed state, so store the current height.
                    this.panelHeight = this.Height;
                    // Collapse the panel
                    this.Height = labelTitle.Height;                   
                    break;
                case PanelState.Expanded:
                    // Entering expanded state, so expand the panel.
                    this.Height = this.panelHeight;                    
                    break;
                default:
                    // Ignore
                    break;
            }
            this.labelTitle.Invalidate();

            OnPanelStateChanged(new PanelEventArgs(this));
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Event handler for the <see cref="PanelEx.PanelStateChanged">PanelStateChanged</see> event.
        /// </summary>
        /// <param name="e">A <see cref="PanelEventArgs">PanelEventArgs</see> that contains the event data.</param>
        protected virtual void OnPanelStateChanged(PanelEventArgs e)
        {
            if (PanelStateChanged != null)
            {
                PanelStateChanged(this, e);
            }
        }

        private void labelTitle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.Parent.BackColor);
            
            int radius = diameter / 2;
            Rectangle bounds = labelTitle.Bounds;                       

            int offsetY = 0;
            if (null != this.image)
            {
                offsetY = this.labelTitle.Height - PanelEx.minTitleHeight;
                if (offsetY < 0)
                {
                    offsetY = 0;
                }
                bounds.Offset(0, offsetY);
                bounds.Height -= offsetY;
            }         
   
            SolidBrush mainBrush = new SolidBrush(this.BackColor);
            Rectangle helperRect = bounds;
            helperRect.Offset( new Point( 0, helperRect.Height / 2) );
            e.Graphics.FillRectangle(mainBrush, helperRect);

            // Create a GraphicsPath with curved top corners
            GraphicsPath path = new GraphicsPath();            
            // Top line
            path.AddLine(bounds.Left + radius, bounds.Top, bounds.Right - radius, bounds.Top);
            // Right-upper
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            // Right line
            path.AddLine(bounds.Right, bounds.Top + radius, bounds.Right, bounds.Bottom - radius);
            // Right-lower
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            // Bottom line
            path.AddLine(bounds.Right - radius, bounds.Bottom, bounds.Left + radius, bounds.Bottom);
            //path.AddLine(bounds.Right, bounds.Bottom, bounds.Left, bounds.Bottom);
            // Left-lower
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            // Left line
            path.AddLine(bounds.Left, bounds.Bottom - radius, bounds.Left, bounds.Top + radius);
            // Left-upper
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);          

            // Create a colour gradient            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (true == this.Enabled)
            {
                LinearGradientBrush brush = new LinearGradientBrush(
                    bounds, this.startColour, this.endColour, LinearGradientMode.Horizontal);

                // Paint the colour gradient into the title label.
                e.Graphics.FillPath(brush, path);
            }
            else
            {
                Color grayStart = new Color();
                grayStart = this.startColour;                
                Color grayEnd = new Color();
                grayEnd = this.endColour;               
                LinearGradientBrush brush = new LinearGradientBrush(
                    bounds, grayStart, grayEnd,
                    LinearGradientMode.Horizontal);

                // Paint the grayscale gradient into the title label.
                e.Graphics.FillPath(brush, path);
            }            

            // Draw the header icon, if there is one
            System.Drawing.GraphicsUnit graphicsUnit = System.Drawing.GraphicsUnit.Display;
            int offsetX = PanelEx.iconBorder;
            if (null != this.image)
            {
                offsetX += this.image.Width + PanelEx.iconBorder;
                // Draws the title icon grayscale when the panel is disabled.            
                RectangleF srcRectF = this.image.GetBounds(ref graphicsUnit);
                Rectangle destRect = new Rectangle(PanelEx.iconBorder,
                    PanelEx.iconBorder, this.image.Width, this.image.Height);
                if (true == this.Enabled)
                {
                    e.Graphics.DrawImage(this.image, destRect, (int)srcRectF.Left, (int)srcRectF.Top,
                        (int)srcRectF.Width, (int)srcRectF.Height, graphicsUnit);
                }
                else
                {
                    e.Graphics.DrawImage(this.image, destRect, (int)srcRectF.Left, (int)srcRectF.Top,
                        (int)srcRectF.Width, (int)srcRectF.Height, graphicsUnit, this.grayAttributes);
                }               
            }

            // Draw the title text.
            SolidBrush textBrush = new SolidBrush(this.TitleFontColour);
            // Title text truncated with an ellipsis where necessary.            
            float left = (float)offsetX + 2 * PanelEx.iconBorder;
            float top = (float)offsetY + (float)PanelEx.expandBorder;
            float width = (float)this.labelTitle.Width - left - this.imageList.ImageSize.Width -
                PanelEx.expandBorder;
            float height = (float)PanelEx.minTitleHeight - (2f * (float)PanelEx.expandBorder);
            RectangleF textRectF = new RectangleF(left, top, width, height);
            StringFormat format = new StringFormat();
            format.Trimming = StringTrimming.EllipsisWord;
            // Draw title text disabled where appropriate.           
            if (true == this.Enabled)
            {
                e.Graphics.DrawString(labelTitle.Text, labelTitle.Font, textBrush,
                    textRectF, format);
            }
            else
            {
                Color disabled = SystemColors.GrayText;
                ControlPaint.DrawStringDisabled(e.Graphics, labelTitle.Text, labelTitle.Font,
                    disabled, textRectF, format);
            }                     

            // Draw the expand/collapse image           
            if ((null != this.imageList) && 
                (this.imageList.Images.Count >= 2) &&
                drawCollapseExpandIcons 
                )
            {
                int xPos = bounds.Right - this.imageList.ImageSize.Width - PanelEx.expandBorder;
                int yPos = bounds.Top + PanelEx.expandBorder;
                RectangleF srcIconRectF = this.ImageList.Images[(int)this.state].GetBounds(ref graphicsUnit);
                Rectangle destIconRect = new Rectangle(xPos, yPos,
                    this.imageList.ImageSize.Width, this.imageList.ImageSize.Height);
                if (true == this.Enabled)
                {
                    e.Graphics.DrawImage(this.ImageList.Images[(int)this.state], destIconRect,
                        (int)srcIconRectF.Left, (int)srcIconRectF.Top, (int)srcIconRectF.Width,
                        (int)srcIconRectF.Height, graphicsUnit);
                }
                else
                {
                    e.Graphics.DrawImage(this.ImageList.Images[(int)this.state], destIconRect,
                        (int)srcIconRectF.Left, (int)srcIconRectF.Top, (int)srcIconRectF.Width,
                        (int)srcIconRectF.Height, graphicsUnit, this.grayAttributes);
                }
            }
        }

        private void labelTitle_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (true == IsOverTitle(e.X, e.Y)))
            {     
                if (this.mouseSensitive)               
                {
                    if (this.state == PanelState.Expanded)
                    {
                        // Currently expanded, so store the current height.
                        this.state = PanelState.Collapsed;
                    }
                    else
                    {
                        // Currently collapsed, so expand the panel.
                        this.state = PanelState.Expanded;
                    }
                    UpdateDisplayedState();                                       
                }
                if (TitlePressed != null)
                {
                    TitlePressed(this, new PanelEventArgs(this));
                } 
            }
        }

        private void labelTitle_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.None) && (true == IsOverTitle(e.X, e.Y)))
            {
                if (this.changeCursor)
                {
                    this.labelTitle.Cursor = Cursors.Hand;
                }
            }
            else
            {
                this.labelTitle.Cursor = Cursors.Default;
            }
        }

        private void PanelEx_BackColorChanged(object sender, EventArgs e)
        {
            this.labelTitle.BackColor = this.BackColor;
        }
       
        private void PanelEx_SizeChanged(object sender, EventArgs e)
        {
            int radius = diameter / 2;
            GraphicsPath path = new GraphicsPath();
            Rectangle bounds = this.Bounds;
            // Top line
            path.AddLine(radius, 0, bounds.Width - radius, 0);
            // Right-upper
            path.AddArc(bounds.Width - diameter, 0, diameter, diameter, 270, 90);
            // Right line
            path.AddLine(bounds.Width, radius, bounds.Width, bounds.Height - radius);
            // Right-lower
            path.AddArc(bounds.Width - diameter, bounds.Height - diameter, diameter, diameter, 0, 90);
            // Bottom line
            path.AddLine(bounds.Width - radius, bounds.Height, radius, bounds.Height);            
            // Left-lower
            path.AddArc(0, bounds.Height - diameter, diameter, diameter, 90, 90);
            // Left line
            path.AddLine(0, bounds.Height - radius, 0, radius);
            // Left-upper
            path.AddArc(0, 0, diameter, diameter, 180, 90);
            this.Region = new Region(path);
        }
        #endregion

        #region Dispose ??
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
        #endregion            
    }
    #endregion

    #region Enumerations
    /// <summary>
    /// Defines the state of a <see cref="PanelEx">PanelEx</see>.
    /// </summary>
    public enum PanelState
    {
        /// <summary>
        /// The <see cref="PanelEx">PanelEx</see> is expanded.
        /// </summary>
        Expanded,
        /// <summary>
        /// The <see cref="PanelEx">PanelEx</see> is collapsed.
        /// </summary>
        Collapsed
    }
    #endregion

    #region Delegates
    /// <summary>
    /// A delegate type for hooking up panel state change notifications.
    /// </summary>
    public delegate void PanelStateChangedEventHandler(object sender, PanelEventArgs e);
    /// <summary>
    /// A delegate type for hooking up title mouse-up notifications.
    /// </summary>
    public delegate void TitlePressedEventHandler(object sender, PanelEventArgs e);
        

    #endregion

    #region PanelEventArgs class
    /// <summary>
    /// Provides data for the <see cref="PanelEx.PanelStateChanged">PanelStateChanged</see> event.
    /// </summary>
    public class PanelEventArgs : System.EventArgs
    {
        #region Private Class data
        private PanelEx panel;
        #endregion

        #region Public Constructors
        /// <summary>
        /// Initialises a new <see cref="PanelEventArgs">PanelEventArgs</see>.
        /// </summary>
        /// <param name="sender">The originating <see cref="PanelEx">PanelEx</see>.</param>
        public PanelEventArgs(PanelEx sender)
        {
            this.panel = sender;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the <see cref="PanelEx">PanelEx</see> that triggered the event.
        /// </summary>
        public PanelEx PanelEx
        {
            get
            {
                return this.panel;
            }
        }

        /// <summary>
        /// Gets the <see cref="PanelState">PanelState</see> of the <see cref="PanelEx">PanelEx</see> that triggered the event.
        /// </summary>
        public PanelState PanelState
        {
            get
            {
                return this.panel.PanelState;
            }
        }
        #endregion
    }
    #endregion
}
