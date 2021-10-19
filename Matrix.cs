using System;
using System.Collections.Generic;
using System.Globalization;

namespace MatrixType
{
    class Matrix
    {
        private double[, ] data;

        public double this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }

        public int Rows => data.GetLength(0);

        public int Columns => data.GetLength(1);

        // размер квадратной матрицы
        public int? Size
        {
            get
            {
                if (IsSquared)
                {
                    return Rows;
                }
                
                return null;
            }
        }
        
        // Является ли матрица квадратной
        public bool IsSquared
        {
            get
            {
                if (Rows == Columns)
                {
                    return true;
                }
                
                return false;
            }
        }
        // Является ли матрица нулевой
        public bool IsEmpty
        {
            get
            {
                foreach (var val in data)
                {
                    if (val != 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        // Является ли матрица единичной
        public bool IsUnity
        {
            get
            {
                if (!IsSquared)
                {
                    return false;
                }
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (i == j && data[i, j] != 1)
                        {
                            return false;
                        }

                        if (i != j && data[i, j] != 0)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }
        // Является ли матрица диагональной
        public bool IsDiagonal
        {
            get
            {
                if (!IsSquared)
                {
                    return false;
                }
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (i != j && data[i, j] != 0)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }
        // Является ли матрица симметричной
        public bool IsSymmetric
        {
            get
            {
                if (!IsSquared)
                {
                    return false;
                }
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = i + 1; j < Columns; j++)
                    {
                        if (data[i, j] != data[j, i])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        // Конструкторы
        public Matrix(int nRows, int nCols)
        {
            data = new double[nRows, nCols];
            // чем она будет заполнено?
        }

        public Matrix(double[,] initData)
        {
            data = initData;
        }
        
        class WrongSizeException : Exception
        {
            public WrongSizeException(string message){ }
        }
        
        // Перегрузка операторов
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
            {
                throw new WrongSizeException("Размеры матриц не равны;");
            }

            var m3 = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m3.Rows; i++)
            {
                for (int j = 0; j < m3.Columns; j++)
                {
                    m3[i, j] = m1[i, j] + m2[i, j];
                }
            }

            return m3;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
            {
                throw new WrongSizeException("Размеры матриц не равны;");
            }

            var m3 = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m3.Rows; i++)
            {
                for (int j = 0; j < m3.Columns; j++)
                {
                    m3[i, j] = m1[i, j] - m2[i, j];
                }
            }

            return m3;
        }

        public static Matrix operator *(Matrix m1, double d)
        {
            var m2 = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m2.Rows; i++)
            {
                for (int j = 0; j < m2.Columns; j++)
                {
                    m2[i, j] = m1[i, j] * d;
                }
            }

            return m2;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Columns != m2.Rows)
            {
                throw new WrongSizeException(
                    "Количество столбцов первой матрицы не равно количеству строк второй матрицы");
            }

            var m3 = new Matrix(m1.Rows, m2.Columns);
            for (int i = 0; i < m3.Rows; i++)
            {
                for (int j = 0; j < m3.Columns; j++)
                {
                    m3[i, j] = 0;
                    for (int r = 0; r < m1.Columns; r++)
                    {
                        m3[i, j] += m1[i, r] * m2[r, j];
                    }
                }
            }

            return m3;
        }
        
        // Оператор преобразования double[,] в Matrix 
        public static explicit operator Matrix(double[,] arr)
        {
            var m1 = new Matrix(arr);
            return m1;
        }
        
        // Преобразование в строку
        public override string ToString()
        {
            var output = "";
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    //output += data[i, j].ToString();
                    output += String.Format(" {0, 4}", data[i, j]);
                }
                
                output += "\n";
            }

            return output;
        }
        
        // Статические методы
        // для порождения единичной
        public static Matrix GetUnity(int Size)
        {
            var inside = new double[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == j)
                        inside[i, j] = 1;
                    else
                        inside[i, j] = 0;
                }
            }
            var m1 = new Matrix(inside);
            return m1;
        }
        // и нулевой матрицы
        public static Matrix GetEmpty(int Size)
        {
            var inside = new double[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    inside[i, j] = 0;
                }
            }
            var m1 = new Matrix(inside);
            return m1;
        }
        
        // для создания матрицы из строки
        public static Matrix Parse(string s)
        {
            if (TryParse(s, out Matrix m1))
                return m1;
            throw new FormatException("Не удалось создать матрицу из введенной строки");
        }

        public static bool TryParse(string s, out Matrix m)
        {
            m = null;
            var arr = s.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var list = new List<String[]>();
            
            foreach (var val in arr)
            {
                list.Add(val.Split(' ', StringSplitOptions.RemoveEmptyEntries)); // как указать сразу оба параметра в Split?
            }

            int lenRows = list.Count;
            int lenCols = list[0].Length;
            var inside = new double[lenRows, lenCols];
            for (var i = 0; i < list.Count; i++)
            {
                var val = list[i];
                if (val.Length != lenCols)
                {
                    return false;
                }

                for (var j = 0; j < val.Length; j++)
                {
                    var num = val[j];
                    try
                    {
                        inside[i, j] = Convert.ToDouble(num, new CultureInfo("en-US"));
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            m = new Matrix(inside);
            return true;
        }
        
        // Экземплярные методы для работы с матрицей
        public Matrix Transpose()
        {
            var replace = new double[Columns, Rows];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    replace[j, i] = data[i, j];
                }
            }

            var m2 = new Matrix(replace);
            return m2;
        }
        
        class NotSquareException : Exception
        {
            public NotSquareException(string message){ }
        }

        public double Trace()
        {
            if (!IsSquared)
            {
                throw new NotSquareException("Матрица не квадратная");
            }

            double tr = 0;
            for (int i = 0; i < Rows; i++)
            {
                tr += data[i, i];
            }

            return tr;
        }
    }
}