using Spectre.Console;
using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Dox.Components
{
    public abstract class IP
    {
        private const string ApiUrl = "http://ipwhois.app/json/";
        private static readonly HttpClient HttpClient = new HttpClient();
        public static void GetIpAddress()
        {
            IPAddress? ipAddress;

            do
            {
                Console.Clear();
                AsciiMenu.Menu.GetTitle();
                Console.Write("[+] IP: ");
            } while (!IPAddress.TryParse(Console.ReadLine(), out ipAddress));
            GetIpInformation(ipAddress);
        }

        private static async void GetIpInformation(IPAddress ip)
        {
            try
            {
                HttpResponseMessage httpResponse = await HttpClient.GetAsync(ApiUrl + ip);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string responseContent = await httpResponse.Content.ReadAsStringAsync();
                    ParseAndPrintInformation(ip, responseContent);
                }
                else
                {
                    Console.WriteLine("[Request Error] Error accessing API");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[Request Error] " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Exception Error] " + ex.Message);
            }
        }

        private static void ParseAndPrintInformation(IPAddress ip, string responseContent)
        {
            // Parse the JSON response and extract required information
            string status = GetJsonValue(responseContent, "\"success\":(.*?),");
            string type = GetJsonValue(responseContent, "\"type\":\"(.*?)\",");
            string continent = GetJsonValue(responseContent, "\"continent\":\"(.*?)\"");
            string continentCode = GetJsonValue(responseContent, "\"continent_code\":\"(.*?)\",");
            string countryCode = GetJsonValue(responseContent, "\"country_code\":\"(.*?)\",");
            string phone = GetJsonValue(responseContent, "\"country_phone\":\"(.*?)\"");
            string region = GetJsonValue(responseContent, "\"region\":\"(.*?)\",");
            string city = GetJsonValue(responseContent, "\"city\":\"(.*?)\",");
            string latitude = GetJsonValue(responseContent, "\"latitude\":(.*?),");
            string longitude = GetJsonValue(responseContent, "\"longitude\":(.*?),");
            string isp = GetJsonValue(responseContent, "\"isp\":\"(.*?)\",");
            string currency = GetJsonValue(responseContent, "\"currency\":\"(.*?)\",");
            PrintInformation(ip, status, type, continent, continentCode, countryCode, phone, region, city, latitude, longitude, isp, currency);
        }
        private static string GetJsonValue(string input, string pattern)
        {
            Match match = Regex.Match(input, pattern);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private static void PrintInformation(IPAddress IP, string status, string type, string continent, string continent_code, string country_code, string phone, string region, string city, string latitude, string longitude, string isp, string currency)
        {
            try
            {
                SaveToFile.SaveToFile.IP_MakeTextFile(IP, status, type, continent, continent_code, country_code, phone, region, city, latitude, longitude, isp, currency);
                var print = new string[] {
                "\t[" + IP + "]" + "\t[" + status.ToUpper() + "]\n",
                "[+] Type: " + type,
                "[+] Continent: " + continent,
                "[+] Continent Code: " + continent_code,
                "[+] Country Code: " + country_code,
                "[+] Mobile Phone: " + phone,
                "[+] Region: " + region,
                "[+] City: " + city,
                "[+] Latitude: " + latitude,
                "[+] Longitude: " + longitude,
                "[+] ISP: " + isp,
                "[+] Currency: " + currency,
                "\n[+] Map Viewer: www.itilog.com/en/gps/" + latitude + "/" + longitude,
                };   
                Console.WriteLine("\n");
                foreach (string line in print)
                {
                    Console.WriteLine(line);
                }
                SaveConfirm(IP, status, type, continent, continent_code, country_code, phone, region, city, latitude, longitude, isp, currency);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private static void SaveConfirm(IPAddress IP, string status, string type, string continent, string continent_code, string country_code, string phone, string region, string city, string latitude, string longitude, string isp, string currency)
        {
            try
            {
                var confirm = AnsiConsole.Confirm("Would you like to save this to your database?");
                Console.ReadLine();
                if (confirm)
                {

                    SaveToFile.SaveToFile.IP_MakeTextFile(IP, status, type, continent, continent_code, country_code, phone, region, city, latitude, longitude, isp, currency);
                }
                else
                {
                    Console.Clear();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}