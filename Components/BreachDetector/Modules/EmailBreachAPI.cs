using Leaf.xNet;
using System.Text.RegularExpressions;
using static Dox.Components.EmailGrabber.Modules.EmailBreachAPI;
using Console = System.Console;

namespace Dox.Components.EmailGrabber.Modules
{
    public class EmailBreachAPI
    {
        private static string? TargetEmail { get; set; }
        private static bool _lock = true;
        private static string _pType = "";
        private static List<string> _proxies = new List<string>().ToList<string>();
        private static string _toDelete = "";

        internal struct RequestStorage
        {
            public static List<string> Breached_sites = new List<string>().ToList<string>();
        }

        public static void GetBreaches()
        {
            try
            {
                do
                {
                    Console.Clear();
                    AsciiMenu.Menu.GetTitle();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[+] Enter email to search: ", Color.Magenta);
                    TargetEmail = Console.ReadLine();
                    try
                    {
                        _proxies = File.ReadAllLines("Proxies/proxies.txt").ToList<string>();
                        for (; _pType != "HTTP" && _pType != "SOCKS4" && _pType != "SOCKS5" && _pType != "NONE"; _pType = Console.ReadLine() ?? "NONE")
                            Console.Write("\n[+] Proxy type (HTTP/SOCKS4/SOCKS5/NONE): ", Color.DarkMagenta);
                    }
                    catch { Console.WriteLine("Couldn't load proxies from \"proxies.txt\"", Color.OrangeRed); }
                }
                while (string.IsNullOrEmpty(TargetEmail));
                new Thread(Title).Start();
            }
            catch
            {
                // ignored
            }

            Retry:
            try
            {
                using (HttpRequest req = new())
                {
                    if (_pType != "NONE")
                    {
                        string proxy = _proxies[new Random().Next(_proxies.Count)];
                        _toDelete = proxy;
                        ProxyClient proxyClient = _pType == "SOCKS4" ? Socks4ProxyClient.Parse(proxy) : (_pType == "SOCKS5" ? Socks5ProxyClient.Parse(proxy) : HttpProxyClient.Parse(proxy));
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
                    Console.Write("\n[+] Would you like to search for these leaked databases? (Y/N): ", Color.Yellow); string opt = Console.ReadLine() ?? "N".ToLower();
                    switch (opt)
                    {
                        case "y":
                            _lock = false;
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
                _proxies.Remove(_toDelete);
                goto Retry;
            }
        }

        private static void Title()
        {
            while (_lock)
            {
                try
                {
                    Console.Title = $"Email Searcher | Target: {TargetEmail}";
                }
                catch
                {
                    // ignored
                }
            }
        }
    }

    public class Extract
    {
        public static void GetSites(string input)
        {
            Regex regex = new("\"site\":\"(.*?)\",", RegexOptions.IgnoreCase);
            Match m = regex.Match(input);
            MatchCollection siteMatches = Regex.Matches(input, "\"site\":\"(.*?)\",");
            while (m.Success)
            {
                for (int i = 0; i < siteMatches.Count; i++)
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
        private static bool _lock2 = true;
        private static List<string> _leaks = new List<string>().ToList<string>();
        private static List<string> _filteredLeaks = new List<string>().ToList<string>();
        private static List<string> _concatdorks = new List<string>().ToList<string>();
        private static int i = 0;

        public static void Database(List<string> sites)
        {
            foreach (string line in sites)
            {
                _concatdorks.Add("inurl:cracked.to " + line);
                _concatdorks.Add("inurl:pastebin.com " + line);
                _concatdorks.Add("site:anonfiles.com " + line);
                _concatdorks.Add("site:crackingx.com " + line);
                _concatdorks.Add("site:nulled.to " + line);
            }
            new Thread(Title2).Start();
            Thread.Sleep(2000);
            while (i < _concatdorks.Count)
            {
                Console.Clear();
                AsciiMenu.Menu.GetTitle();
                Console.WriteLine("[+] Please wait, scanning for leaked databases...", Color.White);
                Console.WriteLine("\n[+] URL's collected: {0}", Color.White, _leaks.Count);
                try
                {
                    if (i >= _concatdorks.Count)
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
                        string request = req.Get("https://www.google.com/search?q=" + _concatdorks[i] + "&num=100&hl=en&complete=0&safe=off&filter=0&btnG=Search&start=0").ToString();
                        Regex regex = new Regex("<a href=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match m = regex.Match(request);
                        MatchCollection matches = Regex.Matches(request, "<a href=\"(.*?)\" data-ved");
                        while (m.Success)
                        {
                            for (int i = 0; i < matches.Count; i++)
                            {
                                Group g = m.Groups[1];
                                _leaks.Add(g.Value);
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
            Console.Clear(); _lock2 = false;
            AsciiMenu.Menu.GetTitle();
            Console.WriteLine("\n[+] Leaks collected: {0}", Color.Magenta, _leaks.Count);
            List<string> noDupes = _leaks.Distinct().ToList();
            for (int x = 0; x < noDupes.Count; x++)
            {
                if (noDupes[x].Contains("https://www.google.com") || noDupes[x].Contains("/search?q") || noDupes[x].Contains("https://maps.google.com") || noDupes[x].Contains("https://books.google.co.uk") || noDupes[x].Contains("https://podcasts.google.com") || noDupes[x].Contains("answers.microsoft.com") || noDupes[x].Contains("support.microsoft.com") || noDupes[x].Contains(" / search?") || noDupes[x].StartsWith("#") || noDupes[x].Contains("/preferences")) { }
                else
                {
                    _filteredLeaks.Add(noDupes[x]);
                }
            }
            Console.WriteLine("[+] Filtered URLs | Urls {0}", Color.Magenta, _filteredLeaks.Count);
            foreach (string line in _filteredLeaks)
            {
                Console.WriteLine(line);
            }
            AsciiMenu.Menu.ReturnMenu();
        }

        private static void Title2()
        {
            try
            {
                while (_lock2)
                {
                    Console.Title = string.Format("Dorks to search: {0}", _concatdorks.Count);
                }
            }
            catch
            {
            }
        }
    }
}