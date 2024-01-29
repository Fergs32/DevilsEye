using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Dox.Components.UsernameGrabber.Modules
{
    internal class Spotify
    {
        public static string CurrentDir = Directory.GetCurrentDirectory() + "\\Spotify.txt";
        public static void Get(string Username)
        {
            CommandExecuter.ExecuteCommand($"curl -s \"Accept - Language: en\" \"https://open.spotify.com/user/" + Username + "\"" + " -L > " + CurrentDir);
            Thread.Sleep(2000);
            string response = File.ReadAllText(CurrentDir);
            if (response.Contains("a user on Spotify"))
            {
                ModulesResults.ResultStorage.HasSpotify = true;
                RequestsCore.Hits++;
                string DisplayName = Regex.Match(response, "<title>(.*) on Spotify</title>").Groups[1].Value;
                string PublicPlaylists = Regex.Match(response, "([0-9]*?) public playlists").Groups[1].Value;
                ModulesResults.CaptureResults.SpotifyCapture = string.Format(" | Display Name: {0} | Public Playlists: {1}", DisplayName, PublicPlaylists);
                File.Delete(CurrentDir);
            }
            else
            {
                ModulesResults.ResultStorage.HasSpotify = false;
                File.Delete(CurrentDir);
            }

        }
    }
}