using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace ProcPuntoSeguro
{
    internal class Program
    {            
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

            ProcesarDatos(filein, fileout);


        }

        private static void ProcesarDatos(string filein, string fileout)
        {
            //IEnumerable<string> lines = File.ReadAllLines(filein);
            var lines = File.ReadAllLines(filein);

            string buffer = null;
            Console.WriteLine($"Se procesarán {lines.Count()} marcaciones.");
            Console.WriteLine($"Presione una tecla para continuar...");
            var pos = Console.GetCursorPosition();
            Console.ReadKey();

            int index = 0;
            while (index <= lines.Count()-1)
            {
                string line = lines[index];
                var datos = line.Split(';');
                int.TryParse(datos[0], out int result);

                if (result == 0)
                {
                    index++;
                    continue;
                }
                var Id = int.Parse(datos[0]);

                while (Id == int.Parse(datos[0]) && index <= lines.Count()-1 )
                {
                    line = lines[index];
                    datos = line.Split(';');

                    Id = int.Parse(datos[0]);
                    var Fecha = datos[1];
                    var HoraE = datos[2];
                    var HoraS = datos[3];

                    try
                    {
                        Fecha = DateTime.Parse(datos[1]).ToString("yyyyMMdd");
                    }
                    catch (Exception)
                    {
                        Fecha = null;
                    }

                    if ( Fecha == "20190702") 
                    {
                        var a = 1;
                    }

                    try
                    {
                        HoraE = DateTime.Parse(datos[2]).ToString("HHmm");
                    }
                    catch (Exception)
                    {
                        HoraE = null;
                    }

                    try
                    {
                        HoraS = DateTime.Parse(datos[3]).ToString("HHmm");
                    }
                    catch (Exception)
                    {
                        HoraS=null;
                    }

                    if (index == 1252)
                    {
                        var b = 1;
                    }

                    if ( Id > 0 && Fecha != null && (HoraE != null || HoraS != null) )
                    {
                        if (HoraE != null)
                        {
                            buffer += $"{Id} {Fecha} {HoraE} 1{(index == lines.Count() - 1 ? (HoraS.Length > 0 ? "\n" : "") : "\n")}";
                        }
                        if (HoraS != null)
                        {
                            buffer += $"{Id} {Fecha} {HoraS} 0{(index == lines.Count() - 1 ? "" : "\n")}";
                        }
                    }
                    // Console.WriteLine($"{Id} {Fecha} {Time} {Tipo}");
                    Console.SetCursorPosition(pos.Left, pos.Top + 2);
                    Console.Write($"Registros procesados: {index}");
                    index++;
                }

            }
            // Grabar archivo
            using (FileStream fs = File.Create(fileout))
            {
                byte[] newLine = new UTF8Encoding(true).GetBytes(buffer);
                fs.Write(newLine, 0, newLine.Length);
            }

            Console.WriteLine($"Se ha generado el archivo de carga a SMC en la siguiente ruta: {fileout}.");
            Console.WriteLine($"Presione una tecla para terminar...");
            Console.ReadKey();

        }

        private static void DisplayOwner()
        {
            Console.Clear();
            var msg = $"{AppDomain.CurrentDomain.FriendlyName} - Xxauro 2020\r\n" +
                "Desarrollado por José Patricio Donoso Moscoso, email: jpdonosom@gmail.com\r\n" +
                "Procesa las marcaciones informadas por Punto Seguro y las prepara para la carga en Sistemas SMC\n\r" +
                "Xxauro 2020 derechos reservados.\r\n\r\n";
            Console.Write(msg);

        }

    }


}
