namespace Dox.Components.UsernameGrabber.Modules
{
    public class Tiktok
    {
        /*
         *
         * Tried going throught 3rd party website (musical.ly) as they have a tiktok api integration, I think it more than likely works but
         * I think the parameters may be wrong?
         *
         *
         *
         *
        public static long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }

        public static void Get(string Username)
        {
            long r = LongRandom(100000000000000000, 999999999999999999, new Random());
            try
            {
                using (HttpRequest httpRequest = new HttpRequest())
                {
                    httpRequest.AddHeader("Host", "api2.musical.ly");
                    httpRequest.AddHeader("Connection", "close");
                    httpRequest.AddHeader("sdk-version", "1");
                    httpRequest.UserAgent = "TikTok 13.3.0 rv:133016 (iPhone; iOS 14.6; onlp@numbers=latn) Cronet";
                    httpRequest.SslCertificateValidatorCallback += (RemoteCertificateValidationCallback)((obj, cert, ssl, error) => (cert as X509Certificate2).Verify());
                    string str = "aid=1233&device_id=" + r + "&keyword=" + Username + "source=discover";
                    var response = httpRequest.Get(String.Format("https://api2.musical.ly/aweme/v1/search/sug/aid=1233&device_id={0}&keyword={1}&source={2}", (object)r, (object)Username, (object)"discover")).ToString();
                    //string input = httpRequest.Get(new Uri("https://api2.musical.ly/aweme/v1/search/sug/"), new BytesContent(Encoding.Default.GetBytes(string.Format("aid=1233&device_id={0}&keyword={1}&source={2}", (object)r, (object)Username, (object)"discover")))).ToString();
                    //string response =  httpRequest("https://api2.musical.ly/aweme/v1/search/sug/", str, "application/x-www-form-urlencoded").ToString();
                    Colorful.Console.WriteLine(response.ToString());
                }
            }
            catch(Exception ex)
            {
                Colorful.Console.WriteLine(ex);
            }
        }
        */
    }
}