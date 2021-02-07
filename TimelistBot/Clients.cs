using System.Net;
using Telegram.Bot;

namespace TimelistBot
{
    static class Clients
    {
        public static ITelegramBotClient botClient;
        public static WebClient webClient;
    }
}
