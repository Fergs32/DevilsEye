using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Dox.Components.UsernameGrabber.Modules
{
    internal abstract class Instagram
    {
        public static string CurrentDir = Directory.GetCurrentDirectory() + "\\Instagram.txt";

        public static void Get(string Username)
        {
            InstaV2Scraper(Username);
        }

        /*
         *
         * When using the CURL method, I don't think the "private" followers count towards or smt? I don't know why this isn't accurate as it's scraped from instagram themselves.
         * pretty fucking clueless of this shit.
         *
         */

        public static void InstaV2Scraper(string Username)
        {
            CommandExecuter.ExecuteCommand($"curl -s \"Accept - Language: en\" \"https://www.instagram.com/" + Username + "\"" + " -L > " + CurrentDir);
            Thread.Sleep(4000);
            string scraper_response2 = File.ReadAllText(CurrentDir);
            if (!scraper_response2.Contains("meta content="))
            {
                ModulesResults.ResultStorage.HasInstagram = false;
                File.Delete(CurrentDir);
            }
            else
            {
                ModulesResults.ResultStorage.HasInstagram = true;
                RequestsCore.Hits++;
                string Followers = Regex.Match(scraper_response2, "meta content=\"(.*?) Followers, ").Groups[1].Value;
                string Following = Regex.Match(scraper_response2, "meta content=\"" + Followers + " Followers, (.*?) Following, ").Groups[1].Value;
                string Posts = Regex.Match(scraper_response2, "meta content=\"" + Followers + " Followers, " + Following + " Following, (.*?) Posts").Groups[1].Value;
                ModulesResults.CaptureResults.InstagramCapture = string.Format(" | Followers: {0} | Following: {1} | Posts: {2}", Followers, Following, Posts);
                File.Delete(CurrentDir);
            }
        }
    }
}