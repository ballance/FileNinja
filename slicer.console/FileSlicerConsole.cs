using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace slicer.console
{
    public class FileSlicerConsole
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Preparing to slice a file...");
            Console.ReadKey();
            var fileSlicer = new FileSlicer();
            fileSlicer.Slice(@"..\..\..\files\excel.xlsxs");
            Console.WriteLine("File was sliced!");

            Console.WriteLine();
            Console.WriteLine("Press any key to reassemble...");
            
            Console.ReadKey();
            Console.WriteLine();
 
            fileSlicer.Reassemble();

            Console.WriteLine("File was reassembled!");
            Console.ReadKey();


        }
    }
}
