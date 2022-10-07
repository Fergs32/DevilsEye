using Leaf.xNet;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Colorful;
using System.Text.RegularExpressions;
using System.IO;
using System;
using Dox.Components.Dorking.Filter;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class GoogleDorks
    {
        private static bool Lock = true;
        private static bool Lock2 = true;

        private static string[] GoogleExt = new string[]
        {
            "insite:",
            "inurl:",
            "indesc:",
            "insite:*.*",
            "site:",
            "intitle:\"Index of /\" inurl:",
        };
        private static List<string> listExt = new List<string>(GoogleExt);
        private static List<string> Proxies = new List<string>().ToList<string>();
        public static List<string> UrlsToProcess = new List<string>().ToList<string>();

        private static string pType = "";

        private static void Get(string input)
        {
            Colorful.Console.WriteLine("Attempting to gather information on " + input + "...", Color.Magenta);
            for (int j = 0; j < GoogleExt.Count();)
            {
            Retry:
                try
                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        if (GoogleDorks.pType != "NONE")
                        {
                            string proxy = GoogleDorks.Proxies[new Random().Next(GoogleDorks.Proxies.Count)];
                            ProxyClient proxyClient = GoogleDorks.pType == "SOCKS4" ? (ProxyClient)Socks4ProxyClient.Parse(proxy) : (GoogleDorks.pType == "SOCKS5" ? (ProxyClient)Socks5ProxyClient.Parse(proxy) : (ProxyClient)HttpProxyClient.Parse(proxy));
                            req.Proxy = proxyClient;
                            Colorful.Console.WriteLine(proxy);
                        }
                        req.IgnoreProtocolErrors = true;
                        req.IgnoreInvalidCookie = true;
                        req.KeepAlive = true;
                        req.KeepAliveTimeout = 10000;
                        req.ConnectTimeout = 10000;
                        req.UserAgentRandomize();
                        req.AllowAutoRedirect = true;
                        string request = req.Get("https://www.google.com/search?q=" + listExt[j] + " " + input + "&num=100&hl=en&complete=0&safe=off&filter=0&btnG=Search&start=0").ToString();
                        List<string> urllist = new List<string>().ToList<string>();
                        Regex regex = new Regex("<a href=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match m = regex.Match(request);
                        MatchCollection Matches = Regex.Matches(request, "<a href=\"(.*?)\" data-ved");
                        while (m.Success)
                        {
                            for (int i = 0; i < Matches.Count; i++)
                            {
                                Group g = m.Groups[1];
                                urllist.Add(g.Value);
                            }
                            m = m.NextMatch();
                        }
                        List<string> noDupes = urllist.Distinct().ToList();
                        for (int x = 0; x < noDupes.Count; x++)
                        {
                            if (noDupes[x].Contains("https://www.google.com") || noDupes[x].Contains("https://maps.google.com") || noDupes[x].Contains("https://books.google.co.uk") || noDupes[x].Contains("https://podcasts.google.com")  || noDupes[x].Contains("answers.microsoft.com") || noDupes[x].Contains("support.microsoft.com") || noDupes[x].Contains(" / search?") || noDupes[x].StartsWith("#") || noDupes[x].Contains("/preferences")) {  } 
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
                    Colorful.Console.WriteLine(ex);
                }
                j++;
            }
            UrlsToProcess = UrlsToProcess.Distinct().ToList();
            Colorful.Console.WriteLine("[+] Processing URLs please wait...", Color.Magenta);
            Lock = false;
            new Thread(new ThreadStart(ConsoleTitle)).Start();
            UrlFilter.Filter(UrlsToProcess);
            Colorful.Console.WriteLine("[+] Completed URL filtering, displaying results...\n", Color.Magenta);
            Thread.Sleep(2000);
            Colorful.Console.WriteLine("[#] Potential IRL Information", Color.Magenta);
            foreach(string line in UrlFilter.Potential_IRL)
            {
                Colorful.Console.WriteLine(line, Color.White);
            }
            Thread.Sleep(1000);
            Colorful.Console.WriteLine("\n[#] Potential Accounts", Color.Magenta);
            foreach (string line in UrlFilter.Potential_Accounts)
            {
                Colorful.Console.WriteLine(line, Color.White);
            }
            Thread.Sleep(1000);
            Colorful.Console.WriteLine("\n[#] Potential Connections", Color.Magenta);
            foreach (string line in UrlFilter.Potential_Connections)
            {
                Colorful.Console.WriteLine(line, Color.White);
            }
            Thread.Sleep(1000);
            Colorful.Console.WriteLine("\n[#] Potential Country", Color.Magenta);
            foreach (string line in UrlFilter.Potential_Country)
            {
                Colorful.Console.WriteLine(line, Color.White);
            }
            Thread.Sleep(1000);
            Colorful.Console.WriteLine("\n[#] Potential Information", Color.Magenta);
            foreach (string line in UrlFilter.Potential_Information)
            {
                Colorful.Console.WriteLine(line, Color.White);
            }
            Lock2 = false;
            AsciiMenu.Menu.ReturnMenu();
        }
        public static void Entry()
        {
            Colorful.Console.Clear();
            AsciiMenu.Menu.GetTitle();
            Colorful.Console.ForegroundColor = Color.White;
            Colorful.Console.Write("[+] Enter anything to Dork {\"real name\", \"username\", \"email\", etc\"}: ", Color.DarkMagenta); string info = Colorful.Console.ReadLine();
            try
            {
                Proxies = File.ReadAllLines("Proxies/proxies.txt").ToList<string>();
                for (; GoogleDorks.pType != "HTTP" && GoogleDorks.pType != "SOCKS4" && GoogleDorks.pType != "SOCKS5" && GoogleDorks.pType != "NONE"; GoogleDorks.pType = Colorful.Console.ReadLine())
                    Colorful.Console.Write("\n[+] Proxy type (HTTP/SOCKS4/SOCKS5/NONE): ", Color.DarkMagenta);
            }
            catch { Colorful.Console.WriteLine("Couldn't load proxies from \"proxies.txt\"", Color.OrangeRed); }
            new Thread(new ThreadStart(ConsoleTitle2)).Start();
            Get(info);
        }
        private static void ConsoleTitle()
        {
            while (Lock2 == true)
            {
                try
                {
                    Colorful.Console.Title = string.Format("DotDox | IRLs: {0} | Country: {1} | Accounts: {2} | Information: {3}", UrlFilter.Potential_IRL.Count, UrlFilter.Potential_Country.Count, UrlFilter.Potential_Accounts.Count, UrlFilter.Potential_Information.Count);
                    Thread.Sleep(100);
                }
                catch
                {

                }
            }
        }
        private static void ConsoleTitle2()
        {
            while (Lock == true)
            {
                try
                {
                    Colorful.Console.Title = string.Format("DotDox | Urls To Process: {0}", GoogleDorks.UrlsToProcess.Count);
                    Thread.Sleep(100);
                }
                catch
                {

                }
            }
        }


    }
}
