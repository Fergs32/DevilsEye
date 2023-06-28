using Spectre.Console;
using System;

namespace Dox.Components.Proxy
{
    public class Proxychecker
    {
        private static string Countries;
        private static string Protocol;
        private static string ProxyTimeout;

        public static void ProxySetup()
        {
            Console.Clear();
            AsciiMenu.Menu.GetTitle();
            InitializeConfigSettings();
            ProxyChecker.GetProxies(Protocol, Countries, ProxyTimeout);
        }

        private static void InitializeConfigSettings()
        {
            Protocol = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose your proxy [darkmagenta]Protocol[/]!")
            .PageSize(3)
            .AddChoices(new[] { "HTTP", "SOCKS4", "SOCKS5", }));

            Countries = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose your proxy's [darkmagenta]Country[/]!")
            .PageSize(11)
            .AddChoices(new[] { "All countries" }));

            ProxyTimeout = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select your proxy [darkmagenta]Timeout[/]!")
            .PageSize(6)
            .AddChoices(new[] { "1000", "2000", "3000", "4000", "5000",}));
        }
    }
}