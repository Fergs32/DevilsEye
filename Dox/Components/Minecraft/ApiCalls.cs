using CG.Web.MegaApiClient;
using Colorful;
using Leaf.xNet;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Dox.Components.Minecraft
{
    public class ApiCalls
    {
        public static string AttemptFriendsList(string UUID)
        {
            List<string> friends = new List<string>().ToList<string>();
            string commaSeparatedList;

            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpResponse input = req.Get("https://api.namemc.com/profile/" + UUID + "/friends");
                    string input1 = input.StatusCode < HttpStatusCode.NotFound ? input.ToString() : throw new System.Exception("stupid shit");
                    Regex regex = new Regex("\"name\": \"(.*?)\"", RegexOptions.IgnoreCase);
                    Match m = regex.Match(input1);
                    MatchCollection Matches = Regex.Matches(input1, "\"name\": \"(.*?)\"");

                    while (m.Success)
                    {
                        for (int i = 0; i < Matches.Count;)
                        {
                            Group g = m.Groups[1];
                            friends.Add(g.Value);
                            i++;
                        }
                        m = m.NextMatch();
                    }

                    friends = friends.Distinct().ToList();
                    if (friends.Count != 0) { commaSeparatedList = friends.Aggregate((a, x) => a + ", " + x); } else { commaSeparatedList = ""; }
                    return commaSeparatedList;
                }
            }
            catch (HttpException)
            {
            }
            return "";
        }

        public static string AttemptSocials(string Username)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.IgnoreProtocolErrors = true;
                    req.KeepAliveTimeout = 5000;
                    req.ReadWriteTimeout = 5000;

                    HttpResponse resp = req.Get("https://api.slothpixel.me/api/players/" + Username);
                    string input1 = resp.StatusCode < HttpStatusCode.NotFound ? resp.ToString() : throw new System.Exception("stupid shit");

                    if (input1.Contains("\"links\":{"))
                    {
                        string twitter = Regex.Match(input1, "{\"TWITTER\":\"(.*?)\",").Groups[1].Value.Replace("null", "");
                        string youtube = Regex.Match(input1, "\"YOUTUBE\":\"(.*?)\",").Groups[1].Value.Replace("null", "");
                        string insta = Regex.Match(input1, "\"INSTAGRAM\":(.*?),").Groups[1].Value.Replace("null", "");
                        string twitch = Regex.Match(input1, "\"TWITCH\":(.*?),").Groups[1].Value.Replace("null", "");
                        string discord = Regex.Match(input1, "\"DISCORD\":\"(.*?)\",").Groups[1].Value.Replace("null", "");
                        string str1 = "";
                        if (twitter != "") str1 = twitter + ", "; if (youtube != "") str1 = str1 + youtube + ", "; if (insta != "") str1 = str1 + insta + ", "; if (twitch != "") str1 = str1 + twitch + ", "; if (discord != "") str1 = str1 + discord + ",";

                        return str1;
                    }
                }
            }
            catch (HttpException)
            {
            }
            return "";
        }

        /*
         *
         * API removed by mojang, as of 13/09/2022
         *
         *
        public static string AttemptPreviousNames(string UUID)
        {
            List<string> names = new List<string>().ToList<string>();
            string commaSeparatedList;

            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpResponse input = req.Get("https://api.mojang.com/user/profiles/" + UUID + "/names");
                    string input1 = input.StatusCode < HttpStatusCode.NotFound ? input.ToString() : throw new System.Exception("stupid shit");
                    Regex regex = new Regex("\"name\":\"(.*?)\"", RegexOptions.IgnoreCase);
                    Match m = regex.Match(input1);
                    MatchCollection Matches = Regex.Matches(input1, "\"name\":\"(.*?)\"");

                    while (m.Success)
                    {
                        for (int i = 0; i < Matches.Count;)
                        {
                            Group g = m.Groups[1];
                            names.Add(g.Value);
                            i++;
                        }
                        m = m.NextMatch();
                    }

                    names = names.Distinct().ToList(); McBase.prev_names = names; McBase.prev_names.Add(UUID);
                    if (names.Count != 0) { commaSeparatedList = names.Aggregate((a, x) => a + ", " + x); } else { commaSeparatedList = ""; }
                    return commaSeparatedList;
                }
            }
            catch (HttpException ex)
            {
                Console.WriteLine(ex);
            }
            return "";
        }
        */

        private static string[] Dbs = new string[]
        {
            "null",
            "null",
            "null",
            "null",
        };

        public static void xx(string Username)
        {
            System.Action onCompleted = () =>
            {
                McBase.TransformIntoList();
                McBase.db = File.ReadLines("output.txt").ToList<string>(); Console.Write(McBase.db.Count + "\n");
                Console.Write("[+]", Color.DarkMagenta); Console.Write(" Attemping to gather information on {0}, please wait...", Color.Magenta, Username);
                Thread.Sleep(2000);
                McBase.MC_Menu(Username);
                File.Delete("output.txt");
            };
            var thread = new Thread(
              () =>
              {
                  try
                  {
                      File1();
                      File2();
                      File3();
                      File4();
                  }
                  finally
                  {
                      onCompleted();
                  }
              });
            thread.Start();
            thread.Priority = ThreadPriority.Highest;
        }

        public static void File1()
        {
            try
            {
                MegaApiClient client = new MegaApiClient();
                client.LoginAnonymous();

                System.Uri fileLink = new System.Uri(Dbs[0]);
                INode node = client.GetNodeFromLink(fileLink);
                client.DownloadFile(fileLink, node.Name);

                client.Logout();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Link was deleted or moved");
            }
        }

        public static void File2()
        {
            try
            {
                MegaApiClient client = new MegaApiClient();
                client.LoginAnonymous();

                System.Uri fileLink = new System.Uri(Dbs[1]);
                INode node = client.GetNodeFromLink(fileLink);
                client.DownloadFile(fileLink, node.Name);

                client.Logout();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Link was deleted or moved");
            }
        }

        public static void File3()
        {
            try
            {
                MegaApiClient client = new MegaApiClient();
                client.LoginAnonymous();

                System.Uri fileLink = new System.Uri(Dbs[2]);
                INode node = client.GetNodeFromLink(fileLink);
                client.DownloadFile(fileLink, node.Name);

                client.Logout();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Link was deleted or moved");
            }
        }

        public static void File4()
        {
            try
            {
                MegaApiClient client = new MegaApiClient();
                client.LoginAnonymous();

                System.Uri fileLink = new System.Uri(Dbs[3]);
                INode node = client.GetNodeFromLink(fileLink);
                client.DownloadFile(fileLink, node.Name);

                client.Logout();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Link was deleted or moved");
            }
        }
    }
}