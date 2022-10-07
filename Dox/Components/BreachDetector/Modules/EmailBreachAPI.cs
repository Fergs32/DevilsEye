using Leaf.xNet;
using Colorful;
using System.Threading;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using static Dox.Components.EmailGrabber.Modules.EmailBreachAPI;
using System.IO;

namespace Dox.Components.EmailGrabber.Modules
{
    public class EmailBreachAPI
    {
        private static string TargetEmail { get; set; }
        private static bool Lock = true;
        private static string pType = "";
        private static List<string> Proxies = new List<string>().ToList<string>();
        private static string ToDelete = "";

        internal struct RequestStorage
        {
            public static List<string> Breached_sites = new List<string>().ToList<string>();
            public static List<string> Revealed_Username = new List<string>().ToList<string>();
            public static List<string> Revealed_Password = new List<string>().ToList<string>();
        }

        public static void GetBreaches()
        {
            try
            {
                do
                {
                    Console.Clear();
                    AsciiMenu.Menu.GetTitle();
                    Console.ForegroundColor = Color.White;
                    Console.Write("[+] Enter email to search: ", Color.Magenta);
                    TargetEmail = Console.ReadLine();
                    try
                    {
                        Proxies = File.ReadAllLines("Proxies/proxies.txt").ToList<string>();
                        for (; pType != "HTTP" && pType != "SOCKS4" && pType != "SOCKS5" && pType != "NONE"; pType = Console.ReadLine())
                            Console.Write("\n[+] Proxy type (HTTP/SOCKS4/SOCKS5/NONE): ", Color.DarkMagenta);
                    }
                    catch { Console.WriteLine("Couldn't load proxies from \"proxies.txt\"", Color.OrangeRed); }
                }
                while (string.IsNullOrEmpty(TargetEmail));
                new Thread(new ThreadStart(Title)).Start();
            } catch { }

        Retry:
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    if (pType != "NONE")
                    {
                        string proxy = Proxies[new System.Random().Next(Proxies.Count)];
                        ToDelete = proxy;
                        ProxyClient proxyClient = pType == "SOCKS4" ? (ProxyClient)Socks4ProxyClient.Parse(proxy) : (pType == "SOCKS5" ? (ProxyClient)Socks5ProxyClient.Parse(proxy) : (ProxyClient)HttpProxyClient.Parse(proxy));
                        req.Proxy = proxyClient;
                        Console.WriteLine("Using proxy: {0}", Color.Red, proxy);
                    }
                    req.ConnectTimeout = 10000;
                    req.KeepAliveTimeout = 10000;
                    req.ReadWriteTimeout = 10000;
                    req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36";
                    req.AddHeader("Vaar-Version", "0");
                    req.AddHeader("Vaar-Header-App-Product-Name", "hackcheck-web-avast");
                    req.AddHeader("Vaar-Header-App-Build-Version", "1.0.0");
                    req.AddHeader("Accept", "*/*");
                    req.AddHeader("Host", "identityprotection.avast.com");
                    req.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    string jsonData = "{\"emailAddresses\":[\"" + TargetEmail + "\"]}";
                    string input = req.Post("https://identityprotection.avast.com/v1/web/query/site-breaches/unauthorized-data", jsonData, "application/json;charset=UTF-8").ToString();
                    Extract.GetSites(input);
                    Thread.Sleep(500);
                    RequestStorage.Breached_sites = RequestStorage.Breached_sites.Distinct().ToList();
                    if (RequestStorage.Breached_sites.Count == 0)
                    {
                        Console.WriteLine("Couldn't retreive all data, possible bad HTTP Request (IP banned by avast)", Color.Red); Thread.Sleep(-1);
                    }
                    else
                    {
                        Console.Clear();
                        AsciiMenu.Menu.GetTitle();
                        Console.WriteLine(">> Breaches Found for {0}\n", Color.White, TargetEmail);
                        for (int j = 0; j < RequestStorage.Breached_sites.Count;)
                        {
                            Console.Write("[+] ", Color.White);
                            Console.Write(RequestStorage.Breached_sites[j] + "\n", Color.Magenta);
                            j++;
                        }
                    }
                    Console.Write("\n[+] Would you like to search for these leaked databases? (Y/N): ", Color.Yellow); string opt = Console.ReadLine().ToLower();
                    switch (opt)
                    {
                        case "y":
                            Lock = false;
                            Find.Database(RequestStorage.Breached_sites);
                            break;
                        case "n":
                            Console.Clear();
                            Program.Main();
                            break;
                    }                                  
                }
            }          
            catch (HttpException)
            {
                Proxies.Remove(ToDelete);
                goto Retry;
            }
        }

        private static void Title()
        {
            while (Lock)
            {
                try
                {
                    Console.Title = string.Format("Email Searcher | Target: {0}", TargetEmail);
                }
                catch { }
            }
        }
    }
    public class Extract
    {
        public static void GetSites(string input)
        {
            Regex regex = new Regex("\"site\":\"(.*?)\",", RegexOptions.IgnoreCase);
            Match m = regex.Match(input);
            MatchCollection Site_Matches = Regex.Matches(input, "\"site\":\"(.*?)\",");
            while (m.Success)
            {
                for (int i = 0; i < Site_Matches.Count; i++)
                {
                    Group g = m.Groups[1];
                    RequestStorage.Breached_sites.Add(g.Value);
                }
                m = m.NextMatch();
            }
        }
    }
    public class Find
    {
        private static bool Lock2 = true;
        private static List<string> leaks = new List<string>().ToList<string>();
        private static List<string> filtered_leaks = new List<string>().ToList<string>();
        private static List<string> concatdorks = new List<string>().ToList<string>();
        private static int i = 0;

        public static void Database(List<string> Sites)
        {
            foreach(string line in Sites)
            {
                concatdorks.Add("inurl:cracked.to " + line);
                concatdorks.Add("inurl:pastebin.com " + line);
                concatdorks.Add("site:anonfiles.com " + line);
                concatdorks.Add("site:crackingx.com " + line);
                concatdorks.Add("site:nulled.to " + line);
            }
            new Thread(new ThreadStart(Title2)).Start();
            Thread.Sleep(2000);
            while(i < concatdorks.Count)
            {
                Console.Clear();
                AsciiMenu.Menu.GetTitle();
                Console.WriteLine("[+] Please wait, scanning for leaked databases...", Color.White);
                Console.WriteLine("\n[+] URL's collected: {0}", Color.White, leaks.Count);
                try
                {
                    if (i >= concatdorks.Count)
                    {
                        Thread.Sleep(-1);
                    }
                    using (HttpRequest req = new HttpRequest())
                    {
                        req.IgnoreProtocolErrors = true;
                        req.IgnoreInvalidCookie = true;
                        req.KeepAlive = true;
                        req.KeepAliveTimeout = 10000;
                        req.ConnectTimeout = 10000;
                        req.UserAgentRandomize();
                        req.AllowAutoRedirect = true;
                        string request = req.Get("https://www.google.com/search?q=" + concatdorks[i] + "&num=100&hl=en&complete=0&safe=off&filter=0&btnG=Search&start=0").ToString();
                        Regex regex = new Regex("<a href=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match m = regex.Match(request);
                        MatchCollection Matches = Regex.Matches(request, "<a href=\"(.*?)\" data-ved");
                        while (m.Success)
                        {
                            for (int i = 0; i < Matches.Count; i++)
                            {
                                Group g = m.Groups[1];
                                leaks.Add(g.Value);
                            }
                            m = m.NextMatch();
                        }
                        i++;
                    }
                    Thread.Sleep(5000);                    
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            Console.Clear(); Lock2 = false;
            AsciiMenu.Menu.GetTitle();
            Console.WriteLine("\n[+] Leaks collected: {0}", Color.Magenta, leaks.Count);
            List<string> noDupes = leaks.Distinct().ToList();
            for (int x = 0; x < noDupes.Count; x++)
            {
                if (noDupes[x].Contains("https://www.google.com") || noDupes[x].Contains("/search?q") || noDupes[x].Contains("https://maps.google.com") || noDupes[x].Contains("https://books.google.co.uk") || noDupes[x].Contains("https://podcasts.google.com") || noDupes[x].Contains("answers.microsoft.com") || noDupes[x].Contains("support.microsoft.com") || noDupes[x].Contains(" / search?") || noDupes[x].StartsWith("#") || noDupes[x].Contains("/preferences")) { }
                else
                {
                    filtered_leaks.Add(noDupes[x]);
                }
            }
            Console.WriteLine("[+] Filtered URLs | Urls {0}", Color.Magenta, filtered_leaks.Count);
            foreach(string line in filtered_leaks)
            {
                Console.WriteLine(line);
            }
            AsciiMenu.Menu.ReturnMenu();
        }
        private static void Title2()
        {
            try
            {
                while (Lock2)
                {
                    Console.Title = string.Format("Dorks to search: {0}", concatdorks.Count);
                }
            }
            catch
            {

            }
        }
    }
}
