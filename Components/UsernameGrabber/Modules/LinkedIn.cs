using System;
using System.Collections.Generic;
using System.Text;
using Leaf.xNet;

namespace Dox.Components.UsernameGrabber.Modules
{
    public class LinkedIn
    {
        public static void Get(string Username)
        {
            using (HttpRequest req = new HttpRequest())
            {
                string resp = req.Get("").ToString();

                Console.WriteLine(resp);
            }

        }
    }
}
