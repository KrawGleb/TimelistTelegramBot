using System;

namespace TimelistBot
{
    static class HtmlDownloader
    {
        public static string DownloadHtml(string path)
        {
            Console.WriteLine("Downloading began...");
            string html = String.Empty;
            try
            {
                html = Web.webClient.DownloadString(path);
                Console.WriteLine("Download is succeful.");
            }
            catch
            {
                Console.WriteLine("Download is failed.");
            }

            return html;
        }
    }
}
