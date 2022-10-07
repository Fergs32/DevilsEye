using System;
using System.Drawing;
using Colorful;
using System.Threading;
using Dox.UI.AsciiMenu.DoxBinLayouts;

namespace Dox.AsciiMenu 
{
    public class DoxBinMenu 
    {

        private static int Option;
        public static void Setup() 
        {
            Colorful.Console.Clear();
            var Logo = new string[]
	        {
				"			    .o oOOOOOOOo                                            OOOo",
				"			   Ob.OOOOOOOo  OOOo.      oOOo.                      .adOOOOOOO",
				@"			    OboO"""""""""""""""""""""""".OOo. .oOOOOOo.    OOOo.oOOOOOo..""""""""""""""""""""""'OO",
				@"			   OOP.oOOOOOOOOOOO ""POOOOOOOOOOOo.   `""OOOOOOOOOP,OOOOOOOOOOOB'",
				@"			   `O'OOOO'     `OOOOo""OOOOOOOOOOO` .adOOOOOOOOO""oOOO'    `OOOOo",
				"			    .OOOO'            `OOOOOOOOOOOOOOOOOOOOOOOOOO'            `OO",
				@"			    OOOOO       <>       '""OOOOOOOOOOOOOOOO""`    <>         oOO",
				"			   oOOOOOba.                .adOOOOOOOOOOba               .adOOOOo.",
				"			  oOOOOOOOOOOOOOba.    .adOOOOOOOOOO@^OOOOOOOba.     .adOOOOOOOOOOOO",
				@"			 OOOOOOOOOOOOOOOOO.OOOOOOOOOOOOOO""`  '""OOOOOOOOOOOOO.OOOOOOOOOOOOOO",
				@"			 OOOOOOOOOOOOOOOOO.OOOOOOOOOOOOOO""`  '""OOOOOOOOOOOOO.OOOOOOOOOOOOOO",
				"			    Y           'OOOOOOOOOOOOOO: .oOOo. :OOOOOOOOOOO?'         :`",
				"			    :            .oO%OOOOOOOOOOo.OOOOOO.oOOOOOOOOOOOO?         .",
				"			    :            .oO%OOOOOOOOOOo.OOOOOO.oOOOOOOOOOOOO?         .",
				@"				                '%o  OOOO""%OOOO%""%OOOOO""OOOOOO""OOO':""",
				@"			                    `$""  `OOOO' `O""Y ' `OOOO'  o             .",
				@"			 .                  .     OP""          : o.",
				"			                             :",
				"			                            .",

	        };
            
			foreach(string line in Logo)
            {
				Colorful.Console.WriteLine(line, Color.DarkMagenta);
            }
            do
			{
				Colorful.Console.WriteLine("\nThis program is for educational and development purposes only, use at your OWN will.", Color.WhiteSmoke); Thread.Sleep(300);
				Colorful.Console.WriteLine("Developed by Fergs32 / Sh1tters |\n ", Color.WhiteSmoke); Thread.Sleep(300);
				Colorful.Console.WriteLine("[ Doxbin Options ]", Color.WhiteSmoke);
				Colorful.Console.WriteLine("[1] Create bin", Color.WhiteSmoke);
				Colorful.Console.Write("\n[+] Option: ", Color.WhiteSmoke);


			} while (!int.TryParse(Colorful.Console.ReadLine(), out DoxBinMenu.Option));
			
			switch(DoxBinMenu.Option)
            {
				case 1:
					BinCreation.DoxList();
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
				case 8:
					break;
				default:
					Colorful.Console.WriteLine("Invalid option selected, closing...");
					Environment.Exit(0);
					break;
			}
        }
    }
}