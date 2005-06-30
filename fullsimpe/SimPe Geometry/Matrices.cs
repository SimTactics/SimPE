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
using System.Globalization;
using System.Collections;

namespace SimPe.Geometry
{
	/// <summary>
	/// Zusammenfassung f�r Matrices.
	/// </summary>
	public class Matrixd
	{
		double[][] m;
		/// <summary>
		/// Representation of a Matrix
		/// </summary>
		/// <param name="col">Number of Columns</param>
		/// <param name="row">Number of Rows</param>
		/// <remarks>Minimum is a 1x1 (rowxcol)Matrix</remarks>
		public Matrixd(int row, int col)
		{
			m = new double[row][];
			for (int i=0; i<row; i++) 
				m[i] = new double[col];
		}

		/// <summary>
		/// Create a new 3x1 Matrix
		/// </summary>
		/// <param name="v">the vecotor that should be represented as a Matrix</param>
		public Matrixd(Vector3f v) : this(3, 1)
		{
			this[0,0] = v.X;
			this[1,0] = v.Y;
			this[2,0] = v.Z;
		}

		/// <summary>
		/// Returns the Vector stored in this matrix or null if not possible!
		/// </summary>
		/// <returns></returns>
		public Vector3f GetVector()
		{
			if ((Rows!=3 || Columns!=1) && ((Rows!=1 || Columns!=3)))return null;
			if (Rows==3) return new Vector3f(m[0][0], m[1][0], m[2][0]);
			return new Vector3f(m[0][1], m[0][1], m[0][2]);
		}

		/// <summary>
		/// Create the Transpose of this Matrix
		/// </summary>
		public Matrixd GetTranspose()
		{
			Matrixd res = new Matrixd(Rows, Columns);
			for (int r=0; r<Rows; r++) 
				for (int c=0; c<Columns; c++) 
					res[c, r] = this[r, c];				

			return res;
		}

		/// <summary>
		/// Create an identity Mareix
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public static Matrixd GetIdentity(int row, int col)
		{
			Matrixd i = new Matrixd(row, col);
			
			for (int r=0; r<row; r++) 
				for (int c=0; c<col; c++) 
				{
					if (r==c) i[r, c] = 1;
					else i[r, c] = 0;
				}

			return i;
		}

		/// <summary>
		/// Number of stored Rows
		/// </summary>
		public int Rows 
		{
			get { return m.Length; }
		}

		/// <summary>
		/// Numbner of stored Columns
		/// </summary>
		public int Columns 
		{
			get {
				if (Rows==0) return 0;
				return m[0].Length; 
			}
		}

		/// <summary>
		/// Integer Indexer (row, column)
		/// </summary>
		public double this[int row, int col]
		{
			get { return m[row][col]; }
			set { m[row][col] = value;; }
		}		

		/// <summary>
		/// Matirx Multiplication
		/// </summary>
		/// <param name="m1">First Matrix</param>
		/// <param name="m2">Second Matrix</param>
		/// <returns>The resulting Matrix</returns>
		/// <exception cref="GeometryException">Thrown if Number of Rows in m1 is not equal to Number of Columns in m2</exception>
		public static Matrixd operator *(Matrixd m1, Matrixd m2) 
		{
			if (m1.Columns!=m2.Rows) throw new GeometryException("Unable to multiplicate Matrices (" + m1.ToString()+" * "+m2.ToString()+")");

			Matrixd m = new Matrixd(m1.Rows, m2.Columns);
			for (int r=0; r<m.Rows; r++) 
				for (int c=0; c<m.Columns; c++)
				{
					double res = 0;
 
					for (int i=0; i<m1.Columns; i++)
						res += m1[r, i] * m2[i, c];					

					m[r, c] = res;
				}//for m
			return m;
		}

		/// <summary>
		/// Scalar Matirx Multiplication
		/// </summary>
		/// <param name="m1">First Matrix</param>
		/// <param name="d">a Scalar</param>
		/// <returns>The resulting Matrix</returns>
		public static Matrixd operator *(Matrixd m1, double d) 
		{
			Matrixd m = new Matrixd(m1.Rows, m1.Columns);
			for (int r=0; r<m.Rows; r++) 
				for (int c=0; c<m.Columns; c++)
					m[r, c] *= m1[r, c] * d;
			return m;
		}

		/// <summary>
		/// Scalar Matirx Multiplication
		/// </summary>
		/// <param name="m1">First Matrix</param>
		/// <param name="d">a Scalar</param>
		/// <returns>The resulting Matrix</returns>
		public static Matrixd operator *(double d, Matrixd m1) 
		{
			return m1*d;
		}

		/// <summary>
		/// Scalar Matirx Multiplication
		/// </summary>
		/// <param name="m1">First Matrix</param>
		/// <param name="d">a Scalar</param>
		/// <returns>The resulting Matrix</returns>
		/// <exception cref="GeometryException">Thrown if User did Attempt to divide By Zero</exception>
		public static Matrixd operator /(Matrixd m1, double d) 
		{			
			if (d==0) throw new GeometryException("Unable to divide by Zero.");
			Matrixd m = new Matrixd(m1.Rows, m1.Columns);
			for (int r=0; r<m.Rows; r++) 
				for (int c=0; c<m.Columns; c++)
					m[r, c] = m1[r, c] / d;
			return m;
		}

		/// <summary>
		/// Scalar Matirx Multiplication
		/// </summary>
		/// <param name="m1">First Matrix</param>
		/// <param name="d">a Scalar</param>
		/// <returns>The resulting Matrix</returns>
		public static Matrixd operator /(double d, Matrixd m1) 
		{
			return m1/d;
		}

		/// <summary>
		/// Calculates the n-th Power
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		/// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
		/// <exception cref="GeometryException">Thrown if this is not a Square Matrix</exception>
		public static Matrixd operator ^(Matrixd m1, float val)
		{
			
			if(m1.Rows != m1.Columns)
				throw new GeometryException("Attempt to find the power of a non square matrix");
			//return null;

			Matrixd result = m1;

			for(int i = 0; i < val; i++)
				result = result * m1;			

			return result;
		}

		/// <summary>
		/// Adds two matrices
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		/// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
		/// <exception cref="GeometryException">Thrown if the MAtrices have diffrent Sizes</exception>
		public static Matrixd operator +(Matrixd m1, Matrixd m2)
		{

			if(m1.Rows != m2.Rows || m1.Columns != m2.Columns)
				throw new GeometryException("Attempt to add matrixes of different sizes.");
			//return null;

			Matrixd result = new Matrixd(m1.Rows,m1.Columns);

			for(int i = 0; i < m1.Rows; i++)
				for(int j = 0; j < m1.Columns; j++)				
					result[i,j] = m1[i,j] + m2[i,j];

			return result;
		}

		/// <summary>
		/// Substract two Matrices
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		/// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>		
		/// <exception cref="GeometryException">Thrown if the MAtrices have diffrent Sizes</exception>
		public static Matrixd operator -(Matrixd m1, Matrixd m2)
		{

			if(m1.Rows != m2.Rows || m1.Columns != m2.Columns)
				throw new GeometryException("Attempt to subtract matrixes of different sizes.");
			//return null;

			Matrixd result = new Matrixd(m1.Rows,m1.Columns);

			for(int i = 0; i < m1.Rows; i++)			
				for(int j = 0; j < m1.Columns; j++)				
					result[i,j] = m1[i,j] - m2[i,j];

			return result;
		}

		/// <summary>
		/// Create the Inverse of a Matrix
		/// </summary>
		/// <param name="m1">The Matrix you want to Invert</param>
		/// <returns>The inverted matrix</returns>
		public static Matrixd operator !(Matrixd m1)
		{
			return m1.GetInverse();
		}

		/// <summary>
		/// Returns the Inverse of this Quaternion
		/// </summary>
		/// <returns>The Inverted Matrix</returns>
		/// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
		/// <exception cref="GeometryException">Thrown if the Matrix is Singular (<see cref="Determinant"/>==0)</exception>		
		public Matrixd GetInverse()
		{
			if(Determinant() == 0)
					throw new GeometryException("Attempt to invert a singular matrix.");				

				//inverse of a 2x2 matrix
				if(Rows == 2 && Columns == 2)
				{
					Matrixd tempMtx = new Matrixd(2,2);

					tempMtx[0,0] = this[1,1];
					tempMtx[0,1] = -this[0,1];
					tempMtx[1,0] = -this[1,0];
					tempMtx[1,1] = this[0,0];

					return tempMtx / Determinant();
				}

				return Adjoint()/Determinant();
		}

		#region Overrides
		/// <summary>
		/// Return a String describing the Matrix
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Rows+"x"+Columns+"-Matrix";
		}

		/// <summary>
		/// Calculate a new HashCode describing the instance
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			double result = 0;

			for(int i = 0; i < Rows; i++)
				for(int j = 0; j < Columns; j++)
					result += this[i,j];
				
			return (int)result;
		}

		/// <summary>
		/// Does the passed Object contain Equal Data?
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(Object obj)
		{
			if (obj.GetType()!=typeof(Matrixd)) return false;

			Matrixd m1 = (Matrixd)obj;
			if(this.Rows != m1.Rows || this.Columns != m1.Columns)
				return false;

			for(int i = 0; i < this.Rows; i++)
				for(int j = 0; j < this.Columns; j++)
					if(this[i,j] != m1[i,j])
						return false;

			return true;
		}
		#endregion


		/// <summary>
		/// SMatirx Multiplication
		/// </summary>
		/// <param name="m1">First Matrix</param>
		/// <param name="v">Vector</param>
		/// <returns>The resulting Vector</returns>
		public static Vector3f operator *(Matrixd m1, Vector3f v) 
		{
			Matrixd m2 = new Matrixd(v);
			m2 = m1 * m2;
			return m2.GetVector();
		}

		/// <summary>
		/// Are tow Matrixces equal
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static bool operator == (Matrixd m1, Matrixd m2)
		{
			return Equals(m1, m2);
		}

		/// <summary>
		/// Are two Matrices inequal?
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static bool operator != (Matrixd m1, Matrixd m2)
		{
			return !(m1 == m2);
		}		

		/// <summary>
		/// Calculate the determinant of a Matrix
		/// </summary>
		/// <returns>The determinant</returns>
		/// <exception cref="GeometryException">Thrown, if the Matrix is not a Square Matrix</exception>
		/// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
		public double Determinant()
		{
			if(this.Rows != this.Columns)
				throw new GeometryException("You can only compute the Determinant of a Square Matrix. (" + ToString() + ")");

			double d = 0;

			//get the determinent of a 2x2 matrix
			if(this.Rows == 2 && this.Columns == 2)
			{
				d = (this[0,0] * this[1,1]) - (this[0,1] * this[1,0]);   
				return d;
			}

			//get the determinent of a 3x3 matrix
			if(this.Rows == 3 && this.Columns == 3)
			{
				d = (this[0,0] * this[1,1] * this[2,2]) 
					+ (this[0,1] * this[1,2] * this[2,0])
					+ (this[0,2] * this[1,0] * this[2,2])
					- (this[0,2] * this[1,1] * this[2,0])
					- (this[0,1] * this[1,0] * this[2,2])
					- (this[0,0] * this[1,2] * this[2,1]);   
				return d;
			}

			Matrixd tempMtx = new Matrixd(this.Rows - 1, this.Columns - 1);
 
			//find the determinent with respect to the first row
			for(int j = 0; j < this.Columns; j++)
			{				
				tempMtx = this.Minor(0, j);

				//recursively add the determinents
				d += (int)Math.Pow(-1, j) * this[0,j] * tempMtx.Determinant();
				
			}

			return d;
		}

		/// <summary>
		/// Calculate the Adjoint of a Matrix
		/// </summary>
		/// <returns>The adjoint</returns>
		/// <exception cref="GeometryException">Thrown if <see cref="Rows"/> or <see cref="Columns"/> is less than 2.</exception>
		/// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
		public Matrixd Adjoint()
		{

			if(this.Rows < 2 || this.Columns < 2)
				throw new GeometryException("Adjoint matrix is not available. (" + ToString() + ")");

			Matrixd tempMtx = new Matrixd(this.Rows-1 , this.Columns-1);
			Matrixd adjMtx = new Matrixd (this.Columns , this.Rows);

			for(int i = 0; i < this.Rows; i++)
				for(int j = 0; j < this.Columns;j++)
				{
					tempMtx = this.Minor(i, j);

					//put the determinent of the minor in the transposed position
					adjMtx[j,i] = (int)Math.Pow(-1,i+j) * tempMtx.Determinant();
				}

			return adjMtx;
		}

		/// <summary>
		/// Create the Minor/cofactor Matrix
		/// </summary>
		/// <param name="row">row that should be removed<</param>
		/// <param name="column">column that should be removed</param>
		/// <exception cref="GeometryException">Thrown if <see cref="Rows"/> or <see cref="Columns"/> is less than 2.</exception>
		/// <returns>The Minor Matrix for the given row/column</returns>
		/// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
		public Matrixd  Minor(int row, int column)
		{

			if(this.Rows < 2 || this.Columns < 2)
				throw new GeometryException("Minor not available. (" + ToString() + ")");

			int i, j = 0;

			Matrixd minom2 = new Matrixd(this.Rows - 1, this.Columns - 1);
 
			//find the minor with respect to the first element
			for(int k = 0; k < minom2.Rows; k++)
			{
				if(k >= row)
					i = k + 1;
				else
					i = k;

				for(int l = 0; l < minom2.Columns; l++)
				{
					if(l >= column)
						j = l + 1;
					else
						j = l;

					minom2[k,l] = this[i,j];
				}
			}

			return minom2;
		}

		/// <summary>
		/// Returns true, if this is the identity matrix
		/// </summary>
		/// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
		public bool Identity
		{
			get 
			{
				if(Rows != Columns)
					return false;

				for(int i = 0; i < Rows; i++)
					for(int j = 0; j < Columns; j++)
					{
						if(i == j)
						{
							if(this[i,j] != 1.0f) return false;
						}
						else
						{
							if(this[i,j] != 0.0f) return false;
						}
					}

				return true;
			}
		}

		/// <summary>
		/// True if this Matrix is invertibale
		/// </summary>
		public bool Invertable
		{
			get { return (this.Determinant() != 0);	}
		}

		/// <summary>
		/// True if the Matrix is Orthogonal
		/// </summary>
		public bool Orthogonal 
		{
			get 
			{
				if (Rows!=Columns) return false;

				Matrixd t = this * this.GetTranspose();
				if (!t.Identity) return false;

				t = this.GetTranspose() * this;
				if (!t.Identity) return false;

				return true;
			}
		}
	}
}