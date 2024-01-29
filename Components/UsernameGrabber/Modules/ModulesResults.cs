using System;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class ModulesResults
    {
        public struct ResultStorage
        {
            public static bool HasInstagram; // Done
            public static bool HasTwitter;  // Done found private api for usernames + emails, probs will be used when this is released.
            public static bool HasSnapchat; // Done
            public static bool HasYoutube; // Done
            public static bool HasGithub; // Done
            public static bool HasTikTok;
            public static bool HasTwitch; // Done
            public static bool HasSpotify; // Done
            public static bool HasEbay; // Done
        }

        public struct CaptureResults
        {
            public static string? TwitchCapture = "";
            public static string? SnapchatCapture = "";
            public static string? GithubCapture = "";
            public static string? InstagramCapture = "";
            public static string? SpotifyCapture = "";
            public static string? EbayCapture;
        }
    }
}