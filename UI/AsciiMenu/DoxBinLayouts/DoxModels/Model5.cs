using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace Dox.UI.AsciiMenu.DoxBinLayouts.DoxModels
{
    public class Model5
    {
        public static void Get()
        {
            Console.Clear();
            string[] art = File.ReadAllLines("Misc/DoxBinLayouts/Model2im.txt");
            foreach (string line in art)
            {
                Console.WriteLine(line, Color.Purple);
            }
            Console.Write("\n\n\n");
        }
    }
}
