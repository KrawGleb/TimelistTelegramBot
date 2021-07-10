using Telegram.Bot.Args;

namespace TimelistBot
{
    interface ITimelist
    {
        void SendTimelist(CallbackQueryEventArgs callback);
        void UpdateTimelist(CallbackQueryEventArgs callback);
    }
}
