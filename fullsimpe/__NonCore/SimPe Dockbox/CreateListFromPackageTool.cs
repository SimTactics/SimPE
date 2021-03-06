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
using SimPe.Interfaces;
using SimPe.Events;

namespace SimPe.Plugin.Tool
{
	/// <summary>
	/// Zusammenfassung f�r ImportSemiTool.
	/// </summary>
	public class CreateListFromPackageTool : SimPe.Interfaces.IToolPlus	
	{
		internal CreateListFromPackageTool() 
		{
			
		}		

		#region ITool Member

		public bool ChangeEnabledStateEventHandler(object sender, ResourceEventArgs e)
		{
			return (e.Loaded);
		}

		public void Execute(object sender, ResourceEventArgs es)
		{
			if (!ChangeEnabledStateEventHandler(sender, es)) return;

			SimPe.Events.ResourceContainers c = new ResourceContainers();
			foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in es.LoadedPackage.Package.Index) 
			{
				SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem fii = new SimPe.Plugin.FileIndexItem(pfd, es.LoadedPackage.Package);
				SimPe.Events.ResourceContainer rc = new ResourceContainer(fii);
				c.Add(rc);
			}

			CreateListFromSelectionTool.Execute(c);
		}

		


		public override string ToString()
		{
			return "Create Description\\from Package...";
		}

		#endregion

		#region IToolExt Member
		public System.Windows.Forms.Shortcut Shortcut
		{
			get
			{
				return System.Windows.Forms.Shortcut.CtrlShiftD;
			}
		}

		public System.Drawing.Image Icon
		{
			get
			{
				return System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.Tool.Dockable.package.png"));
			}
		}

		public virtual bool Visible 
		{
			get {return true;}
		}

		#endregion
	}
}
