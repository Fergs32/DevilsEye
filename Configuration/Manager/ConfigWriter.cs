using Newtonsoft.Json;
using System.Text.Json;

namespace Dox.Configuration.Manager
{
    internal class ConfigWriter
    {
        public static void BuildConfig()
        {
            Thread.Sleep(1000);
            string json = System.Text.Json.JsonSerializer.Serialize(new
            {
                Settings = new
                {
                    DebugMode = Config.ConfigSettings.DebugMode,
                    ProxyDebug = Config.ConfigSettings.ProxyDebug,
                    ProxyDump = Config.ConfigSettings.ProxyDump,
                    DatabaseAPIKey = Config.ConfigSettings.DatabaseAPIKey,
                    IPHistory = Config.ConfigSettings.IPHistory,
                    TrestleAPIKey = Config.ConfigSettings.TrestleAPIKey
                }
            });
            File.WriteAllText(Directory.GetCurrentDirectory() + "/config.json", json);
            Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Configuration settings written to config.json");
        }
    }
}
