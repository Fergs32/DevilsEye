using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Dox.UI.AsciiMenu.DoxBinLayouts.DoxModels
{
    public class Model2
    {
        public static void Get()
        {
            Console.Clear();
            string[] art = File.ReadAllLines("Misc/DoxBinLayouts/GrimReaper.txt");
            foreach (string line in art)
            {
                Console.WriteLine(line, Color.Purple);
            }
            Console.Write("\n\n\n");
        }
    }
}
