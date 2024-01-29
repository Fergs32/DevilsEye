namespace Dox.Components.PhoneDorker
{
    internal class FilterHelper
    {
        public static List<string> filteredResults = new List<string>();
        // New 6.0 nullable thing
        public static void FilterResults(List<string> results)
        {
            try
            {
                foreach (string result in results)
                {
                    if (result.Contains("url=https:")) filteredResults.Add(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Thread.Sleep(1000);
            File.AppendAllLines(Directory.GetCurrentDirectory() + @"\PhoneDorker\filtered_results.txt", filteredResults);   
        } 
    }
}
