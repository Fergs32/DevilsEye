using Leaf.xNet;
using System.Drawing;
using System.Threading;
using Colorful;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;
using System;

namespace Dox.Components.Ddos
{
    public class StartAttack
    {
        private static int newport;

        public static void Init()
        {
            string mode = "";
            string serv_ip;
            Colorful.Console.ForegroundColor = Color.White;
            try
            {
                Colorful.Console.Write("[+] Enter MC Server IP: ", Color.DarkMagenta); serv_ip = Colorful.Console.ReadLine();
                using (HttpRequest req = new HttpRequest())
                {
                    string input = req.Get("https://mcapi.xdefcon.com/server/" + serv_ip + "/full/json").ToString();
                    if (!input.Contains("\"serverStatus\":\"offline\""))
                    {
                        string server_ip = Regex.Match(input, "\"serverip\": \"(.*?)\",").Groups[1].Value;
                        Start_Threads(server_ip, "25565");
                    }
                }
                
            }
            catch(System.Exception ex)
            {
                Colorful.Console.WriteLine(ex);
            }
        }

        private static void Start_Threads(string ip, string port)
        {
            Colorful.Console.WriteLine(ip + ":" + port);
            newport = int.Parse(port);
            Colorful.Console.WriteLine(newport); Colorful.Console.ReadLine();

            Colorful.Console.Clear(); AsciiMenu.Menu.GetTitle();
        }

    }
}
