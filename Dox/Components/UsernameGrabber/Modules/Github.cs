using System;
using Leaf.xNet;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic; 
using static Dox.Components.UsernameGrabber.Modules.ModulesResults;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class Github
    {
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

                    string repos = Regex.Match(input, "([0-9]*?) repositories available").Groups[1].Value;
                    string starred = Regex.Match(input, "data-view-component=\"true\" class=\"Counter\">(.*?)</span>").Groups[1].Value;

                    Regex regex = new Regex("class=\"text-bold color-fg-default\">(.*?)</span>", RegexOptions.IgnoreCase);
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
                    string followers = FollowExtractionList[0];
                    string following = FollowExtractionList[2];

                    CaptureResults.GithubCapture = string.Format("Repositories: {0} | Stars: {1} | Followers: {2} | Following {3}", repos, starred, followers, following);
                    ResultStorage.HasGithub = true;                  
                }
            }
            catch(Exception)
            {
            }
        }
    }
}
