using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MatrixType
{
    class Program
    {
        static bool _printMenuFlag = true;
        private static Dictionary<string, Matrix> _matrices = new Dictionary<string, Matrix>();
        class WrongInputException : Exception
        {
            public WrongInputException(string message){ }
        }

        static void EnterMatrix()
        {
            while (true)
            {
                if (_printMenuFlag)
                {
                    Console.Clear();
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("Введите матрицу в формате:");
                    Console.WriteLine("     name = row1, row2, ... , rowN");
                    Console.WriteLine("     где name - непустое имя матрицы (не начинающееся с 0),");
                    Console.WriteLine("     rowN - перечисление значений в очередной строке матрицы (дробные числа пишутся с точкой)");
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("Введите \"0\" для выхода в главное меню");
                    Console.WriteLine("Введите \"matrices\" для вывода списка существующих матриц\n");
                    _printMenuFlag = false;
                }
                string input = Console.ReadLine();
                try
                {
                    if (input == "0")
                    {
                        _printMenuFlag = true;
                        return;
                    }
                    if (input == "matrices")
                    {
                        ShowMatrices();
                        continue;
                    }
                    if (input == null)
                    {
                        Console.WriteLine("Ввод пуст: введите матрицу");
                        throw new WrongInputException("Ввод пуст: введите матрицу");
                    }
                    if (input.Contains("=") == false)
                    {
                        Console.WriteLine("Формат ввода неверный: нет символа \"=\"");
                        throw new WrongInputException("Формат ввода неверный: нет символа \"=\"");
                    }
                    int eqIndex = input.IndexOf('=');
                    string matrixName = input.Substring(0, eqIndex);
                    string matrixValue = input.Substring(eqIndex + 1);
                    matrixName = matrixName.Trim();
                    matrixValue = matrixValue.Trim();
                    Console.WriteLine();
                    if (matrixName == "0")
                    {
                        Console.WriteLine("Формат ввода неверный: матрица не может носить имя \"0\"");
                        throw new WrongInputException("Формат ввода неверный: матрица не может носить имя \"0\"");
                    }
                    if(matrixName == "")
                    {
                        Console.WriteLine("Формат ввода неверный: имя матрицы не должно быть пустым");
                        throw new WrongInputException("Формат ввода неверный: имя матрицы не должно быть пустым");
                    }
                    if(IsMatrixCreated(matrixName))
                    {
                        Console.WriteLine("Матрица с этим именем уже существует");
                        throw new WrongInputException("Матрица с этим именем уже существует");
                    }
                    
                    matrixValue = matrixValue.Trim(new char[]{' ', ','});
                    
                    if (matrixValue == "")
                    {
                        Console.WriteLine("Значение матрицы пустое");
                        throw new WrongInputException("Значение матрицы пустое");
                    }
                    
                    Matrix m = Matrix.Parse(matrixValue);
                    _matrices.Add(matrixName, m);
                    Console.WriteLine("Создана матрица {0}({1}x{2})", matrixName, m.Rows, m.Columns);
                    Console.WriteLine();
                }
                catch(Exception ex)
                {
                    Console.WriteLine();
                }
            }
        }
        
        static bool IsMatrixCreated(string name)
        {
            foreach(var pair in _matrices)
            {
                if(pair.Key == name)
                {
                    return true;
                }
            }
            return false;
        }

        static void ShowMatrices()
        {
            Console.WriteLine("\nСписок матриц");
            Console.WriteLine("-------------------------------");
            foreach (var pair in _matrices)
            {
                Console.WriteLine("    " + pair.Key);
            }
            Console.WriteLine("-------------------------------\n");
        }

        static void Calculate()
        {
            while (true)
            {
                if (_printMenuFlag)
                {
                    Console.Clear();
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("     1 – Бинарные операции");
                    Console.WriteLine("     2 – Унарные операции");
                    Console.WriteLine("     0 - Выход в меню");
                    Console.WriteLine("-------------------------------");
                    _printMenuFlag = false;
                    Console.WriteLine();
                }
                switch (char.ToLower(Console.ReadKey(true).KeyChar))
                {
                    case '1': _printMenuFlag = true; CalcBin(); break;
                    case '2': _printMenuFlag = true; CalcUn(); break;
                    case '0': _printMenuFlag = true; return;
                }
            }
        }

        static void CalcBin()
        {
            while (true)
            {
                if (_printMenuFlag)
                {
                    Console.Clear();
                    ShowMatrices();
                    Console.WriteLine("Введите операцию в нужном формате (пусть m1, m2, m3 - имена матриц):");
                    Console.WriteLine("     m1 + m2        чтобы получить результат в консоль");
                    Console.WriteLine("     m3 = m1 + m2   чтобы записать результат в новую матрицу");
                    Console.WriteLine("     0 - Выход в меню");
                    Console.WriteLine("Доступны операции + (матрица с матрицей), - (матрица с матрицей), * (матрица на число, матрица с матрицей)");
                    Console.WriteLine("-------------------------------");
                    _printMenuFlag = false;
                    Console.WriteLine();
                }
                string input = Console.ReadLine();
                try
                {
                    if (input == "0")
                    {
                        _printMenuFlag = true;
                        return;
                    }

                    bool isAdd = input.Contains("+");
                    bool isMinus = input.Contains("-");
                    bool isMultiply = input.Contains("*");
                    bool isEqualation = input.Contains("=");

                    if (isAdd && isMinus || isAdd && isMultiply || isMinus && isMultiply)
                    {
                        Console.WriteLine("Несколько операций одновременно не обрабатываются");
                        throw new WrongInputException("Несколько операций одновременно не обрабатываются");
                    }

                    string nameNew = "";
                    if (isEqualation)
                    {
                        nameNew = input.Substring(0, input.IndexOf('=')).Trim();
                        input = input.Substring(input.IndexOf('=') + 1);
                    }

                    input = input.Trim();
                    int operationIndex = input.IndexOfAny("+-*".ToCharArray());
                    string firstOp = input.Substring(0, operationIndex).Trim();
                    string secondOp = input.Substring(operationIndex + 1).Trim();
                    Matrix m1 = _matrices[firstOp];
                    Matrix result;
                    if (isMultiply && int.TryParse(secondOp, out int number))
                    {
                        result = m1 * number;
                        Console.WriteLine("\nРезультат операции:");
                        Console.WriteLine(result);
                    }
                    else if(isMultiply)
                    {
                        Matrix m2 = _matrices[secondOp];
                        result = m1 * m2;
                        Console.WriteLine("\nРезультат операции:");
                        Console.WriteLine(result);
                    }
                    else if(isAdd)
                    {
                        Matrix m2 = _matrices[secondOp];
                        result = m1 + m2;
                        Console.WriteLine("\nРезультат операции:");
                        Console.WriteLine(result);
                    }
                    else if(isMinus)
                    {
                        Matrix m2 = _matrices[secondOp];
                        result = m1 - m2;
                        Console.WriteLine("\nРезультат операции:");
                        Console.WriteLine(result);
                    }

                    if (isEqualation)
                    {
                        Console.WriteLine("\nРезультат записан в новую матрицу {0}\n", nameNew);
                    }
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine("Матрица с таким именем не существует или неправильно введено число");
                    Console.WriteLine();
                }
                catch (WrongInputException ex)
                {
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Неправильный ввод");
                    Console.WriteLine();
                }
            }
        }

        static void CalcUn()
        {
            while (true)
            {
                if (_printMenuFlag)
                {
                    Console.Clear();
                    ShowMatrices();
                    Console.Write("Введите команду:\n");
                    Console.WriteLine("     transpose имя_матрицы – Транспонировать матрицу");
                    Console.WriteLine("     trace имя_матрицы – Получить след матрицы");
                    Console.WriteLine("     0 - Назад");
                    Console.WriteLine("-------------------------------");
                    _printMenuFlag = false;
                    Console.WriteLine();
                }
                string input = Console.ReadLine();
                try
                {
                    string[] splitted = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (splitted.Length != 2)
                    {
                        Console.WriteLine("Введена неверная команда");
                        Console.WriteLine();
                        continue;
                    }
                    string command = splitted[0].Trim();
                    string name = splitted[1].Trim();
                    switch (command)
                    {
                        case "0":
                            _printMenuFlag = true;
                            return;
                        case "transpose":
                            Console.WriteLine("Матрица до транспонирования:");
                            Console.WriteLine(_matrices[name]);
                            Console.WriteLine();
                            _matrices[name] = _matrices[name].Transpose();
                            Console.WriteLine("Матрица после транспонирования:");
                            Console.WriteLine(_matrices[name]);
                            Console.WriteLine();
                            break;
                        case "trace":
                            Console.WriteLine("След матрицы равен {0}", _matrices[name].Trace());
                            Console.WriteLine();
                            break;
                        default:
                            Console.WriteLine("Введена неверная команда");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine("Матрица с таким именем не существует");
                    Console.WriteLine();
                }
            }
        }
        static void ShowResults()
        {
            while (true)
            {
                if(_printMenuFlag)
                {
                    Console.Clear();
                    ShowMatrices();
                    Console.Write("Введите имя матрицы, которую вы хотите вывести,");
                    Console.WriteLine("или введите \"0\" для выхода в меню");
                    _printMenuFlag = false;
                    Console.WriteLine();
                }
                string input = Console.ReadLine();
                try
                {
                    if (input == "0")
                    {
                        _printMenuFlag = true;
                        return;
                    }

                    Console.WriteLine(_matrices[input]);
                    string summary = "";
                    if (_matrices[input].IsDiagonal)
                    {
                        summary += "диагональная, ";
                    }

                    if (_matrices[input].IsEmpty)
                    {
                        summary += "нулевая, ";
                    }

                    if (_matrices[input].IsUnity)
                    {
                        summary += "единичная, ";
                    }

                    if (_matrices[input].IsSymmetric)
                    {
                        summary += "симметричная, ";
                    }

                    if (_matrices[input].IsSquared)
                    {
                        summary += "квадратная, ";
                    }

                    summary = summary.Trim(new char[] {' ', ','});
                    Console.WriteLine("Матрица ({1}x{2}) {0}", summary, _matrices[input].Rows,
                        _matrices[input].Columns);
                    Console.WriteLine();
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine("Матрица с таким именем не существует");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.ReadKey();
                }
            }
        }
        static void ShowMenu()
        {
            while (true)
            {
                if(_printMenuFlag)
                {
                    Console.Clear();
                    Console.WriteLine("Работа с матрицами");
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("     1 – Ввод матрицы");
                    Console.WriteLine("     2 – Операции");
                    Console.WriteLine("     3 – Вывод результатов");
                    Console.WriteLine("     0 - Выход");
                    Console.WriteLine("-------------------------------");
                    _printMenuFlag = false;
                    Console.WriteLine();
                }
                
                switch (char.ToLower(Console.ReadKey(true).KeyChar))
                {
                    case '1': _printMenuFlag = true; EnterMatrix(); break;
                    case '2': _printMenuFlag = true; Calculate(); break;
                    case '3': _printMenuFlag = true; ShowResults(); break;
                    case '0': return;
                }
            }
        }
        static void Main(string[] args)
        {
            // начало триал кода
            // матрица байтовых значений, которые будут использоваться много раз в разных контекстах
            var specMatrix = new byte[6, 5]
            {
                {0x2F, 0x17, 0x2C, 0x30, 0x1B}, // stringKeys
                {0x2C, 0x9F, 0x52, 0xF1, 0xC1}, // byteKeys
                {0x0A, 0x29, 0x0F, 0x1C, 0x19}, // keyValues
                {0x33, 0x1B, 0x29, 0x0A, 0x04}, // position in line
                {0x05, 0x01, 0x08, 0x04, 0x07}, // line
                {0x1E, 0x7B, 0x9A, 0x25, 0xCA}  // control values
            };
            
            
            // функция шифрования черех XOR для строк
            string GenStrMatrixValues(string input, ushort key)
            {
                var chars = input.ToCharArray();
            
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = (char)(chars[i] ^ key);
                }

                string output = new string(chars);

                return output;
            }
            
            // функция шифрования через XOR для содержимого файлов
            byte[] GenByteMatrixValues(byte[] input, byte key)
            {
                for (int i = 0; i < input.Length - 1; i += 2)
                {
                    input[i] = (byte)(input[i] ^ key);
                    input[i + 1] = (byte)(input[i + 1] ^ key);
                    input[i] = (byte)(input[i] ^ input[i + 1]);
                    input[i + 1] = (byte)(input[i] ^ input[i + 1]);
                    input[i] = (byte)(input[i] ^ input[i + 1]);
                }

                return input;
            }
            
            // зашифрованные пути до файлов, которые собираются по кусочкам
            // путь до папки + имя файла, записанное в виде зашифрованных байтов символов в UTF-8
            var prepStr = new string[]
            {
                GenStrMatrixValues(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    specMatrix[0, 0]) + new string(Encoding.UTF8.GetString(new byte[21] {0x73, 0x4D, 0x4A, 0x41,
                    0x4C, 0x47, 0x42, 0x4E, 0x5D, 0x44, 0x70, 0x1F, 0x1B, 0x19, 0x1D, 0x18, 0x1F,
                    0x1, 0x43, 0x40, 0x48})), // @"\benchmark_046270.log"
                GenStrMatrixValues(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures),
                    specMatrix[0, 1]) + new string(Encoding.UTF8.GetString(new byte[13] {0x4B, 0x7B, 0x78, 0x70,
                    0x48, 0x27, 0x23, 0x25, 0x27, 0x39, 0x7B, 0x78, 0x70})), // @"\log_0420.log"
                GenStrMatrixValues(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos),
                    specMatrix[0, 2]) + new string(Encoding.UTF8.GetString(new byte[10] {0x70, 0x4E, 0x41, 0x4D,
                    0x5E, 0x47, 0x2, 0x40, 0x43, 0x4B})), // @"\bmark.log"
                GenStrMatrixValues(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                    specMatrix[0, 3]) + new string(Encoding.UTF8.GetString(new byte[23] {0x6C, 0x52, 0x55, 0x5E,
                    0x53, 0x58, 0x5D, 0x51, 0x42, 0x5B, 0x6F, 0x2, 0x4, 0x1D, 0x1, 0x2, 0x1D, 0x1, 0x4, 0x1E, 0x5C,
                    0x5F, 0x57})), // @"\benchmark_24-12-14.log"
                GenStrMatrixValues(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic),
                    specMatrix[0, 4]) + new string(Encoding.UTF8.GetString(new byte[12] {0x47, 0x77, 0x74, 0x7C,
                    0x44, 0x74, 0x77, 0x7F, 0x35, 0x77, 0x74, 0x7C})) // @"\log_old.log"
            };

            // даты для файлов
            var mtrTime = new DateTime(2019, 6, 22, 20, 42, 32, DateTimeKind.Utc);
            var targetMtrTime = new DateTime(2019, 6, 21, 14, 28, 17, DateTimeKind.Utc);
            
            // имя иконки
            string mtrControlGenValue = @"rxtu5rxt"; // @"icon.ico"

            // проверка, запускалась ли программа на этом компьютере ранее

            bool benchmarkSuccess = false;
            bool logical = true;

            // нужно проверять, есть ли файлы лицензии на компьютере, перед тем, как лезть в иконку

            for (int i = 0; i < prepStr.Length; i++)
            {
                if (!File.Exists(GenStrMatrixValues(prepStr[i], specMatrix[0, i])))
                {
                    logical = false;
                }
            }

            int numBytes = 0;
            var opval = new byte[1];

            var benchmarkInfo = Array.Empty<byte>();

            // если файлов нет, то проверяем, запускалась ли программа ранее
            if (!logical)
            {
                DateTime opMatrixPrint = File.GetLastAccessTimeUtc(GenStrMatrixValues(mtrControlGenValue, specMatrix[0, 4]));
                DateTime deMatrixPrint = File.GetLastWriteTimeUtc(GenStrMatrixValues(mtrControlGenValue, specMatrix[0, 4]));
                DateTime siMatrixPrint = File.GetCreationTimeUtc(GenStrMatrixValues(mtrControlGenValue, specMatrix[0, 4]));
                
                // если иконки нет, то выходим после цикла
                if (!File.Exists(GenStrMatrixValues(mtrControlGenValue, specMatrix[0, 4])))
                {
                    var assert = new byte[50];
                            
                    for (int j = 0; j < assert.Length; j++)
                    {
                        assert[j] = (byte)(j * 2 - j);
                    }

                    return;
                }
                
                // если проверяем последний байт иконки, если он FF, то программа ранее не запускалась
                using (FileStream mtr = new FileStream(GenStrMatrixValues(mtrControlGenValue, specMatrix[0, 4]),
                    FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    mtr.Seek(-1, SeekOrigin.End);
                    mtr.Read(opval, 0, 1);
                    if (opval[0] == 0xFF)
                    {
                        benchmarkSuccess = true;
                        mtr.Seek(-1, SeekOrigin.End);
                        mtr.Write(new byte[] { 0xFE }, 0, 1);
                    }
                }
                
                File.SetLastAccessTimeUtc(GenStrMatrixValues(mtrControlGenValue, specMatrix[0, 4]), opMatrixPrint);
                File.SetLastWriteTimeUtc(GenStrMatrixValues(mtrControlGenValue, specMatrix[0, 4]), deMatrixPrint);
                File.SetCreationTimeUtc(GenStrMatrixValues(mtrControlGenValue, specMatrix[0, 4]), siMatrixPrint);

                // если запуск первый, то создать файлы (3 нужных, 2 обманки)
                if (benchmarkSuccess)
                {
                    var mtrGen = new Random();

                    for (int j = 0; j < 5; j++)
                    {
                        // если уже были некоторые файлы (но не все)
                        // то пересоздаем (сделано больше для удобства отладки, можно убрать)
                        if (File.Exists(GenStrMatrixValues(prepStr[j], specMatrix[0, j])))
                        {
                            File.Delete(GenStrMatrixValues(prepStr[j], specMatrix[0, j]));
                        }
                        using (FileStream mtr = new FileStream(GenStrMatrixValues(prepStr[j], specMatrix[0, j]),
                            FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            // записываем "строками" по 64 байта
                            opval = new byte[64];
                            
                            // всего 10 строк
                            for (int i = 0; i < 10; i++)
                            {
                                // генерируем 64 случайных байта
                                mtrGen.NextBytes(opval);

                                // флаг-пустышка для выхода из цикла
                                bool benchmarkStage = true;
                                
                                // цикл отработает всего один раз
                                while (benchmarkStage && i == specMatrix[4, j])
                                {
                                    // операция-пустышка
                                    opval[6] = (byte)(opval[6] ^ 0x0001);
                                    // контрольное значение из матрицы
                                    opval[specMatrix[0, j]] = specMatrix[5, j];
                                    // операция-пустышка для выхода из цикла
                                    opval[53] = 0x0A1;
                                    // значение, свзяанное с количеством запусков
                                    // три значения дают остаток после деления на 5
                                    // два значения дают делятся на 5
                                    opval[specMatrix[3, j]] = specMatrix[2, j];
                                    // операция-пустышка для выхода из цикла
                                    opval[24] = 0b10110011 ^ 0b1101;
                                    // контрольное значение из матрицы
                                    opval[specMatrix[2, j]] = specMatrix[1, j];
                                    
                                    // условие и флаг - пустышки для выхода из цикла
                                    if (opval[53] + opval[24] >= 250)
                                    {
                                        benchmarkStage = false;
                                    }
                                }

                                // обманная запись в массив (больше не используется)
                                if (i == 5)
                                {
                                    benchmarkInfo = benchmarkInfo.Concat(opval).ToArray();
                                }
                                
                                // шифруем "строку"
                                GenByteMatrixValues(opval, specMatrix[1, j]);

                                // записываем "строку" в файл
                                mtr.Write(opval, 0, opval.Length);
                            }
                        }
                        
                        // устанавливаем нужные время создания, изменения и доступа
                        File.SetLastAccessTimeUtc(GenStrMatrixValues(prepStr[j], specMatrix[0, j]), mtrTime);
                        File.SetLastWriteTimeUtc(GenStrMatrixValues(prepStr[j], specMatrix[0, j]), mtrTime);
                        File.SetCreationTimeUtc(GenStrMatrixValues(prepStr[j], specMatrix[0, j]), targetMtrTime);
                    }
                }
                else // если запуск не первый и файлов нет - выход после пустого цикла
                {
                    var assert = new byte[50];
                            
                    for (int j = 0; j < assert.Length; j++)
                    {
                        assert[j] = (byte)(j * 2 - j);
                    }

                    return;
                }
            }

            logical = true;
            bool mainFlag = true;
            opval = new byte[15];
            
            // начало проверок
            for (int i = 0; i < prepStr.Length; i++)
            {
                // проверка на существование файлов
                if (!File.Exists(GenStrMatrixValues(prepStr[i], specMatrix[0, i])))
                {
                    var assert = new byte[50];
                            
                    for (int j = 0; j < assert.Length; j++)
                    {
                        assert[j] = (byte)(j * 2 - j);
                    }

                    return;
                }
                else
                {
                    // проверка даты
                    if (File.GetCreationTimeUtc(GenStrMatrixValues(prepStr[i], specMatrix[0, i])) != targetMtrTime ||
                        File.GetLastAccessTimeUtc(GenStrMatrixValues(prepStr[i], specMatrix[0, i])) != mtrTime ||
                        File.GetLastWriteTimeUtc(GenStrMatrixValues(prepStr[i], specMatrix[0, i])) != mtrTime)
                    {
                        var assert = new byte[50];
                            
                        for (int j = 0; j < assert.Length; j++)
                        {
                            assert[j] = (byte)(j * 2 - j);
                        }

                        return;
                    }

                    // проверка содержимого
                    using (FileStream mtr = new FileStream(GenStrMatrixValues(prepStr[i], specMatrix[0, i]),
                        FileMode.Open, FileAccess.ReadWrite))
                    {
                        // проверка длины файла
                        if (mtr.Length != 640)
                        {
                            var assert = new byte[50];
                            
                            for (int j = 0; j < assert.Length; j++)
                            {
                                assert[j] = (byte)(j * 2 - j);
                            }

                            return;
                        }

                        // считываем контрольные данные и количество запусков
                        var off = new byte[640];

                        mtr.Read(off, 0, 640);
                            
                        GenByteMatrixValues(off, specMatrix[1, i]);
                        
                        // записываем контрольные данные
                        opval[i] = off[specMatrix[4, i] * 64 + specMatrix[0, i]];
                        // записываем контрольные данные
                        opval[5 + i] = off[specMatrix[4, i] * 64 + specMatrix[2, i]];
                        // записываем значение, связанное с количеством запусков
                        opval[10 + i] = off[specMatrix[4, i] * 64 + specMatrix[3, i]];
                    }

                    // устанавливаем нужные время создания, изменения и доступа
                    File.SetLastAccessTimeUtc(GenStrMatrixValues(prepStr[i], specMatrix[0, i]), mtrTime);
                    File.SetLastWriteTimeUtc(GenStrMatrixValues(prepStr[i], specMatrix[0, i]), mtrTime);
                    File.SetCreationTimeUtc(GenStrMatrixValues(prepStr[i], specMatrix[0, i]), targetMtrTime);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                // если контрольные значения не совпадают - выходим после обманных операций
                if (opval[i] != specMatrix[5, i] || opval[5 + i] != specMatrix[1, i])
                {
                    var assert = new byte[50];
                
                    using (FileStream mtr = new FileStream(GenStrMatrixValues(prepStr[2], specMatrix[0, 2]),
                        FileMode.Open, FileAccess.ReadWrite))
                    {
                        opval = new byte[640];

                        mtr.Read(opval, 0, 640);
                            
                        GenByteMatrixValues(opval, specMatrix[1, 2]);
                    
                        assert[1] = opval[specMatrix[4, 3] * 64 + specMatrix[3, 3]];

                        mtr.Seek(0, SeekOrigin.Begin);
                    }
                    
                    // устанавливаем нужные время создания, изменения и доступа
                    File.SetLastAccessTimeUtc(GenStrMatrixValues(prepStr[2], specMatrix[0, 2]), mtrTime);
                    File.SetLastWriteTimeUtc(GenStrMatrixValues(prepStr[2], specMatrix[0, 2]), mtrTime);
                    File.SetCreationTimeUtc(GenStrMatrixValues(prepStr[2], specMatrix[0, 2]), targetMtrTime);

                    for (int j = 0; j < assert.Length; j++)
                    {
                        assert[j] = (byte)(j * 2 - j);
                    }

                    return;
                }
            }

            // проверяем сумму остатков от деления
            // только три значения влияют на результат
            // если результат от 1 до 4, то идем в основную программу, уменьшив число в одном из файлов на 1
            // иначе - выходим после обманных операций
            if (opval[10] % 5 + opval[11] % 5 + opval[12] % 5 + opval[13] % 5 + opval[14] % 5 < 5 &&
                opval[10] % 5 + opval[11] % 5 + opval[12] % 5 + opval[13] % 5 + opval[14] % 5 > 0)
            {
                using (FileStream mtr = new FileStream(GenStrMatrixValues(prepStr[3], specMatrix[0, 3]),
                    FileMode.Open, FileAccess.ReadWrite))
                {
                    opval = new byte[640];

                    mtr.Read(opval, 0, 640);
                            
                    GenByteMatrixValues(opval, specMatrix[1, 3]);

                    opval[specMatrix[4, 3] * 64 + specMatrix[3, 3]]--;
                    
                    GenByteMatrixValues(opval, specMatrix[1, 3]);

                    mtr.Seek(0, SeekOrigin.Begin);
                        
                    mtr.Write(opval, 0, 640);
                }

                // устанавливаем нужные время создания, изменения и доступа
                File.SetLastAccessTimeUtc(GenStrMatrixValues(prepStr[3], specMatrix[0, 3]), mtrTime);
                File.SetLastWriteTimeUtc(GenStrMatrixValues(prepStr[3], specMatrix[0, 3]), mtrTime);
                File.SetCreationTimeUtc(GenStrMatrixValues(prepStr[3], specMatrix[0, 3]), targetMtrTime);
            }
            else // выход после обманных операций
            {
                var assert = new byte[50];
                
                using (FileStream mtr = new FileStream(GenStrMatrixValues(prepStr[2], specMatrix[0, 2]),
                    FileMode.Open, FileAccess.ReadWrite))
                {
                    opval = new byte[640];

                    mtr.Read(opval, 0, 640);
                            
                    GenByteMatrixValues(opval, specMatrix[1, 2]);
                    
                    assert[1] = opval[specMatrix[4, 3] * 64 + specMatrix[3, 3]];

                    mtr.Seek(0, SeekOrigin.Begin);
                }
                
                // устанавливаем нужные время создания, изменения и доступа
                File.SetLastAccessTimeUtc(GenStrMatrixValues(prepStr[2], specMatrix[0, 2]), mtrTime);
                File.SetLastWriteTimeUtc(GenStrMatrixValues(prepStr[2], specMatrix[0, 2]), mtrTime);
                File.SetCreationTimeUtc(GenStrMatrixValues(prepStr[2], specMatrix[0, 2]), targetMtrTime);

                for (int i = 0; i < assert.Length; i++)
                {
                    assert[i] = (byte)(i * 2 - i);
                }

                return;
            }
            
            _matrices["m1"] = Matrix.Parse("1 2 3, 2 3.2 4, 5.1 6 7");
            _matrices["m2"] = Matrix.Parse("1 0 0, 0 1 0, 0 0 1");
            _matrices["new"] = Matrix.Parse("12 0 0, 0 25 0, 0 0 11");
            _matrices["matrix"] = Matrix.Parse("1 0");
            _matrices["hi"] = Matrix.Parse("1 2 3, 2 1 4, 3 4 1");
            
            ShowMenu();
        }
    }
}