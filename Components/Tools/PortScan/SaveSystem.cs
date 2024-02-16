namespace Dox.Components.Tools.PortScan
{
    internal class SaveSystem : PortScannerInterface
    {

        public void SaveScanResults(Span<int> ports, Span<int> closedports, string? ip)
        {
           string? currentFileDirectory = Directory.GetCurrentDirectory() + "\\Portscan";
           if (!ports.IsEmpty || ports != null)
            {
                string fileName = ip + ".txt";
                if (!Directory.Exists(currentFileDirectory))
                {
                    Directory.CreateDirectory(currentFileDirectory);
                }
                if (!File.Exists(currentFileDirectory))
                {
                    if (!File.Exists(currentFileDirectory + $"\\{fileName}"))
                    {
                        using (StreamWriter sw = File.CreateText(currentFileDirectory + $"\\{fileName}"))
                        {
                            sw.WriteLine("Port Scan Results for: " + ip);
                            sw.WriteLine("Open Ports: ");
                            foreach (int port in ports)
                            {
                                sw.WriteLine(port);
                            }
                            sw.WriteLine("Closed Ports: ");
                            foreach (int port in closedports)
                            {
                                sw.WriteLine(port);
                            }
                            Console.WriteLine("[+] Successfully saved the scan results to: " + currentFileDirectory + $"\\{fileName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[!] File already exists, please delete the file or rename it.");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    if (File.Exists(currentFileDirectory + $"\\{fileName}"))
                    {
                        Console.WriteLine(fileName + " already exists, would you like to overwrite it? (y/n)");  
                        string? response = Console.ReadLine();
                        if (response == "y")
                        {
                            using (StreamWriter sw = File.CreateText(currentFileDirectory + $"\\{fileName}"))
                            {
                                sw.WriteLine("Open Ports: ");
                                foreach (int port in ports)
                                {
                                    sw.WriteLine(port);
                                }
                                sw.WriteLine("Closed Ports: ");
                                foreach (int port in closedports)
                                {
                                    sw.WriteLine(port);
                                }
                                Console.WriteLine("[+] Successfully saved the scan results to: " + currentFileDirectory + $"\\{fileName}");
                            }
                        }
                    }
                }
            }
        }
    }
}
