using System;
using System.IO;


namespace LoopThroughFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryName;

            if (args.Length == 0)
            {
                // Получаем имя текущего каталога
                directoryName = Directory.GetCurrentDirectory();
            } else {
                // в противном случае рассматриваем первый аргумент 
                // в качестве имени рабочего каталога 
                directoryName = args[0];
            }

            Console.WriteLine(directoryName);
            // Получаем список файлов в каталоге
            FileInfo[] files = GetFileList(directoryName);

            foreach(var file in files)
            {
                Console.WriteLine("Имя файла {0}", file.FullName);
                // вывод дампа файлав в 16 исчеслении
                DumpHex(file);
                // Ожидание перед выводом следующего файла .
                Console.WriteLine("\nHaжмитe <Enter> для продолжения");
                Console.ReadLine();
            }
            // Работа сделана !
            Console.WriteLine("Фaйлoв больше нет");
            // Ожидание подтверждения пользователя
            Console.WriteLine("Haжмитe <Enter> для " +
                "завершения программы . . . ");
            Console.Read();
        }
        /// <summary>
        /// Вывод информации для заданого файла
        /// </summary>
        /// <param name="file"></param>
        private static void DumpHex(FileInfo file)
        {
            FileStream fs;
            BinaryReader reader;
            try
            {
                fs = file.OpenRead();
                reader = new BinaryReader(fs);
            }
            catch(Exception ex)
            {
                Console.WriteLine("\nHe могу читать \" " +
                                file.FullName + "\"");
                Console.WriteLine(ex.Message);
                return;
            }

            for (int line = 1; true; line++)
            {
                byte[] buffer = new byte[10];

                int numBytes = reader.Read(buffer, 0, buffer.Length);
                
                if (numBytes == 0)
                {
                    return;
                }

                Console.Write("{0:D3} - ", line);
                DumpBuffer(buffer, numBytes);

                if((line % 20) == 0)
                {
                    Console.WriteLine("Haжмитe <Enter> для вывода " +
                        "очередных 20 строк");
                    Console.ReadLine();
                }
            }
        }
        /// <summary>
        /// Вывод буфера символов в виде единой строки в 16-ом формате
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="numBytes"></param>
        private static void DumpBuffer(byte[] buffer, int numBytes)
        {
           for (int i = 0; i < numBytes; i++)
            {
                byte b = buffer[i];
                Console.Write("{0:X2}, ", b);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Получение списка всех файлов в указанном каталоге
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        private static FileInfo[] GetFileList(string directoryName)
        {
            FileInfo[] files = new FileInfo[0];
            try
            {
                DirectoryInfo di = new DirectoryInfo(directoryName);
                files = di.GetFiles();
            } catch (Exception ex)
            {
                Console.WriteLine("Kaтaлoг \"" + directoryName +
                    "\" неверен");

                Console.WriteLine(ex.Message);
            }
            return files;
        }
    }
}
