﻿using Leaf.xNet;
using System;
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
                using (HttpRequest httpRequest = new())
                {
                    httpRequest.UserAgentRandomize();
                    HttpResponse resp = httpRequest.Get("https://www.snapchat.com/add/" + Username);
                    string input1 = resp.StatusCode < HttpStatusCode.NotFound ? resp.ToString() : throw new Exception("stupid shit");
                    if (input1.Contains("is on Snapchat!"))
                    {
                        string potential_irl = Regex.Match(input1, "\"pageDescription\":{\"value\":\"(.*?) is on Snapchat!").Groups[1].Value;
                        ResultStorage.HasSnapchat = true;
                        RequestsCore.Hits++;
                        CaptureResults.SnapchatCapture = string.Format("Displayed Name: {0} | Bitmoji: {1}", potential_irl, "https://app.snapchat.com/web/deeplink/snapcode?username=" + Username + "&type=SVG&bitmoji=enable");
                    }
                    else
                    {
                        Console.WriteLine(input1);
                        ResultStorage.HasSnapchat = false;
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