using System;
using System.Linq;
using AngleSharp.Html.Parser;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;


namespace TimelistBot
{
    class IcePalaceTimelist : ITimelist
    {
        const string website = "http://www.brest-hockey.by/";
        public void SendTimelist(CallbackQueryEventArgs callback)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
               new InlineKeyboardButton()
               {
                   Text = "Обновить",
                   CallbackData = "Update"
               }
                );

            long chatId = callback.CallbackQuery.Message.Chat.Id;
            string text = GetTimelist();

            Bot.SendMessageAsync(chatId, text, inlineKeyboard);
        }

        public void UpdateTimelist(CallbackQueryEventArgs callback)
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
            long chatId = callback.CallbackQuery.Message.Chat.Id;
            int messageId = callback.CallbackQuery.Message.MessageId;
            
            Bot.EditMessageTextAsync(chatId, messageId, freshTimelist, inlineKeyboard);
            Console.WriteLine("Updated message with id: " + callback.CallbackQuery.Message.MessageId);
        }

        static string GetTimelist()
        {
            string html = HtmlDownloader.DownloadHtml(website);

            HtmlParser parser = new HtmlParser();

            var allTd = parser.ParseDocument(html).QuerySelector("table").QuerySelectorAll("td");
            string timeTable = "";
            int i = 0;
            foreach (var item in allTd)
            {
                string nextLine = "";
                i++;
                if (!(item.TextContent.IndexOf("КатОК") >= 0))
                {
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
                else
                {
                    continue;
                }
            }

            return timeTable;
        }
    }
}
