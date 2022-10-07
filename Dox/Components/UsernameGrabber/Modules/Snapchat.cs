using System;
using Leaf.xNet;
using System.Text.RegularExpressions;
using static Dox.Components.UsernameGrabber.Modules.ModulesResults;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class Snapchat
    {
        public static void Get(string Username)
        {
            try
            {
                using (HttpRequest httpRequest = new HttpRequest())
                {
                    httpRequest.UserAgentRandomize();
                    HttpResponse resp = httpRequest.Get("https://www.snapchat.com/add/" + Username);
                    string input1 = resp.StatusCode < HttpStatusCode.NotFound ? resp.ToString() : throw new Exception("stupid shit");
                    if (input1.Contains(Username.ToLower() + " on Snapchat"))
                    {
                        string potential_irl = Regex.Match(input1, "\"pageDescription\":{\"value\":\"(.*?) is on Snapchat!").Groups[1].Value;
                        ResultStorage.HasSnapchat = true;
                        CaptureResults.SnapchatCapture = string.Format("Displayed Name: {0} | Bitmoji: {1}", potential_irl, "https://app.snapchat.com/web/deeplink/snapcode?username=" + Username + "&type=SVG&bitmoji=enable");

                    }
                    else
                    {
                        ResultStorage.HasSnapchat = false;
                    }
                    httpRequest.Dispose();
                }
            }
            catch(Exception)
            {
            }
        }
    }
}
