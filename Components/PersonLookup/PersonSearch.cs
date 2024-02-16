using Leaf.xNet;
using Spectre.Console;
using Color = System.Drawing.Color;
using Spectre.Console.Json;
using System.Drawing.Text;
using Panel = Spectre.Console.Panel;

namespace Dox.Components.PersonLookup
{
    internal class PersonSearch : PersonInterface
    {
        internal struct Person
        {
            public string? Name { get; set; }
            public string? City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string PostalCode { get; set; }
        }
        public static void GetPerson()
        {
            /*
             *  PLEASE DO NOT USE THIS MODULE, IT IS NOT FINISHED AND WILL NOT WORK CURRENTLY.
             */
            Person person = new Person();   
            try
            {
                AnsiConsole.Markup("\n[bold]{!} Enter the person's name:[/] ");
                person.Name = Console.ReadLine();
                AnsiConsole.Markup("\n[bold]{!} City (Hit Enter if no):[/] ");
                person.City = Console.ReadLine();
                if (string.IsNullOrEmpty(person.Name) || person.Name.Length < 1 && person.City is "")
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Incorrect Input", Color.Magenta);
                    GetPerson();
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Searching for {person.Name} | No optional", Color.Magenta);
                    FetchResultsNC(person.Name, person.City);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot validate input, please ensure it's a name.");
            }
        }
        private static void FetchResultsNC(string? Name, string? city)
        {
            try
            {
                using (HttpRequest client = new HttpRequest())
                {
                    client.UserAgentRandomize();
                    client.AddHeader("x-api-key", "API-KEY-HERE");
                    var request = client.Get($"https://api.trestleiq.com/3.0/person?name={Name}&address.state_code=US");

                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Request was successful", Color.Magenta);
                        var json = new JsonText(request.ToString());
                        AnsiConsole.Write(
                               new Panel(json)
                                   .Header("Phone Lookup Table")
                                   .Collapse()
                                   .RoundedBorder()
                                   .BorderColor(Spectre.Console.Color.CadetBlue));
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while fetching the results.");
            }
        }
    }
}
