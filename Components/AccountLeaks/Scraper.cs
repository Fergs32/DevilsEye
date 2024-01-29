using System;
using Leaf.xNet;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Dox.Components.AccountLeaks
{
    internal class Scraper
    {
        private static string CurrentDir = Directory.GetCurrentDirectory() + "\\Misc\\AccountScraper\\";
        private static List<string> FileInfo = new List<string>().ToList<string>();
        private static List<string> TimeStamps = new List<string>().ToList<string>();
        private static int TimeStampElement = 0;
        public static void Start()
        {
            using (HttpRequest req = new())
            {
                req.KeepAliveTimeout = 3000;
                string response = req.Get("https://paste.fo/recent").ToString();
                GetTimeStamps(response);
                GetMatches(response);

                FileInfo = FileInfo.Distinct<string>().ToList<string>();
                foreach(string line in FileInfo)
                {
                    Console.WriteLine(line);
                }

                Console.WriteLine($"[!] Downloaded All files to {CurrentDir}");
                
            }
        }
        private static void GetMatches(string resp)
        {
            Regex regex = new("<a href=\"/(.*?)\"", RegexOptions.IgnoreCase);
            Match m = regex.Match(resp);
            while (m.Success)
            {
                Group g = m.Groups[1];
                if (g.Length > 10)
                {
                   FileInfo.Add(GetFileName(resp, g.Value));
                    TimeStampElement++;
                }
                m = m.NextMatch();
            }
        }
        private static string GetFileName(string resp, string id)
        {
            string Filename = Regex.Match(resp, "<a href=\"/" + id + "\">(.*?)</a>").Groups[1].Value;
            Download.File(id, Filename);
            return string.Format($"File ID: {id} | File Name: {Filename} | Time Created: {TimeStamps[TimeStampElement]}");
        }
        private static void GetTimeStamps(string resp)
        {
            Regex regex = new("<td class=\"td-time\">(.*?)</td>", RegexOptions.IgnoreCase);
            Match m = regex.Match(resp);
            while (m.Success)
            {
                Group g = m.Groups[1];
                TimeStamps.Add(g.Value);
                m = m.NextMatch();
            }
        }
    }
    public class Download
    {
        private static string CurrentDir = Directory.GetCurrentDirectory() + "\\Misc\\AccountScraper\\";
        public static void File(string ID, string Filename)
        {
            using (HttpRequest req = new HttpRequest())
            {
                var resp = req.Get("https://paste.fo/" + ID);
                resp.ToFile(CurrentDir + $"{Filename + "-" + ID}.txt");
            }
        }
    }
}
