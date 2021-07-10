using System.Net;
using System.Text;

namespace TimelistBot
{
    static class Web
    {
        public static WebClient webClient = new WebClient();
        
        static Web()
        {
            webClient.Encoding = Encoding.UTF8;

            string name = "user-agent";
            string value = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
            webClient.Headers.Add(name, value);
        }
    }
}
