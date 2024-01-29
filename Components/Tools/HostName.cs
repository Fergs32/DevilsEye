using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Dox.Components.Tools
{
    public class HostName
    {
        public class DNS
        {
            private static string? pHostEntry;
            private static IPAddress? address;
            private static int OpenPorts;
            private static int ClosedPorts;
            private static int Threads;

            private static List<int> openPortsList = new List<int>();

            public static void GetDNS()
            {
                try
                {
                    Colorful.Console.Clear();
                    AsciiMenu.Menu.GetTitle();
                    Colorful.Console.Write("[+] Hostname: ");
                    pHostEntry = Colorful.Console.ReadLine();
                }
                catch (Exception ex) { Colorful.Console.WriteLine("[Error] " + ex, Color.Red); }
                try
                {
                    Colorful.Console.Write("[+] Threads: ");
                    Threads = int.Parse(Colorful.Console.ReadLine());
                }
                catch (Exception ex) { Colorful.Console.WriteLine("[Error] " + ex, Color.Red); }
                try
                {
                    if (pHostEntry?.Any() == null)
                    {
                        Console.WriteLine("Invalid String");
                    }
                    Colorful.Console.WriteLine("[+] Checking for open ports {0}", Color.WhiteSmoke, address);
                    Colorful.Console.Write("\n[+] Would you like to scan for the default ports | 80, 8080, 53, 25 etc | (Y/N): ");
                    string DefaultOrNot = Colorful.Console.ReadLine();
                    switch (DefaultOrNot)
                    {
                        case "Y":
                            AdvancedPortScan(address);
                            break;

                        default:
                            Colorful.Console.WriteLine("[Error] The input you provided entered was invalid.");
                            break;
                    }
                    AsciiMenu.Menu.ReturnMenu();
                }
                catch (SocketException e) { Colorful.Console.WriteLine("[Socket Exception] " + e, Color.Red); }
                catch (ArgumentNullException e) { Colorful.Console.WriteLine("[Null Exception] " + e, Color.Red); }
                catch (Exception e) { Colorful.Console.WriteLine("[Exception] " + e); }
            }

            private static void AdvancedPortScan(IPAddress IP)
            {
                new Thread(new ThreadStart(Title)).Start();
                int startPort = 1;
                int endPort = 10000;

                int threadCount = Threads;

                // Only logic I could think of, parellism threading doesn't work correctly on this. Still don't know why, so I divided the x amount of ports
                // in endPort & startPort into threading sectors.
                int portsPerThread = (endPort - startPort + 1) / threadCount;

                List<Task> tasks = new();

                for (int i = 0; i < threadCount; i++)
                {
                    // This will get the first non-taken port
                    int threadStartPort = startPort + i * portsPerThread;

                    // This one took some time to figure out , (i == threadCount -1) will check if its the end port. The second condition after ? is known as a ternary condition
                    // which allows me to check if the first thread is the last thread, if so, then set threadEndPort is set to endPort. Otherwise if it's not, then set it to
                    // threadStartPort + portsPerThread - 1
                    int threadEndPort = (i == threadCount - 1) ? endPort : threadStartPort + portsPerThread - 1;

                    // This will add tasks into a list, each having StartNew which will initiate a new thread appon adding.
                    tasks.Add(Task.Factory.StartNew(() => ScanPorts(pHostEntry, threadStartPort, threadEndPort)));
                }

                Task.WaitAll(tasks.ToArray());

                Console.WriteLine("[+] Portscan complete.", Color.Snow);
            }

            static void ScanPorts(string targetIP, int startPort, int endPort)
            {
                for (int port = startPort; port <= endPort; port++)
                {
                    if (IsPortOpen(targetIP, port))
                    {
                        OpenPorts++;
                        openPortsList.Add(port);
                    }
                    else
                    {
                        ClosedPorts++;
                    }
                }
            }

            static bool IsPortOpen(string targetIP, int port)
            {
                try
                {
                    using (TcpClient tcpClient = new())
                    {
                        tcpClient.Connect(targetIP, port);
                        return true;
                    }
                }
                catch (SocketException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private static void Title()
            {
                while (true)
                {
                    try
                    {
                        Console.Clear();
                        AsciiMenu.Menu.GetTitle();
                        Console.Write("\n\n[+]", Color.Red);
                        Console.Write(" Open Ports [{0}]  |  Closed Ports [{1}]", OpenPorts, ClosedPorts);
                        foreach(int line in openPortsList)
                        {
                            Console.WriteLine("[+] {0}", Color.Snow, line);
                        }
                        Thread.Sleep(2000);
                    }
                    catch { }
                }
            }
        }
    }
}