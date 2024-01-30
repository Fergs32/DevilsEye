using Dox.Components.UsernameGrabber.Modules;
using static Dox.Components.UsernameGrabber.Modules.ModulesResults;

namespace Dox.Components.UsernameGrabber
{
    public class Core
    {
        private static string? GetUsername { get; set; }

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
        private static bool _loopLock = true;
        public static int Hits;

        public static void MakeRequests(string username)
        {
            try
            {
                LinkedIn.Get(username);
                Thread.Sleep(-1);
            } catch(Exception ex) { Console.WriteLine(ex); }
            try
            {
                new Thread(new ThreadStart(SetTitle)).Start();
                Instagram.Get(username);
                Colorful.Console.Write("[+] Instagram: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasInstagram + CaptureResults.InstagramCapture + "\n", Color.Magenta);
                Twitter.Get(username);
                Colorful.Console.Write("[+] Twitter: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasTwitter + "\n", Color.Magenta);
                Twitch.Get(username);
                Colorful.Console.Write("[+] Twitch: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasTwitch + " | Capture: " + CaptureResults.TwitchCapture + "\n", Color.Magenta);
                Snapchat.Get(username);
                Colorful.Console.Write("[+] Snapchat: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasSnapchat + " | Capture: " + CaptureResults.SnapchatCapture + "\n", Color.Magenta);
                Github.Get(username);
                Colorful.Console.Write("[+] Github: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasGithub + CaptureResults.GithubCapture + "\n", Color.Magenta);
                Youtube.Get(username);
                Colorful.Console.Write("[+] Youtube: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasYoutube + "\n", Color.Magenta);
                Spotify.Get(username);
                Colorful.Console.Write("[+] Spotify: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasSpotify  + CaptureResults.SpotifyCapture + "\n", Color.Magenta);
                Ebay.Get(username);
                Colorful.Console.Write("[+] Ebay: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasEbay + CaptureResults.EbayCapture + " // Possible rate limit!\n", Color.Magenta);
                _loopLock = false;
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
                while (_loopLock)
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