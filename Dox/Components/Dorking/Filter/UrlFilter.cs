using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Dox.Components.Dorking.Filter
{
    public class UrlFilter
    {
        public static List<string> Potential_IRL = new List<string>();
        public static List<string> Potential_Country = new List<string>();
        public static List<string> Potential_Connections = new List<string>();
        public static List<string> Potential_Accounts = new List<string>();
        public static List<string> Potential_Information = new List<string>();

        public static void Filter(List<string> urls)
        {
            List<string> urls2process = urls.ToList<string>();
            
            foreach(string line in urls2process)
            {
                if (line.Contains("https://www.192.com/atoz/people/outcodes/")) { Potential_IRL.Add(line); }
                else if (line.Contains("https://www.192.com/atoz/people")) { Potential_IRL.Add(line); }
                else if (line.Contains("linkedin.com")) { Potential_Accounts.Add(line); Potential_Connections.Add(line); }
                else if (line.Contains("www.instagram.com")) { Potential_Accounts.Add(line); }
                else if (line.Contains("https://github.com")) { Potential_Accounts.Add(line); }
                else if (line.Contains("twitter.com")) { Potential_Accounts.Add(line); }
                else if (line.Contains("www.ancestry")) { Potential_IRL.Add(line); }
                else if (line.Contains("facebook.com")) { Potential_Connections.Add(line); }
                else if (line.Contains("www.ukphonebook.com")) { Potential_IRL.Add(line); Potential_Country.Add(line); }
                else
                {
                    Potential_Information.Add(line);
                }
                Thread.Sleep(150);
            }
        }
    }
}
