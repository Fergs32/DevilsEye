namespace Dox.Components.Dorking.Filter
{
    public class UrlFilter
    {
        public static List<string> PotentialIrl = new();
        public static List<string> PotentialCountry = new();
        public static List<string> PotentialConnections = new();
        public static List<string> PotentialAccounts = new();
        public static List<string> PotentialInformation = new();

        public static void Filter(List<string> urls)
        {
            List<string> urls2Process = urls.ToList();

            foreach (string line in urls2Process)
            {
                if (line.Contains("https://www.192.com/atoz/people/outcodes/")) { PotentialIrl.Add(line); }
                else if (line.Contains("https://www.192.com/atoz/people")) { PotentialIrl.Add(line); }
                else if (line.Contains("linkedin.com")) { PotentialAccounts.Add(line); PotentialConnections.Add(line); }
                else if (line.Contains("www.instagram.com")) { PotentialAccounts.Add(line); }
                else if (line.Contains("https://github.com")) { PotentialAccounts.Add(line); }
                else if (line.Contains("twitter.com")) { PotentialAccounts.Add(line); }
                else if (line.Contains("www.ancestry")) { PotentialIrl.Add(line); }
                else if (line.Contains("facebook.com")) { PotentialConnections.Add(line); }
                else if (line.Contains("www.ukphonebook.com")) { PotentialIrl.Add(line); PotentialCountry.Add(line); }
                else
                {
                    PotentialInformation.Add(line);
                }
                Thread.Sleep(150);
            }
        }
    }
}