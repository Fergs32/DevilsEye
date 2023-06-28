using Colorful;
using System.Drawing;
using System.IO;

namespace Dox.UI.AsciiMenu.DoxBinLayouts.DoxModels
{
    public class Model1
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
            PreviewInformation();
        }

        private static void PreviewInformation()
        {
            var DoxInfo = new string[]
            {
            "------------------------------------Reason For Dox------------------------------------------------------",
            "",
            "- Example",
            "- Example",
            "- Example",
            "",
            "------------------------------------Personal Information------------------------------------------------",
            "",
            "Name:",
            "Address:",
            "DOB:",
            "Number:",
            "Photo 1",
            "Photo 2",
            "Photo 3",
            "",
            "------------------------------------Family Information--------------------------------------------------",
            "",
            "Moms name:  | Age: ",
            "Fathers Name:  | Age: ",
            "Siblings List:  | Age(s): ",
            "Family Relations: ",
            "Address(s):",
            "Number(s): ",
            "",
            "-----------------------------------Social Information---------------------------------------------------",
            "",
            "* Twitter",
            "-----------------------------------",
            "- Example",
            "___________________________________",
            "* Facebook",
            "-----------------------------------",
            "- Example",
            "___________________________________",
            "* Instagram",
            "-----------------------------------",
            "- Example",
            "___________________________________",
            "* Youtube",
            "-----------------------------------",
            "- Example",
            "___________________________________",
            "* NameMC",
            "-----------------------------------",
            "- Example",
            "___________________________________",
            "* Reddit",
            "-----------------------------------",
            "- Example",
            "___________________________________",
            "* Discord",
            "-----------------------------------",
            "Discord: Example#0001",
            "Discord ID: - Example",
            "___________________________________",
            "* Skype",
            "-----------------------------------",
            "- Example",
            "___________________________________",
            };

            foreach (string line in DoxInfo)
            {
                Console.WriteLine(line, Color.DarkMagenta);
            }
        }
    }
}