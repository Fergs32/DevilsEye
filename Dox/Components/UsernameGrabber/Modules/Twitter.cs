using Leaf.xNet;
using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class Twitter
    {
        /*
         *
         *         // https://api.twitter.com/i/users/email_available.json?email=
         *  Task List:
         *
         *  Need to find a proper twitter api bypass, or we discard this option
         *  Once api is found, we need to pipe the information into RegexSearch()
         *
         *  If we can't find a api, we can resort to checking if the username simply exists, maybe a HEAD request?
         *  Do general research
         *
         *
         */
        public static string CurrentDir = Directory.GetCurrentDirectory() + "\\2.txt";

        public static void Get(string Username)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    CommandExecuter.ExecuteCommand($"curl -s " + "https://api.twitter.com/i/users/username_available.json?username=" + Username + " > " + CurrentDir);
                    Thread.Sleep(550);
                    try
                    {
                        string api_response = File.ReadAllText(CurrentDir);
                        if (api_response.Contains("Username has already been taken"))
                        {
                            File.Delete(CurrentDir);
                            ModulesResults.ResultStorage.HasTwitter = true;
                            RequestsCore.Hits++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message, Color.Red);
                    }
                }
            }
            catch { }
        }
    }
}