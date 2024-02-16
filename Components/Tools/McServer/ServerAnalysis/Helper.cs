using System.Diagnostics;
using System.Text.RegularExpressions;
using Console = System.Console;

namespace Dox.Components.Tools.ServerAnalysis
{
    public class Helper
    {
        private static string? AddressesList;
        private static string? NameServersList;
        private static string? MXRecordList;

        private static string? string_extracted_values;

        public static string ExtractAddresses(string input)
        {
            List<string> addresses = new List<string>().ToList<string>();
            Regex regex = new Regex("\"address\":\"(.*?)\"", RegexOptions.IgnoreCase);
            Match m = regex.Match(input);
            MatchCollection Site_Matches = Regex.Matches(input, "\"address\":\"(.*?)\"");
            while (m.Success)
            {
                for (int i = 0; i < Site_Matches.Count; i++)
                {
                    Group g = m.Groups[1];
                    addresses.Add(g.Value);
                }
                m = m.NextMatch();
            }
            addresses = addresses.Distinct().ToList();
            if (addresses.Count != 0) { AddressesList = addresses.Aggregate((a, x) => a + ", " + x); } else { AddressesList = "No Addresses Found"; }
            return AddressesList;
        }

        public static string ExtractNameServers(string input)
        {
            List<string> name_servers = new List<string>().ToList<string>();
            Regex regex = new Regex("\"nameserver\":\"(.*?)\"", RegexOptions.IgnoreCase);
            Match m = regex.Match(input);
            MatchCollection Site_Matches = Regex.Matches(input, "\"nameserver\":\"(.*?)\"");
            while (m.Success)
            {
                for (int i = 0; i < Site_Matches.Count; i++)
                {
                    Group g = m.Groups[1];
                    name_servers.Add(g.Value);
                }
                m = m.NextMatch();
            }
            name_servers = name_servers.Distinct().ToList();
            if (name_servers.Count != 0) { NameServersList = name_servers.Aggregate((a, x) => a + ", " + x); } else { NameServersList = "No Name Servers"; }
            return NameServersList;
        }

        public static string ExtractMXRecords(string input)
        {
            List<string> mx_records = new List<string>().ToList<string>();
            Regex regex = new Regex("\"exchange\":\"(.*?)\"", RegexOptions.IgnoreCase);
            Match m = regex.Match(input);
            MatchCollection Site_Matches = Regex.Matches(input, "\"exchange\":\"(.*?)\"");
            while (m.Success)
            {
                for (int i = 0; i < Site_Matches.Count; i++)
                {
                    Group g = m.Groups[1];
                    mx_records.Add(g.Value);
                }
                m = m.NextMatch();
            }
            mx_records = mx_records.Distinct().ToList();
            if (mx_records.Count != 0) { MXRecordList = mx_records.Aggregate((a, x) => a + ", " + x); } else { MXRecordList = "No MX Records"; }
            return MXRecordList;
        }

        public static void ExecuteCommand(string command)
        {
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c " + command; // cmd.exe spesific implementation
            p.StartInfo = startInfo;
            p.Start();
        }

        public static string TextFileExtract()
        {
            Thread.Sleep(220); // For some PC's, the file doesn't generate properly or delayed, so I put this here for pre-caution.
            List<string> textfile = File.ReadLines("text.txt").ToList<string>();

            List<string> strings_extracted = new List<string>().ToList<string>();

            foreach (string line in textfile)
            {
                if (line.Contains("\""))
                {
                    string newstring;
                    newstring = ReplaceWhitespace(line, "");
                    strings_extracted.Add(newstring);
                }
            }
            strings_extracted = strings_extracted.Distinct().ToList();
            if (strings_extracted.Count != 0) { string_extracted_values = strings_extracted.Aggregate((a, x) => a + ", " + x); } else { string_extracted_values = "No TXT Records"; }
            return string_extracted_values;
        }

        private static readonly Regex sWhitespace = new Regex(@"\s+");

        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }

        public static void AnalysisWebsites(string q_ip)
        {
            Console.WriteLine("\n[Analysis Links]", Color.LightCoral);
            Console.WriteLine("\n[+] Certificates -> https://crt.sh/?q=" + q_ip, Color.LightCoral);
            Console.WriteLine("[+] Subdomains -> https://securitytrails.com/list/apex_domain/" + q_ip, Color.LightCoral);
        }
    }
}