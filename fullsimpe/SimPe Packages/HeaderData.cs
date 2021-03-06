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
using System.IO;
using System.ComponentModel;

namespace SimPe.Packages
{
	/// <summary>
	/// Structural Data of a .package Header
	/// </summary>
	public class HeaderData : Interfaces.Files.IPackageHeader, System.IDisposable
	{
		/// <summary>
		/// Constructor for the class
		/// </summary>
		internal HeaderData ()
		{
			lidl = false;
			index = new HeaderIndex(this);
			hole = new HeaderHole();
			id = new char[4];
			reserved_00 = new Int32[3];
			reserved_02 = new Int32[8];

			id[0] = 'D';
			id[1] = 'B';
			id[2] = 'P';
			id[3] = 'F';

			majorversion = 1;
			minorversion = 1;
			index.Type = 7;

			indextype = Data.MetaData.IndexTypes.ptLongFileIndex;
		}

		/// <summary>
		/// Identifier of the File
		/// </summary>
		internal char[] id;

		/// <summary>
		/// Returns the Identifier of the File
		/// </summary>
		/// <remarks>This value should be DBPF</remarks>
		[DescriptionAttribute("Package Identifier"), DefaultValueAttribute("DBPF")]		
		public string Identifier
		{
			get 
			{
				string ret = "";
				foreach (char c in id) ret += c;
				return ret;
			}
		}
		


		/// <summary>
		/// The Major Version (part before the .) of the Package File Format
		/// </summary>
		internal Int32 majorversion;

		/// <summary>
		/// Returns the Major Version of The Packages FileFormat
		/// </summary>
		/// <remarks>This value should be 1</remarks>
		[DescriptionAttribute("Major Version Number of this Package"), CategoryAttribute("Version"), DefaultValueAttribute(1)]		
		public Int32 MajorVersion
		{
			get
			{
				return majorversion;
			}
		}



		/// <summary>
		/// The Minor Version (part after the .) of the Package File Format
		/// </summary>
		internal Int32 minorversion;

		/// <summary>
		/// Returns the Minor Version of The Packages FileFormat 
		/// </summary>
		/// <remarks>This value should be 0 or 1</remarks>
		[DescriptionAttribute("Minor Version Number of this Package"), CategoryAttribute("Version"), DefaultValueAttribute(1)]		
		public Int32 MinorVersion
		{
			get
			{
				return minorversion;
			}
		}

		/// <summary>
		/// Returns the Overall Version of this Package
		/// </summary>
		[DescriptionAttribute("Overall Versionnumber of this Package"), CategoryAttribute("Version"), DefaultValueAttribute(4294967297)]		
		public long Version 
		{
			get 
			{
				return (long)((ulong)MajorVersion << 0x20) | (uint)MinorVersion;
			}
		}


		/// <summary>
		/// 3 dwords of reserved Data
		/// </summary>
#if DEBUG
		public Int32[] reserved_00;
#else
		internal Int32[] reserved_00;
#endif


		/// <summary>
		/// Createion Date of the File
		/// </summary>
#if DEBUG
		public uint created;

		[DescriptionAttribute("Creation Date of the Package"), CategoryAttribute("Debug"), ReadOnly(true)]						
		
#else
		public uint Ident
		{
			get {return Created;}
		}
		internal uint created;
		[DescriptionAttribute("Creation Date of the Package"), CategoryAttribute("Debug"), Browsable(false)]
#endif
		public uint Created 
		{
			get { return created; }
			set { created = value; }
		}

		/// <summary>
		/// Modification Date of the File
		/// </summary>
#if DEBUG
		public Int32 modified;

		[DescriptionAttribute("Modification Date of the Package"), CategoryAttribute("Debug")]						
		public Int32 Modified 
		{
			get { return modified; }
		}
#else
		internal Int32 modified;
#endif


		/// <summary>
		/// holds Index Informations stored in the Header
		/// </summary>
		internal HeaderIndex index;		

		/// <summary>
		/// Returns Index Informations stored in the Header
		/// </summary>
		[BrowsableAttribute(false)]
		public SimPe.Interfaces.Files.IPackageHeaderIndex Index
		{
			get 
			{
				return index;
			}
		}

		/// <summary>
		/// Holds Hole Index Informations stored in the Header
		/// </summary>
		internal HeaderHole hole;

		/// <summary>
		/// Returns Hole Index Informations stored in the Header
		/// </summary>
		[BrowsableAttribute(false)]
		public SimPe.Interfaces.Files.IPackageHeaderHoleIndex HoleIndex
		{
			get
			{
				return hole;
			}
		}


		/// <summary>
		/// Only available for versions >= 1.1
		/// </summary>
		private Data.MetaData.IndexTypes indextype;

		/// <summary>
		/// Returns or Sets the Type of the Package
		/// </summary>
		[DescriptionAttribute("The Indextype used in the Package. ptLongFileIndex allows the use of the \"Instance (high)\" Value."), DefaultValueAttribute(Data.MetaData.IndexTypes.ptLongFileIndex)]				
		public Data.MetaData.IndexTypes IndexType
		{
			get 
			{
				return (Data.MetaData.IndexTypes)indextype;				
			}
			set 
			{
				indextype = value;
			}
		}

		/// <summary>
		/// 8 dwords of reserved Data
		/// </summary>
#if DEBUG
		public Int32[] reserved_02;

		[DescriptionAttribute("Reserved Values"), CategoryAttribute("Debug")]						
		public Int32[] Reserved 
		{
			get { return reserved_02; }
		}
#else
		internal Int32[] reserved_02;
#endif


		/// <summary>
		/// true if the version is greater or equal than 1.1
		/// </summary>
		[BrowsableAttribute(false)]
		public bool IsVersion0101
		{
			get 
			{
				return (Version>=0x100000001);//((majorversion>1) || ((majorversion==1) && (minorversion>=1)));
			}
		}

		bool lidl;
		internal bool LockIndexDuringLoad
		{
			get {return lidl;}
			set {lidl = value;}
		}


		#region File Processing Methods
        static string spore = "\r\n\r\nSimPe is a package editor for Sims2 packages."
            + "\r\nSpore packages are NOT supported.";
		/// <summary>
		/// Initializes the Structure from a BinaryReader
		/// </summary>
		/// <param name="reader">The Reader representing the Package File</param>
		/// <remarks>Reader must be on the correct Position since no Positioning is performed</remarks>
		internal void Load(BinaryReader reader)
		{
			//this.id = new char[4];
			for (uint i=0; i<this.id.Length; i++) this.id[i] = reader.ReadChar();
            if (!Helper.AnyPackage && Identifier != "DBPF")
                throw new InvalidOperationException("SimPe does not support this type of file." + spore);
			
			this.majorversion = reader.ReadInt32();
            if (!Helper.AnyPackage && this.majorversion > 1)
                throw new InvalidOperationException("SimPe does not support this version of DBPF file." + spore);

			this.minorversion = reader.ReadInt32();

			//this.reserved_00 = new Int32[3];
			for (uint i=0; i<this.reserved_00.Length; i++) this.reserved_00[i] = reader.ReadInt32();

			this.created = reader.ReadUInt32();
			this.modified = reader.ReadInt32();
			
			
			this.index.type = reader.ReadInt32();
			if (!lidl)
			{
				this.index.count = reader.ReadInt32();
				this.index.offset = reader.ReadUInt32();
				this.index.size = reader.ReadInt32();
			} 
			else 
			{
				reader.ReadInt32(); reader.ReadInt32(); reader.ReadInt32(); //count, offset, size
			}

			this.hole.count = reader.ReadInt32();
			this.hole.offset = reader.ReadUInt32();
			this.hole.size = reader.ReadInt32();

			if (IsVersion0101) this.indextype = (Data.MetaData.IndexTypes)reader.ReadUInt32();

			//this.reserved_02 = new Int32[8];
			for (uint i=0; i<this.reserved_02.Length; i++) this.reserved_02[i] = reader.ReadInt32();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <remarks>Writer must be on the correct Position since no Positioning is performed</remarks>
		internal void Save(BinaryWriter writer) 
		{
			for (uint i=0; i<this.id.Length; i++) writer.Write(this.id[i]);
			
			writer.Write(this.majorversion);
			writer.Write(this.minorversion);

			for (uint i=0; i<this.reserved_00.Length; i++) writer.Write(this.reserved_00[i]);

			writer.Write(this.created );
			writer.Write(this.modified);

			writer.Write(this.index.type);
			writer.Write(this.index.count);
			writer.Write(this.index.offset);
			writer.Write(this.index.size);

			writer.Write(this.hole.count);
			writer.Write(this.hole.offset);
			writer.Write(this.hole.size);

			if (IsVersion0101) writer.Write((uint)this.IndexType);

			for (uint i=0; i<this.reserved_02.Length; i++) writer.Write(this.reserved_02[i]);
		}
		#endregion

		#region IDisposable Member

		public void Dispose()
		{
			this.hole = null;
			this.index = null;
			this.reserved_00 = null;
			this.reserved_02 = null;
			this.id = null;
		}

		#endregion

		public object Clone()
		{
			HeaderData iph = new HeaderData();
			iph.created = this.created;
			iph.id = this.id;
			iph.indextype = this.indextype;
			iph.majorversion = this.majorversion;
			iph.minorversion = this.minorversion;
			iph.modified = this.modified;

			iph.reserved_00 = this.reserved_00;
			iph.reserved_02 = this.reserved_02;
			
			return (SimPe.Interfaces.Files.IPackageHeader)iph;
		}
	}
}
