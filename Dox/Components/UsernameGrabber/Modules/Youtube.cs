using Leaf.xNet;
using System;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class Youtube
    {
        public static void Get(string Username)
        {
            try
            {
                using (HttpRequest httpRequest = new HttpRequest())
                {
                    httpRequest.UserAgentRandomize();
                    HttpResponse resp = httpRequest.Get("https://www.youtube.com/results?search_query=" + Username + "&sp=EgIQAg%253D%253D");
                    string input = resp.ToString();
                    if (input.Contains("\"text\":\"" + Username + "\",\"navigationEndpoint\""))
                    {
                        ModulesResults.ResultStorage.HasYoutube = true;
                        RequestsCore.Hits++;
                    }
                    else
                    {
                        ModulesResults.ResultStorage.HasYoutube = false;
                    }
                }
            }
            catch (Exception) { }
        }
    }
}