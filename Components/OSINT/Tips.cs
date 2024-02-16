using Spectre.Console;

namespace Dox.Components.OSINT
{
    internal class Tips
    {
        public static void GetTips()
        {
            var module = AnsiConsole.Prompt(new SelectionPrompt<string>()
              .Title("[darkmagenta]OSINT Tips[/]")
                 .PageSize(10)
                     .MoreChoicesText("[grey](Navigate down to find more modules)[/]")
                        .AddChoices(new[] {
                            "USA DB Records" }));

            switch (module)
            {
                /*
                 * TODO
                 */
            }
        }
    }
}
