using Dox.AsciiMenu;
using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Dox.Components.TempMail
{
    public class Mail
    {
        private static string Email_Full = "";
        private static string Username = "";
        private static string Email_id = "";
        private static int Received_mails;
        private static int Errors;
        private static int ThreadsInUse;
        private static List<string> inbox_messages = new List<string>().ToList();

        public static void CreateEmail()
        {
            Console.Clear();
            Menu.PooheadMenu();
            Thread.Sleep(500);
            Colorful.Console.WriteLine("Please wait while setting up temp mail virtual environment...");
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            new Thread(new ThreadStart(Title)).Start();
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    string resp = req.Get(MailAPIs.GenerateEmail()).ToString();
                    Email_Full = Regex.Match(resp, "\"(.*?)\"").Groups[1].Value;
                    Email_id = Email_Full.Substring(Email_Full.IndexOf('@') + 1);
                    int index = Email_Full.LastIndexOf("@");
                    Username = Email_Full.Substring(0, index);
                    req.Dispose();

                    new Thread(new ThreadStart(GetInbox)).Start();
                    Thread.Sleep(2000);
                    Table();
                }
            }
            catch (Exception)
            {
                Errors++;
            }
        }

        private static void Table()
        {
            while (true)
            {
                Console.Clear();
                Menu.PooheadMenu();
                Colorful.Console.Write("\t\t\t[Your Email]: ", Color.DarkMagenta); Colorful.Console.Write(Email_Full + "\n\n", Color.White);
                Colorful.Console.Write("\t\t\t     [ Inbox | Received: ", Color.DarkMagenta); Colorful.Console.Write(Received_mails, Color.White); Colorful.Console.Write(" ]\n\n", Color.DarkMagenta);
                foreach (string message in inbox_messages)
                {
                    Colorful.Console.WriteLine(message);
                }
                Thread.Sleep(6000);
            }
        }

        private static void GetInbox()
        {
            List<string> Dupes = new List<string>().ToList();
            string id;
            string sender;
            string subject;

            while (true)
            {
                try
                {
                    using (HttpRequest inbox = new HttpRequest())
                    {
                        string message = inbox.Get("https://www.1secmail.com/api/v1/?action=getMessages&login=" + Username + "&domain=" + Email_id).ToString();
                        id = Regex.Match(message, "\"id\":(.*?)\"").Groups[1].Value.Replace(",", "");
                        sender = Regex.Match(message, "\"from\":\"(.*?)\",").Groups[1].Value;
                        subject = Regex.Match(message, "\"subject\":\"(.*?)\",").Groups[1].Value;

                        string preformatted = string.Format("Sender: {0} | Subject: {1} | ID: {2} | Dumped Response: {3}", sender, subject, id, Directory.GetCurrentDirectory() + "\\TempMail");

                        if (inbox_messages.Contains(preformatted) || message == "[]")
                        {
                            Dupes.Add(message);
                            Dupes.Clear();
                        }
                        else
                        {
                            Received_mails++;
                            inbox_messages.Add(preformatted);
                            GetInnerMessage(id);
                        }
                        Thread.Sleep(6000);
                    }
                }
                catch (Exception) { Errors++; }
            }
        }

        private static void GetInnerMessage(string ID)
        {
            if (File.Exists("TempMail/" + ID + ".txt"))
            {
                File.Delete("TempMail/" + ID + ".txt");
                Thread.Sleep(1000);
            }
            try
            {
                using (HttpRequest httpRequest = new HttpRequest())
                {
                    string inner_message = httpRequest.Get("https://www.1secmail.com/api/v1/?action=readMessage&login=" + Username + "&domain=" + Email_id + "&id=" + ID).ToString();
                    File.AppendAllText("TempMail/" + ID + ".txt", inner_message);
                }
            }
            catch (Exception) { Errors++; }
        }

        private static void Title()
        {
            while (true)
            {
                try
                {
                    ThreadsInUse = Process.GetCurrentProcess().Threads.Count;
                    Console.Title = string.Format("Email: {0}  | Received Mails: {1}  | Potential Errors: {2} | Threads In Use: {3}", Email_Full, Received_mails, Errors, ThreadsInUse);
                    Thread.Sleep(2000);
                }
                catch { }
            }
        }
    }
}