﻿namespace SMatrix
{
    public class Matrix
    {
        public double[,] elements = new double[0,0];

        #region Fillers

        /// <summary>
        /// Fills Matrix sized nxm with given nubmer.
        /// </summary>
        /// <param name="n">Nubmer of rows </param>
        /// <param name="m">Number of columns.</param>
        /// <param name="number"></param>
        public void FillWithNumber(int  n, int m, double number)
        {
            elements = new double[n,m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    elements[i,j] = number;
                }
            }
        }
        /// <summary>
        /// Create a Identity matrix with size of nxn.
        /// </summary>
        /// <param name="n">Size of matrix.</param>
        public void FillIdentityMatrix(int n)
        {
            elements = new double[n, n];
            for (int i = 0;i < n; i++)
            {
                elements[i, i] = 1;
            }
        }
        /// <summary>
        /// Create a Matrix Unit. 
        /// </summary>
        /// <param name="i">Row position.</param>
        /// <param name="j">Coloumn position.</param>
        /// <param name="n">Nubmer of rows </param>
        /// <param name="m">Number of columns.</param>
        public void FillMatrixUnit(int i , int j, int n , int m)
        {
            elements =new double[n,m];
            elements[i, j] = 1;
        }

        #endregion

        #region Addition

        /// <summary>
        /// Add matrix to another matrix.
        /// </summary>
        public void AddMatrix(Matrix obj)
        {
            if (elements.GetLength(0) != obj.elements.GetLength(0) || elements.GetLength(1) != obj.elements.GetLength(1))
            { 
                throw new InvalidOperationException("Cannot add two matrixes with different sizes."); 
            }
            else
            {
                for (int i = 0; i < elements.GetLength(0); i++)
                {
                    for (int j = 0; j < elements.GetLength(1); j++)
                    {
                        this.elements[i, j] += obj.elements[i, j];
                    }
                }
            }
        }

        /// <summary>
        /// Add number to matrix.
        /// </summary>
        /// <param name="number">Number that will be added.</param>
        public void AddNumber(double number)
        {
            for (int i = 0; i < elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    this.elements[i, j] += number;
                }
            }
        }

        #endregion

        #region Difference
        /// <summary>
        /// Subtract the matrix.
        /// </summary>
        public void SubtractMatrix(Matrix obj)
        {
            if (elements.GetLength(0) != obj.elements.GetLength(0) || elements.GetLength(1) != obj.elements.GetLength(1))
            {
                throw new InvalidOperationException("Cannot substract two matrixes with different sizes.");
            }
            else
            {
                for (int i = 0; i < elements.GetLength(0); i++)
                {
                    for (int j = 0; j < elements.GetLength(1); j++)
                    {
                        this.elements[i, j] -= obj.elements[i, j];
                    }
                }
            }
        }
        /// <summary>
        /// Subtract number from matrix.
        /// </summary>
        /// <param name="number">Number that will be subtracted.</param>
        public void SubtractNumber(double number)
        {
            for (int i = 0; i < elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    this.elements[i, j] -= number;
                }
            }
        }
        #endregion

        #region Transition
        /// <summary>
        /// Transpose the matrix.
        /// </summary>
        public void Transition()
        {
            double[,] clone = new double[elements.GetLength(0), elements.GetLength(1)];

            for (int i = 0; i < elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    clone[i, j] = elements[i, j];
                }
            }

            elements = new double[clone.GetLength(1), clone.GetLength(0)];

            for (int i = 0; i < elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    elements[i, j] = clone[j, i];
                }
            }

        }
        #endregion

        #region ConsoleCommands
        /// <summary>
        /// Print matrix in console.
        /// </summary>
        public void ConsoleOutput()
        {
            for (int i = 0; i < elements.GetLength(0); i++, Console.WriteLine("\n"))
            {
                for (int j = 0; j < elements.GetLength(1); j++, Console.Write("\t"))
                {
                    Console.Write(elements[i, j]);
                }
            }
        }
        /// <summary>
        /// Input matrix from console.
        /// </summary>
        public void ConsoleInput()
        {
            Console.WriteLine("Enter number of rows");
            int numberOfRows = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter number of columns:");
            int numberOfCols = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfCols; j++)
                {
                    Console.WriteLine($"Enter element at position [{i},{j}]: ");
                    elements[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }    
        }
        #endregion

        #region Multiplication
        /// <summary>
        /// Multiplies every element of matrix by given number.
        /// </summary>
        /// <param name="number"></param>
        public void MultiplyByNumber(double number)
        {
            for (int i = 0; i <elements.GetLength(0) ; i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    elements[i, j] *= number;
                }
            }
        }

        public void MatrixMultiplication(Matrix obj)
        {
            double[,] mult = new double[elements.GetLength(0), obj.elements.GetLength(1)];

            if (elements.GetLength(0) != obj.elements.GetLength(1))
            {
                throw new InvalidOperationException("Cannot multiply two matrixes.");
            }
            else
            {
                for (int i = 0; i < elements.GetLength(0); i++)
                {
                    for (int j = 0; j < obj.elements.GetLength(1); j++)
                    {
                        mult[i, j] = 0;
                        for (int k = 0; k < elements.GetLength(0); k++)
                        {
                            mult[i, j] += this.elements[i, k] * obj.elements[k, j];
                        }
                    }
                }

                //output
                /*for (int i = 0; i < mult.GetLength(0); i++, Console.WriteLine("\n"))
                {
                    for (int j = 0; j < mult.GetLength(1); j++, Console.Write("\t"))
                    {
                        Console.Write(elements[i, j]);
                    }
                }*/
            }
        }
        #endregion

        #region Inversion

        void Upper(int size, int k, double[,] ident)
        {
            for (int j = 0; j < size; j++)
            {
                ident[k, j] /= elements[k, k];
            }

            for (int i = k + 1; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    ident[i, j] -= ident[k, j] * elements[i, k];
                }
            }
        }

        void Lower(int size, int k, double[,] ident)
        {
            for (int i = size - k - 2; i >= 0; i--)
            {
                for (int j = 0; j < size; j++)
                {
                    ident[i, j] -= ident[size - k - 1, j] * elements[i, size - k - 1];
                }
            }
        }

        public void InversionMatrix()
        {
            double[,] ident = new double[elements.GetLength(0), elements.GetLength(1)];

            if (elements.GetLength(0) != elements.GetLength(1))
            {
                throw new InvalidOperationException("Cannot find inverse matrix.");
            }
            else
            {
                for (int i = 0; i < ident.GetLength(0); i++)
                {
                    for (int j = 0; j < ident.GetLength(1); j++)
                    {
                        if (i != j)
                        {
                            ident[i, j] = 0;
                        }
                        else
                        {
                            ident[i, j] = 1;
                        }
                    }
                }

                for (int k = 0; k < elements.GetLength(0); k++)
                {
                    Upper(elements.GetLength(0), k, ident);
                }

                for (int k = 0; k < elements.GetLength(0); k++)
                {
                    Lower(elements.GetLength(0), k, ident);
                }
            }

        }

        #endregion

        #region SumOfDiagonal

        public void MainDiagonal()
        {
            double main = 0;
            for (int i = 0; i < elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        main += elements[i, j];
                    }
                }
            }
            //output
            //Console.WriteLine(main);
        }

        public void AdditionalDiagonal()
        {
            double add = 0;
            for (int i = 0; i < elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    if (i == elements.GetLength(1) - j - 1)
                    {
                        add += elements[i, j];
                    }
                }
            }
            //output
            //Console.WriteLine(add);
        }

        #endregion

        #region Rank

        public void SwapRows(int row1, int row2, int col)
        {
            for (int i = 0; i < col; i++)
            {
                double temp = elements[row1, i];
                elements[row1, i] = elements[row2, i];
                elements[row2, i] = temp;
            }
        }

        public void RankOfMatrix()
        {
            int rank = elements.GetLength(1);

            for (int row = 0; row < rank; row++)
            {
                if (elements[row, row] != 0)
                {
                    for (int col = 0; col < elements.GetLength(0); col++)
                    {
                        if (col != row)
                        {
                            double mult = (double)elements[col, row] / elements[row, row];
                            for (int i = 0; i < rank; i++)
                            {
                                elements[col, i] -= (int)mult * elements[row, i];
                            }
                        }
                    }
                }
                else
                {
                    bool reduce = true;

                    for (int i = row + 1; i < elements.GetLength(0); i++)
                    {
                        if (elements[i, row] != 0)
                        {
                            SwapRows(row, i, rank);
                            reduce = false;
                            break;
                        }
                    }

                    if (reduce)
                    {
                        rank--;

                        for (int i = 0; i < elements.GetLength(0); i++)
                        {
                            elements[i, row] = elements[i, rank];
                        }
                    }

                    row--;
                }
            }

            //output
            //Console.WriteLine(rank);
        }

        #endregion
    }
}