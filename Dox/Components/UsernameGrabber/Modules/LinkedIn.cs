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
            //https://www.linkedin.com/search/results/people/?firstName=Ben&lastName=Ferguson&origin=SEO_PSERP&sid=P_r

            using (HttpRequest req = new HttpRequest())
            {
                string resp = req.Get("https://fr.linkedin.com/pub/dir/ben/ferguson").ToString();

                Console.WriteLine(resp);
            }

        }
    }
}
