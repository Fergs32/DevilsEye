using Leaf.xNet;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace Dox.Components.UsernameGrabber.Modules
{
    internal class Ebay
    {
        public static string CurrentDir = Directory.GetCurrentDirectory() + "\\Ebay.txt";
        public static void Get(string Username)
        {
            /*
             * 
             * This module has a rate limit on scrapes per IP, as discovered during research (3 requests per 1-2h)
             * so if you don't wanna get your ip rate limited by ebay i'd suggest to just comment this module out.
             * 
             */
            using (HttpRequest req = new HttpRequest())
            {
                CommandExecuter.ExecuteCommand($"curl -s \"Accept - Language: en\" \"https://www.ebay.com/usr/" + Username + "\"" + " -L > " + CurrentDir);
                Thread.Sleep(2000);
                string response = File.ReadAllText(CurrentDir);

                if (response.Contains("Followers"))
                {
                    ModulesResults.ResultStorage.HasEbay = true;
                    RequestsCore.Hits++;
                    string ItemsSold = Regex.Match(response, "<div title=\"([0-9]*?) Items sold\">").Groups[1].Value;
                    string Followers = Regex.Match(response, "<div title=\"([0-9]*?) Followers\">").Groups[1].Value;

                    ModulesResults.CaptureResults.EbayCapture = string.Format(" | Followers: {0} | Items Sold: {1}", Followers, ItemsSold);
                    File.Delete(CurrentDir);

                }
                else
                {
                    ModulesResults.ResultStorage.HasEbay = false;
                    File.Delete(CurrentDir);
                }
            }

        }
    }
}
