using System.Drawing;
using System.Threading;

namespace Dox.AsciiMenu
{
    public class Menu
    {
        public static void GetTitle()
        {
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
            foreach (string line in Logo)
            {
                Colorful.Console.WriteLine(line, Color.DarkMagenta);
            }
        }

        public static void PooheadMenu()
        {
            var LogoV2 = new string[]
            {
                "",
                "",
                                 "\t▄▄▄█████▓▓█████  ███▄ ▄███▓ ██▓███   ███▄ ▄███▓ ▄▄▄       ██▓ ██▓",
                                "\t▓  ██▒ ▓▒▓█   ▀ ▓██▒▀█▀ ██▒▓██░  ██▒▓██▒▀█▀ ██▒▒████▄    ▓██▒▓██▒ ",
                                "\t▒ ▓██░ ▒░▒███   ▓██    ▓██░▓██░ ██▓▒▓██    ▓██░▒██  ▀█▄  ▒██▒▒██░ ",
                                "\t░ ▓██▓ ░ ▒▓█  ▄ ▒██    ▒██ ▒██▄█▓▒ ▒▒██    ▒██ ░██▄▄▄▄██ ░██░▒██░    ",
                                "\t  ▒██▒ ░ ░▒████▒▒██▒   ░██▒▒██▒ ░  ░▒██▒   ░██▒ ▓█   ▓██▒░██░░██████▒",
                                "\t  ▒ ░░   ░░ ▒░ ░░ ▒░   ░  ░▒▓▒░ ░  ░░ ▒░   ░  ░ ▒▒   ▓▒█░░▓  ░ ▒░▓  ░",
                                "\t      ░     ░ ░  ░░  ░      ░░▒ ░     ░  ░      ░  ▒   ▒▒ ░ ▒ ░░ ░ ▒  ",
                                "\t   ░      ░   ░░       ░      ░     ░   ▒    ▒ ░  ░ ░   ",
                                "\t   ░  ░       ░                   ░         ░  ░ ░      ░  ░",
                                "",
                                "",
                                "",
            };
            foreach (string line in LogoV2)
            {
                Colorful.Console.WriteLine(line, Color.DarkMagenta);
            }
        }

        public static void PhonePrint()
        {
            var PhonePrintLogo = new string[]
            {
                "",
                "",
                               "\t ██▓███   ██░ ██  ▒█████   ███▄    █ ▓█████  ██▓███   ██▀███   ██▓ ███▄    █ ▄▄▄█████▓",
                               "\t▓██░  ██▒▓██░ ██▒▒██▒  ██▒ ██ ▀█   █ ▓█   ▀ ▓██░  ██▒▓██ ▒ ██▒▓██▒ ██ ▀█   █ ▓  ██▒ ▓▒",
                               "\t ▓██░ ██▓▒▒██▀▀██░▒██░  ██▒▓██  ▀█ ██▒▒███   ▓██░ ██▓▒▓██ ░▄█ ▒▒██▒▓██  ▀█ ██▒▒ ▓██░ ▒░",
                               "\t ▒██▄█▓▒ ▒░▓█ ░██ ▒██   ██░▓██▒  ▐▌██▒▒▓█  ▄ ▒██▄█▓▒ ▒▒██▀▀█▄  ░██░▓██▒  ▐▌██▒░ ▓██▓ ░",
                               "\t ▒██▒ ░  ░░▓█▒░██▓░ ████▓▒░▒██░   ▓██░░▒████▒▒██▒ ░  ░░██▓ ▒██▒░██░▒██░   ▓██░  ▒██▒ ░",
                               "\t ▒▓▒░ ░  ░ ▒ ░░▒░▒░ ▒░▒░▒░ ░ ▒░   ▒ ▒ ░░ ▒░ ░▒▓▒░ ░  ░░ ▒▓ ░▒▓░░▓  ░ ▒░   ▒ ▒   ▒ ░░",
                               "\t ░▒ ░      ▒ ░▒░ ░  ░ ▒ ▒░ ░ ░░   ░ ▒░ ░ ░  ░░▒ ░       ░▒ ░ ▒░ ▒ ░░ ░░   ░ ▒░    ░",
                               "\t ░░        ░  ░░ ░░ ░ ░ ▒     ░   ░ ░    ░   ░░         ░░   ░  ▒ ░   ░   ░ ░   ░",
                               "\t           ░  ░  ░    ░ ░           ░    ░  ░            ░      ░           ░",
                               "",
                               "",
            };
            foreach (string line in PhonePrintLogo)
            {
                Colorful.Console.WriteLine(line, Color.DarkMagenta);
            }
        }
        public static void Illict()
        {
            var IllictLogo = new string[]
            {
                "",
                "",
"██▓ ██▓     ██▓     ██▓ ▄████▄  ▄▄▄█████▓     ██████ ▓█████  ██▀███   ██▒   █▓ ██▓ ▄████▄  ▓█████   ██████ ",
"▓██▒▓██▒    ▓██▒    ▓██▒▒██▀ ▀█  ▓  ██▒ ▓▒   ▒██    ▒ ▓█   ▀ ▓██ ▒ ██▒▓██░   █▒▓██▒▒██▀ ▀█  ▓█   ▀ ▒██    ▒ ",
"▒██▒▒██░    ▒██░    ▒██▒▒▓█    ▄ ▒ ▓██░ ▒░   ░ ▓██▄   ▒███   ▓██ ░▄█ ▒ ▓██  █▒░▒██▒▒▓█    ▄ ▒███   ░ ▓██▄   ",
"░██░▒██░    ▒██░    ░██░▒▓▓▄ ▄██▒░ ▓██▓ ░      ▒   ██▒▒▓█  ▄ ▒██▀▀█▄    ▒██ █░░░██░▒▓▓▄ ▄██▒▒▓█  ▄   ▒   ██▒",
"░██░░██████▒░██████▒░██░▒ ▓███▀ ░  ▒██▒ ░    ▒██████▒▒░▒████▒░██▓ ▒██▒   ▒▀█░  ░██░▒ ▓███▀ ░░▒████▒▒██████▒▒",
"░▓  ░ ▒░▓  ░░ ▒░▓  ░░▓  ░ ░▒ ▒  ░  ▒ ░░      ▒ ▒▓▒ ▒ ░░░ ▒░ ░░ ▒▓ ░▒▓░   ░ ▐░  ░▓  ░ ░▒ ▒  ░░░ ▒░ ░▒ ▒▓▒ ▒ ░",
" ▒ ░░ ░ ▒  ░░ ░ ▒  ░ ▒ ░  ░  ▒       ░       ░ ░▒  ░ ░ ░ ░  ░  ░▒ ░ ▒░   ░ ░░   ▒ ░  ░  ▒    ░ ░  ░░ ░▒  ░ ░",
" ▒ ░  ░ ░     ░ ░    ▒ ░░          ░         ░  ░  ░     ░     ░░   ░      ░░   ▒ ░░           ░   ░  ░  ░  ",
" ░      ░  ░    ░  ░ ░  ░ ░                        ░     ░  ░   ░           ░   ░  ░ ░         ░  ░      ░  ",
"                        ░                                                  ░       ░                        ",
"                                         Credit -  Telegram: @miyakoyako",
            };
            foreach (string line in IllictLogo)
            {
                Colorful.Console.WriteLine(line, Color.DarkMagenta);
            }
        }

        public static void ReturnMenu()
        {
            Colorful.Console.Write("\n[+] Would you like to return to Menu? (Y/N): ");
            string URply = Colorful.Console.ReadLine();
            switch (URply)
            {
                case "Y":
                    Program.Main();
                    break;

                case "N":
                    Thread.Sleep(-1);
                    break;

                default:
                    Colorful.Console.WriteLine("[Error] Invalid answer", Color.Red);
                    break;
            }
        }
    }
}