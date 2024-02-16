namespace Dox.Components.Company
{
    internal class Search
    {
        internal struct UrlParams
        {
            public string? query { get; set; }
            public int companies_per_page { get; set; }

            public int start_index { get; set; }
        }

        public static void GetCompany()
        {
            Console.WriteLine("This module is currently in development, please check back later.");
            UrlParams urlParams = new UrlParams();
            try
            {
                Console.Write("Enter the company's name: ");
                urlParams.query = Console.ReadLine();
                Console.Write("Enter the number of companies per page: ");
                urlParams.companies_per_page = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter the start index: ");
                urlParams.start_index = Convert.ToInt32(Console.ReadLine());
                if (string.IsNullOrEmpty(urlParams.query) || urlParams.query.Length < 1)
                {
                    Console.WriteLine("Incorrect Input");
                    GetCompany();
                }
                else
                {
                    Console.WriteLine($"Searching for {urlParams.query}");
                    FetchResults(urlParams.query, urlParams.companies_per_page, urlParams.start_index);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot validate input, please ensure it's a name.");
            }
        }

        private static void FetchResults(string q, int per_page, int index)
        {
            /*
             * Needs implementation TODO (uk gov database)
             */
        }
    }
}
