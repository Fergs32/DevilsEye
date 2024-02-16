using Dox.Configuration.Manager;
using Leaf.xNet;
using Spectre.Console;
using Spectre.Console.Json;
using System.Text.Json;
using Color = System.Drawing.Color;

namespace Dox.Components.PhoneDorker.CNAM
{
    internal class Lookup
    {
        public static void GetNumber()
        {
            try
            {
                AnsiConsole.Markup("\n[bold]Enter the phone number (US ONLY):[/] ");
                string? number = Console.ReadLine();
                if (string.IsNullOrEmpty(number) || number.Length < 1)
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Incorrect Input", Color.Magenta);
                    GetNumber();
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Requesting info on {number} with CNAM API", Color.Magenta);
                    cnamAPI(number);
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Value cannot be null. (Parameter 'value')"))
                {
                    Console.WriteLine("Invalid API Key or missing API Key in x-api-key header. Refer to line 39 (Reverse.cs || config.json)", Color.Magenta);
                }
            }
        }

        private static void cnamAPI(string Number)
        {
            using (HttpRequest client = new HttpRequest())
            {
                client.UserAgentRandomize();
                client.AddHeader("x-api-key", Config.ConfigSettings.TrestleAPIKey);
                var request = client.Get($"https://api.trestleiq.com/3.1/cnam?phone={Number}&phone.country_hint=US");
                if (request.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Request was successful", Color.Magenta);
                    var json = new JsonText(request.ToString());
                    AnsiConsole.Write(
                              new Spectre.Console.Panel(json)
                              .Header($"CNAM Report for {Number}")
                               .Collapse());
                    Thread.Sleep(1000);
                    var save = AnsiConsole.Confirm(Number + " - Save results to file?", false);
                    if (save)
                    {
                        SaveResults(Number, request.ToString());
                        Thread.Sleep(2000);
                        AsciiMenu.Menu.ReturnMenu();
                    }
                    else
                    {
                        Console.Clear();
                        AsciiMenu.Menu.ReturnMenu();
                    }
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Missing API Key or invalid authorization.", Color.Magenta);
                }
            }
        }

        private static void SaveResults(string Number, string json)
        {
            string path = Directory.GetCurrentDirectory() + "\\CNAM";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllLines(path + $"\\{Number}.json", new string[] { PrettyJson(json) });
            Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Results saved to {path}\\{Number}.json", Color.Magenta);
        }

        private static string PrettyJson(string data)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(data);

            return JsonSerializer.Serialize(jsonElement, options);
        }
    }
}
