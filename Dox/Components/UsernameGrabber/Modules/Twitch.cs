using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class Twitch
    {
        public static void Get(string Username)
        {
            // SDE (Sensitive Data Exposure) e.g cookies
            // https://api.twitch.tv/helix/users?login=
            try
            {
                using (HttpRequest httpRequest = new HttpRequest())
                {
                    httpRequest.UserAgentRandomize();
                    HttpResponse resp = httpRequest.Get("https://m.twitch.tv/" + Username); string input = resp.ToString();

                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        ModulesResults.ResultStorage.HasTwitch = false;
                    }
                    else if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        ModulesResults.ResultStorage.HasTwitch = true;
                        RequestsCore.Hits++;
                        string followers = Regex.Match(input, "\"CoreText-sc-cpl358-0 eOusxk\">(.*?) followers</p>").Groups[1].Value;
                        string LastLive = Regex.Match(input, "class=\"CoreText-sc-smutr2-0 brjxRj\">(.*?)</p>").Groups[1].Value;

                        List<string> SocialList = new List<string>().ToList<string>();
                        Regex regex = new Regex("class=\"CoreText-sc-smutr2-0 fhsthX\">(.*?)</p>", RegexOptions.IgnoreCase);
                        Match m = regex.Match(input);
                        MatchCollection Matches = Regex.Matches(input, "class=\"CoreText-sc-smutr2-0 fhsthX\">(.*?)</p>");

                        while (m.Success)
                        {
                            for (int i = 0; i < Matches.Count; i++)
                            {
                                Group g = m.Groups[1];
                                SocialList.Add(g.Value);
                            }
                            m = m.NextMatch();
                        }

                        string commaSeparatedList;
                        if (SocialList.Count != 0) { commaSeparatedList = SocialList.Aggregate((a, x) => a + ", " + x); } else { commaSeparatedList = ""; }

                        ModulesResults.CaptureResults.TwitchCapture = string.Format("Followers: {0} | Last Live: {1} | Socials: {2}", followers, LastLive, commaSeparatedList);
                    }
                    else
                    {
                        ModulesResults.ResultStorage.HasTwitch = false;
                    }
                    httpRequest.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}