using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Dox.Components.UsernameGrabber.Modules.ModulesResults;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class Github
    {
        private static string repos = "0";
        private static string starred = "0";
        private static string following = "0";
        private static string followers = "0";

        public static void Get(string Username)
        {
            List<string> FollowExtractionList = new List<string>().ToList<string>();
            try
            {
                using (HttpRequest httpRequest = new HttpRequest())
                {
                    httpRequest.UserAgentRandomize();
                    HttpResponse resp = httpRequest.Get("https://github.com/" + Username);
                    string input = resp.StatusCode < HttpStatusCode.NotFound ? resp.ToString() : throw new Exception("stupid shit");
                    if (Regex.Match(input, "([0-9]*?) repositories available").Groups[1].Value != "")
                    {
                        repos = Regex.Match(input, "([0-9]*?) repositories available").Groups[1].Value;
                    }
                    if (Regex.Match(input, "data-view-component=\"true\" class=\"Counter\">(.*?)</span>").Groups[1].Value != "")
                    {
                        starred = Regex.Match(input, "data-view-component=\"true\" class=\"Counter\">(.*?)</span>").Groups[1].Value;
                    }
                    var regex = new Regex("class=\"text-bold color-fg-default\">(.*?)</span>", RegexOptions.IgnoreCase);
                    Match m = regex.Match(input);
                    MatchCollection Matches = Regex.Matches(input, "class=\"text-bold color-fg-default\">(.*?)</span>");
                    while (m.Success)
                    {
                        for (int i = 0; i < Matches.Count; i++)
                        {
                            Group g = m.Groups[1];
                            FollowExtractionList.Add(g.Value);
                        }
                        m = m.NextMatch();
                    }
                    if (FollowExtractionList.Count > 0)
                    {
                        followers = FollowExtractionList[0];
                        following = FollowExtractionList[2];
                    }
                    ResultStorage.HasGithub = true;
                    RequestsCore.Hits++;
                    ModulesResults.CaptureResults.GithubCapture = FormatString(repos, starred, followers, following);
                }
            }
            catch (Exception)
            {
            }
        }

        private static string FormatString(string repos, string starred, string followers, string following)
        {
            string BASE_STRING = "  | Capture: ";

            if (string.IsNullOrEmpty(repos)) { BASE_STRING = BASE_STRING + "Repos: 0"; } else { BASE_STRING = BASE_STRING + "Repos: " + repos; }
            if (string.IsNullOrEmpty(starred)) { BASE_STRING = BASE_STRING + " |  Starred: 0"; } else { BASE_STRING = BASE_STRING + " |  Starred: " + starred; }
            if (string.IsNullOrEmpty(followers)) { BASE_STRING = BASE_STRING + " |  Followers: 0"; } else { BASE_STRING = BASE_STRING + " |  Followers: " + followers; }
            if (string.IsNullOrEmpty(following)) { BASE_STRING = BASE_STRING + " |  Following: 0"; } else { BASE_STRING = BASE_STRING + " |  Following: " + following; };

            return BASE_STRING;
        }
    }
}