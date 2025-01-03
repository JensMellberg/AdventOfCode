﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Matrix<T>
	{
		public T[,] Array { get; set; }

		public int RowCount => this.Array.GetLength(1);

		public int ColumnCount => this.Array.GetLength(0);

		public Matrix(T[,] array)
		{
			this.Array = array;
		}

		public Matrix<T> Copy()
		{
			var newArray = new T[this.ColumnCount, this.RowCount];
			for (var x = 0; x < this.ColumnCount; x++)
			{
                for (var y = 0; y < this.RowCount; y++)
                {
					newArray[x, y] = this[x, y];
                }
            }

			return new Matrix<T>(newArray);
		}

        public override bool Equals(object obj)
        {
			if (!(obj is Matrix<T> matrix))
			{
				return false;
			}

			if (this.RowCount != matrix.RowCount || this.ColumnCount != matrix.ColumnCount)
			{
				return false;
			}


			var allValues = this.AllValues().ToList();
			var otherValues = matrix.AllValues().ToList();
			for (var i = 0; i < allValues.Count; i++)
			{
				if (!allValues[i].Equals(otherValues[i]))
				{
					return false;
				}
			}

			return true;
        }

        public override int GetHashCode()
        {
			return this.Array.GetHashCode();
        }

        public IEnumerable<IEnumerable<T>> GetRows()
		{
			for (var i = 0; i < this.RowCount; i++)
			{
				yield return GetRow(i);
			}
		}

		public IEnumerable<IEnumerable<T>> GetColumns()
		{
			for (var i = 0; i < this.ColumnCount; i++)
			{
				yield return GetColumn(i);
			}
		}

		public IEnumerable<T> AllValues()
		{
			foreach (var row in GetRows())
			{
				foreach (var cell in row)
				{
					yield return cell;
				}
			}
		}

		public IEnumerable<T> GetRow(int row)
		{
			for (var i = 0; i < this.ColumnCount; i++)
			{
				yield return this.Array[i, row];
			}
		}

        public IEnumerable<(int x, int y)> GetAdjacentCoordinates(int x, int y)
		{
            if (this.IsInBounds(x - 1, y))
            {
                yield return (x - 1, y);
            }

            if (this.IsInBounds(x + 1, y))
            {
                yield return (x + 1, y);
            }

            if (this.IsInBounds(x, y + 1))
            {
                yield return (x, y + 1);
            }

            if (this.IsInBounds(x, y - 1))
            {
                yield return (x, y - 1);
            }
        }


        public IEnumerable<(int x, int y)> GetAdjacentCoordinatesDiagonally(int x, int y)
		{
            if (this.IsInBounds(x - 1, y - 1))
            {
                yield return (x - 1, y - 1);
            }

            if (this.IsInBounds(x - 1, y + 1))
            {
                yield return (x - 1, y + 1);
            }

            if (this.IsInBounds(x + 1, y + 1))
            {
                yield return (x + 1, y + 1);
            }

            if (this.IsInBounds(x + 1, y - 1))
            {
                yield return (x + 1, y - 1);
            }

			foreach (var (x2, y2) in GetAdjacentCoordinates(x, y))
			{
				yield return (x2, y2);
			}
        }

		public bool IsInBoundsX(int x) => x >= 0 && x < this.ColumnCount;

        public bool IsInBoundsY(int y) => y >= 0 && y < this.RowCount;

		public bool IsInBounds(int x, int y) => this.IsInBoundsX(x) && this.IsInBoundsY(y);

		public IEnumerable<T> GetColumn(int column)
		{
			for (var i = 0; i < this.RowCount; i++)
			{
				yield return this.Array[column, i];
			}
		}

		public T this[int x, int y]
		{
			get => this.Array[x, y];
			set => this.Array[x, y] = value;
		}

		public string ToString(Func<T, string> stringify, string separator)
		{
			var result = "";
			foreach (var row in GetRows())
			{
				if (result != "")
				{
					result += '\n';
				}

				var isFirst = true;
				foreach (var cell in row)
				{
					result += isFirst ? "" : separator;
					isFirst = false;
					result += stringify(cell);
				}
			}

			return result;
		}

		public (int x, int y) Find(T value)
		{
			for (var i = 0; i < this.ColumnCount; i++)
			{
				for (var f = 0; f < this.RowCount; f++)
				{
					if (Array[i, f].Equals(value))
					{
						return (i, f);
					}
				}
			}

			return (-1, -1);
		}

		public override string ToString()
		{
			return this.ToString(x => x.ToString(), ",");
		}
	}

	public static class Matrix
	{
		public static Matrix<F> InitWithStartValue<F>(int rows, int columns, F startValue) => InitWithStartValue(rows, columns, () => startValue);
		public static Matrix<F> InitWithStartValue<F>(int rows, int columns, Func<F> startValue)
		{
			var matrix = new F[columns, rows];
			for (var c = 0; c < columns; c++)
			{
				for (var r = 0; r < rows; r++)
				{
					matrix[c, r] = startValue();
				}
			}

			return new Matrix<F>(matrix);
		}

		public static Matrix<F> FromTestInput<F>(IEnumerable<string> testInput, Func<char, object> Converter)
		{
			var list = testInput.ToList();
			var matrix = new F[list[0].Length, list.Count];
			for (var i = 0; i < list.Count; i++)
			{
				for (var x = 0; x < list[0].Length; x++)
				{
					matrix[x, i] = (F)Converter(list[i][x]);
				}
			}

			return new Matrix<F>(matrix);
		}
		public static Matrix<F> FromTestInput<F>(IEnumerable<string> testInput)
		{
			return FromTestInput<F>(testInput, ConvertToType);

			object ConvertToType(char s)
			{
				if (typeof(F) == typeof(int))
				{
					return (int)(s - '0');
				}
				else if (typeof(F) == typeof(char))
				{
					return s;
				}

				throw new Exception("T needs to be of type char or int");
			}
		}
	}
}
