// Name:        PanelExGroup.cs
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
using System.Drawing;
using System.Windows.Forms;

namespace PanelsEx
{
    #region PanelExGroup class
    /// <summary>
    /// An ExplorerBar-type extended Panel for containing <see cref="PanelEx">PanelEx</see> objects.
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
    public class PanelExGroup : System.Windows.Forms.Panel, System.ComponentModel.ISupportInitialize
    {
        #region Private data
        private bool                initialising = false;
        private int                 border = 8;
        private int                 spacing = 8;
        private PanelExCollection  panels = new PanelExCollection();
        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }

        #endregion

        #region Public Constructors
        /// <summary>
        /// Initialises a new instance of <see cref="PanelExGroup">PanelExGroup</see>.
        /// </summary>
        public PanelExGroup()
        {
            InitializeComponent();            
        }
        #endregion                     

        #region Public Methods
        /// <summary>
        /// Signals the object that initialization is starting.
        /// </summary>
        public void BeginInit()
        {
            this.initialising = true;
        }

        /// <summary>
        /// Signals the object that initialization is complete.
        /// </summary>
        public void EndInit()
        {
            this.initialising = false;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the <see cref="PanelsExCollection">PanelsExCollection</see> collection.
        /// </summary>
        [Browsable(false)]
        public PanelExCollection PanelsExCollection
        {
            get
            {
                return this.panels;
            }
        }

        /// <summary>
        /// Gets/sets the border around the panels. 
        /// </summary>
        public int Border
        {
            get
            {
                return this.border;
            }
            set
            {
                this.border = value;
                //UpdatePositions(this.panels.Count - 1);
            }
        }

        /// <summary>
        /// Gets/sets the vertical spacing between adjacent panels.
        /// </summary>
        public int Spacing
        {
            get
            {
                return this.spacing;
            }
            set
            {
                this.spacing = value;
                //UpdatePositions(this.panels.Count - 1);
            }
        }
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Updates panels from <see cref="PanelExGroup.panels">panels</see> from 0 to specified index.
        /// </summary>
        /// <param name="index">Determine lower index from which panels will be updated.</param>
        private void UpdatePositions()
        {
            for (int i = 0; i < panels.Count; i++)
            {
                PanelEx test = panels.Item(i);
                if (i == 0)
                {                
                    this.panels.Item(i).Top = this.border;
                }
                else
                {
                    this.panels.Item(i).Top = this.panels.Item(i - 1).Bottom + this.border;
                }
                
                this.panels.Item(i).Left = this.spacing;
                this.panels.Item(i).Width = this.Width - (2 * this.spacing);                
                
                if (true == this.VScroll)
                {
                    this.panels.Item(i).Width -= SystemInformation.VerticalScrollBarWidth;
                }                
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Event handler for the <see cref="Control.ControlAdded">ControlAdded</see> event.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.ControlEventArgs">ControlEventArgs</see> that contains the event data.</param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);          
            if (e.Control is PanelEx)            
            {
                // Adjust the docking property to Left | Right | Top
                e.Control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

                //if (true == initialising)
                {
                    // In the middle of InitializeComponent call.
                    // Generated code adds panels in reverse order, so add to end
                    this.panels.Add((PanelEx)e.Control);

                    this.panels.Item(this.panels.Count - 1).PanelStateChanged +=
                        new PanelStateChangedEventHandler(this.panel_StateChanged);
                }
                /*else
                {
                    // Add the panel to the beginning of the internal collection.
                    panels.Insert(0, (PanelEx)e.Control);

                    panels.Item(0).PanelStateChanged +=
                       new PanelStateChangedEventHandler(this.panel_StateChanged);
                } */               

                // Update the size and position of the panels
                UpdatePositions();
            }
        }

        /// <summary>
        /// Event handler for the <see cref="Control.ControlRemoved">ControlRemoved</see> event.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.ControlEventArgs">ControlEventArgs</see> that contains the event data.</param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            if (e.Control is PanelEx)            
            {
                // Get the index of the panel within the collection.
                int index = this.panels.IndexOf((PanelEx)e.Control);
                if (-1 != index)
                {
                    // Remove this panel from the collection.
                    this.panels.Remove(index);
                    // Update the position of any remaining panels.
                    UpdatePositions();
                }
            }
        }
        #endregion

        #region Event handlers
        private void panel_StateChanged(object sender, PanelEventArgs e)
        {
            UpdatePositions();
            /*// Get the index of the control that just changed state.
            int index = this.panels.IndexOf(e.PanelEx);
            if (-1 != index)
            {
                // Now update the position of all subsequent panels.
                UpdatePositions(--index);
            }*/
        }
        #endregion
    }
    #endregion 
}
