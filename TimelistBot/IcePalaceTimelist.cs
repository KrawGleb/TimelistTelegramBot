using System;
using System.Linq;
using System.Collections.Generic;
using AngleSharp.Html.Parser;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace TimelistBot
{
    class IcePalaceTimelist : ITimelist
    {
        const string path = "http://www.brest-hockey.by/";
        public async void SendTimelist(CallbackQueryEventArgs callback)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
               new InlineKeyboardButton()
               {
                   Text = "Обновить",
                   CallbackData = "Update"
               }
                );
            await Clients.botClient.SendTextMessageAsync(
            chatId: callback.CallbackQuery.Message.Chat.Id,
            text: GetTimelist(),
            replyMarkup: inlineKeyboard,
            parseMode: ParseMode.Markdown
            );
        }

        public async void UpdateTimelist(CallbackQueryEventArgs callback)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
               new InlineKeyboardButton()
               {
                   Text = "Обновить",
                   CallbackData = "Update"
               }
                );
            string freshTimelist = GetTimelist();
            freshTimelist += "*Последнее обновление:* " + DateTime.Now.ToString();


            await Clients.botClient.EditMessageTextAsync(
                callback.CallbackQuery.Message.Chat.Id,
                callback.CallbackQuery.Message.MessageId,
                freshTimelist,
                replyMarkup: inlineKeyboard,
                parseMode: ParseMode.Markdown);

            Console.WriteLine("Updated message with id: " + callback.CallbackQuery.Message.MessageId);

        }


        static string GetTimelist()
        {
            string html = HtmlDownloader.DownloadHtml(path);

            List<string> tableTags = new List<string>();

            HtmlParser parser = new HtmlParser();

            var allTd = parser.ParseDocument(html).QuerySelector("table").QuerySelectorAll("td");
            string timeTable = "";
            int i = 0;
            foreach (var item in allTd)
            {
                string nextLine = "";
                i++;
                nextLine = new String(item.TextContent.Where(ch => ch != '\n').ToArray());
                if (i % 2 == 0)
                {
                    nextLine += "\n";

                }
                else
                {
                    nextLine = "*" + nextLine + "*";
                }
                nextLine += "\n";
                timeTable += nextLine;
            }

            return timeTable;
        }
    }
}
