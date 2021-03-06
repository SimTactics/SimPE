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

namespace SimPe.Packages
{
	/// <summary>
	/// Maintains a List of all opened Packages
	/// </summary>
	public class PackageMaintainer
	{
		static PackageMaintainer me;
		/// <summary>
		/// Returns the active package maintainer
		/// </summary>
		public static PackageMaintainer Maintainer
		{
			get 
			{
				if (me==null) me = new PackageMaintainer();
				return me;
			}
		}

		Hashtable ht;
		SimPe.Interfaces.Scenegraph.IScenegraphFileIndex localfileindex;
		/// <summary>
		/// Set or Get the FileIndex used to hold loaded Packages
		/// </summary>
		public SimPe.Interfaces.Scenegraph.IScenegraphFileIndex FileIndex
		{
			get { return localfileindex;}
			set { if (localfileindex==null) localfileindex = value; }
		}

		/// <summary>
		/// Create a new instance
		/// </summary>
		internal PackageMaintainer()
		{
            ht = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
		}

		/// <summary>
		/// Remove a given Package from the Maintainer
		/// </summary>
		/// <param name="pkg"></param>
		internal void RemovePackage(GeneratableFile pkg)
		{
			if (!Helper.WindowsRegistry.UsePackageMaintainer) return;
			if (pkg==null) return;			

			RemovePackage(pkg.FileName);
		}

		/// <summary>
		/// Remove a given Package from the Maintainer
		/// </summary>
		/// <param name="pkg"></param>
		public void RemovePackagesInPath(string folder)
		{
			if (folder==null) return;
			folder = folder.Trim().ToLower();
			

			ArrayList list = new ArrayList();
			foreach (string k in ht.Keys)
				if (k.Trim().ToLower().StartsWith(folder)) list.Add(k);

			foreach (string k in list)
				RemovePackage(k);
		}

		/// <summary>
		/// Remove a given Package from the Maintainer
		/// </summary>
		/// <param name="pkg"></param>
		internal void RemovePackage(string flname)
		{
			if (flname==null) return;
			

			if (ht.ContainsKey(flname)) 
			{
				SimPe.FileTableBase.FileIndex.ClosePackage((GeneratableFile)ht[flname]);
				//((GeneratableFile)ht[filename]).Close(true);
				ht.Remove(flname);
			}
		}

		internal void SyncFileIndex(GeneratableFile pkg)
		{
			this.FileIndex.Clear();		
			if (pkg.Index.Length<=Helper.WindowsRegistry.BigPackageResourceCount)
				this.FileIndex.AddIndexFromPackage(pkg);
		}

		/// <summary>
		/// Checks if the package on the passed Filename is already maintained here
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool Contains(string filename)
		{
			return ht.ContainsKey(filename);
		}

		/// <summary>
		/// Load a Package File from the Maintainer
		/// </summary>
		/// <param name="filename">the name of the package</param>
		/// <param name="sync">true, if the package should be synchronized with the Filesystem befor it is returned</param>
		/// <returns>an instance of <see cref="SimPe.Packages.GeneratableFile"/> for the given Filename</returns>
		/// <remarks>
		/// If the package was loaded once in this session, this Method will return an instance to the
		/// last loaded Version. Otherwise it wil create a new instance
		/// </remarks>
		public GeneratableFile LoadPackageFromFile(string filename, bool sync) 
		{
			GeneratableFile ret = null;			
			if (filename==null) ret =  GeneratableFile.CreateNew();
			else 
			{

			
				if (!Helper.WindowsRegistry.UsePackageMaintainer)  ret = new GeneratableFile(filename);
				else 
				{

					if (!ht.ContainsKey(filename)) ht[filename] = new GeneratableFile(filename);
					else if (sync) 
					{				
						SimPe.FileTableBase.FileIndex.ClosePackage((GeneratableFile)ht[filename]);
						//((GeneratableFile)ht[filename]).Close(true);
						((GeneratableFile)ht[filename]).ReloadFromFile(filename);
								
					}

					ret = (GeneratableFile)ht[filename];
				}				
			}

			if (sync) 
			{
				this.SyncFileIndex(ret);
			}

			return ret;
		}
	}
}
