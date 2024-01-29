using Spectre.Console;
using System;
using System.Drawing;
using System.IO;

namespace Dox.Components.Proxy
{
    public abstract class Proxychecker
    {
        private static string? _countries;
        private static string? _protocol;
        private static string? _proxyTimeout;

        public static void ProxySetup()
        {
            Console.Clear();
            AsciiMenu.Menu.GetTitle();
            InitializeConfigSettings();
            ProxyChecker.GetProxies(_protocol ?? "null", _countries ?? "null", _proxyTimeout ?? "null");
        }

        private static void InitializeConfigSettings()
        {
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Proxies"))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Proxies");
                if (File.Exists(Directory.GetCurrentDirectory() + @"\Proxies\proxies.txt"))
                {
                    var confirm = AnsiConsole.Confirm("Proxy file found! would you like to delete and refresh");
                    if (confirm)
                    {
                        File.Delete(Directory.GetCurrentDirectory() + @"\Proxies\proxies.txt");
                        Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", System.Drawing.Color.Magenta); Console.Write(" Deleted proxies.txt\n", System.Drawing.Color.DarkMagenta);
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            _protocol = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose your proxy [darkmagenta]Protocol[/]!")
            .PageSize(3)
            .AddChoices(new[] { "HTTP", "SOCKS4", "SOCKS5", }));

            _countries = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose your proxy's [darkmagenta]Country[/]!")
            .PageSize(11)
            .AddChoices(new[] { "All countries" }));

            _proxyTimeout = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select your proxy [darkmagenta]Timeout[/]!")
            .PageSize(6)
            .AddChoices(new[] { "1000", "2000", "3000", "4000", "5000",}));
        }
    }
}