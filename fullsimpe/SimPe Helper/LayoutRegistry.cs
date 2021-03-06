/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/
using System;
using System.Collections;
using System.IO;
using System.Xml;
#if MAC
#else
using Microsoft.Win32;
#endif


namespace SimPe
{
	/// <summary>
	/// Handles Layout Settings for the Application
	/// </summary>
	/// <remarks>You cannot create instance of this class, use the 
	/// <see cref="SimPe.Helper.WindowsRegistry.Layout"/> Field to access the LayoutRegistry</remarks>
	public class LayoutRegistry
	{
		#region Attributes		

		/// <summary>
		/// The Root Registry Key for this Application
		/// </summary>
		XmlRegistryKey xrk;
		#endregion

		#region Management
		XmlRegistry reg;
		/// <summary>
		/// Creates a new Instance
		/// </summary>
		/// <param name="layoutkey">Key to the Layout</param>
        internal LayoutRegistry(XmlRegistryKey layoutkey)
        {
            reg = new XmlRegistry(Helper.DataFolder.Layout2XREG, Helper.DataFolder.Layout2XREGW, true);
            xrk = reg.CurrentUser.CreateSubKey(@"Software\Ambertation\SimPe\Layout");
        }	

		

		/// <summary>
		/// Returns the Registry Key you can use to store Optional Plugin Data
		/// </summary>
		public XmlRegistryKey LayoutRegistryKey
		{
			get 
			{
				return xrk.CreateSubKey("PluginLayout");
			}
		}

		/// <summary>
		/// Descturtor 
		/// </summary>
		/// <remarks>
		/// Will flsuh the XmlRegistry to the disk
		/// </remarks>
		~LayoutRegistry()
		{
			//Flush();
		}

		/// <summary>
		/// Write the Settings to the Disk
		/// </summary>
		public void Flush() 
		{
			if (reg!=null) reg.Flush();
		}
		
		#endregion

        /// <summary>
        /// returns a list of Strings that hold the names of all available ToolbarButtons
        /// </summary>
        /// <remarks>Adding to tha list will not update the value! You have to use the Setter again!</remarks>
        public ArrayList VisibleToolbarButtons
        {
            get
            {
                object o = xrk.GetValue("TBButtons", new ArrayList());
                return o as ArrayList;
            }
            set
            {
                xrk.SetValue("TBButtons", value);
            }
        }
		

		/// <summary>
		/// true, if the user wantsto use the package Maintainer
		/// </summary>
		public byte SelectedTheme
		{
			get 
			{
				object o = xrk.GetValue("ThemeID", (int)1);
				return (byte)Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("ThemeID", (int)value);
			}
		}		
		
		/// <summary>
		/// true if the taskBox should be presented expanded
		/// </summary>
		public bool DefaultActionBoxExpanded
		{
			get 
			{
				object o = xrk.GetValue("ActionDefExpanded", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				xrk.SetValue("ActionDefExpanded", value);
			}
		}

		/// <summary>
		/// true if the taskBox should be presented expanded
		/// </summary>
		public bool ToolActionBoxExpanded
		{
			get 
			{
				object o = xrk.GetValue("ActionToolExpanded", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				xrk.SetValue("ActionToolExpanded", value);
			}
		}

		/// <summary>
		/// true if the taskBox should be presented expanded
		/// </summary>
		public bool PluginActionBoxExpanded
		{
			get 
			{
				object o = xrk.GetValue("ActionPlugExpanded", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				xrk.SetValue("ActionPlugExpanded", value);
			}
		}


        static string[] colNames = new string[] { "Name", "Type", "Group", "InstHi", "Inst", "Offset", "Size" };
        public System.Collections.Generic.List<string> ColumnOrder
        {
            get
            {
                string[] s = xrk.GetValue("ColumnOrder", String.Join(",", colNames)).ToString().Split(',');
                System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>(s);
                System.Collections.Generic.List<string> lc = new System.Collections.Generic.List<string>(colNames);
                foreach (string v in s) if (!lc.Contains(v)) ls.Remove(v);
                foreach (string v in colNames) if (!ls.Contains(v)) ls.Add(v);
                return ls;
            }
            set
            {
                string[] s = value.ToArray();
                System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>(s);
                System.Collections.Generic.List<string> lc = new System.Collections.Generic.List<string>(colNames);
                foreach (string v in s) if (!lc.Contains(v)) ls.Remove(v);
                foreach (string v in colNames) if (!ls.Contains(v)) ls.Add(v);
                xrk.SetValue("ColumnOrder", String.Join(",", ls.ToArray()));
            }
        }

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int TypeColumnWidth
		{
			get 
			{
				object o = xrk.GetValue("TypeColumnWidth", (int)204);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("TypeColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int GroupColumnWidth
		{
			get 
			{
				object o = xrk.GetValue("GroupColumnWidth", (int)100);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("GroupColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int InstanceHighColumnWidth
		{
			get 
			{
				object o = xrk.GetValue("InstanceHighColumnWidth", (int)100);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("InstanceHighColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int InstanceColumnWidth
		{
			get 
			{
				object o = xrk.GetValue("InstanceColumnWidth", (int)100);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("InstanceColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int OffsetColumnWidth
		{
			get 
			{
				object o = xrk.GetValue("OffsetColumnWidth", (int)100);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("OffsetColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int SizeColumnWidth
		{
			get 
			{
				object o = xrk.GetValue("SizeColumnWidth", (int)100);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("SizeColumnWidth", value);
			}
		}
	}
}
