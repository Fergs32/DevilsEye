using System;
using Leaf.xNet;
using System.Drawing;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Dox.Components.UsernameGrabber.Modules
{
    abstract class Instagram
    {
        public static void Get(string Username)
        {
            try
            {
                using (HttpRequest request = new HttpRequest())
                {
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36";
                    request.AddHeader(HttpHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.ConnectTimeout = 4000;
                    string resp_string = request.Get("https://www.instagram.com/web/search/topsearch/?context=blended&query=" + Username + "&rank_token=0.3953592318270893&count=1").ToString();                   
                    if (resp_string.Contains("\"users\":") || resp_string.Contains("\"username\":\"" + Username + "\"") || resp_string.Contains("\"status\":\"ok\""))
                    {
                        ModulesResults.ResultStorage.HasInstagram = true;
                        switch(ModulesResults.ResultStorage.WriteToFiles)
                        {
                            case "y":
                                break;
                            case "n":
                                break;
                        }                        
                    }
                    else
                    {
                        ModulesResults.ResultStorage.HasInstagram = false;
                    }
                    request.Dispose();
                }
            }
            catch(HttpException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
