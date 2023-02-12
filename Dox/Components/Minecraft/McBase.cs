using Colorful;
using Leaf.xNet;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Dox.Components.Minecraft
{
    public class McBase
    {
        private static string UUID = "";
        public static List<string> prev_names = new List<string>().ToList<string>();
        public static List<string> db = new List<string>().ToList<string>();

        private static void GetByName(string Username)
        {
            try
            {
                Console.Write("[+]", Color.DarkMagenta); Console.Write(" Validating name please wait...\n", Color.Magenta);
                if (ValidatePlayer(Username) != true)
                {
                    Console.Write("[+]", Color.DarkMagenta); Console.Write(" The IGN you provided is invalid.", Color.Magenta);
                    Thread.Sleep(-1);
                }
                Console.Write("[+]", Color.DarkMagenta); Console.Write(" Success {0} is a registered name.\n", Color.Magenta, Username);
                Console.Write("[+]", Color.DarkMagenta); Console.Write(" Loading DB please wait: "); ApiCalls.xx(Username);
                db = File.ReadLines("output.txt").ToList<string>(); Console.Write(db.Count + "\n");
                Console.Write("[+]", Color.DarkMagenta); Console.Write(" Attemping to gather information on {0}, please wait...", Color.Magenta, Username);
                Thread.Sleep(2000);
                MC_Menu(Username);
            }
            catch
            {
            }
        }

        public static void GetName()
        {
            Console.Clear();
            AsciiMenu.Menu.GetTitle();
            Console.ForegroundColor = Color.White;
            try
            {
                Console.Write("[+]", Color.DarkMagenta); Console.Write(" Enter IGN: ", Color.Magenta); string username = Console.ReadLine();
                GetByName(username);
            }
            catch { }
        }

        private static bool ValidatePlayer(string Username)
        {
            bool IsValid = false;
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpResponse MojangResp = req.Get("https://api.mojang.com/users/profiles/minecraft/" + Username);
                    if (MojangResp.StatusCode == HttpStatusCode.OK)
                    {
                        string c_s = MojangResp.ToString();
                        UUID = Regex.Match(c_s, "\"id\":\"(.*?)\"").Groups[1].Value;
                        IsValid = true;
                    }
                }
            }
            catch (HttpException)
            {
            }
            return IsValid;
        }

        public static void MC_Menu(string Username)
        {
            List<string> leaks_sorted = new List<string>().ToList<string>();
            Console.Clear();
            AsciiMenu.Menu.GetTitle();

            Console.WriteLine("|---------------------------------------------------------------------------|", Color.Magenta);
            Console.WriteLine("| IGN: {0}   | Registered: True", Color.Magenta, Username);
            Console.WriteLine("| Socials Found: {0}", Color.Magenta, ApiCalls.AttemptSocials(Username));
            Console.WriteLine("| Known as: [API Removed By Mojang]", Color.Magenta);
            Console.WriteLine("| Potential Friends: {0}\n", Color.Magenta, ApiCalls.AttemptFriendsList(UUID));

            Console.WriteLine("| Please wait, scanning database...");

            for (int x = 0; x < 1;)
            {
                Parallel.ForEach(db, (line) =>
                {
                    if (line.Contains(Username))
                    {
                        leaks_sorted.Add(line);
                    }
                });
                x++;
            }
            leaks_sorted = leaks_sorted.Distinct().ToList();
            foreach (string line in leaks_sorted)
            {
                Console.WriteLine("| [+] " + line, Color.Green);
            }
            AsciiMenu.Menu.ReturnMenu();
        }

        public static void TransformIntoList()
        {
            using (var output = File.Create("output.txt"))
            {
                foreach (var file in new[] { "", "", "null", "null" })
                {
                    using (var input = File.OpenRead(file))
                    {
                        input.CopyTo(output);
                    }
                }
            }
            try
            {
                File.Delete("null"); File.Delete("null"); File.Delete("null"); File.Delete("null");
            }
            catch { }
        }
    }
}