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
using System.Reflection;

namespace SimPe
{
	/// <summary>
	/// This is the default descriptive Serializer
	/// </summary>
	public class CsvSerializer : AbstractSerializer
	{
		public CsvSerializer() : base()
		{
			
		}

		public override string Seperator 
		{
			get { return  ","; }
		}

		public override string SaveStr(string val)
		{
			return val.Replace(",", ";").Replace("\n", " ");
		}

		public override string SubProperty(string name, string val)
		{
			if (val==null) val="";
			return val;
		}

		public override string Property(string name, string val)
		{
			if (val==null) val="";
			return SaveStr(val);
		}

		public override string NullProperty(string name)
		{
			return "NULL";
		}				

	}
}
