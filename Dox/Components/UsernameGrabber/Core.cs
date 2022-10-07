using System;
using System.Drawing;
using Dox.Components.UsernameGrabber.Modules;
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
                System.Console.Clear();
                AsciiMenu.Menu.GetTitle();
                RequestsCore.MakeRequests(GetUsername);
            }
            catch (Exception ex) { Colorful.Console.WriteLine("[Exception] " + ex, Color.Red); }
        }
    }

    abstract class RequestsCore
    {
        public static void MakeRequests(string Username)
        {
            try
            {
                Colorful.Console.Write("[+] Instagram: ", Color.DarkMagenta); Colorful.Console.Write("Patched #ID84923"+ "\n", Color.Magenta);
                Colorful.Console.Write("[+] Twitter: ", Color.DarkMagenta); Colorful.Console.Write("Patched #ID84924" + "\n", Color.Magenta);
                Twitch.Get(Username);
                Colorful.Console.Write("[+] Twitch: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasTwitch + " | Capture: " + CaptureResults.TwitchCapture + "\n", Color.Magenta);
                Snapchat.Get(Username);
                Colorful.Console.Write("[+] Snapchat: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasSnapchat + " | Capture: " + CaptureResults.SnapchatCapture + "\n", Color.Magenta);
                Github.Get(Username);
                Colorful.Console.Write("[+] Github: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasGithub + " | Capture: " + CaptureResults.GithubCapture + "\n", Color.Magenta);
                Youtube.Get(Username);
                Colorful.Console.Write("[+] Youtube: ", Color.DarkMagenta); Colorful.Console.Write(ResultStorage.HasYoutube + "\n", Color.Magenta);
                AsciiMenu.Menu.ReturnMenu();
            }
            catch(Exception ex)
            {
                Colorful.Console.WriteLine(ex);
            }
        }
    }
}
