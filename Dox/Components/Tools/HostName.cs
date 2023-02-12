using System;
using System.Diagnostics;
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
            private static string pHostEntry;
            private static IPAddress[] address;
            private static int OpenPorts;
            private static int ClosedPorts;
            private static int Checked;

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
                    IPHostEntry host_Entry = Dns.GetHostByName(pHostEntry);
                    address = host_Entry.AddressList;
                    Colorful.Console.Write("[+] IPs Found for [{0}]: ", Color.WhiteSmoke, host_Entry.HostName);
                    for (int index = 0; index < address.Length; index++)
                    {
                        Colorful.Console.Write(address[index] + ", ");
                    }
                    Colorful.Console.WriteLine("");
                    GetAliases();
                    Colorful.Console.WriteLine("[+] Checking for open ports {0}", Color.WhiteSmoke, address.First());
                    Colorful.Console.Write("\n[+] Would you like to scan for the default ports | 80, 8080, 53, 25 etc | (Y/N): ");
                    string DefaultOrNot = Colorful.Console.ReadLine();
                    switch (DefaultOrNot)
                    {
                        case "Y":
                            SetDefaultPortTitle();
                            CheckDefaultPorts(address.First());
                            break;

                        case "N":
                            SetConsoleTitle();
                            Portscan(address.First());
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

            private static void GetAliases()
            {
                for (int i = 0; i < address.Length; i++)
                {
                    try
                    {
                        IPHostEntry _pHostEntry = Dns.GetHostEntry(address[i]);
                        string[] alias = _pHostEntry.Aliases;
                        //
                        if (alias.Length > 0)
                        {
                            Colorful.Console.WriteLine("[+] Aliases found for {0}: ", Color.WhiteSmoke, address[i]);
                            foreach (string line in alias)
                            {
                                Colorful.Console.WriteLine("[+] {0}", Color.WhiteSmoke, line);
                            }
                        }
                        else
                        {
                            Colorful.Console.WriteLine("[+] No aliases found for {0}: ", Color.WhiteSmoke, address[i]);
                        }
                    }
                    catch (SocketException e) { Colorful.Console.WriteLine("[Socket Exception] " + e, Color.Red); }
                    catch (ArgumentNullException e) { Colorful.Console.WriteLine("[Null Exception] " + e, Color.Red); }
                    catch (Exception e) { Colorful.Console.WriteLine("[Exception] " + e); }
                }
            }

            private static void Portscan(IPAddress IP)
            {
                for (int i = 0; i < 65535; i++)
                {
                    Colorful.Console.WriteLine("[+] Scanning port {0} on {1}", Color.WhiteSmoke, i, IP);
                    string IPAddress_ = IP.ToString();
                    TcpClient TcpScan = new TcpClient();
                    try
                    {
                        TcpScan.Connect(IPAddress_, i);
                        Colorful.Console.WriteLine("[+] Port {0} is open on {1}", Color.WhiteSmoke, i, IP);
                        ++OpenPorts;
                        ++Checked;
                    }
                    catch (Exception)
                    {
                        ++ClosedPorts;
                        ++Checked;
                    }
                }
            }

            private static void CheckDefaultPorts(IPAddress IP)
            {
                Stopwatch sw = Stopwatch.StartNew();
                foreach (int Port in Ports.CheckPorts)
                {
                    string IPAddress_ = IP.ToString();
                    TcpClient TcpScan = new TcpClient();
                    try
                    {
                        TcpScan.Connect(IPAddress_, Port);
                        Colorful.Console.WriteLine("[+] Port {0} is open on {1}", Color.WhiteSmoke, Port, IP);
                        ++OpenPorts;
                        ++Checked;
                    }
                    catch (Exception)
                    {
                        Colorful.Console.WriteLine("[+] Port {0} is closed on {1}", Color.Red, Port, IP);
                        ++ClosedPorts;
                        ++Checked;
                    }
                }
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Colorful.Console.WriteLine("[{0}] Port scan completed, found {1} open ports on {2}", Color.WhiteSmoke, elapsedTime, OpenPorts, IP);
            }

            public static void SetConsoleTitle()
            {
                Task.Factory.StartNew(delegate ()
                {
                    while (Checked != 65535)
                    {
                        System.Console.Title = string.Format("Scanning for ports... | Checked: {0}/65535 - Open Ports: {1} - Closed Ports: {2}", (object)Checked, (object)OpenPorts, (object)ClosedPorts);
                        Thread.Sleep(30);
                    }
                });
            }

            public static void SetDefaultPortTitle()
            {
                Task.Factory.StartNew(delegate ()
                {
                    while (Checked < Ports.CheckPorts.Count())
                    {
                        System.Console.Title = string.Format("Scanning for ports... | Checked: {0}/21 - Open Ports: {1} - Closed Ports: {2}", (object)Checked, (object)OpenPorts, (object)ClosedPorts);
                        Thread.Sleep(30);
                    }
                });
            }
        }

        public class Ports
        {
            public static readonly int[] CheckPorts = new int[]
            {
                80,
                8080,
                21,
                22,
                23,
                25,
                53,
                110,
                115,
                135,
                139,
                143,
                194,
                443,
                445,
                1433,
                3306,
                3389,
                5632,
                5900,
                6112,
            };
        }
    }
}