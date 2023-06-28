namespace Dox.Components.TempMail
{
    public class MailAPIs
    {
        public static string GenerateEmail()
        {
            string DOMAIN_BASE_URL = "https://www.1secmail.com/api/v1/?action=genRandomMailbox&count=1"; return DOMAIN_BASE_URL;
        }
    }
}