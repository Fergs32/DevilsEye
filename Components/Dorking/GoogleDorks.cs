using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Dox.AsciiMenu;
using Dox.Components.Dorking.Filter;
using Leaf.xNet;
using Console = Colorful.Console;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class GoogleDorks
    {
        private static bool _lock = true;
        private static bool _lock2 = true;

        private static string[] _googleExt = {
            "insite:",
            "inurl:",
            "indesc:",
            "insite:*.*",
            "site:",
            "intitle:\"Index of /\" inurl:",
        };

        private static List<string> _listExt = new(_googleExt);
        private static List<string> _proxies = new List<string>().ToList();
        public static List<string> UrlsToProcess = new List<string>().ToList();

        private static string _pType = "";

        private static void Get(string input)
        {
            Console.WriteLine("Attempting to gather information on " + input + "...", Color.Magenta);
            for (int j = 0; j < _googleExt.Length;)
            {
                Retry:
                try
                {
                    using (HttpRequest req = new())
                    {
                        if (_pType != "NONE")
                        {
                            string proxy = _proxies[new Random().Next(_proxies.Count)];
                            ProxyClient proxyClient = _pType == "SOCKS4" ? Socks4ProxyClient.Parse(proxy) : (_pType == "SOCKS5" ? Socks5ProxyClient.Parse(proxy) : (ProxyClient)HttpProxyClient.Parse(proxy));
                            req.Proxy = proxyClient;
                            Console.WriteLine(proxy);
                        }
                        req.IgnoreProtocolErrors = true;
                        req.IgnoreInvalidCookie = true;
                        req.KeepAlive = true;
                        req.KeepAliveTimeout = 10000;
                        req.ConnectTimeout = 10000;
                        req.UserAgentRandomize();
                        req.AllowAutoRedirect = true;
                        string request = req.Get("https://www.google.com/search?q=" + _listExt[j] + " " + input + "&num=100&hl=en&complete=0&safe=off&filter=0&btnG=Search&start=0").ToString();
                        List<string> urlList = new List<string>().ToList();
                        Regex regex = new("<a href=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match m = regex.Match(request);
                        MatchCollection matches = Regex.Matches(request, "<a href=\"(.*?)\" data-ved");
                        while (m.Success)
                        {
                            for (int i = 0; i < matches.Count; i++)
                            {
                                Group g = m.Groups[1];
                                urlList.Add(g.Value);
                            }
                            m = m.NextMatch();
                        }
                        List<string> noDupes = urlList.Distinct().ToList();
                        for (int x = 0; x < noDupes.Count; x++)
                        {
                            if (noDupes[x].Contains("https://www.google.com") || noDupes[x].Contains("https://maps.google.com") || noDupes[x].Contains("https://books.google.co.uk") || noDupes[x].Contains("https://podcasts.google.com") || noDupes[x].Contains("answers.microsoft.com") || noDupes[x].Contains("support.microsoft.com") || noDupes[x].Contains(" / search?") || noDupes[x].StartsWith("#") || noDupes[x].Contains("/preferences")) { }
                            else
                            {
                                UrlsToProcess.Add(noDupes[x]);
                            }
                        }
                        Thread.Sleep(5000);
                    }
                }
                catch (HttpException ex)
                {
                    if (ex.Message.Contains("recaptcha"))
                    {
                        goto Retry;
                    }
                    Console.WriteLine(ex);
                }
                j++;
            }
            UrlsToProcess = UrlsToProcess.Distinct().ToList();
            Console.WriteLine("[+] Processing URLs please wait...", Color.Magenta);
            _lock = false;
            new Thread(ConsoleTitle).Start();
            UrlFilter.Filter(UrlsToProcess);
            Console.WriteLine("[+] Completed URL filtering, displaying results...\n", Color.Magenta);
            Thread.Sleep(2000);
            Console.WriteLine("[#] Potential IRL Information", Color.Magenta);
            foreach (string line in UrlFilter.PotentialIrl)
            {
                Console.WriteLine(line, Color.White);
            }
            Thread.Sleep(1000);
            Console.WriteLine("\n[#] Potential Accounts", Color.Magenta);
            foreach (string line in UrlFilter.PotentialAccounts)
            {
                Console.WriteLine(line, Color.White);
            }
            Thread.Sleep(1000);
            Console.WriteLine("\n[#] Potential Connections", Color.Magenta);
            foreach (string line in UrlFilter.PotentialConnections)
            {
                Console.WriteLine(line, Color.White);
            }
            Thread.Sleep(1000);
            Console.WriteLine("\n[#] Potential Country", Color.Magenta);
            foreach (string line in UrlFilter.PotentialCountry)
            {
                Console.WriteLine(line, Color.White);
            }
            Thread.Sleep(1000);
            Console.WriteLine("\n[#] Potential Information", Color.Magenta);
            foreach (string line in UrlFilter.PotentialInformation)
            {
                Console.WriteLine(line, Color.White);
            }
            _lock2 = false;
            Menu.ReturnMenu();
        }

        public static void Entry()
        {
            Console.Clear();
            Menu.GetTitle();
            Console.ForegroundColor = Color.White;
            Console.Write("[+] Enter anything to Dork {\"real name\", \"username\", \"email\", etc\"}: ", Color.DarkMagenta); string info = Console.ReadLine();
            try
            {
                _proxies = File.ReadAllLines("Proxies/proxies.txt").ToList();
                for (; _pType != "HTTP" && _pType != "SOCKS4" && _pType != "SOCKS5" && _pType != "NONE"; _pType = Console.ReadLine())
                    Console.Write("\n[+] Proxy type (HTTP/SOCKS4/SOCKS5/NONE): ", Color.DarkMagenta);
            }
            catch { Console.WriteLine("Couldn't load proxies from \"proxies.txt\"", Color.OrangeRed); }
            new Thread(ConsoleTitle2).Start();
            Get(info);
        }

        private static void ConsoleTitle()
        {
            while (_lock2)
            {
                try
                {
                    Console.Title =
                        $"DotDox | IRLs: {UrlFilter.PotentialIrl.Count} | Country: {UrlFilter.PotentialCountry.Count} | Accounts: {UrlFilter.PotentialAccounts.Count} | Information: {UrlFilter.PotentialInformation.Count}";
                    Thread.Sleep(100);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static void ConsoleTitle2()
        {
            while (_lock)
            {
                try
                {
                    Console.Title = $"DotDox | Urls To Process: {UrlsToProcess.Count}";
                    Thread.Sleep(100);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}