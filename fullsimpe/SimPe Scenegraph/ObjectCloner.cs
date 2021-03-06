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
using SimPe.Packages;
using SimPe.Interfaces.Files;
using SimPe.Interfaces.Scenegraph;

namespace SimPe.Plugin
{
	/// <summary>
	/// Determins the basic Settings for the <see cref="ObjectCloner"/>.
	/// </summary>
	public class CloneSettings 
	{
		public class StrIntsanceAlias : SimPe.Data.Alias
		{
			internal StrIntsanceAlias(uint inst, uint type, string ext) : base(inst, ext, new object[] {type})
			{
			}

			public uint Type
			{
				get {return (uint)Tag[0];}
			}

			public uint Instance
			{
				get {return this.Id;}
			}

			public string Extension
			{
				get {return Name;}
			}
		}
		public enum BaseResourceType : byte
		{
			Objd = 0x01,
			Ref = 0x02,
			Xml = 0x04
		}
		bool includeWallmask;
		bool onlydefault;
		bool updateguid;
		bool exception;
		bool loadanim;
		bool keepmesh;
		BaseResourceType use3idr;

		/// <summary>
		/// true, if you want to load Files referenced by 3IDR Resoures
		/// </summary>
		public BaseResourceType BaseResource
		{
			get { return use3idr; }
			set { use3idr = value; }
		}

		/// <summary>
		/// true, if you do not want to include the Mesh Data into the package
		/// </summary>
		public bool KeepOriginalMesh
		{
			get { return keepmesh; }
			set { keepmesh = value; }
		}

		/// <summary>
		/// true, if the clone should include Wallmasks
		/// </summary>
		public bool IncludeWallmask
		{
			get { return includeWallmask; }
			set { includeWallmask = value; }
		}

		/// <summary>
		/// true if you only want default MMAT Files
		/// </summary>
		public bool OnlyDefaultMmats
		{
			get { return onlydefault; }
			set { onlydefault = value; }
		}

		/// <summary>
		/// update the GUIDs in the MMAT Files
		/// </summary>
		public bool UpdateMmatGuids
		{
			get { return updateguid; }
			set { updateguid = value; }
		}

		/// <summary>
		/// true if you want to throw an exception when something goes wrong
		/// </summary>
		public bool ThrowExceptions
		{
			get { return exception; }
			set { exception = value; }
		}

		/// <summary>
		/// true if Animation Resources should be included in the package
		/// </summary>
		public bool IncludeAnimationResources
		{
			get { return loadanim; }
			set { loadanim = value; }
		}

		StrIntsanceAlias[] pullfromstrinst;
		/// <summary>
		/// The INstances of StrResources, that can contain valid Links to Scenegraph Resources
		/// </summary>
		public StrIntsanceAlias[] StrInstances
		{
			get { return pullfromstrinst; }
			set {pullfromstrinst = value;}
		}

		bool pullfromstr;
		/// <summary>
		/// If true, SimPE will check all Str resources with the instance listed in <see cref="PullFromStrInstances"/>
		/// and pull all Resources linked from there too
		/// </summary>
		public bool PullResourcesByStr
		{
			get { return pullfromstr;}
			set {pullfromstr = value;}
		}
		
		/// <summary>
		/// Create a new Instance and set everything to default
		/// </summary>
		public CloneSettings()
		{
			pullfromstrinst = new StrIntsanceAlias[] {new StrIntsanceAlias(0x88, Data.MetaData.TXMT, "_txmt")};
			pullfromstr = true;
			includeWallmask = true;
			exception = true;
			updateguid = true;
			onlydefault = true;
			loadanim = false;
			keepmesh = false;
			use3idr = BaseResourceType.Objd;
		}
	}

	/// <summary>
	/// This Class provides Methods to clone ingame Objects
	/// </summary>
	public class ObjectCloner
	{
		/// <summary>
		/// The Base Package
		/// </summary>
		IPackageFile package;

		/// <summary>
		/// The Base Package
		/// </summary>
		public IPackageFile Package
		{
			get { return package; }
		}

		CloneSettings setup;

		/// <summary>
		/// The Settings for this Cloner
		/// </summary>
		public CloneSettings Setup 
		{
			get { return setup; }
			set { setup = value; }
		}
		/// <summary>
		/// Creates a new Isntance based on an existing Package
		/// </summary>
		/// <param name="package">The Package that should receive the Clone</param>
		public ObjectCloner(IPackageFile package) 
		{
			this.package = package;
			setup = new CloneSettings();
		}

		/// <summary>
		/// Creates a new Instance and a new Package
		/// </summary>
		public ObjectCloner() 
		{
			package = GeneratableFile.LoadFromStream((System.IO.BinaryReader)null);
			setup = new CloneSettings();
		}

		/// <summary>
		/// Find the second MMAT that matches the state
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Interfaces.Files.IPackedFileDescriptor[] FindStateMatchingMatd(string name, IPackageFile package)
		{
			Interfaces.Files.IPackedFileDescriptor[] pfds = null;

			//railingleft1 railingleft2 railingleft3 railingleft4
			if (name.EndsWith("_clean")) 
			{
				name = name.Substring(0, name.Length-6)+"_dirty";
				pfds = package.FindFile(name+"_txmt", 0x49596978);
			} 
			else if (name.EndsWith("_dirty")) 
			{
				name = name.Substring(0, name.Length-6)+"_clean";
				pfds = package.FindFile(name+"_txmt", 0x49596978);
			}
			else if (name.EndsWith("_lit")) 
			{
				name = name.Substring(0, name.Length-4)+"_unlit";
				pfds = package.FindFile(name+"_txmt", 0x49596978);
			}
			else if (name.EndsWith("_unlit")) 
			{
				name = name.Substring(0, name.Length-6)+"_lit";
				pfds = package.FindFile(name+"_txmt", 0x49596978);
			}
			else if (name.EndsWith("_on")) 
			{
				name = name.Substring(0, name.Length-3)+"_off";
				pfds = package.FindFile(name+"_txmt", 0x49596978);
			}
			else if (name.EndsWith("_off")) 
			{
				name = name.Substring(0, name.Length-4)+"_on";
				pfds = package.FindFile(name+"_txmt", 0x49596978);
			} 
			else if (name.EndsWith("_shadeinside")) 
			{
				name = name.Substring(0, name.Length-4)+"_shadeoutside";
				pfds = package.FindFile(name+"_txmt", 0x49596978);
			}
			else if (name.EndsWith("_shadeoutside")) 
			{
				name = name.Substring(0, name.Length-4)+"_shadeinside";
				pfds = package.FindFile(name+"_txmt", 0x49596978);
			}
			return pfds;
		}		

		/// <summary>
		/// Returns the Primary Guid of the Object
		/// </summary>
		/// <returns>0 or the default guid</returns>
		public uint GetPrimaryGuid()
		{
			uint guid = 0;
			SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFile(Data.MetaData.OBJD_FILE, 0, 0x41A7);
			if (pfds.Length==0) pfds = package.FindFiles(Data.MetaData.OBJD_FILE);
			if (pfds.Length>0) 
			{
				SimPe.PackedFiles.Wrapper.ExtObjd objd = new SimPe.PackedFiles.Wrapper.ExtObjd();
				objd.ProcessData(pfds[0], package);
				guid = objd.Guid;
			}
			return guid;
		}

		/// <summary>
		/// Load a List of all available GUIDs in the package
		/// </summary>
		/// <returns>The list of GUIDs</returns>
		public ArrayList GetGuidList()
		{
			ArrayList list = new ArrayList();
			SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFiles(Data.MetaData.OBJD_FILE);
			
			foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds) 
			{
				SimPe.PackedFiles.Wrapper.ExtObjd objd = new SimPe.PackedFiles.Wrapper.ExtObjd();
				objd.ProcessData(pfd, package);
				list.Add(objd.Guid);
			}

			return list;
		}

		/// <summary>
		/// Updates the MMATGuids
		/// </summary>
		/// <param name="guids">list of allowed GUIDS</param>
		/// <param name="primary">the guid you want to use if the guid was not allowed</param>
		public void UpdateMMATGuids(ArrayList guids, uint primary)
		{
			if (primary==0) return;

			SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFiles(Data.MetaData.MMAT);
			
			foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds) 
			{
				SimPe.Plugin.MmatWrapper mmat = new MmatWrapper();
				mmat.ProcessData(pfd, package);

				
				//this seems to cause problems with slave Objects
				/*if (!guids.Contains(mmat.ObjectGUID)) 
				{					
					mmat.ObjectGUID = primary;
					mmat.SynchronizeUserData();
				}*/
			}
		}

		/// <summary>
		/// Clone a InGame Object based on the relations of the RCOL Files
		/// </summary>
		/// <param name="modelname">The Name of the Model</param>
		/// <param name="onlydefault">true if you want to load Parent Objects</param>
		public void RcolModelClone(string modelname) 
		{
			if (modelname==null) return;

			string[] ms = new string[1];
			ms[0] = modelname;
			RcolModelClone(ms);
		}

		/// <summary>
		/// Clone a InGame Object based on the relations of the RCOL Files
		/// </summary>
		/// <param name="modelnames">The Name of the Model</param>
		/// <param name="onlydefault">true if you only want default MMAT Files</param>
		public void RcolModelClone(string[] modelnames) {
			RcolModelClone(modelnames, new ArrayList());
		}

		/// <summary>
		/// Returns a List of all stored Anim Resources
		/// </summary>
		/// <param name="instances">Instances of TextLists that should be searched</param>
		/// <param name="ext">extension (in lowercase) that should be added (can be null for none)</param>
		/// <returns>List of found Names</returns>
		public string[] GetNames(ArrayList instances, string ext) 
		{
			ArrayList list = new ArrayList();

			SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = this.Package.FindFiles(Data.MetaData.STRING_FILE);
			foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pfds) 
			{
				if (instances.Contains(pfd.Instance)) 
				{
					SimPe.PackedFiles.Wrapper.Str str = new SimPe.PackedFiles.Wrapper.Str();
					str.ProcessData(pfd, Package);
					foreach (SimPe.PackedFiles.Wrapper.StrToken si in str.Items) 
					{
						string s = si.Title.Trim();
						if (s!="") 
						{
							if (ext!=null) if (!s.ToLower().EndsWith(ext)) s+=ext;
							list.Add(s);
						}
					}
				}
			}

			string[] ret = new string[list.Count];
			list.CopyTo(ret);
			return ret;
		}

		/// <summary>
		/// Returns a List of all stored Anim Resources
		/// </summary>
		/// <returns></returns>
		public string[] GetAnimNames() 
		{
			ArrayList instances = new ArrayList();
			instances.Add((uint)0x81); instances.Add((uint)0x82); instances.Add((uint)0x86); instances.Add((uint)0x192);

			return GetNames(instances, "_anim");
		}

		/// <summary>
		/// Clone a InGane Object based on the relations of the RCOL Files
		/// </summary>
		/// <param name="onlydefault">true if you only want default MMAT Files</param>
		/// <param name="exclude">List of ReferenceNames that should be excluded</param>
		public void RcolModelClone(string[] modelnames, ArrayList exclude) 
		{
			if (modelnames==null) return;

			Scenegraph.FileExcludeList = Scenegraph.DefaultFileExcludeList;

			SimPe.FileTable.FileIndex.Load();
            if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Walking Scenegraph");
			Scenegraph sg = new Scenegraph(modelnames, exclude, this.setup);
			if ((Setup.BaseResource & CloneSettings.BaseResourceType.Ref) == CloneSettings.BaseResourceType.Ref) 
			{
                if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Reading 3IDR References");
				sg.AddFrom3IDR(package);
			}
			if ((Setup.BaseResource & CloneSettings.BaseResourceType.Xml) == CloneSettings.BaseResourceType.Xml) 
			{
                if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Reading XObject Definition");
				sg.AddFromXml(package);
			}
			if (Setup.IncludeWallmask) 
			{
                if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Scanning for Wallmasks");
				sg.AddWallmasks(modelnames);
			}
			if (Setup.PullResourcesByStr) 
			{
                if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Scanning for #Str-linked Resources");
				sg.AddStrLinked(package, Setup.StrInstances);
			}
			if (Setup.IncludeAnimationResources) 
			{
                if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Scanning for Animations");
				sg.AddAnims(this.GetAnimNames());
			}
            if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Collect Slave TXMTs");
			sg.AddSlaveTxmts(sg.GetSlaveSubsets());

            if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Building Package");
			sg.BuildPackage(package);
            if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Collect MMAT Files");
			sg.AddMaterialOverrides(package, setup.OnlyDefaultMmats, true, setup.ThrowExceptions);
            if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Collect Slave TXMTs");
			Scenegraph.AddSlaveTxmts(package, Scenegraph.GetSlaveSubsets(package));
			

			if (setup.UpdateMmatGuids) 
			{
                if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Fixing MMAT Files");
				this.UpdateMMATGuids(this.GetGuidList(), this.GetPrimaryGuid());			
			}
		}

		/// <summary>
		/// Add all Files that could be borrowed from the current package by the passed one, to the passed package
		/// </summary>
		/// <param name="orgmodelnames">List of available modelnames in this package</param>
		/// <param name="pkg">The package that should receive the Files</param>
		/// <remarks>Simply Copies MMAT, LIFO, TXTR and TXMT Files</remarks>
		public void AddParentFiles(string[] orgmodelnames, SimPe.Interfaces.Files.IPackageFile pkg) 
		{
            if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Loading Parent Files");
			ArrayList names = new ArrayList();
			foreach (string s in orgmodelnames) names.Add(s);

			ArrayList types = new ArrayList();
			types.Add(Data.MetaData.MMAT);
			types.Add(Data.MetaData.TXMT);
			types.Add(Data.MetaData.TXTR);
			types.Add(Data.MetaData.LIFO);

			foreach (uint type in types) 
			{
				SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFiles(type);
				foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pfds)
				{
					if (pkg.FindFile(pfd)!=null) continue;

					SimPe.Interfaces.Files.IPackedFile file = package.Read(pfd);
					pfd.UserData = file.UncompressedData;

					//Update the modeName in the MMAT
					if ((pfd.Type==Data.MetaData.MMAT) && (names.Count>0))
					{
						SimPe.Plugin.MmatWrapper mmat = new MmatWrapper();
						mmat.ProcessData(pfd, package);

						string n = mmat.ModelName.Trim().ToLower();
						if (!n.EndsWith("_cres")) n+= "_cres";

						if (!names.Contains(n))
						{
							n = names[0].ToString();
							//n = n.Substring(0, n.Length-5);
							mmat.ModelName = n;
							mmat.SynchronizeUserData();
						}
					}

					pkg.Add(pfd);					
				}
			} //foreach type
		}

		/// <summary>
		/// Remove all Files that are referenced by a SHPE and belong to a subset as named in the esclude List
		/// </summary>
		/// <param name="exclude">List of subset names</param>
		/// <param name="modelnames">null or a List of allowed Modelnames. If a List is passed, only references to files 
		/// starting with one of the passed Modelnames will be keept</param>
		public void RemoveSubsetReferences(ArrayList exclude, string[] modelnames)
		{
            if (WaitingScreen.Running) WaitingScreen.UpdateMessage("Removing unwanted Subsets");
			//Build the ModelName List
			ArrayList mn = new ArrayList();
			if (modelnames!=null) 
			{				
				foreach (string s in modelnames) 
				{
					string n = s;
					if (s.EndsWith("_cres")) n=s.Substring(0, s.Length-5);
					mn.Add(n);
				}
			}

			bool deleted = false;
			Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFiles(Data.MetaData.SHPE);
			foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds) 
			{
				Rcol rcol = new GenericRcol(null, false);
				rcol.ProcessData(pfd, package);

				foreach (ShapePart p in ((Shape)rcol.Blocks[0]).Parts) 
				{					
					string s = p.Subset.Trim().ToLower();
					bool remove = exclude.Contains(s) ;

					if ((modelnames!=null) && !remove)
					{
						remove = true;
						string fl = p.FileName.Trim().ToLower();
						foreach (string n in mn) 
						{
							if (fl.StartsWith(n)) 
							{
								remove = false;
								continue;
							}
						}
					}
					
					if (remove) 
					{
						string n = p.FileName.Trim().ToLower();
						if (!n.EndsWith("_txmt")) n += "_txmt";

						ArrayList names = new ArrayList();
						Interfaces.Files.IPackedFileDescriptor[] rpfds = package.FindFile(n, Data.MetaData.TXMT);
						foreach (Interfaces.Files.IPackedFileDescriptor rpfd in rpfds) names.Add(rpfd);
						int pos = 0;
						while (pos<names.Count) 
						{
							Interfaces.Files.IPackedFileDescriptor rpfd = (Interfaces.Files.IPackedFileDescriptor)names[pos++];
							rpfd = package.FindFile(rpfd);
							
							if (rpfd!=null) 
							{
								rpfd.MarkForDelete = true;
								deleted = true;
								GenericRcol fl = new GenericRcol(null, false);
								fl.ProcessData(rpfd, package);
							
								Hashtable ht = fl.ReferenceChains;
								foreach(string k in ht.Keys) 
									foreach (Interfaces.Files.IPackedFileDescriptor lpfd in (ArrayList)ht[k]) 
										if (!names.Contains(lpfd)) names.Add(lpfd);								
							}
						} //while
					}
				} //foreach p
			} //foreach SHPE

			//now remova all deleted Files from the Index
			if (deleted) package.RemoveMarked();
		}
	}
}
