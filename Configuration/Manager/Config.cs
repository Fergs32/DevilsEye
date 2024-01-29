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
        }        
        public static void ConfigurationImpl()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
            IConfigurationSection section = config.GetSection("Settings");
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
                if (section["DatabaseAPIKey"] == "true")
                {
                    // TODO: Implement Database API Key
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
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Proies");
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
            Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", Color.Magenta); Console.Write("Validated output folders\n", Color.DarkMagenta);
        }
    }  
}
