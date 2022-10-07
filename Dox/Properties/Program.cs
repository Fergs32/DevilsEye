using System.Threading;
using System.Drawing;
using Dox.Components.UsernameGrabber;
using Dox.Components.TempMail;
using Dox.Components.Tools;
using Dox.Components.UsernameGrabber.Modules;
using Dox.Components.EmailGrabber.Modules;
using Dox.Components.Minecraft;
using Dox.Components.Ddos;
using System;
using Dox.AsciiMenu;

namespace Dox
{
    class Program
    {
		internal struct Data
        {
			public static int Option;
        }

		public static void Main()
        {
			Console.Title = ".DotOffical | Coded by https://github.com/Fergs32";
			do
			{
				Colorful.Console.Clear();
				Menu.GetTitle();
				Colorful.Console.WriteLine("\nThis program is for educational and development purposes only, use at your OWN will.", Color.WhiteSmoke); Thread.Sleep(300);
				Colorful.Console.WriteLine("Developed by Fergs32 |\n ", Color.WhiteSmoke); Thread.Sleep(300);
				Colorful.Console.WriteLine("[ Doxing Options ]               [ Tools ]                                   \t\t[Key Values]", Color.WhiteSmoke);
				Colorful.Console.WriteLine("[1] IP Lookup  [/]               [7] Get Information on Host  [/]            \t\t[*] Requires Proxies", Color.WhiteSmoke);
				Colorful.Console.WriteLine("[2] Username Lookup  [#]         [8] Create Temp Mail Server  [/]            \t\t[/] Proxyless", Color.WhiteSmoke);
				Colorful.Console.WriteLine("[3] Breach Detector  [#]         [9] Minecraft Server Info  [/]              \t\t[#] May Need Proxies", Color.WhiteSmoke);
				Colorful.Console.WriteLine("[4] First & Lastname  [--]       [10] DDOS [--]                              \t\t[--] In Progress", Color.WhiteSmoke);
				Colorful.Console.WriteLine("[5] Google Dork Target [#]       [11] Dox Bin Layouts  [/]                   \t\t[!!] Contact owner", Color.WhiteSmoke);
				Colorful.Console.WriteLine("[6] Minecraft Dox  [!!]\n", Color.WhiteSmoke);
				Colorful.Console.Write("[+] Option: ", Color.WhiteSmoke);


			} while (!int.TryParse(Colorful.Console.ReadLine(), out Data.Option));

			switch (Data.Option)
			{
				case 1:
					Components.IP.IP_Address();
					Colorful.Console.ReadLine();
					break;
				case 2:
					Core.GetUsernameInfo();
					break;
				case 3:
					EmailBreachAPI.GetBreaches();
					break;
				case 5:
					GoogleDorks.Entry();
					break;
				case 6:
					McBase.GetName();
					break;
				case 7:
					HostName.DNS.GetDNS();
					break;
				case 8:
					Mail.CreateEmail();
					break;
				case 9:
					McServerInfo.Get();
					break;
				case 10:
					StartAttack.Init();
					break;
				case 11:
					DoxBinMenu.Setup();
					break;

					default:
					Colorful.Console.WriteLine("[Error] Invalid Input", Color.Red);
					break;
			}
		}
	}
}