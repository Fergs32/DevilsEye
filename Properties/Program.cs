using Dox.AsciiMenu;
using Dox.Components.Ddos;
using Dox.Components.EmailGrabber.Modules;
using Dox.Components.Minecraft;
using Dox.Components.Proxy;
using Dox.Components.TempMail;
using Dox.Components.Tools;
using Dox.Components.AccountLeaks;
using Dox.Components.UsernameGrabber;
using Dox.Components.UsernameGrabber.Modules;
using Spectre.Console;
using System;
using Dox.Components.PhoneDorker;
using System.Threading;
using System.Threading.Tasks;
using Dox.Configuration.Manager;
using Color = Spectre.Console.Color;

namespace Dox
{
    internal class Program
    {
        internal struct Data
        {
            public static int Option;
        }

        private static void ProgrammingStatistics()
        {
            AnsiConsole.Write(new BreakdownChart()
            .Width(40)
            .AddItem("C#", 100, Color.Red));
        }

        public static void Main()
        {
            try
            {
                Config.ConfigurationImpl();
                Task.Factory.StartNew(() => ExitSettings.ExitEventHandler(ExitSettings.handle));
                Config.FolderCheck();
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            Console.Title = ".DoxOfficial | Coded by https://github.com/Fergs32";
            do
            {
                Console.Clear();
                Menu.GetTitle();
                Console.Write("\t\t\t\t\t  [!] ", Color.Blue); Console.Write("Follow The Crumbs", Color.DarkMagenta); Console.Write(" [!] \n\n", Color.Blue);
                ProgrammingStatistics();
                var module = AnsiConsole.Prompt(
                 new SelectionPrompt<string>()
                  .Title("[darkmagenta]Modules[/][red] {[/][green]14[/][red]}[/]")
                     .PageSize(10)
                         .MoreChoicesText("[grey](Navigate down to find more modules)[/]")
                            .AddChoices(new[] {
                            "IP Lookup", "Username Lookup", "Breach Detector",
                            "First & Lastname", "Google Dork Target", "Minecraft Dox",
                            "Phone Dorker", "Get Information on Host/Domain", "Create Temp Mail Server (free)",
                            "Minecraft Server Info", "DDOS", "Dox Bin Layouts", "Proxy Scraper & Tester", "Email Scraper/Accounts", "99"
            }));
                switch (module)
                {
                    case "IP Lookup":
                        Components.IP.GetIpAddress();
                        break;
                    case "Username Lookup":
                        Core.GetUsernameInfo();
                        break;
                    case "Breach Detector":
                        EmailBreachAPI.GetBreaches();
                        break;
                    case "First & Lastname":
                        Console.WriteLine("In Development");
                        Console.ReadLine();
                        break;
                    case "Google Dork Target":
                        GoogleDorks.Entry();
                        break;
                    case "Minecraft Dox":
                        McBase.GetName();
                        break;
                    case "Phone Dorker":
                        PhoneDork.Initialise();
                        break;
                    case "Get Information on Host/Domain":
                        HostName.DNS.GetDNS();
                        break;
                    case "Create Temp Mail Server (free)":
                        Mail.CreateEmail();
                        break;
                    case "Minecraft Server Info":
                        McServerInfo.Get();
                        break;
                    case "DDOS":
                        StartAttack.Get();
                        break;
                    case "Dox Bin Layouts":
                        DoxBinMenu.Setup();
                        break;
                    case "Proxy Scraper & Tester":
                        Proxychecker.ProxySetup();
                        break;
                    case "Email Scraper/Accounts":
                        Scraper.Start();
                        break;
                    default:
                        Colorful.Console.WriteLine("[Error] Invalid Input", System.Drawing.Color.Red);
                        break;
                    case "99":
                        Console.WriteLine(1 * 60000L);
                        break;
                }
            } while (!int.TryParse(Colorful.Console.ReadLine(), out Data.Option));
        }
    }
}