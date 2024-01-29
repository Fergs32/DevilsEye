using System;
using System.Collections.Generic;
using Dox.AsciiMenu;
using System.Threading;
using Spectre.Console;
using System.Drawing;
using Console = Colorful.Console;
using Color = System.Drawing.Color;
using System.Net.Http;
using System.Threading.Tasks;
using Leaf.xNet;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Authentication;

namespace Dox.Components.PhoneDorker
{
    public class PhoneDork
    {
        // create a function that will take in a google dork and a phone number and return a list of results
                                           

        protected static string? PhoneNumber = null;
        private static List<string> CompleteDorks = new List<string>();
        private static List<string> test = new List<string>();
        public static void Initialise()
        {

            Console.Clear();
            Thread.Sleep(500);
            Menu.PhonePrint();
            try
            {
                do
                {
                    Console.Write("[+] Phone Number: ", Color.DarkMagenta); PhoneNumber = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(PhoneNumber ?? "null"));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", Color.Magenta); Console.Write("Starting...\n", Color.DarkMagenta);
                LocalPhoneScan.Start(PhoneNumber ?? "null"); // Start phone scan
                Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", Color.Magenta); Console.Write("Phone scan completed!\n", Color.DarkMagenta);
                var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Choose an [white]option[/] below: ")
                    .PageSize(3)
                    .AddChoices(new[] { "Google Scan (Efficent Dorking)", "Return to Menu",}));
                if (option.Equals("Google Scan (Efficent Dorking)"))
                {
                    Console.WriteLine("[+] The option you selected may require proxies, please ensure you have proxies in \"Proxies\\proxies.txt\" as they may be used if selected.", Color.DarkMagenta);
                    Console.WriteLine("[+] Press enter to acknowledge");
                    Console.ReadLine();
                    GenerateDorks();

                }
                else
                {
                    Console.Clear();
                    Program.Main();
                }

            }
        }
        private static void GenerateDorks()
        {
            try
            {
                int entries = LocalPhoneScan.PhoneStorage.sig_nums_prefixes.Count;
               foreach(string line in LocalPhoneScan.PhoneStorage.sig_nums_prefixes)
               {
                    string num = line.Split(" | ")[0];
                    string prefix = line.Split(" | ")[1];
                    string numPrefix = prefix + num;
                    test.Add($"intext:\"{numPrefix}\" OR intext:\"+{numPrefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:*.* intext:\"{numPrefix}\" OR intext:\"+{numPrefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:facebook.com intext:\"{numPrefix}\" OR intext:\"+{numPrefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:twitter.com intext:\"{numPrefix}\" OR intext:\"+{numPrefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:linkedin.com intext:\"{numPrefix}\" OR intext:\"+{numPrefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:instagram.com intext:\"{numPrefix}\" OR intext:\"+{numPrefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:vk.com intext:\"{numPrefix}\" OR intext:\"+{numPrefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");
                    test.Add($"site:reddit.com intext:\"{numPrefix}\" OR intext:\"+{numPrefix}\" OR intext:\"0{num}\" OR intext:\"{num}\"");

                    // we need to start the ScanDorksAsync function here

                    Task scan = ScanDorksAsync();
                    scan.Wait();

               }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        // now we need to scan each dork using http request to scape any potential results
        private static Task ScanDorksAsync()
        {
            string? Proxy = null;
            using (var client = new HttpRequest())
            {
                foreach(string dork in test)
                {
                    try
                    {
                        List<string> proxies = File.ReadAllLines("Proxies/proxies.txt").ToList();
                        Proxy = proxies[new Random().Next(proxies.Count)];

                        Console.WriteLine("[+] Using Proxy: " + Proxy, Color.DarkMagenta);

                        client.IgnoreProtocolErrors = true;
                        client.AcceptEncoding = "gzip, deflate";
                        client.AddHeader("Accept-Language", "en-US,en;q=0.9");
                        client.AddHeader("Upgrade-Insecure-Requests", "1");
                        client.UserAgent = Http.RandomUserAgent();
                        client.UseCookies = false;
                        client.AllowAutoRedirect = false;
                        client.ConnectTimeout = 10000;
                        client.KeepAliveTimeout = 10000;
                        client.ReadWriteTimeout = 10000;
                        client.SslProtocols = SslProtocols.Tls | SslProtocols.Tls12 | SslProtocols.Tls11;
                        client.SslCertificateValidatorCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                        client.Proxy = HttpProxyClient.Parse(Proxy);
                        var response = client.Get($"https://www.google.com/search?q={dork}&num=100&hl=en&complete=0&safe=off&filter=0&btnG=Search&start=0").ToString();
                        string content = response.ToString();
                        File.AppendAllLines("results.txt", new[] { content });
                        if (content.Contains("The document has moved") || content.Contains("302 Moved"))
                        {
                            Console.WriteLine($"[-] Proxy [{Proxy}] failed, retrying...", Color.Red);
                        }
                        else
                        {
                            ExtractResults(content);
                        }
                    }
                    catch(HttpException)
                    {
                        Console.WriteLine($"[-] Proxy [{Proxy}] failed, retrying...", Color.Red);
                    }
                    catch(Exception)
                    {
                        Console.WriteLine($"[-] Proxy [{Proxy}] failed, retrying...", Color.Red);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static void ExtractResults(string content)
        {
            // dump the results into the executable directory of each content
            Console.WriteLine("[+] Extracting results...");
            File.AppendAllText("results.txt", content);
            List<string> urlList = new List<string>().ToList();
            Regex regex = new Regex("<a href=\"(.*?)\" data-ved", RegexOptions.IgnoreCase);
            Match m = regex.Match(content);
            MatchCollection matches = Regex.Matches(content, "<a href=\"(.*?)\" data-ved");
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
                    CompleteDorks.Add(noDupes[x]);
                }
            }
            FilterHelper.FilterResults(CompleteDorks);
        }
    }
}
