using Leaf.xNet;
using System;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;

namespace Dox.Components
{
    public class IP
    {
        public static IPAddress pAddress;
        private static readonly HttpRequest req = null;

        public static void IP_Address()
        {
            do
            {
                Colorful.Console.Clear();
                AsciiMenu.Menu.GetTitle();
                Colorful.Console.Write("[+] IP: ");
            }
            while (!IPAddress.TryParse(Colorful.Console.ReadLine(), out pAddress));

            IP_Information(pAddress);
        }

        private static void IP_Information(IPAddress IP)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader(HttpHeader.ContentType, "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    req.AddHeader(HttpHeader.ContentEncoding, "gzip, deflate, br");
                    req.AddHeader(HttpHeader.CacheControl, "max-age=0");
                    req.AddHeader("Host", "ipwhois.app");
                    req.AddHeader(HttpHeader.AcceptLanguage, "en-US,en;q=0.9");
                    req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.88 Safari/537.36";
                    Leaf.xNet.HttpResponse Resp = req.Get(new Uri("http://ipwhois.app/json/" + IP));
                    string input1 = Resp.StatusCode < Leaf.xNet.HttpStatusCode.BadRequest ? Resp.ToString() : throw new Exception("Banned proxy");
                    if (input1.Contains("success"))
                    {
                        string Status = Regex.Match(input1, "\"success\":(.*?),").Groups[1].Value;
                        string Type = Regex.Match(input1, "\"type\":\"(.*?)\",").Groups[1].Value;
                        string Continent = Regex.Match(input1, "\"continent\":\"(.*?)\"").Groups[1].Value;
                        string Continent_code = Regex.Match(input1, "\"continent_code\":\"(.*?)\",").Groups[1].Value;
                        string Country_code = Regex.Match(input1, "\"country_code\":\"(.*?)\",").Groups[1].Value;
                        string NumberID = Regex.Match(input1, "\"country_phone\":\"(.*?)\"").Groups[1].Value;
                        string Region = Regex.Match(input1, "\"region\":\"(.*?)\",").Groups[1].Value;
                        string City = Regex.Match(input1, "\"city\":\"(.*?)\",").Groups[1].Value;
                        string Lat = Regex.Match(input1, "\"latitude\":(.*?),").Groups[1].Value;
                        string Long = Regex.Match(input1, "\"longitude\":(.*?),").Groups[1].Value;
                        string ISP = Regex.Match(input1, "\"isp\":\"(.*?)\",").Groups[1].Value;
                        string Currency = Regex.Match(input1, "\"currency\":\"(.*?)\",").Groups[1].Value;
                        Print_Information(IP, Status, Type, Continent, Continent_code, Country_code, NumberID, Region, City, Lat, Long, ISP, Currency);
                    }
                    else
                    {
                        Colorful.Console.WriteLine("[Request Error] Error accessing api", Color.Red);
                    }
                }
            }
            catch (HttpException ex)
            {
                Colorful.Console.WriteLine("[Request Error] " + ex.Message, Color.Red);
            }
            catch (Exception ex)
            {
                Colorful.Console.WriteLine("[Exception Error] " + ex.Message, Color.Red);
            }
            finally
            {
                req?.Dispose();
            }
        }

        private static void Print_Information(IPAddress IP, string status, string type, string continent, string continent_code, string country_code, string phone, string region, string city, string latitude, string longitude, string isp, string currency)
        {
            var Print = new string[]
            {
                "\t[" + IP + "]" + "\t[" + status.ToUpper() + "]\n",
                "[+] Type: " + type,
                "[+] Cotinent: " + continent,
                "[+] Cotinent Code: " + continent_code,
                "[+] Country Code: " + country_code,
                "[+] Mobile Phone: " + phone,
                "[+] Region: " + region,
                "[+] City: " + city,
                "[+] Latitude: " + latitude,
                "[+] Longitude: " + longitude,
                "[+] ISP: " + isp,
                "[+] Currency: " + currency,
            };
            Colorful.Console.WriteLine("\n");
            foreach (string line in Print)
            {
                Colorful.Console.WriteLine(line, Color.WhiteSmoke);
            }

            Colorful.Console.Write("\n[+] Would you like to save this to your database? (Y/N): ");
            string URply = Colorful.Console.ReadLine();
            switch (URply)
            {
                case "Y":
                    SaveToFile.SaveToFile.IP_MakeTextFile(IP, status, type, continent, continent_code, country_code, phone, region, city, latitude, longitude, isp, currency);
                    break;

                case "N":
                    Colorful.Console.Clear();
                    Program.Main();
                    break;

                default:
                    Colorful.Console.WriteLine("[Error] Invalid answer", Color.Red);
                    break;
            }
        }
    }
}