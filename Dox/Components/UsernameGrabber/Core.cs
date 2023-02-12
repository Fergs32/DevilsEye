using Dox.Components.UsernameGrabber.Modules;
using System;
using System.Drawing;
using System.Threading;
using static Dox.Components.UsernameGrabber.Modules.ModulesResults;

namespace Dox.Components.UsernameGrabber
{
    public class Core
    {
        private static string GetUsername { get; set; }

        public static void GetUsernameInfo()
        {
            Colorful.Console.Clear();
            AsciiMenu.Menu.GetTitle();
            Colorful.Console.Write("[+] Username: "); GetUsername = Colorful.Console.ReadLine();
            try
            {
                Console.Clear();
                AsciiMenu.Menu.GetTitle();
                RequestsCore.MakeRequests(GetUsername);
            }
            catch (Exception ex) { Colorful.Console.WriteLine("[Exception] " + ex, Color.Red); }
        }
    }

    internal abstract class RequestsCore
    {
        private static Boolean Loop_Lock = true;
        public static int Hits;

        public static void MakeRequests(string Username)
        {
            try
            {
                new Thread(new ThreadStart(SetTitle)).Start();
                Instagram.Get(Username);
                Colorful.Console.Write("[+] Instagram: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasInstagram + CaptureResults.InstagramCapture + "\n", Color.Magenta);
                Twitter.Get(Username);
                Colorful.Console.Write("[+] Twitter: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasTwitter + "\n", Color.Magenta);
                Twitch.Get(Username);
                Colorful.Console.Write("[+] Twitch: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasTwitch + " | Capture: " + CaptureResults.TwitchCapture + "\n", Color.Magenta);
                Snapchat.Get(Username);
                Colorful.Console.Write("[+] Snapchat: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasSnapchat + " | Capture: " + CaptureResults.SnapchatCapture + "\n", Color.Magenta);
                Github.Get(Username);
                Colorful.Console.Write("[+] Github: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasGithub + CaptureResults.GithubCapture + "\n", Color.Magenta);
                Youtube.Get(Username);
                Colorful.Console.Write("[+] Youtube: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasYoutube + "\n", Color.Magenta);
                Spotify.Get(Username);
                Colorful.Console.Write("[+] Spotify: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasSpotify  + CaptureResults.SpotifyCapture + "\n", Color.Magenta);
                Ebay.Get(Username);
                Colorful.Console.Write("[+] Ebay: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasEbay + CaptureResults.EbayCapture + " // Possible rate limit!\n", Color.Magenta);
                Loop_Lock = false;
                AsciiMenu.Menu.ReturnMenu();
            }
            catch (Exception ex)
            {
                Colorful.Console.WriteLine(ex);
            }
        }

        private static void SetTitle()
        {
            try
            {
                while (Loop_Lock)
                {
                    Console.Title = string.Format("Username Search | Developed by Fergs32 | Linked Accounts: {0} | Not found: {1} | Errors: {2}", RequestsCore.Hits, 6 - RequestsCore.Hits, 0);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something happened when trying to set console title, maybe incompatible OS?");
            }
        }
    }
}