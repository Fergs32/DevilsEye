using System;
using System.Collections.Generic;
using System.Text;

namespace Dox.Components.IllictServices
{
	public class CloudflareDecryption
	{
		/*
		 * 
		 *  This is only implemented if you ever get a encrypted email by cloudflare, sometimes it happens for obviously security reasons, if so, just put 
		 *  the encrypted email into this method and it'll output the original string
		 * 
		 */
		public static string cfDecodeEmail(string encodedString)
		{
			string email = "";
			int r = Convert.ToInt32(encodedString[..2], 16), n, i;
			for (n = 2; encodedString.Length - n > 0; n += 2)
			{
				i = Convert.ToInt32(encodedString.Substring(n, 2), 16) ^ r;
				char character = (char)i;
				email += Convert.ToString(character);
			}

			return email;
		}
	}
}
