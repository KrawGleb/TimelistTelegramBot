using System;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TimelistBot
{
    class Program
    {
        static ITimelist targetTimelist;
        static void Main(string[] args)
        {
            string TOKEN = "1302268641:AAF4QtLhE78WpTGSN5BgKU_GRv-U_p2tPaA";
            Clients.botClient = new TelegramBotClient(TOKEN);

            Clients.webClient = new WebClient();
            Clients.webClient.Encoding = System.Text.Encoding.UTF8;
            Clients.webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");

            Clients.botClient.OnMessage += TextMessageHandler;
            Clients.botClient.OnCallbackQuery += CallbackQueryHandler;
            Clients.botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

            Clients.botClient.StartReceiving();
        }

        static async void TextMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                // Очень плохо
                if (e.Message.Text == "/start")
                {
                    await Clients.botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat.Id,
                    text: "Меню",
                    replyMarkup: new InlineKeyboardMarkup(
                        new InlineKeyboardButton[]
                        {
                            new InlineKeyboardButton()
                            {
                                Text = "Ледовый дворец",
                                CallbackData = "IcePalaceTimelist"
                            },

                            new InlineKeyboardButton()
                            {
                                Text = "Театр драмы",
                                CallbackData = "DramaTheatreTimelist"
                            }
                        }
                        )
                    );
                    return;
                }
            }
        }

        static void CallbackQueryHandler(object sender, CallbackQueryEventArgs callback)
        {
            if (callback.CallbackQuery.Data == "IcePalaceTimelist")
            {
                targetTimelist = new IcePalaceTimelist();
                targetTimelist.SendTimelist(callback);
            }
            if (callback.CallbackQuery.Data == "DramaTheatreTimelist")
            {
                targetTimelist = new DramaTheatreTimelist();
                targetTimelist.SendTimelist(callback);
            }
            if (callback.CallbackQuery.Data == "Update")
            {
                targetTimelist?.UpdateTimelist(callback);
            }
        }

    }
}
