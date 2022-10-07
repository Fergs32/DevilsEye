using Colorful;
using Leaf.xNet;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using Dox.Components.Tools.ServerAnalysis;

namespace Dox.Components.Minecraft
{
    public class McServerInfo
    {
        public static void Get()
        {
            string serv_ip = "";
            Console.Clear();
            AsciiMenu.Menu.GetTitle();
            Console.ForegroundColor = Color.White;
            do
            {
                Console.Write("[+] Server IP: ", Color.Magenta); serv_ip = Console.ReadLine();
                Console.Clear(); AsciiMenu.Menu.GetTitle();
            }
            while (string.IsNullOrEmpty(serv_ip));
            Console.WriteLine("[+] Please wait while we query about {0}", serv_ip);
            Thread.Sleep(1000);
            Query(serv_ip);
        }

        private static void Query(string ip)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    string input = req.Get("https://mcapi.xdefcon.com/server/" + ip + "/full/json").ToString();
                    if (!input.Contains("\"serverStatus\":\"offline\""))
                    {
                        string server_ip = Regex.Match(input, "\"serverip\": \"(.*?)\",").Groups[1].Value;
                        string version = Regex.Match(input, "\"version\": \"Waterfall (.*?)\",").Groups[1].Value;
                        string server_status = Regex.Match(input, "\"serverStatus\": \"(.*?)\",").Groups[1].Value;
                        string players_online = Regex.Match(input, "\"players\": (.*?),").Groups[1].Value;
                        string max_players = Regex.Match(input, "\"maxplayers\": (.*?),").Groups[1].Value;
                        string motd = Regex.Match(input, "\"text\": \"      (.*?)\",").Groups[1].Value;
                        string ping_to_server = Regex.Match(input, "\"ping\": ([0-9]*?),").Groups[1].Value;

                        ConcatResponse(ip, server_ip, version, server_status, players_online, max_players, motd, ping_to_server);
                    }
                }
            }
            catch(HttpException ex)
            {
                Console.WriteLine("EXCEPTION: {0}", ex);
            }
        }

        private static void ConcatResponse(string n, string ip, string ver, string stat, string players, string max, string motd, string ping)
        {      
            Console.Clear(); AsciiMenu.Menu.GetTitle();
            Console.Write("[+] ", Color.DarkMagenta); Console.Write("Queried IP: ", Color.Magenta); Console.Write(n + "\n\n", Color.White);

            Console.Write("[+] ", Color.DarkMagenta); Console.Write("Server IP: ", Color.Magenta); Console.Write(ip + "\n", Color.White);
            Console.Write("[+] ", Color.DarkMagenta); Console.Write("Version: ", Color.Magenta); Console.Write(ver + "\n", Color.White);
            Console.Write("[+] ", Color.DarkMagenta); Console.Write("Status: ", Color.Magenta); Console.Write(stat[0].ToString().ToUpper() + stat.Substring(1) + "\n", Color.White);
            Console.Write("[+] ", Color.DarkMagenta); Console.Write("Players Online: ", Color.Magenta); Console.Write(players + "\n", Color.White);
            Console.Write("[+] ", Color.DarkMagenta); Console.Write("Max Players: ", Color.Magenta); Console.Write(max + "\n", Color.White);
            Console.Write("[+] ", Color.DarkMagenta); Console.Write("MOTD: ", Color.Magenta); Console.Write(motd + "\n", Color.White);
            Console.Write("[+] ", Color.DarkMagenta); Console.Write("Ping: ", Color.Magenta); Console.Write(ping + "\n", Color.White);

            /*   
             *    
             *     Server analysis tool merged into this
             *    
             */


            Analysis.E_P(n);
            AsciiMenu.Menu.ReturnMenu();

        }
    }
}
