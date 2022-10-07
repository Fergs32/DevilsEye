using System.Text.RegularExpressions;
using Colorful;
using System.Drawing;
using Leaf.xNet;
using System.IO;

namespace Dox.Components.Tools.ServerAnalysis
{
    public class Analysis
    {
        public static string CurrentDir = Directory.GetCurrentDirectory() + "\\text.txt";
        public static void E_P(string server)
        {
            DNS_WhoIs(server);
            DNS_Lookup(server);
            Helper.AnalysisWebsites(server);

        }

        private static void DNS_WhoIs(string ServerIP)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpResponse res = req.Get("https://networkcalc.com/api/dns/whois/" + ServerIP);
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        string response = res.ToString();
                        string hostname = Regex.Match(response, "\"hostname\":\"(.*?)\"").Groups[1].Value;
                        string domain_register = Regex.Match(response, "\"registrar\":\"(.*?)\"").Groups[1].Value;
                        string register_date = Regex.Match(response, "\"registry_created_date\":\"(.*?)\"").Groups[1].Value;
                        string domain_id = Regex.Match(response, "\"registry_domain_id\":\"(.*?)\"").Groups[1].Value;
                        string exp = Regex.Match(response, "\"registry_expiration_date\":\"(.*?)\",").Groups[1].Value;

                        Console.WriteLine("\n[WhosIs Lookup]\n\n[+] Hostname: {0}\n[+] Domain Registerer: {1}\n[+] Register Date: {2}\n[+] Domain ID: {3}\n[+] Domain Expiry: {4}\n", Color.Coral, hostname, domain_register, register_date, domain_id, exp);
                    }
                    else
                    {
                        Console.WriteLine("Unable to contact {0} | Error code: {1}", Color.Red, "https://networkcalc.com/api/dns/whois/", res.StatusCode);
                    }
                }
            }
            catch(HttpException ex)
            {
                Console.WriteLine(ex, Color.Red);
            }
        }

        private static void DNS_Lookup(string ServerIP)
        {
            try
            {
                using(HttpRequest req = new HttpRequest())
                {
                    HttpResponse resp = req.Get("https://networkcalc.com/api/dns/lookup/" + ServerIP);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string response = resp.ToString();
                        Console.WriteLine("[DNS Lookup]\n", Color.Coral);
                        Console.WriteLine("[+] IP's Linked: " + Helper.ExtractAddresses(response), Color.Coral);
                        Console.WriteLine("[+] Name Server(s): " + Helper.ExtractNameServers(response), Color.Coral);
                        Console.WriteLine("[+] MX Record Value(s): " + Helper.ExtractMXRecords(response), Color.Coral);
                        // Execute a cmd command "nslookup -q=txt {host}" which will give us the txt records of that domain if any are stored,
                        // then pipe that information into a text file, extract information needed, and delete.
                        Helper.ExecuteCommand("nslookup -q=txt " + ServerIP + " > " + CurrentDir);
                        Console.WriteLine("[+] TXT Records: " + Helper.TextFileExtract(), Color.LightCoral);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
