using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using Colorful;
using Newtonsoft.Json.Linq;
using Leaf.xNet;
using System.Threading;

/*
 * 
 * 
 *  DEPRECATED MODULE
 *  
 *  
namespace Dox.Components.IllictServices
{
    public static class DataExtraction
    {
        public static List<string> dobs = new List<string>();
        public static List<string> gender = new List<string>();
        public static List<string> ips = new List<string>();
        public static List<string> notes = new List<string>();
        private struct TargetData
        {
            public static string first_name = null;
            public static string alibi = null;
            public static List<string> usernames = new List<string>();
            public static List<string> DOB = new List<string>();
            public static List<string> passwords_hashed = new List<string>();
        }
        public static void JsonExtraction(string response, string target)
        {
            var details = JObject.Parse(response);
            MatchCollection matchCollection = Regex.Matches(response, "\"id\"");
            int TotalBreaches = matchCollection.Count;
            //
            //
            Colorful.Console.WriteLine("\n\t\t\t\t\t    Total Breaches: {0}\n\n", Color.White, TotalBreaches);
            DisplayBuilder(target);
            Colorful.Console.Write("---* [FOUND]", Color.Green); Colorful.Console.Write(" Breached sites\n");
            for (int i = 0; i < TotalBreaches;)
            {
                var field_data = details["records"][i]["fields"] ?? "";
                foreach (string line in field_data)
                {
                    if (line.Contains("source:"))
                    {
                        string new_string = line.Replace("source: ", "Data Origin: ");
                        Colorful.Console.WriteLine($"--] {i}. {new_string}");
                    }
                }
                i++;
            }
            Colorful.Console.Write("\n\n---* [FOUND]", Color.Green); Colorful.Console.Write(" Passwords\n");
            for (int i = 0; i < TotalBreaches;)
            {
                var passwords = details["records"][i]["passwords"] ?? "";
                foreach (string line in passwords)
                {
                    Colorful.Console.WriteLine($"--] {i}. {line}");
                }
                i++;
            }
            Colorful.Console.Write("\n\n---* [FOUND]", Color.Green); Colorful.Console.Write(" Usernames\n");
            for (int i = 0; i < TotalBreaches;)
            {
                var usernames = details["records"][i]["usernames"] ?? "";
                foreach (string line in usernames)
                {
                    if (!line.Contains("INSERT") && !line.Contains("INTO") && !line.Contains("VALUES") && !line.Contains(".com") && !line.Contains("uc_user") && line.Length < 30)
                    {
                        Colorful.Console.WriteLine($"--] {i}. {line}");
                    }

                }
                i++;
            }
            Colorful.Console.Write("\n\n---* [FOUND]", Color.Green); Colorful.Console.Write(" Personal Information\n", Color.Red);
            for (int i = 0; i < TotalBreaches;)
            {
                var firstNames = details["records"][i]["firstName"] ?? "";
                var personal_fields = details["records"][i]["fields"] ?? "";
                var ip_records = details["records"][i]["links"] ?? "";

                foreach(string line in firstNames)
                {
                    Colorful.Console.WriteLine($"--] Names: {line}");
                }
                foreach(string line in ip_records)
                {
                    ips.Add(line);
                }
                foreach(string line in personal_fields)
                {
                    if (line.Contains("dob"))
                    {
                        dobs.Add(line);
                        //Colorful.Console.WriteLine($"--] {i}. DOB: {line}");
                    }
                    else if (line.Contains("gender"))
                    {
                        gender.Add(line);
                        //Colorful.Console.WriteLine($"--] {i}. Gender: {line}");
                    }
                    else if (line.Contains("links"))
                    {
                        ips.Add(line);
                    }
                    else if (line.Contains("notes"))
                    {
                        notes.Add(line);
                    }
                }
                i++;
            }
            LineHelper();
            Colorful.Console.Write("\n\n---* [FOUND]", Color.Green); Colorful.Console.Write(" Email Addresses\n", Color.Red);
            for (int i = 0; i < TotalBreaches;)
            {
                var field_data = details["records"][i]["emails"] ?? "";
                foreach (string line in field_data)
                {
                   Colorful.Console.WriteLine($"--] {i}. {line}");            
                }
                i++;
            }
            Colorful.Console.Write("\n\n---* [FOUND]", Color.Green); Colorful.Console.Write(" Misc\n", Color.Red);
            for (int i = 0; i < TotalBreaches;)
            {
                var field_data = details["records"][i]["fields"] ?? "";
                foreach (string line in field_data)
                {
                    if (line.Contains("line"))
                    {
                        string new_string = line.Replace("line: ", "");
                        Colorful.Console.WriteLine($"--] {i}. {line}");
                    }
                }
                i++;
            }

        }
        private static void DisplayBuilder(string Target)
        {
            Colorful.Console.Write($"[{DateTime.Now.ToString("h:mm:ss tt")}] ", Color.AliceBlue); Colorful.Console.Write("Service starting...\n", Color.Orange);
            Colorful.Console.Write($"[{DateTime.Now.ToString("h:mm:ss tt")}] ", Color.AliceBlue); Colorful.Console.Write("Checking Illict endpoint... [", Color.Orange);
            if (CheckIllictEndpoint())
            {
                Colorful.Console.Write("Success", Color.Green); Colorful.Console.Write("]\n\n", Color.Orange);
            } else { Colorful.Console.Write("Failed", Color.Red); Colorful.Console.WriteLine("Aborting..."); Colorful.Console.Clear(); Program.Main(); }
            Thread.Sleep(1000);

        }
        private static Boolean CheckIllictEndpoint()
        {
            Boolean Up = false;
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpResponse resp = req.Get("https://search.illicit.services/records?usernames=random&wt=json");
                    if (resp.StatusCode == HttpStatusCode.OK || resp.StatusCode == HttpStatusCode.NotModified)
                    {
                        Up = true;
                    }
                    else
                    {
                        Up = false;
                    }
                }
            } 
            catch (Exception e)
            {
                Colorful.Console.WriteLine(e.Message);
            }
            return Up;
        }
        private static void LineHelper()
        {
            Colorful.Console.Write("--] DOB: ");
            foreach (string line in dobs)
            {
                string new_line = line.Replace("dob: ", "");
                Colorful.Console.Write(new_line + "\t");
            }

            Colorful.Console.Write("\n--] Gender: ");
            foreach (string line in gender)
            {
                string new_line = line.Replace("gender: ", "");
                Colorful.Console.Write(new_line + "\t");
            }
            Colorful.Console.Write("\n--] IPS: ");
            foreach (string line in ips)
            {
                string new_line = line.Replace("links: ", "");
                Colorful.Console.Write(new_line + "\t");
            }
        }
    }
}
*/
