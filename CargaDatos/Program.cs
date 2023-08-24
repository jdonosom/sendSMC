using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CargaDatos
{
    internal class Program
    {
        //
        // 
        //
        static string fileout;
        static string filein;
        static bool conEncabezado;
        static bool lnofileout;

        static void Main(string[] args)
        {
            DisplayOwner();

            if (args.Length == 0)
            {
                Console.WriteLine("No se ha proporcoionado el archivo a procesar");
            }

            filein = args[0];
            var file = Path.GetFileName(filein);
            var path = Path.GetDirectoryName(filein);
            if (string.IsNullOrEmpty(path)) path = System.IO.Directory.GetCurrentDirectory();
            filein = $"{path}\\{file}";

            if (!File.Exists(filein))
            {
                Console.WriteLine("Archivo origen no existe!");
                Environment.Exit(1);
            }

            // Archivo salida
            path = Path.GetDirectoryName(filein);
            fileout = Path.GetFileNameWithoutExtension(filein);
            if (string.IsNullOrEmpty(path)) path = System.IO.Directory.GetCurrentDirectory();
            fileout = $"{path}\\{fileout}.txt";

            conEncabezado = true;

            //int index = 0;
            //foreach (string arg in args)
            //{
            //    conEncabezado = true;
            //
            //    // Hacer algo
            //    if (arg.Equals("-ne"))
            //        conEncabezado = false;
            //
            //    if (arg.Equals("-in"))
            //    {
            //        filein = args[index + 1];
            //        var file = Path.GetFileName(filein);
            //        var path = Path.GetDirectoryName(filein);
            //        if (string.IsNullOrEmpty(path)) path = System.IO.Directory.GetCurrentDirectory();
            //        filein = $"{path}\\{file}";
            //
            //        if (!File.Exists(filein))
            //        {
            //            Console.WriteLine("Archivo origen no existe!");
            //            Environment.Exit(1);
            //        }
            //    }
            //
            //    if (arg.StartsWith("-out"))
            //    {
            //        lnofileout = true;
            //        fileout = args[index + 1];
            //        var path = Path.GetDirectoryName(fileout);
            //        fileout = Path.GetFileNameWithoutExtension(fileout);
            //
            //        if (string.IsNullOrEmpty(path)) path = System.IO.Directory.GetCurrentDirectory();
            //        fileout = $"{path}\\{fileout}.txt";
            //    }
            //    index++;
            //}
            //
            //if (!lnofileout)
            //{
            //    var file = Path.GetFileName(filein);
            //    var path = Path.GetDirectoryName(filein);
            //    fileout = $"{path}\\{file}.txt";
            //
            //}

            CargaDatos(filein, fileout);
        }

        private static void CargaDatos(string filecsv, string fileout)
        {
            IEnumerable<string> lines =
                File.ReadAllLines(filecsv);

            string buffer = null;
            Console.WriteLine($"Se procesarán {lines.Count()} marcaciones.");
            Console.WriteLine($"Presione una tecla para continuar...");
            var pos = Console.GetCursorPosition();
            Console.ReadKey();
            
            int nline = 0;
            foreach (string line in lines)
            {
                if (nline == 0 && conEncabezado)
                {
                    nline++;
                    continue;
                }
                var datos = line.Split(',');
                if (datos.Length.Equals(4))
                {
                    var Id = int.Parse(datos[0]);
                    var Fecha = Convert.ToDateTime(datos[1]).ToString("yyyyMMdd");
                    var Time = Convert.ToDateTime(datos[2]).ToString("HHmm");
                    var Tipo = datos[3].Contains("Salida") ? 0 : 1;

                    // Console.WriteLine($"{Id} {Fecha} {Time} {Tipo}");
                    Console.SetCursorPosition(pos.Left, pos.Top + 2);
                    Console.Write($"Registros procesados: {nline}");
                    var newLine = $"{Id} {Fecha} {Time} {Tipo}{(nline == lines.Count() - 1 ? "" : "\n")}";
                    buffer += newLine;
                }
                
                nline++;
            }

            //
            using (FileStream fs = File.Create(fileout))
            {
                byte[] newLine = new UTF8Encoding(true).GetBytes(buffer);
                fs.Write(newLine, 0, newLine.Length);
            }
        }


        /// <summary>
        /// Desplegar datos del dueño de la solución
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
         private static void DisplayOwner()
        {
            Console.Clear();
            var msg = $"{AppDomain.CurrentDomain.FriendlyName} - Xxauro 2020\r\n" +
                "Desarrollado por José Patricio Donoso Moscoso, email: jpdonosom@gmail.com\r\n" +
                "Xxauro 2020 derechos reservados.\r\n\r\n";

            Console.Write(msg);

        }
    }
}
