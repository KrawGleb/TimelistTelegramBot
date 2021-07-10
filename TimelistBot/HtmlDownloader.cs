using System;
using NLog;

namespace TimelistBot
{
    static class HtmlDownloader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static string DownloadHtml(string path)
        {
            logger.Trace("Downloading began");
            string html = String.Empty;
            try
            {
                html = Web.webClient.DownloadString(path);
                logger.Trace("Download is succeful");
            }
            catch (Exception exc)
            {
                logger.Error("Download is failed");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }

            return html;
        }
    }
}
