namespace SMatrix
{
    /// <summary>
    /// wrapper for double[,] with arithmetical and matrix-specific operations.
    /// Most of the methods are pure, i.e. they return a new instance
    /// instead of modifying the one they are called on.
    /// </summary>
    public class Matrix
    {
        private readonly double[,] _content;

        #region Contructors

        /// <summary>
        /// NxM matrix of zeros.
        /// </summary>
        /// <param name="n">Number of rows</param>
        /// <param name="m">Number of columns</param>
        public Matrix(int n, int m)
        {
            _content = new double[n, m];
        }

        /// <summary>
        /// Square NxN matrix of zeros.
        /// </summary>
        /// <param name="n">Number of rows and columns</param>
        public Matrix(int n) : this(n, n)
        { }

        /// <summary>
        /// NxM matrix, filled via <paramref name="filler"/>
        /// </summary>
        /// <param name="n">Number of rows</param>
        /// <param name="m">Number of columns</param>
        /// <param name="filler">Function, that returns value for each position (row, column)</param>
        public Matrix(int n, int m, Func<int, int, double> filler) : this(n, m)
        {
            for (var i = 0; i < Rows; ++i)
            {
                for (var j = 0; j < Columns; ++j)
                    _content[i, j] = filler(i, j);
            }
        }

        /// <summary>
        /// Square NxN matrix, filled via <paramref name="filler"/>
        /// </summary>
        /// <param name="n">Number of rows and columns</param>
        /// <param name="filler">Function, that returns value for each position (row, column)</param>
        public Matrix(int n, Func<int, int, double> filler) : this(n, n, filler)
        { }

        /// <summary>
        /// NxM matrix, filled with <paramref name="value"/>
        /// </summary>
        /// <param name="n">Number of rows</param>
        /// <param name="m">Number of columns</param>
        /// <param name="value">Value to fill the matrix with</param>
        public Matrix(int n, int m, double value) : this(n, m, (_, _) => value)
        { }

        /// <summary>
        /// Square NxN matrix, filled with <paramref name="value"/>
        /// </summary>
        /// <param name="n">Number of rows and columns</param>
        /// <param name="value">Value to fill the matrix with</param>
        public Matrix(int n, double value) : this(n, n, value)
        { }

        /// <summary>
        /// Matrix from two-dimensional array
        /// </summary>
        /// <param name="matrix"></param>
        public Matrix(double[,] matrix) : this(matrix.GetLength(0), matrix.GetLength(1), (i, j) => matrix[i, j])
        { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Number of rows of this matrix, i.e. the size of the first dimension.
        /// </summary>
        public int Rows => _content.GetLength(0);

        /// <summary>
        /// Number of columns of this matrix, i.e. the size of the second dimension.
        /// </summary>
        public int Columns => _content.GetLength(1);

        /// <summary>
        /// Get or set the value at specific position
        /// </summary>
        /// <param name="i">Row index</param>
        /// <param name="j">Column index</param>
        public double this[int i, int j]
        {
            get => _content[i, j];
            set => _content[i, j] = value;
        }

        #endregion

        #region Named Contstructors

        /// <summary>
        /// NxM matrix, filled with 1.
        /// </summary>
        /// <param name="n">Number of rows</param>
        /// <param name="m">Number of columns</param>
        public static Matrix Ones(int n, int m)
        {
            return new Matrix(n, m, 1);
        }

        /// <summary>
        /// Square NxN matrix, filled with 1.
        /// </summary>
        /// <param name="n">Number of rows and columns</param>
        public static Matrix Ones(int n)
        {
            return Ones(n, n);
        }

        /// <summary>
        /// NxM matrix, where each element on the major diagonal is 1, and the rest are 0.
        /// Values at [i, j] where i or j >= max(n, m) are 0 as well.
        /// </summary>
        /// <param name="n">Number of rows</param>
        /// <param name="m">Number of columns</param>
        public static Matrix Identity(int n, int m)
        {
            return new Matrix(n, n, (i, j) => Convert.ToDouble(i == j));
        }

        /// <summary>
        /// Square NxN matrix, where each element on the major diagonal is 1, and the rest are 0.
        /// </summary>
        /// <param name="n">Number of rows and columns</param>
        public static Matrix Identity(int n)
        {
            return Identity(n, n);
        }

        /// <summary>
        /// NxM matrix, where the value at [i, j] is 1, and the rest are 0.
        /// </summary>
        /// <param name="n">Number of rows</param>
        /// <param name="m">Number of columns</param>
        /// <param name="i">Row index of 1</param>
        /// <param name="j">Columns index of 1</param>
        public static Matrix Unit(int n, int m, int i, int j)
        {
            var matrix = new Matrix(n, m) { [i, j] = 1 };
            return matrix;
        }

        #endregion

        #region IO

        /// <summary>
        /// Write this.ToString() to the console.
        /// </summary>
        public void WriteToConsole()
        {
            Write(Console.Out);
        }

        /// <summary>
        /// Write this.ToString() to a file
        /// </summary>
        /// <param name="filename">Destination file</param>
        public void WriteToFile(string filename)
        {
            using var writer = new StreamWriter(filename);
            Write(writer);
        }

        private void Write(TextWriter writer)
        {
            writer.WriteLine($"{Rows} {Columns}");
            writer.WriteLine(ToString());
        }

        /// <summary>
        /// Read a matrix from the console
        /// </summary>
        public static Matrix ReadFromConsole()
        {
            Console.Write("Enter the number of rows and columns: ");
            return Read(Console.In);
        }

        /// <summary>
        /// Deserialize a matrix from a file.
        /// Expects the shape of the matrix in the format "Rows Columns" as the first row.
        /// </summary>
        /// <param name="filename"></param>
        public static Matrix ReadFromFile(string filename)
        {
            using var reader = new StreamReader(filename);
            return Read(reader);
        }

        private static Matrix Read(TextReader reader)
        {
            int[] size = Utils.ReadVector(reader, Convert.ToInt32, 2, 0);
            (int rows, int columns) = (size[0], size[1]);
            var matrix = new Matrix(size[0], size[1]);

            for (var i = 0; i < rows; ++i)
            {
                double[] xs = Utils.ReadVector(reader, Utils.ParseDouble, columns, i + 1);

                for (var j = 0; j < columns; ++j)
                    matrix[i, j] = xs[j];
            }

            return matrix;
        }

        #endregion

        #region Matrix-Specific Operations

        /// <summary>
        /// Transposed matrix, i.e. elements are swapped over the major diagonal,
        /// So the element at [i, j] is at [j, i].
        /// </summary>
        public Matrix T => new Matrix(Columns, Rows, (i, j) => _content[j, i]);

        /// <summary>
        /// Dot product of this matrix with that, i.e. matrix multiplication.
        /// </summary>
        /// <param name="that">Matrix to be used as the right hand side of the multiplication</param>
        /// <exception cref="InvalidOperationException">If this.Rows != that.Columns</exception>
        public Matrix Dot(Matrix that)
        {
            return Rows == that.Columns
                ? new Matrix(Rows,
                             that.Columns,
                             (i, j) => Enumerable.Range(0, Rows).Sum(k => _content[i, k] * that._content[k, j]))
                : throw new ArgumentException(
                    "Rows number of left hand side matrix doesn't match columns number of right hand side matrix",
                    nameof(that));
        }

        #endregion

        #region Inversion

        /// <summary>
        /// Inversed matrix
        /// </summary>
        /// <exception cref="InvalidOperationException">If this matrix is not square</exception>
        public Matrix Inv()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Cannot find inverse matrix of non-square matrix");

            Matrix identity = Identity(Rows, Columns);

            for (var k = 0; k < Rows; ++k)
                Upper(Rows, k, identity);

            for (var k = 0; k < Rows; ++k)
                Lower(Rows, k, identity);

            return identity;
        }

        private void Upper(int size, int k, Matrix identity)
        {
            for (var j = 0; j < size; ++j)
                identity[k, j] /= _content[k, k];

            for (int i = k + 1; i < size; ++i)
            {
                for (var j = 0; j < size; ++j)
                    identity[i, j] -= identity[k, j] * _content[i, k];
            }
        }

        private void Lower(int size, int k, Matrix identity)
        {
            for (int i = size - k - 2; i >= 0; --i)
            {
                for (var j = 0; j < size; ++j)
                    identity[i, j] -= identity[size - k - 1, j] * _content[i, size - k - 1];
            }
        }

        #endregion

        #region SumOfDiagonal

        /// <summary>
        /// Total of the major diagonal values, i.e. values at [i, i]
        /// </summary>
        public double MajorDiagonalSum()
        {
            return Enumerable.Range(0, Math.Min(Rows, Columns)).Sum(i => _content[i, i]);
        }

        /// <summary>
        /// Total of the minor diagonal values, i.e. values at [i, n - i - 1 ], where n = min(Rows, Columns)
        /// </summary>
        public double MinorDiagonalSum()
        {
            int n = Math.Min(Rows, Columns);
            return Enumerable.Range(0, n).Sum(i => _content[i, n - i - 1]);
        }

        #endregion

        #region Rank

        public void Rank()
        {
            int rank = Columns;

            for (var row = 0; row < rank; ++row)
            {
                if (_content[row, row] != 0)
                {
                    for (var col = 0; col < Rows; ++col)
                    {
                        if (col == row)
                            continue;
                        var mult = (int)(_content[col, row] / _content[row, row]);
                        for (var i = 0; i < rank; ++i)
                            _content[col, i] -= mult * _content[row, i];
                    }
                }
                else
                {
                    var reduce = true;

                    for (int i = row + 1; i < Rows; ++i)
                    {
                        if (_content[i, row] == 0)
                            continue;
                        SwapRows(row, i, rank);
                        reduce = false;
                        break;
                    }

                    if (reduce)
                    {
                        ++rank;
                        for (var i = 0; i < Rows; ++i)
                            _content[i, row] = _content[i, rank];
                    }

                    --row;
                }
            }
        }

        private void SwapRows(int row1, int row2, int col)
        {
            for (var i = 0; i < col; ++i)
                (_content[row1, i], _content[row2, i]) = (_content[row2, i], _content[row1, i]);
        }

        #endregion

        #region Min/Max

        /// <summary>
        /// Get the min value among the whole matrix.
        /// </summary>
        public double Min()
        {
            return Enumerable.Range(0, Rows)
                             .Min(i => Enumerable.Range(0, Columns).Min(j => _content[i, j]));
        }

        /// <summary>
        /// Get the max value among the whole matrix.
        /// </summary>
        public double Max()
        {
            return Enumerable.Range(0, Rows)
                             .Max(i => Enumerable.Range(0, Columns).Max(j => _content[i, j]));
        }

        #endregion

        #region Binary Algebraic Operators

        /// <summary>
        /// Add <paramref name="rhs"/> to each element of the <paramref name="lhs"/>.
        /// </summary>
        /// <param name="lhs">Left hand side</param>
        /// <param name="rhs">Right hand side</param>
        public static Matrix operator +(Matrix lhs, double rhs)
        {
            return new Matrix(lhs.Rows, lhs.Columns, (i, j) => lhs[i, j] + rhs);
        }

        /// <summary>
        /// Add two matrices element-wise.
        /// </summary>
        /// <param name="lhs">Left hand side</param>
        /// <param name="rhs">Right hand side</param>
        /// <exception cref="InvalidOperationException">Shape of matrices doesn't match</exception>
        public static Matrix operator +(Matrix lhs, Matrix rhs)
        {
            return lhs.Rows != rhs.Rows || lhs.Columns != rhs.Columns
                ? throw new InvalidOperationException("Cannot add matrices of different size.")
                : new Matrix(lhs.Rows, lhs.Columns, (i, j) => lhs[i, j] + rhs[i, j]);
        }

        /// <summary>
        /// Subtract <paramref name="rhs"/> from each element of the <paramref name="lhs"/>.
        /// </summary>
        /// <param name="lhs">Left hand side</param>
        /// <param name="rhs">Right hand side</param>
        public static Matrix operator -(Matrix lhs, double rhs)
        {
            return new Matrix(lhs.Rows, lhs.Columns, (i, j) => lhs[i, j] - rhs);
        }

        /// <summary>
        /// Subtract two matrices element-wise.
        /// </summary>
        /// <param name="lhs">Left hand side</param>
        /// <param name="rhs">Right hand side</param>
        /// <exception cref="InvalidOperationException">Shape of matrices doesn't match</exception>
        public static Matrix operator -(Matrix lhs, Matrix rhs)
        {
            return lhs.Rows != rhs.Rows || lhs.Columns != rhs.Columns
                ? throw new InvalidOperationException("Cannot subtract matrices of different size.")
                : new Matrix(lhs.Rows, lhs.Columns, (i, j) => lhs[i, j] - rhs[i, j]);
        }

        /// <summary>
        /// Multiply each element of the <paramref name="lhs"/> by <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">Left hand side</param>
        /// <param name="rhs">Right hand side</param>
        public static Matrix operator *(Matrix lhs, double rhs)
        {
            return new Matrix(lhs.Rows, lhs.Columns, (i, j) => lhs[i, j] * rhs);
        }

        /// <summary>
        /// Multiply two matrices element-wise.
        /// </summary>
        /// <param name="lhs">Left hand side</param>
        /// <param name="rhs">Right hand side</param>
        /// <exception cref="InvalidOperationException">Shape of matrices doesn't match</exception>
        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            return lhs.Rows != rhs.Rows || lhs.Columns != rhs.Columns
                ? throw new InvalidOperationException("Cannot multiply matrices of different size.")
                : new Matrix(lhs.Rows, rhs.Columns, (i, j) => lhs[i, j] * rhs[i, j]);
        }

        /// <summary>
        /// Divide each element of the <paramref name="lhs"/> by <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">Left hand side</param>
        /// <param name="rhs">Right hand side</param>
        public static Matrix operator /(Matrix lhs, double rhs)
        {
            return new Matrix(lhs.Rows, lhs.Columns, (i, j) => lhs[i, j] * rhs);
        }

        /// <summary>
        /// Divide two matrices element-wise
        /// </summary>
        /// <param name="lhs">Left hand side</param>
        /// <param name="rhs">Right hand side</param>
        /// <exception cref="InvalidOperationException">Shape of matrices doesn't match</exception>
        public static Matrix operator /(Matrix lhs, Matrix rhs)
        {
            return lhs.Rows != rhs.Rows || lhs.Columns != rhs.Columns
                ? throw new InvalidOperationException("Cannot divide matrices of different size.")
                : new Matrix(lhs.Rows, lhs.Columns, (i, j) => lhs[i, j] / rhs[i, j]);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Check if two matrices are equal, i.e. the size and content is the same.
        /// </summary>
        /// <param name="lhs">First matrix</param>
        /// <param name="rhs">Second matrix</param>
        public static bool operator ==(Matrix lhs, Matrix rhs)
        {
            return lhs.Rows == rhs.Rows
                && lhs.Columns == rhs.Columns
                && Enumerable.Range(0, lhs.Rows)
                             .All(i => Enumerable.Range(0, lhs.Columns).All(j => lhs[i, j] == rhs[i, j]));
        }

        /// <summary>
        /// Check if two matrices differ, i.e. the size or content is not absolutely the same.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static bool operator !=(Matrix lhs, Matrix rhs)
        {
            return lhs.Rows != rhs.Rows
                || lhs.Columns != rhs.Columns
                || Enumerable.Range(0, lhs.Rows)
                             .Any(i => Enumerable.Range(0, lhs.Columns).Any(j => lhs[i, j] != rhs[i, j]));
        }

        #endregion

        #region Object Methods

        public override string ToString()
        {
            return string.Join('\n',
                               Enumerable.Range(0, Rows)
                                         .Select(RowToString));
        }

        private string RowToString(int i)
        {
            return string.Join(' ',
                               Enumerable.Range(0, Columns)
                                         .Select(
                                              j => Utils.DoubleToString(
                                                  _content[i, j])));
        }

        public override bool Equals(object? that)
        {
            return that is not Matrix matrix || this == matrix;
        }

        public override int GetHashCode()
        {
            return _content.GetHashCode();
        }

        #endregion
    }
}
