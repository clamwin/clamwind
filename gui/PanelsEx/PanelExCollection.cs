// Name:        PanelExCollection.cs
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
using System.Collections;
using System.Text;

namespace PanelsEx
{
    #region PanelExCollection class
    /// <summary>
    /// PanelEx collection class.
    /// </summary>
    public class PanelExCollection : CollectionBase
    {
        #region Public Constructors
		/// <summary>
        /// Initialises a new instance of <see cref="PanelsExCollection">PanelsExCollection</see>.
		/// </summary>
        public PanelExCollection()
		{
		}
		#endregion

        #region Public Methods
        /// <summary>
        /// Adds a <see cref="PanelEx">PanelEx</see> to the end of the collection.
        /// </summary>
        /// <param name="panel">The <see cref="PanelEx">PanelEx</see> to add.</param>
        public void Add(PanelEx panel)
        {
            this.List.Add(panel);
        }

        /// <summary>
        /// Removes the <see cref="PanelEx">PanelEx</see> from the collection at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="PanelEx">PanelEx</see> to remove.</param>
        public void Remove(int index)
        {
            // Ensure the supplied index is valid
            if ((index >= this.Count) || (index < 0))
            {
                throw new IndexOutOfRangeException("The supplied index is out of range");
            }
            this.List.RemoveAt(index);
        }

        /// <summary>
        /// Gets a reference to the <see cref="PanelEx">PanelEx</see> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="PanelEx">PanelEx</see> to retrieve.</param>
        /// <returns></returns>
        public PanelEx Item(int index)
        {
            // Ensure the supplied index is valid
            if ((index >= this.Count) || (index < 0))
            {
                throw new IndexOutOfRangeException("The supplied index is out of range");
            }
            return (PanelEx)this.List[index];
        }

        /// <summary>
        /// Inserts a <see cref="PanelEx">PanelEx</see> at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <i>panel</i> should be inserted.</param>
        /// <param name="panel">The <see cref="PanelEx">PanelEx</see> to insert into the collection.</param>
        public void Insert(int index, PanelEx panel)
        {
            this.List.Insert(index, panel);
        }

        /// <summary>
        /// Copies the elements of the collection to a <see cref="System.Array">System.Array</see>, starting at a particular index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array">System.Array</see> that is the destination of the elements. The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <i>array</i> at which copying begins.</param>
        public void CopyTo(System.Array array, System.Int32 index)
        {
            this.List.CopyTo(array, index);
        }

        /// <summary>
        /// Searches for the specified <see cref="PanelEx">PanelEx</see> and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="panel">The <see cref="PanelEx">PanelEx</see> to search for.</param>
        /// <returns></returns>
        public int IndexOf(PanelEx panel)
        {
            return this.List.IndexOf(panel);
        }
        #endregion
    }
    #endregion
}
