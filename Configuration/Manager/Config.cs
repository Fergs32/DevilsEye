using Microsoft.Extensions.Configuration;

namespace Dox.Configuration.Manager
{
    internal class Config
    {
        /*
         * 
         * Implementation for MCDatabase Module, this will also give configuration settings for the program.
         * 
         */
        internal struct ConfigSettings
        {
            public static bool DebugMode;
            public static bool ProxyDebug;
            public static bool ProxyDump;
            public static bool DatabaseAPIKey;
            public static bool IPHistory;
            public static string TrestleAPIKey = "YOUR-API-KEY-HERE"; // Do not change this to your api key, change it in config.json;
        }        

        public static IConfiguration? section { get; set; }
        public static void ConfigurationImpl()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"/config.json"))
            {
                Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Found config.json");
                if (File.ReadAllText(Directory.GetCurrentDirectory() + @"/config.json") == "")
                {
                    Thread.Sleep(2000);
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Empty config.json");
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Writing configuration settings to config.json...");
                    ConfigWriter.BuildConfig();
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Reading configuration settings from config.json...");
                }
            }
            else
            {
                using (var file = File.Create(Directory.GetCurrentDirectory() + "/config.json"))
                {
                    file.Dispose();
                }
                Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Created config.json");
                Thread.Sleep(2000);
                Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] Writing configuration settings to config.json...");
                ConfigWriter.BuildConfig();
            }
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
            section = config.GetSection("Settings");
            try
            {
                if (section["DebugMode"] == "true")
                {
                    Console.WriteLine("[+] Debug Mode Enabled");
                    ConfigSettings.DebugMode = true;
                }
                if (section["ProxyDebug"] == "true")
                {
                    Console.WriteLine("[+] Proxy Debug Enabled");
                    ConfigSettings.ProxyDebug = true;
                }
                if (section["ProxyDump"] == "true")
                {
                    Console.WriteLine("[+] Proxy Dump Enabled");
                    ConfigSettings.ProxyDump = true;
                }
                if (section["IPHistory"] == "true")
                {
                    Console.WriteLine("[+] IP History Enabled");
                    ConfigSettings.IPHistory = true;
                }
                if (section["DatabaseAPIKey"] == "true")
                {
                    // TODO: Implement Database API Key
                }
                if (section["TrestleAPIKey"] != "YOUR-API-KEY-HERE")
                {
                    Console.WriteLine("[+] Trestle API Key Found");
                    ConfigSettings.TrestleAPIKey = section["TrestleAPIKey"] ?? "YOUR-API-KEY-HERE";
                    Console.WriteLine("[+] Trestle API Key: " + ConfigSettings.TrestleAPIKey);
                }
                Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", System.Drawing.Color.Magenta); Console.Write("Registered Interal Configuration Settings\n", System.Drawing.Color.DarkMagenta);
                Thread.Sleep(1000);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void FolderCheck()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/PhoneDorker"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/PhoneDorker");
                Console.WriteLine("[+] Created PhoneDorker Folder");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/TempMail"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/TempMail");
                Console.WriteLine("[+] Created TempMail Folder");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Proxies"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Proxies");
                Console.WriteLine("[+] Created Proxies Folder");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/IPDatabase"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/IPDatabase");
                Console.WriteLine("[+] Created IPDatabase Folder");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Ddos"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Ddos");
                Console.WriteLine("[+] Created Ddos Folder");
            }
            Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", System.Drawing.Color.Magenta); Console.Write("Validated output folders\n", System.Drawing.Color.DarkMagenta);
        }
    }  
}
