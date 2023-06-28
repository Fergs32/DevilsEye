using System;
using System.Collections.Generic;
using Dox.AsciiMenu;
using System.Threading;
using Spectre.Console;
using System.Drawing;
using Console = Colorful.Console;
using Color = System.Drawing.Color;

namespace Dox.Components.PhoneDorker
{
    public class PhoneDork
    {
        protected static string PhoneNumber = null;
        private static List<string> CompleteDorks = new List<string>();
        private static List<string> test = new List<string>();

        private static List<string> optionList = new List<string>
        {
            // Check all google sites containing the requested number
            "site:*.* intext:\"+44NUMBER\" OR intext:\"+NUMBER\" OR intext:\"NUMBER\"",

            // Social Media targetted dorks
            "\"site:twitter.com intext:\"NUMBER\" OR intext:\"++NUMBER\"",
            "\"site:linkedin.com intext:\"NUMBER\" OR intext:\"++NUMBER\"",
            "\"site:instagram.com intext:\"NUMBER\" OR intext:\"++NUMBER\"",

            // Paste sites

            "site:pastebin.com intext:\"NUMBER\"",

        };
        public static void Initialise()
        {
            Console.Clear();
            Thread.Sleep(500);
            Menu.PhonePrint();
            try
            {
                do
                {
                    Console.Write("[+] Phone Number: ", Color.DarkMagenta); PhoneNumber = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(PhoneNumber));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.Write($"[{DateTime.Now.ToString("h:mm:ss tt")}] ", Color.Magenta); Console.Write("Starting...\n", Color.DarkMagenta);
                LocalPhoneScan.Start(PhoneNumber); // Start phone scan
                Console.Write($"[{DateTime.Now.ToString("h:mm:ss tt")}] ", Color.Magenta); Console.Write("Phone scan completed!\n", Color.DarkMagenta);
                var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Choose an [white]option[/] below: ")
                    .PageSize(2)
                    .AddChoices(new[] { "Google Scan (Efficent Dorking)", "Return to Menu",}));
                if (option.Equals("Google Scan (Efficent Dorking)"))
                {
                    Console.WriteLine("[+] The option you selected may require proxies, please ensure you have proxies in \"Proxies\"proxies.txt\" as they may be used if selected.", Color.DarkMagenta);
                    Console.WriteLine("[+] Press enter to acknowledge");
                    Console.WriteLine("[+] IN DEVELOPMENT, WILL BE RELEASED NEXT RELEASE");

                }
                else
                {
                    Console.Clear();
                    Program.Main();
                }

            }
        }
        private static void GenerateDorks()
        {
            try
            {
                int entries = LocalPhoneScan.PhoneStorage.sig_nums_prefixes.Count;
               foreach(string line in LocalPhoneScan.PhoneStorage.sig_nums_prefixes)
               {
                    string num = line.Split(" | ")[0];
                    string prefix = line.Split(" | ")[1];
                    string num_prefix = prefix + num;
                    /*
                     * "number" site:twitter.com/asterisk_here/status
                     * "0927-4515683" intext:"carrier"
                     * "07933872247" intext:"location"
                     * "0927-4515683" intext:"owner" OR intext:"name"
                     * "07933872247" intext:"email"
                     */
                    test.Add($"intext:\"{num_prefix}\" OR intext:\"+{num_prefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:*.* intext:\"{num_prefix}\" OR intext:\"+{num_prefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:facebook.com intext:\"{num_prefix}\" OR intext:\"+{num_prefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:twitter.com intext:\"{num_prefix}\" OR intext:\"+{num_prefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:linkedin.com intext:\"{num_prefix}\" OR intext:\"+{num_prefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:instagram.com intext:\"{num_prefix}\" OR intext:\"+{num_prefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:vk.com intext:\"{num_prefix}\" OR intext:\"+{num_prefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:reddit.com intext:\"{num_prefix}\" OR intext:\"+{num_prefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                
            }
        }
    }
}
