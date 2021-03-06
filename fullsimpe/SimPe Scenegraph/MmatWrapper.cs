using System;
using System.Collections;

namespace SimPe.Plugin
{
	/// <summary>
	/// Zusammenfassung f�r MmatWrapper.
	/// </summary>
	public class MmatWrapper : SimPe.PackedFiles.Wrapper.Cpf, SimPe.Interfaces.Scenegraph.IScenegraphBlock, SimPe.Interfaces.Scenegraph.IScenegraphItem
	{
		static SimPe.PackedFiles.UserInterface.CpfUI.ExecutePreview prev;
		public static SimPe.PackedFiles.UserInterface.CpfUI.ExecutePreview GlobalCpfPreview
		{
			get {return prev;}
			set {prev=value;}
		}
		#region IScenegraphBlock Member

		public void ReferencedItems(System.Collections.Hashtable refmap, uint parentgroup)
		{
			ArrayList list = new ArrayList();
			string name = this.GetSaveItem("modelName").StringValue.Trim();
			if (!name.ToLower().EndsWith("_cres")) name += "_cres";
			list.Add(SimPe.Plugin.ScenegraphHelper.BuildPfd(name, SimPe.Plugin.ScenegraphHelper.CRES, parentgroup));			
			refmap["CRES"] = list;

			list = new ArrayList();
			name = this.GetSaveItem("name").StringValue.Trim();
			if (!name.ToLower().EndsWith("_txmt")) name += "_txmt";
			list.Add(SimPe.Plugin.ScenegraphHelper.BuildPfd(name, SimPe.Plugin.ScenegraphHelper.TXMT, parentgroup));			
			refmap["TXMT"] = list;
		}

		/// <summary>
		/// Registers the Object in the given Hashtable
		/// </summary>
		/// <param name="listing"></param>
		/// <returns>The Name of the Class Type</returns>
		public string Register(Hashtable listing)
		{
			return "";
		}
		#endregion

		protected override SimPe.Interfaces.Plugin.IPackedFileUI CreateDefaultUIHandler()
		{
			return new SimPe.PackedFiles.UserInterface.CpfUI(prev);
		}

		/// <summary>
		/// Returns a Human Readable Description of this Wrapper
		/// </summary>
		/// <returns>Human Readable Description</returns>
		protected override SimPe.Interfaces.Plugin.IWrapperInfo CreateWrapperInfo()
		{
			return new SimPe.Interfaces.Plugin.AbstractWrapperInfo(
				"MMAT Wrapper",
				"Quaxi",
				"This File describes a ColorOption for a Mesh Group / Subset. It is needed to provide an additional Color for Objects.",
				4,
				System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.mmat.png"))
				);   
		}

		/// <summary>
		/// Returns a list of File Type this Plugin can process
		/// </summary>
		public override uint[] AssignableTypes
		{
			get
			{
				uint[] types = {
								   0x4C697E5A //MMAT
							   };
			
				return types;
			}
		}

		/// <summary>
		/// Load and return the referenced CRES File (null if none was available)
		/// </summary>
		/// <remarks>
		/// You should store this value in a temp var if you need it multiple times, 
		/// as the File is reloaded with each call
		/// </remarks>
		public GenericRcol CRES 
		{
			get 
			{
				Hashtable refs = this.ReferenceChains;
				ArrayList cress = (ArrayList)refs["CRES"];
				if (cress!=null) 
				{
					if (cress.Count>0) 
					{
						Interfaces.Files.IPackedFileDescriptor pfd = package.FindFile((Interfaces.Files.IPackedFileDescriptor)cress[0]);
						if (pfd==null) //fallback code
						{
							Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFile(((Interfaces.Files.IPackedFileDescriptor)cress[0]).Filename, Data.MetaData.CRES);
							if (pfds.Length>0) pfd = pfds[0];
						}

						if (pfd!=null) 
						{
							GenericRcol cres = new GenericRcol(null, false);
							cres.ProcessData(pfd, package);

							return cres;
						}

						if (pfd==null) //FileTable fallback code
						{
							Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFileDiscardingGroup((Interfaces.Files.IPackedFileDescriptor)cress[0]);
							if (items.Length>0) 
							{
								GenericRcol cres = new GenericRcol(null, false);
								cres.ProcessData(items[0].FileDescriptor, items[0].Package);

								return cres;
							}
						}
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Load and return the referenced TXMT File (null if none was available)
		/// </summary>
		/// <remarks>
		/// You should store this value in a temp var if you need it multiple times, 
		/// as the File is reloaded with each call
		/// </remarks>
		public GenericRcol TXMT 
		{
			get 
			{
				Hashtable refs = this.ReferenceChains;
				ArrayList txmts = (ArrayList)refs["TXMT"];
				if (txmts!=null) 
				{
					if (txmts.Count>0) 
					{
						Interfaces.Files.IPackedFileDescriptor pfd = package.FindFile((Interfaces.Files.IPackedFileDescriptor)txmts[0]);
						if (pfd==null) //fallback code
						{
							Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFile(((Interfaces.Files.IPackedFileDescriptor)txmts[0]).Filename, Data.MetaData.TXMT);
							if (pfds.Length>0) pfd = pfds[0];
						}

						if (pfd!=null) 
						{
							GenericRcol txmt = new GenericRcol(null, false);
							txmt.ProcessData(pfd, package);

							return txmt;
						}

						if (pfd==null) //FileTable fallback code
						{
							Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFileDiscardingGroup((Interfaces.Files.IPackedFileDescriptor)txmts[0]);
							if (items.Length>0) 
							{
								GenericRcol txmt = new GenericRcol(null, false);
								txmt.ProcessData(items[0].FileDescriptor, items[0].Package);

								return txmt;
							}
						}
					}
				}
			return null;
			}
		}

		/// <summary>
		/// Load a Texture belonging to a TXMT
		/// </summary>
		/// <param name="txmt">a valid txmt</param>
		/// <returns>the Texture or null</returns>
		public GenericRcol GetTxtr(GenericRcol txmt) 
		{
			if (txmt==null) return null;
			Hashtable refs = txmt.ReferenceChains;
			ArrayList txtrs = (ArrayList)refs["stdMatBaseTextureName"];//["TXTR"];
			if (txtrs!=null) 
			{
				if (txtrs.Count>0) 
				{
					Interfaces.Files.IPackedFileDescriptor pfd = package.FindFile((Interfaces.Files.IPackedFileDescriptor)txtrs[0]);
					if (pfd==null) //fallback code
					{
						Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFile(((Interfaces.Files.IPackedFileDescriptor)txtrs[0]).Filename, Data.MetaData.TXTR);
						if (pfds.Length>0) pfd = pfds[0];
					}							
					if (pfd!=null) 
					{
						GenericRcol txtr = new GenericRcol(null, false);
						txtr.ProcessData(pfd, package);

						return txtr;
					}

					if (pfd==null) //FileTable fallback code
					{
						Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFileDiscardingGroup((Interfaces.Files.IPackedFileDescriptor)txtrs[0]);
						if (items.Length>0) 
						{
							GenericRcol txtr = new GenericRcol(null, false);
							txtr.ProcessData(items[0].FileDescriptor, items[0].Package);

							return txtr;
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Load and return the referenced TXTR File (through the TXMT, null if none was available)
		/// </summary>
		/// <remarks>
		/// You should store this value in a temp var if you need it multiple times, 
		/// as the File is reloaded with each call
		/// </remarks>
		public GenericRcol TXTR 
		{
			get 
			{
				GenericRcol txmt = this.TXMT;
				return GetTxtr(txmt);
				
			}
		}

		protected void FindRcolr(Interfaces.Files.IPackedFileDescriptor pfd)
		{
		}

		protected GenericRcol GetGmdc()
		{
			GenericRcol rcol = CRES;
			if (rcol!=null)
			{
				Hashtable refs = rcol.ReferenceChains;
				ArrayList shps = (ArrayList)refs["Generic"];
				if (shps!=null)
					if (shps.Count>0)
					{
						Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFile((Interfaces.Files.IPackedFileDescriptor)shps[0], null);
						if (items.Length>0) 
						{
							GenericRcol shpe = new GenericRcol(null, false);
							shpe.ProcessData(items[0].FileDescriptor, items[0].Package);

							refs = shpe.ReferenceChains;
							ArrayList gmnds = (ArrayList)refs["Models"];
							if (gmnds!=null) 
								if (gmnds.Count>0)
								{
									items = FileTable.FileIndex.FindFile((Interfaces.Files.IPackedFileDescriptor)gmnds[0], null);
									if (items.Length>0) 
									{
										GenericRcol gmnd = new GenericRcol(null, false);
										gmnd.ProcessData(items[0].FileDescriptor, items[0].Package);

										refs = gmnd.ReferenceChains;
										ArrayList gmdcs = (ArrayList)refs["Generic"];
										if (gmdcs!=null) 
											if (gmdcs.Count>0)
											{
												items = FileTable.FileIndex.FindFile((Interfaces.Files.IPackedFileDescriptor)gmdcs[0], null);
												if (items.Length>0) 
												{
													GenericRcol gmdc = new GenericRcol(null, false);
													gmdc.ProcessData(items[0].FileDescriptor, items[0].Package);

													return gmdc;
												}
											}
									}
								}
						}
					}
			}

			return null;
		}

		/// <summary>
		/// Load and return the used GMDC File (through the CRES, null if none was available)
		/// </summary>
		/// <remarks>
		/// You should store this value in a temp var if you need it multiple times, 
		/// as the File is reloaded with each call
		/// </remarks>
		public GenericRcol GMDC 
		{
			get 
			{
				return GetGmdc();				
			}
		}

		public override string Description
		{
			get
			{
				string str = "objectGUID=0x"+Helper.HexString(this.ObjectGUID)+"; subset="+this.SubsetName+"; references=";
				Hashtable map = this.ReferenceChains;
				foreach (string s in map.Keys) 
				{
					str += s+":";
					foreach (Interfaces.Files.IPackedFileDescriptor pfd in (ArrayList)map[s])
					{
						str += pfd.Filename+" ("+pfd.ToString()+") | ";						
					}
					if (((ArrayList)map[s]).Count>0) str = str.Substring(0, str.Length-2);
					str += ",";
				}
				if (map.Count>0) str = str.Substring(0, str.Length-1);

				return str;
			}
		}

		#region IScenegraphItem Member

		public Hashtable ReferenceChains
		{
			get
			{
				Hashtable refmap = new Hashtable();
				this.ReferencedItems(refmap, this.FileDescriptor.Group);
				return refmap;
			}
		}

		#endregion

		#region Default Attribute
		public string Creator 
		{
			get { return this.GetSaveItem("creator").StringValue; }
			set { this.GetSaveItem("creator").StringValue = value; }
		}

		public bool DefaultMaterial 
		{
			get { return this.GetSaveItem("defaultMaterial").BooleanValue; }
			set { this.GetSaveItem("defaultMaterial").BooleanValue = value; }
		}

		public string Family 
		{
			get { return this.GetSaveItem("family").StringValue; }
			set { this.GetSaveItem("family").StringValue = value; }
		}

		public uint Flags 
		{
			get { return this.GetSaveItem("flags").UIntegerValue; }
			set { this.GetSaveItem("flags").UIntegerValue = value; }
		}

		public uint MaterialStateFlags 
		{
			get { return this.GetSaveItem("materialStateFlags").UIntegerValue; }
			set { this.GetSaveItem("materialStateFlags").UIntegerValue = value; }
		}

		public string ModelName 
		{
			get { return this.GetSaveItem("modelName").StringValue; }
			set { this.GetSaveItem("modelName").StringValue = value; }
		}

		public string Name 
		{
			get { return this.GetSaveItem("name").StringValue; }
			set { this.GetSaveItem("name").StringValue = value; }
		}

		public uint ObjectGUID 
		{
			get { return this.GetSaveItem("objectGUID").UIntegerValue; }
			set { this.GetSaveItem("objectGUID").UIntegerValue = value; }
		}

		public int ObjectStateIndex 
		{
			get { return this.GetSaveItem("objectStateIndex").IntegerValue; }
			set { this.GetSaveItem("objectStateIndex").IntegerValue = value; }
		}

		public string SubsetName 
		{
			get { return this.GetSaveItem("subsetName").StringValue; }
			set { this.GetSaveItem("subsetName").StringValue = value; }
		}
		#endregion		
	}
}
