using System;
using Dox.AsciiMenu;
using System.Drawing;
using Leaf.xNet;
using System.Linq;
using System.IO;
using Spectre.Console;

namespace Dox.Components.IllictServices
{
    public class Search
    {
        private static string SelectedInput;
        private static string SelectedURL = null;
        private static string Target = null;
        public static void Start()
        {
            Console.Clear();
            Menu.Illict();
            SetupSpectreInterface();
            IllictEndpointRequest(GetTargetAndURL());
            Menu.ReturnMenu();

        }
        private static void SetupSpectreInterface()
        {
            SelectedInput = GetSelectedInput();
            switch(SelectedInput)
            {
                case "Email Search":
                    SelectedURL = "https://search.illicit.services/records?emails=";
                    break;
                case "Username":
                    SelectedURL = "https://search.illicit.services/records?usernames=";
                    break;
                case "First name":
                    SelectedURL = "https://search.illicit.services/records?firstName=";
                    break;
                case "Last name":
                    SelectedURL = "https://search.illicit.services/records?lastName=";
                    break;
                case "Password":
                    SelectedURL = "https://search.illicit.services/records?passwords=";
                    break;
                case "Phone":
                    SelectedURL = "https://search.illicit.services/records?phoneNumbers=";
                    break;
                default:
                    Colorful.Console.WriteLine("The option you selected isnt there idiot, choose another one", System.Drawing.Color.Red);
                    Console.Clear();
                    Start();
                    break;
            }
        }
        private static string GetTargetAndURL()
        {
            Colorful.Console.WriteLine("\n");
            Target = AnsiConsole.Ask<string>("Please enter the [red]TARGET:[/]");
            return SelectedURL += Target + "&wt=json";
        }
        private static string GetSelectedInput()
        {
            var SelectMode = "";
            try
            {
                var UserInput = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("[springgreen1]Choose your[/] [white]option[/] [springgreen1]option below[/]").PageSize(5).AddChoices(new[] {
                "Email Search", "Username", "First name",
                "Last name", "Password", "Phone",}));

                SelectMode = UserInput;
            }
            catch (Exception ex)
            {
                Colorful.Console.WriteLine("[!] " + ex);
            }
            return SelectMode;
        }
        private static void IllictEndpointRequest(string URL)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36";
                    req.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                    req.KeepAliveTimeout = 20000;
                    req.IgnoreProtocolErrors = true;
                    string request = req.Get(URL).ToString();
                    DataExtraction.JsonExtraction(request, Target);
                }
            }
            catch (HttpException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
