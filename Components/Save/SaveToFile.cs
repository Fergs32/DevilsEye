using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;

namespace Dox.Components.SaveToFile
{
    public class SaveToFile
    {
        // IP directory
        public static string CurrentDir = Directory.GetCurrentDirectory();

        public static string d = CurrentDir + "\\IPDatabase\\";
        public static string Insta = CurrentDir + "\\InstaDatabase\\";
        public static string Facebook = CurrentDir + "\\FacebookDatabase\\";

        
        public static void IP_MakeTextFile(IPAddress IP, string status, string type, string continent, string continent_code, string country_code, string phone, string region, string city, string latitude, string longitude, string isp, string currency)
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
            try
            {
                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                    Thread.Sleep(2000);
                    return;
                }
                if (Directory.Exists(d))
                {
                    Colorful.Console.Write("[Log] Writing data to " + d + IP + ".txt", Color.LightGoldenrodYellow);
                    Console.ReadLine();
                    if (File.Exists(d + IP + ".txt"))
                    {
                        Colorful.Console.Write("\n[Duplicate] You already have {0} in your database, would you like to over-write the current data? (Y/N): ", Color.OrangeRed, IP);
                        string OverwriteData = Colorful.Console.ReadLine();
                        switch (OverwriteData)
                        {
                            case "Y":
                                try
                                {
                                    File.Delete(d + IP + ".txt");
                                    Thread.Sleep(2000);
                                    Colorful.Console.WriteLine("[Log] Successfully deleted {0} from the database", Color.LightGoldenrodYellow, d + IP + ".txt");
                                }
                                catch (IOException ex)
                                {
                                    Colorful.Console.WriteLine("[Error] " + ex, Color.Red);
                                }
                                Thread.Sleep(2000); // Make sure the file gets deleted
                                Colorful.Console.WriteLine("[Log] Re-writing data to {0}", Color.LightGoldenrodYellow, d + IP + ".txt");
                                foreach (string line in Print)
                                {
                                    File.AppendAllText("IPDatabase/" + IP + ".txt", line + Environment.NewLine);
                                }
                                Thread.Sleep(1000);
                                Colorful.Console.WriteLine("[Log] Successfully written to {0}\n", Color.LightGoldenrodYellow, d + IP + ".txt");
                                Colorful.Console.Write("[Input] Would you like to go back to main menu? (Y/N): ", Color.LightGoldenrodYellow);
                                string Menu = Colorful.Console.ReadLine();
                                switch (Menu)
                                {
                                    case "Y":
                                        Program.Main();
                                        break;

                                    case "N":
                                        Environment.Exit(0);
                                        break;

                                    default:
                                        Colorful.Console.WriteLine("[Error] Invalid Input", Color.Red);
                                        break;
                                }
                                break;

                            case "N":
                                Program.Main();
                                break;

                            default:
                                Colorful.Console.WriteLine("[Error] Invalid Input", Color.Red);
                                break;
                        }
                    }
                    else
                    {
                        foreach (string line in Print)
                        {
                            File.AppendAllText("IPDatabase/" + IP + ".txt", line + Environment.NewLine);
                        }
                    }
                    Colorful.Console.Write("\n[Log] Completed writing data to " + d + IP + ".txt\n", Color.LightGoldenrodYellow);
                    Colorful.Console.Write("[Input] Would you like to go back to main menu? (Y/N): ", Color.LightGoldenrodYellow);
                    string Menu2 = Colorful.Console.ReadLine();
                    switch (Menu2)
                    {
                        case "Y":
                            Program.Main();
                            break;

                        case "N":
                            Environment.Exit(0);
                            break;

                        default:
                            Colorful.Console.WriteLine("[Error] Invalid Input", Color.Red);
                            break;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); Console.ReadLine(); }
        }
    }
}