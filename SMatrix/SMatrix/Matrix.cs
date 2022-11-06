using System.Globalization;

namespace SMatrix
{
    public class Matrix
    {
        public double[,] Elements = new double[0,0];

        public int Rows => Elements.GetLength(0);
        public int Columns => Elements.GetLength(1);

        #region Fillers

        /// <summary>
        /// Fills Matrix sized nxm with given number.
        /// </summary>
        /// <param name="n">Number of rows</param>
        /// <param name="m">Number of columns</param>
        /// <param name="number">Value to fill with</param>
        public void FillWithNumber(int  n, int m, double number)
        {
            Elements = new double[n,m];
            for (var i = 0; i < n; ++i)
            {
                for (var j = 0; j < m; ++j)
                    Elements[i,j] = number;
            }
        }
        
        /// <summary>
        /// Create an Identity matrix.
        /// </summary>
        /// <param name="n">Size of matrix</param>
        public void FillIdentityMatrix(int n)
        {
            Elements = new double[n, n];
            for (var i = 0;i < n; ++i)
                Elements[i, i] = 1;
        }
        
        /// <summary>
        /// Create a Matrix Unit, i.e. where all the values are 0, but only the [i,j] is 1.
        /// </summary>
        /// <param name="i">Row position of 1</param>
        /// <param name="j">Column position of 1</param>
        /// <param name="n">Number of rows</param>
        /// <param name="m">Number of columns</param>
        public void FillMatrixUnit(int i , int j, int n , int m)
        {
            Elements =new double[n,m];
            Elements[i, j] = 1;
        }

        #endregion

        #region Addition

        /// <summary>
        /// Add a matrix to this instance, element-wise.
        /// </summary>
        /// <param name="that">Matrix to add to this instance</param>
        public void AddMatrix(Matrix that)
        {
            if (Elements.GetLength(0) != that.Elements.GetLength(0) || Elements.GetLength(1) != that.Elements.GetLength(1))
                throw new InvalidOperationException("Cannot add two matrices with different sizes.");
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                    Elements[i, j] += that.Elements[i, j];
            }
        }

        /// <summary>
        /// Add number to each element in this matrix.
        /// </summary>
        /// <param name="number">Value to add</param>
        public void AddNumber(double number)
        {
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                    Elements[i, j] += number;
            }
        }

        /// <summary>
        /// Add number to the specific element in matrix.
        /// </summary>
        /// <param name="number">Value to add</param>
        /// <param name="i">Row position of the element.</param>
        /// <param name="j">Column position of the element.</param>
        public void AddNumberSpecific(double number, int i, int j)
        {
            this.Elements[i, j] += number;
        }

        #endregion

        #region Difference
        /// <summary>
        /// Subtract a matrix from this instance, element-wise.
        /// </summary>
        /// <param name="that">Matrix to subtract</param>
        public void SubtractMatrix(Matrix that)
        {
            if (Elements.GetLength(0) != that.Elements.GetLength(0) || Elements.GetLength(1) != that.Elements.GetLength(1))
                throw new InvalidOperationException("Cannot subtract two matrices with different sizes.");
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                    Elements[i, j] -= that.Elements[i, j];
            }
        }
        /// <summary>
        /// Subtract number from each element in this matrix.
        /// </summary>
        /// <param name="number">Value to subtract</param>
        public void SubtractNumber(double number)
        {
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                    Elements[i, j] -= number;
            }
        }
        #endregion

        #region Transition
        /// <summary>
        /// Transpose the matrix.
        /// </summary>
        public void Transition()
        {
            var clone = new double[Elements.GetLength(0), Elements.GetLength(1)];

            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                    clone[i, j] = Elements[i, j];
            }

            Elements = new double[clone.GetLength(1), clone.GetLength(0)];

            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                    Elements[i, j] = clone[j, i];
            }

        }
        #endregion

        #region ConsoleCommands
        /// <summary>
        /// Print matrix in console.
        /// </summary>
        public void ConsoleOutput()
        {
            for (var i = 0; i < Elements.GetLength(0); ++i, Console.WriteLine("\n"))
            {
                for (var j = 0; j < Elements.GetLength(1); ++j, Console.Write("\t"))
                    Console.Write(Elements[i, j]);
            }
        }
        /// <summary>
        /// Input matrix from console.
        /// </summary>
        public void ConsoleInput()
        {
            Console.WriteLine("Enter number of rows");
            var numberOfRows = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter number of columns:");
            var numberOfCols = Convert.ToInt32(Console.ReadLine());
            for (var i = 0; i < numberOfRows; ++i)
            {
                for (var j = 0; j < numberOfCols; ++j)
                {
                    Console.WriteLine($"Enter element at position [{i},{j}]: ");
                    Elements[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }    
        }

        public void WriteToConsole()
        {
            Write(Console.Out);
        }

        public void WriteToFile(string filename)
        {
            using var writer = new StreamWriter(filename);
            Write(writer);
        }

        private void Write(TextWriter writer)
        {
            writer.WriteLine($"{Rows} {Columns}");
            writer.WriteLine(string.Join('\n', Enumerable.Range(0, Rows).Select(
                                         i => string.Join(' ', Enumerable.Range(0, Columns).Select(
                                                              j => DoubleToString(Elements[i, j]))))));
        }

        public void ReadFromConsole()
        {
            Console.Write("Enter the number of rows and columns: ");
            Read(Console.In);
        }

        public void ReadFromFile(string filename)
        {
            using var reader = new StreamReader(filename);
            Read(reader);
        }

        private void Read(TextReader reader)
        {
            int[] size = ReadVector(reader, Convert.ToInt32, 0, 2);
            Elements = new double[size[0], size[1]];
            
            for (var i = 0; i < Rows; ++i)
            {

                double[] xs = ReadVector(reader, ParseDouble, i+1, Columns);
                
                for (var j = 0; j < xs.Length; ++j)
                    Elements[i, j] = xs[j];
            }
        }

        private static T[] ReadVector<T>(TextReader reader, 
                                         Func<string, T> parseValue,
                                         int lineIndex,
                                         int expectedLength)
        {
            // StringSplitOptions.RemoveEmptyEntries = 1
            // StringSplitOptions.TrimEntries = 2
            const StringSplitOptions options = (StringSplitOptions)(1 + 2);
            T[] xs = reader.ReadLine()?
                           .Split(new[]{' ', '\t'}, options)
                           .Select(parseValue)
                           .ToArray() 
                  ?? throw new ArgumentException(
                         $"Expected to read {lineIndex}th line, but reader is empty", nameof(reader));
            if (xs.Length != expectedLength)
                throw new FormatException(
                    $"The {lineIndex}th line expected to contain {expectedLength} values, but got {xs.Length}");
            return xs;
        }

        private static double ParseDouble(string s)
        {
            return double.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture);
        }

        private static string DoubleToString(double x)
        {
            return x.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region Multiplication
        /// <summary>
        /// Multiplies each element of this matrix by given number.
        /// </summary>
        /// <param name="number">Value to multiply by</param>
        public void MultiplyByNumber(double number)
        {
            for (var i = 0; i <Elements.GetLength(0) ; ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                    Elements[i, j] *= number;
            }
        }

        /// <summary>
        /// Perform matrix multiplication
        /// </summary>
        /// <param name="that">Matrix to be used as right hand side of the multiplication</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void MatrixMultiplication(Matrix that)
        {
            if (Elements.GetLength(0) != that.Elements.GetLength(1))
                throw new ArgumentException(
                    "Rows number of left hand side matrix doesn't match columns number of right hand side matrix", 
                    nameof(that));
            
            var mult = new double[Elements.GetLength(0), that.Elements.GetLength(1)];

            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < that.Elements.GetLength(1); ++j)
                {
                    mult[i, j] = 0;
                    for (var k = 0; k < Elements.GetLength(0); ++k)
                        mult[i, j] += Elements[i, k] * that.Elements[k, j];
                }
            }
        }
        #endregion

        #region Inversion

        private void Upper(int size, int k, double[,] ident)
        {
            for (var j = 0; j < size; ++j)
                ident[k, j] /= Elements[k, k];

            for (int i = k + 1; i < size; ++i)
            {
                for (var j = 0; j < size; ++j)
                    ident[i, j] -= ident[k, j] * Elements[i, k];
            }
        }

        private void Lower(int size, int k, double[,] ident)
        {
            for (int i = size - k - 2; i >= 0; --i)
            {
                for (var j = 0; j < size; ++j)
                    ident[i, j] -= ident[size - k - 1, j] * Elements[i, size - k - 1];
            }
        }

        public void InversionMatrix()
        {
            if (Elements.GetLength(0) != Elements.GetLength(1))
                throw new InvalidOperationException("Cannot find inverse matrix.");
            
            var ident = new double[Elements.GetLength(0), Elements.GetLength(1)];

            for (var i = 0; i < ident.GetLength(0); ++i)
            {
                for (var j = 0; j < ident.GetLength(1); ++j)
                    ident[i, j] = Convert.ToInt32(i == j);
            }

            for (var k = 0; k < Elements.GetLength(0); ++k)
                Upper(Elements.GetLength(0), k, ident);

            for (var k = 0; k < Elements.GetLength(0); ++k)
                Lower(Elements.GetLength(0), k, ident);
        }

        #endregion

        #region SumOfDiagonal

        public void MainDiagonal()
        {
            double main = 0;
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                {
                    if (i == j)
                        main += Elements[i, j];
                }
            }
        }

        public void AdditionalDiagonal()
        {
            double add = 0;
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                {
                    if (i == Elements.GetLength(1) - j - 1)
                        add += Elements[i, j];
                }
            }
        }

        #endregion

        #region Rank

        public void SwapRows(int row1, int row2, int col)
        {
            for (var i = 0; i < col; ++i)
                (Elements[row1, i], Elements[row2, i]) = (Elements[row2, i], Elements[row1, i]);
        }

        public void RankOfMatrix()
        {
            int rank = Elements.GetLength(1);

            for (var row = 0; row < rank; ++row)
            {
                if (Elements[row, row] != 0)
                {
                    for (var col = 0; col < Elements.GetLength(0); ++col)
                    {
                        if (col != row)
                        {
                            double mult = Elements[col, row] / Elements[row, row];
                            for (var i = 0; i < rank; ++i)
                                Elements[col, i] -= (int)mult * Elements[row, i];
                        }
                    }
                }
                else
                {
                    var reduce = true;

                    for (int i = row + 1; i < Elements.GetLength(0); ++i)
                    {
                        if (Elements[i, row] != 0)
                        {
                            SwapRows(row, i, rank);
                            reduce = false;
                            break;
                        }
                    }

                    if (reduce)
                    {
                        ++rank;

                        for (var i = 0; i < Elements.GetLength(0); ++i)
                            Elements[i, row] = Elements[i, rank];
                    }

                    --row;
                }
            }

            //output
            //Console.WriteLine(rank);
        }

        #endregion

        #region Division by a scalar
        /// <summary>
        /// Divides all elements by specific number.
        /// </summary>
        /// <param name="scalar">Number that the matrix is divided by.</param>
        public void DivideByScalar(double scalar)
        {
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                    Elements[i, j] /= scalar;
            }
        }


        #endregion

        #region Find Min/Max values

        /// <summary>
        /// Finds and returns the min element of matrix.
        /// </summary>
        public double FindMin()
        {
            double min = Elements[0, 0];
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                {
                    if (Elements[i, j] <= min)
                        min = Elements[i, j];
                }
            }
            return min;
        }

        /// <summary>
        /// Finds and returns the max element of matrix.
        /// </summary>
        public double FindMax()
        {
            double max = Elements[0, 0];
            for (var i = 0; i < Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < Elements.GetLength(1); ++j)
                {
                    if (Elements[i, j] >= max)
                        max = Elements[i, j];
                }
            }
            return max;
        }

        #endregion

        #region Operators

        public static Matrix operator +(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix.Elements.GetLength(0) != firstMatrix.Elements.GetLength(0) 
                || firstMatrix.Elements.GetLength(1) != firstMatrix.Elements.GetLength(1))
                throw new InvalidOperationException("Cannot add two matrices with different sizes.");
            
            var result = new Matrix
            {
                Elements = new double[firstMatrix.Elements.GetLength(0), firstMatrix.Elements.GetLength(1)]
            };
            
            for (var i = 0; i < result.Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < result.Elements.GetLength(1); ++j)
                    result.Elements[i, j]=firstMatrix.Elements[i, j]+secondMatrix.Elements[i,j];
            }


            return result;
        }
        
        public static Matrix operator *(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix.Elements.GetLength(0) != secondMatrix.Elements.GetLength(1))
                throw new InvalidOperationException("Cannot multiply two matrixes.Number of rows in first matrix " +
                                                    "has to be equal to number of column in second.");

            var result = new Matrix();
            result.Elements = new double[firstMatrix.Elements.GetLength(0), secondMatrix.Elements.GetLength (1)];


            for (var i = 0; i < result.Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < result.Elements.GetLength(1); ++j)
                {
                    for (var k = 0; k < result.Elements.GetLength(0); ++k)
                        result.Elements[i, j] += firstMatrix.Elements[i, k] * secondMatrix.Elements[k, j];
                }
            }

            return result;
        }
        
        public static Matrix operator -(Matrix firstMatrix, Matrix secondMatrix)
        {
            
            if (firstMatrix.Elements.GetLength(0) != firstMatrix.Elements.GetLength(0)
                || firstMatrix.Elements.GetLength(1) != firstMatrix.Elements.GetLength(1))
                throw new InvalidOperationException("Cannot substract two matrixes with different sizes.");
            var result = new Matrix();
            for (var i = 0; i < result.Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < result.Elements.GetLength(1); ++j)
                    result.Elements[i, j] = firstMatrix.Elements[i, j] - secondMatrix.Elements[i, j];
            }

            return result;
        }

        public static Matrix operator +(Matrix firstMatrix, double number)
        {
            var result = new Matrix();
            result.Elements = new double[firstMatrix.Elements.GetLength(0), firstMatrix.Elements.GetLength(1)];
            for (var i = 0; i < result.Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < result.Elements.GetLength(1); ++j)
                    result.Elements[i, j] = firstMatrix.Elements[i, j] + number;
            }
            return result;
        }
        
        public static Matrix operator -(Matrix firstMatrix, double number)
        {
            var result = new Matrix();
            result.Elements = new double[firstMatrix.Elements.GetLength(0), firstMatrix.Elements.GetLength(1)];
            for (var i = 0; i < result.Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < result.Elements.GetLength(1); ++j)
                    result.Elements[i, j] = firstMatrix.Elements[i, j] - number;
            }
            return result;
        }
        
        public static Matrix operator *(Matrix firstMatrix, double number)
        {
            var result = new Matrix
            {
                Elements = new double[firstMatrix.Elements.GetLength(0), firstMatrix.Elements.GetLength(1)]
            };
            
            for (var i = 0; i < result.Elements.GetLength(0); ++i)
            {
                for (var j = 0; j < result.Elements.GetLength(1); ++j)
                    result.Elements[i, j] = firstMatrix.Elements[i, j] * number;
            }
            return result;
        }

        public static bool operator ==(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix.Elements.GetLength(0)!= secondMatrix.Elements.GetLength(0) ||
                firstMatrix.Elements.GetLength(1) != secondMatrix.Elements.GetLength(1))
                return false;
            
            for (int i = 0; i < firstMatrix.Elements.GetLength(0); ++i)
            {
                for (int j = 0; j < firstMatrix.Elements.GetLength(1); ++j)
                {
                    if (firstMatrix.Elements[i, j] != secondMatrix.Elements[i, j])
                        return false;
                }
            }

            return true;
        }
        
        public static bool operator !=(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix.Elements.GetLength(0) != secondMatrix.Elements.GetLength(0) ||
               firstMatrix.Elements.GetLength(1) != secondMatrix.Elements.GetLength(1))
                return true;
            
            for (int i = 0; i < firstMatrix.Elements.GetLength(0); ++i)
            {
                for (int j = 0; j < firstMatrix.Elements.GetLength(1); ++j)
                {
                    if (firstMatrix.Elements[i, j] != secondMatrix.Elements[i, j])
                        return true;
                }
            }

            return false;
        }
        #endregion

        #region Overriden Functions

        public override bool Equals(object? obj)
        {
            return obj is not Matrix matrix || this == matrix;
        }

        #endregion

    }
}