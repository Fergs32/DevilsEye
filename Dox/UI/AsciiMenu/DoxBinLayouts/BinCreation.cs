using System.Collections.Generic;
using System.Text;
using Colorful;
using System.Drawing;
using Dox.UI.AsciiMenu.DoxBinLayouts.DoxModels;
using Dox.AsciiMenu;

namespace Dox.UI.AsciiMenu.DoxBinLayouts
{
    public class BinCreation
    {
        public static void DoxList()
        {
            Console.Clear();
            Menu.GetTitle();
            Colorful.Console.WriteLine("\nThis program is for educational and development purposes only, use at your OWN will.", Color.WhiteSmoke);
            Console.Write("\n[Dox Bin Layouts]\n\n[1] Example 1\n[2] Example 1\n[3] Example 1\n[4] Example 1\n[5] Example 1\n[6] Example 1\n", Color.WhiteSmoke);
            Console.Write("\n[+] Option: ", Color.DarkMagenta); int opt = int.Parse(Console.ReadLine());
            switch(opt)
            {
                case 1:
                    Model1.Get();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                default:
                    Console.WriteLine("Incorrect input");
                    break;
            }
        }
    }
}
