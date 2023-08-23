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
            int index = 0;
            foreach (string arg in args)
            {
                conEncabezado = true;

                // Hacer algo
                if (arg.Equals("-ne"))
                    conEncabezado = false;

                if (arg.Equals("-in"))
                {
                    filein = args[index + 1];
                    var file = Path.GetFileName(filein);
                    var path = Path.GetDirectoryName(filein);
                    if (string.IsNullOrEmpty(path)) path = System.IO.Directory.GetCurrentDirectory();
                    filein = $"{path}\\{file}";

                    if (!File.Exists(filein))
                    {
                        Console.WriteLine("Archivo origen no existe!");
                        Environment.Exit(1);
                    }
                }

                if (arg.StartsWith("-out"))
                {
                    lnofileout = true;
                    fileout = args[index + 1];
                    var path = Path.GetDirectoryName(fileout);
                    fileout = Path.GetFileNameWithoutExtension(fileout);

                    if (string.IsNullOrEmpty(path)) path = System.IO.Directory.GetCurrentDirectory();
                    fileout = $"{path}\\{fileout}.txt";
                }
                index++;
            }

            if (!lnofileout)
            {
                var file = Path.GetFileName(filein);
                var path = Path.GetDirectoryName(filein);
                fileout = $"{path}\\{file}.txt";

            }

            CargaDatos(filein, fileout);
        }

        private static void CargaDatos(string filecsv, string fileout)
        {
            IEnumerable<string> lines =
                File.ReadAllLines(filecsv);

            string buffer = null;
            Console.WriteLine($"Se procesarán {lines.Count()} marcaciones.");

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
                    var Tipo = datos[3].Equals("Salida") ? 0 : 1;

                    Console.WriteLine($"{Id} {Fecha} {Time} {Tipo}");
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
    }
}
