using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;   
using System.Net;
using System.Threading.Tasks;

namespace Dox.Components.Proxy
{
    public class ProxyChecker
    {
        private static List<string> built_urls = new List<string>().ToList<string>();

        public static void GetProxies(string Protocol, string Country, string Timeout)
        {
            try
            {
                BuildLinks(Protocol, Country, Timeout);
            }
            catch(Exception ex) { Colorful.Console.WriteLine(ex); }
        }

        private static void BuildLinks(string Protocol, string Country, string Timeout)
        {
            if (Country.Contains("All countries")) { Country = Country.Replace("All countries", "all"); }
            try
            {
                List<string> no_country_urls = File.ReadAllLines("Proxies/no_country_urls.txt").ToList<string>();
                foreach (string line in no_country_urls)
                {
                    string new_line;
                    new_line = line.Replace("PROTOCOL_HERE", Protocol.ToLower()).Replace("TIMEOUT_HERE", Timeout).Replace("COUNTRY_HERE", Country);
                    built_urls.Add(new_line);
                }
                DownloadProxies(built_urls, Protocol);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void DownloadProxies(List<string> urls, string protocol)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Proxies\\downloaded_proxies.txt"))
                File.Delete(Directory.GetCurrentDirectory() + "\\Proxies\\downloaded_proxies.txt");
            foreach(string url in urls)
            {
                try
                {
                    using (HttpRequest httpRequest = new HttpRequest())
                    {
                        WebClient wc = new WebClient();
                        string proxies = wc.DownloadString(url);
                        File.AppendAllText(Directory.GetCurrentDirectory() + "\\Proxies\\downloaded_proxies.txt", proxies);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            Colorful.Console.WriteLine("\t\tTARGET: https://www.google.com/ [CHANGE IN TESTPROXIES METHOD]", Color.White);
            Colorful.Console.WriteLine($"Successfully downloaded {File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Proxies\\downloaded_proxies.txt").Count()} proxies!");
            TestProxies(protocol);
        }

        private static void TestProxies(string protocol)
        {
            
            List<string> proxy_file = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Proxies\\downloaded_proxies.txt").ToList<string>();

            Parallel.ForEach(proxy_file, new ParallelOptions { MaxDegreeOfParallelism = 100 }, proxy =>
            {
                var ip_segments = proxy.Split(".");
                if (ip_segments.Length == 4)
                {
                    try
                    {
                        using (HttpRequest req = new HttpRequest())
                        {
                            ProxyClient proxyClient = protocol == "SOCKS4" ? (ProxyClient)Socks4ProxyClient.Parse(proxy) : (protocol == "SOCKS5" ? (ProxyClient)Socks5ProxyClient.Parse(proxy) : (ProxyClient)HttpProxyClient.Parse(proxy));
                            req.Proxy = proxyClient;
                            req.KeepAliveTimeout = 3000;
                            HttpResponse resp = req.Head("https://www.google.com/");
                            if (resp.StatusCode == Leaf.xNet.HttpStatusCode.OK)
                            {
                                Colorful.Console.WriteLine($"[+] {proxy}", Color.Green);
                                File.AppendAllText(Directory.GetCurrentDirectory() + "\\Proxies\\good_proxies.txt", proxy + Environment.NewLine);
                            }
                            else
                            {
                                Colorful.Console.WriteLine($"[+] {proxy}", Color.Red);
                            }
                        }
                    }
                    catch (HttpException ex)
                    {
                    }
                }
                else
                {
                    Colorful.Console.WriteLine($"[!] Invalid IP: {proxy}");
                }
            });
        }

    }
}