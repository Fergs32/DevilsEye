using System.Net;
using Leaf.xNet;
using Console = Colorful.Console;
using HttpStatusCode = Leaf.xNet.HttpStatusCode;

namespace Dox.Components.Proxy
{
    public abstract class ProxyChecker
    {
        private static List<string> _builtUrls = new List<string>().ToList();

        public static void GetProxies(string protocol, string country, string timeout)
        {
            try
            {
                BuildLinks(protocol, country, timeout);
            }
            catch(Exception ex) { Console.WriteLine(ex); }
        }

        private static void BuildLinks(string protocol, string country, string timeout)
        {
            if (country.Contains("All countries")) { country = country.Replace("All countries", "all"); }
            try
            {
                List<string> noCountryUrls = File.ReadAllLines("Proxies/no_country_urls.txt").ToList();
                foreach (string line in noCountryUrls)
                {
                    var newLine = line.Replace("PROTOCOL_HERE", protocol.ToLower()).Replace("TIMEOUT_HERE", timeout).Replace("COUNTRY_HERE", country);
                    _builtUrls.Add(newLine);
                }
                DownloadProxies(_builtUrls, protocol);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        private static void DownloadProxies(List<string> urls, string protocol)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Proxies\downloaded_proxies.txt"))
                File.Delete(Directory.GetCurrentDirectory() + @"\Proxies\downloaded_proxies.txt");
            foreach(string url in urls)
            {
                try
                {
                    using HttpRequest httpRequest = new HttpRequest();
                    WebClient wc = new WebClient();
                    string proxies = wc.DownloadString(url);
                    File.AppendAllText(Directory.GetCurrentDirectory() + @"\Proxies\downloaded_proxies.txt", proxies);
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
            Console.WriteLine("\t\tTARGET: https://www.google.com/ [CHANGE IN TESTPROXIES METHOD]", Color.White);
            Console.WriteLine($"Successfully downloaded {File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Proxies\downloaded_proxies.txt").Count()} proxies!");
            TestProxies(protocol);
        }

        private static void TestProxies(string protocol)
        {
            
            List<string> proxyFile = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Proxies\downloaded_proxies.txt").ToList();

            Parallel.ForEach(proxyFile, new ParallelOptions { MaxDegreeOfParallelism = 100 }, proxy =>
            {
                var ipSegments = proxy.Split(".");
                if (ipSegments.Length == 4)
                {
                    try
                    {
                        using (HttpRequest req = new())
                        {
                            ProxyClient proxyClient = protocol == "SOCKS4" ? Socks4ProxyClient.Parse(proxy) : (protocol == "SOCKS5" ? Socks5ProxyClient.Parse(proxy) : (ProxyClient)HttpProxyClient.Parse(proxy));
                            req.Proxy = proxyClient;
                            req.KeepAliveTimeout = 3000;
                            HttpResponse resp = req.Head("https://www.google.com/");
                            if (resp.StatusCode == HttpStatusCode.OK)
                            {
                                Console.WriteLine($"[+] {proxy}", Color.Green);
                                File.AppendAllText(Directory.GetCurrentDirectory() + @"\Proxies\proxies.txt", proxy + Environment.NewLine);
                            }
                            else
                            {
                                Console.WriteLine($"[+] {proxy}", Color.Red);
                            }
                        }
                    }
                    catch (HttpException)
                    {
                        
                    }
                }
                else
                {
                    Console.WriteLine($"[!] Invalid IP: {proxy}");
                }
            });
        }

    }
}