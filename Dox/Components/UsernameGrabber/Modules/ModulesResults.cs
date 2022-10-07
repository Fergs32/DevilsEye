using System;
using System.Collections.Generic;
using System.Text;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class ModulesResults
    {
        public struct ResultStorage
        {
            public static string WriteToFiles = "";

            public static bool HasInstagram; // Done
            public static Boolean HasTwitter; 
            public static Boolean HasSnapchat; // Done
            public static bool HasYoutube; // Done
            public static Boolean HasGithub; // Done
            public static Boolean HasTikTok;
            public static bool HasTwitch; // Done
        }

        public struct CaptureResults
        {
            public static string TwitchCapture = "";
            public static string SnapchatCapture = "";
            public static string GithubCapture = "";
        }
    }
}
