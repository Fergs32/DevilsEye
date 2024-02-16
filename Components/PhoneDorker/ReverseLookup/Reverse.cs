using Dox.Configuration.Manager;
using Leaf.xNet;
using Spectre.Console;
using Spectre.Console.Json;
using System.Text.Json;
using Color = System.Drawing.Color;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Panel = Spectre.Console.Panel;
namespace Dox.Components.PhoneDorker.ReverseLookup
{
    internal class Reverse : ReverseInterfance
    {
        private static string CurrentDirectory = Directory.GetCurrentDirectory() + "\\ReversePhone";
        public static void GetNumber()
        {
            Console.Clear();
            AsciiMenu.Menu.ReversePhone();
            try
            {
                GetHistory();
                AnsiConsole.Markup("\n[bold]{!} Enter the phone number (USA ONLY):[/] ");
                string? number = Console.ReadLine();
                if (string.IsNullOrEmpty(number) || number.Length < 1)
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Incorrect Input", Color.Magenta);
                    GetNumber();
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Searching for {number}", Color.Magenta);
                    APICall(number);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot validate input, please ensure it's a number.");
            }
        }
        private static void APICall(string Number)
        {
            try
            {
                using (HttpRequest client = new HttpRequest())
                {
                    /*
                     * 
                     * This is the API call to TrestleIQ's API, it will return the results of the phone number.
                     *  https://trestleiq.com/ - Sign up for free trial to get the API Key, less than 1 minute.
                     */
                    client.UserAgentRandomize();
                    client.AddHeader("x-api-key", Config.ConfigSettings.TrestleAPIKey);
                    var request = client.Get($"https://api.trestleiq.com/3.1/phone?phone={Number}");

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
                        SaveResults(request.ToString(), Number);
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Missing API Key in x-api-key header. Refer to line 39 (Reverse.cs)", Color.Magenta);
                    }
                    else if (request.StatusCode == HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Incorrect parameter information, such as non-existent area code on a phone number.", Color.Magenta);
                    }
                    else if (request.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Too many requests, please wait a few minutes before trying again.", Color.Magenta);
                    }
                    else
                    {
                        Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Request failed with status code: {request.StatusCode}", Color.Magenta);
                    }

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
        private static void SaveResults(string request, string number)
        {
            try
            {
                var confirm = AnsiConsole.Confirm("Would you like to save the results to a file?", false);
                if (confirm)
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Saving results to file...", Color.Magenta);
                    File.WriteAllText(CurrentDirectory + $"\\{number}.json", PrettyJson(request));
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Results saved to {CurrentDirectory}\\{number}.json", Color.Magenta);
                    AsciiMenu.Menu.ReturnMenu();
                }
                else
                {
                    Console.Clear();
                    AsciiMenu.Menu.ReturnMenu();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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

        private static void GetHistory()
        {
            var confirm = AnsiConsole.Confirm("{!} Would you like to view the history of phone numbers searched?: ");
            if (confirm)
            {
                if (Directory.Exists(CurrentDirectory))
                {
                    DirectoryInfo di = new DirectoryInfo(CurrentDirectory);
                    FileInfo[] files = di.GetFiles("*.json");
                    if (files.Length > 0)
                    {
                        foreach (FileInfo file in files)
                        {
                            Console.WriteLine("[+] -> " + file.Name);
                        }
                        Console.Write($"\n[{DateTime.Now:h:mm:ss tt}] Please choose a file to view the contents of (inc. extension): ");
                        string? fileTarget = Console.ReadLine();
                        if (File.Exists(CurrentDirectory + "\\" + fileTarget))
                        {
                            string contents = File.ReadAllText(CurrentDirectory + "\\" + fileTarget);
                            var json = new JsonText(contents);
                            Console.WriteLine("");
                            AnsiConsole.Write(
                                   new Panel(json)
                                       .Header($"History Viewer")
                                       .Collapse()
                                       .RoundedBorder()
                                       .BorderColor(Spectre.Console.Color.CadetBlue));
                            AsciiMenu.Menu.ReturnMenu();
                        }
                        else
                        {
                            Console.WriteLine("Error reading file or file cannot be found.");
                            AsciiMenu.Menu.ReturnMenu();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No files found in the directory.");
                        AsciiMenu.Menu.ReturnMenu();
                    }
                }
                else
                {
                    Console.WriteLine("No Directory found");
                    AsciiMenu.Menu.ReturnMenu();
                }
            }
        }
    }
}
