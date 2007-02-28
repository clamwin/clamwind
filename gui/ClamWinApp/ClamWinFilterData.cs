//-----------------------------------------------------------------------------
// Name:        ClamwinFilterDATA.cs
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
using System.Collections;
using System.Text.RegularExpressions;

namespace ClamWinApp
{
    #region
    public class ClamWinFilterData
    {
        #region Public Data
        /// <summary>
        /// Exclude Patterns Collection
        /// </summary>
        public ArrayList ExcludePatterns = new ArrayList();
        /// <summary>
        /// Scan Only Patterns Collection
        /// </summary>
        public ArrayList IncludePatterns = new ArrayList();
        /// <summary>
        /// Data Name
        /// </summary>
        public string Name = "";
        #endregion

        #region Public Constructor
        public ClamWinFilterData(string name)
        {
            ExcludePatterns.Add("*.txt");
            ExcludePatterns.Add("*.dbx");
            ExcludePatterns.Add("*.tbb");
            ExcludePatterns.Add("*.pst");
            ExcludePatterns.Add("*.dat");
            ExcludePatterns.Add("*.log");
            ExcludePatterns.Add("*.evt");
            ExcludePatterns.Add("*.nsf");
            ExcludePatterns.Add("*.ntf");

            Name = name;
        }
        #endregion

        #region Public Functions
        public static string WildcardToRegex(string wildcard)
        {
	        StringBuilder sb = new StringBuilder(wildcard.Length + 8);

	        sb.Append("^");

	        for (int i = 0; i < wildcard.Length; i++)
	        {
		        char c = wildcard[i];
		        switch(c)
		        {
			        case '*':
				        sb.Append(".*");
				        break;
			        case '?':
				        sb.Append(".");
				        break;
			        case '\\':
				        if (i < wildcard.Length - 1)
					        sb.Append(Regex.Escape(wildcard[++i].ToString()));
				        break;
			        default:
				        sb.Append(Regex.Escape(wildcard[i].ToString()));
				        break;
		        }
	        }

	        sb.Append("$");

	        return sb.ToString();
        }
        #endregion

        #region
        #endregion
    }
    #endregion
}
