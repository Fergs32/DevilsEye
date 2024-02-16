using Dox.Configuration.Manager;
using Spectre.Console;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Color = System.Drawing.Color;

namespace Dox.Components.Tools.PortScan
{
    public class Portscanner
    {
        public class DNS
        {
            private static string? pHostEntry;
            private static int OpenPorts;
            private static int ClosedPorts;
            private static bool paused = false;
            private static int Threads;
            private static List<int> closedPortsList = new();
            private static List<int> openPortsList = new();
            private static SaveSystem SaveSystem = new();

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
                    Colorful.Console.WriteLine("[+] Checking for open ports {0}", Color.WhiteSmoke, pHostEntry);
                    Colorful.Console.Write("\n[+] Would you like to scan for the default ports | 80, 8080, 53, 25 etc | (Y/N): ");
                    string DefaultOrNot = Colorful.Console.ReadLine();
                    switch (DefaultOrNot)
                    {
                        case "Y":
                            AdvancedPortScan();
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

            private static void AdvancedPortScan()
            {
                new Thread(new ThreadStart(Title)).Start();
                int startPort = 1;
                int endPort = 100;

                int threadCount = Threads;

                // Only logic I could think of, parellism threading doesn't work correctly on this. Still don't know why, so I divided the x amount of ports
                // in endPort & startPort into threading sectors.
                int portsPerThread = (endPort - startPort + 1) / threadCount;


                List<Task> tasks = new();
                Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
                for (int i = 0; i < threadCount; i++)
                {
                    if (!paused)
                    {
                        int threadStartPort = startPort + i * portsPerThread;

                        // This one took some time to figure out , (i == threadCount -1) will check if its the end port. The second condition after ? is known as a ternary condition
                        // which allows me to check if the first thread is the last thread, if so, then set threadEndPort is set to endPort. Otherwise if it's not, then set it to
                        // threadStartPort + portsPerThread - 1
                        int threadEndPort = i == threadCount - 1 ? endPort : threadStartPort + portsPerThread - 1;

                        // This will add tasks into a list, each having StartNew which will initiate a new thread appon adding.
                        tasks.Add(Task.Factory.StartNew(() => ScanPorts(pHostEntry, threadStartPort, threadEndPort)));
                    }   
                }

                Task.WaitAll(tasks.ToArray());
                Console.WriteLine("[+] Portscan complete.", Color.Snow);
                var save = AnsiConsole.Confirm("Would you like to save the open ports to a file?");
                if (save)
                {
                    SaveSystem saveSystem = new();
                    saveSystem.SaveScanResults(CollectionsMarshal.AsSpan(openPortsList), CollectionsMarshal.AsSpan(closedPortsList), pHostEntry);
                }
            }

            static void ScanPorts(string? targetIP, int startPort, int endPort)
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
                        closedPortsList.Add(port);
                        if (Config.section?["PortScanDebug"] == "true")
                        {
                            Console.WriteLine("[+] Port {0} is closed", Color.Red, port);
                        }
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
                while (!paused)
                {
                    try
                    {
                        Console.Clear();
                        AsciiMenu.Menu.GetTitle();
                        Console.Write("\n\n[+]", Color.Red);
                        Console.Write(" Open Ports [{0}]  |  Closed Ports [{1}]\n", OpenPorts, ClosedPorts);
                        Console.Write("   CTRL + C to save progress / exit", OpenPorts, ClosedPorts);
                        foreach (int line in openPortsList)
                        {
                            Console.WriteLine("[+] {0}", Color.Snow, line);
                        }
                        Thread.Sleep(1000);
                    }
                    catch { }
                }
            }

            protected static void myHandler(object? sender, ConsoleCancelEventArgs? args)
            {
                paused = true;
                Console.WriteLine($"\n[Thread] {Thread.CurrentThread.ManagedThreadId} [+] Would you like to save the progress? (y/n)");
                var answer = Console.ReadLine();
                if (answer?.ToUpper() != "N")
                {
                    Console.WriteLine("[+] Saving progress...");
                    SaveSystem.SaveScanResults(CollectionsMarshal.AsSpan(openPortsList), CollectionsMarshal.AsSpan(closedPortsList), pHostEntry);
                }
                args.Cancel = answer != "y";
                paused = false;
            }
        }
    }
}