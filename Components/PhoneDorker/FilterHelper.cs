namespace Dox.Components.PhoneDorker
{
    internal class FilterHelper
    {
        public static List<string>? filteredResults = new ();
        // New 6.0 nullable thing
        public static void FilterResults(List<string>? results)
        {
            try
            {
                if (results is not null)
                {
                    foreach (string result in results)
                    {
                        if (result.Contains("url=https:")) filteredResults?.Add(result);
                    }
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] ", Color.Magenta); Console.Write(" No results to filter!", Color.Red);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Thread.Sleep(1000);
            Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", Color.Magenta); Console.Write("Dumped results to " + Directory.GetCurrentDirectory() + @"\PhoneDorker\filtered_results.txt\n", Color.Green);
            if (filteredResults is not null)
            {
                File.AppendAllLines(Directory.GetCurrentDirectory() + @"\PhoneDorker\filtered_results.txt", filteredResults);
            }
            // Clear the list so there's no duplicates
            filteredResults?.Clear();
        } 
    }
}
