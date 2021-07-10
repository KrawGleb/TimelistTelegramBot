using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

using AngleSharp.Html.Parser;

namespace TimelistBot
{
    class CinemaTimelist : ITimelist
    {
        const string website = "https://afisha.relax.by/place/id/109505/";
        public void SendTimelist(CallbackQueryEventArgs callback)
        {
            var timelist = GetTimelist();
            long chatId = callback.CallbackQuery.Message.Chat.Id;
            string curDate = "";
            string chunk = "";
            List<List<InlineKeyboardButton>> linksButtons = new List<List<InlineKeyboardButton>>();

            foreach (var item in timelist)
            {
                if (item.Date != curDate)
                {
                    InlineKeyboardMarkup markup = new InlineKeyboardMarkup(linksButtons);

                    var task = Task.Factory.StartNew(() => Bot.SendMessage(chatId, chunk, markup));
                    task.Wait();
                    
                    chunk = "";
                    linksButtons.Clear();
                    
                    chunk += $"*{item.Date}*\n";
                    curDate = item.Date;
                }
                chunk += item.ToString();
                List<InlineKeyboardButton> buttonRow = new List<InlineKeyboardButton>();
                buttonRow.Add(item.InlineKeyboardButton);
                linksButtons.Add(buttonRow);
            }
        }

        public void UpdateTimelist(CallbackQueryEventArgs callback)
        {
            throw new NotImplementedException();
        }

        private static List<TimelistItem> GetTimelist()
        {
            string html = HtmlDownloader.DownloadHtml(website);
            HtmlParser parser = new HtmlParser();
            List<TimelistItem> timelistItems = new List<TimelistItem>();

            var scheduleLists = parser.ParseDocument(html).QuerySelectorAll("div.schedule__list");
            Regex regex = new Regex("\\S.*?\\s");

            foreach (var scheduleList in scheduleLists)
            {
                var scheduleItem = scheduleList.QuerySelectorAll("div.schedule__item");
                string dateContent = scheduleList.QuerySelector("h5.h5").TextContent;
                MatchCollection collection = regex.Matches(dateContent);
                string date = "";

                if (collection.Count > 0)
                {
                    foreach (var item in collection)
                    {
                        date += item;
                    }
                }

                foreach (var schedule in scheduleItem)
                {
                    var texts = schedule.QuerySelectorAll("a.link");
                    var scheduleTimes = schedule.QuerySelectorAll("a.js-buy-ticket");

                    string title = texts.First().TextContent.Trim();
                    string href = texts.First().GetAttribute("href");

                    List<string> times = new List<string>();
                    foreach (var item in scheduleTimes)
                    {
                        times.Add(item.TextContent);
                    }

                    timelistItems.Add(new TimelistItem(date, title, times, href));
                }
            }

            return timelistItems;
        }
    }

    class TimelistItem
    {
        public string Date { get; set; }
        string Title { get; set; }
        List<string> Times { get; set; }
        public InlineKeyboardButton InlineKeyboardButton { get; set; }

        public TimelistItem(string date, string title, List<string> times, string href)
        {
            Date = date;
            Title = title;
            Times = times;
            InlineKeyboardButton = new InlineKeyboardButton()
            {
                Text = Title,
                Url = href
            };
        }

        public override string ToString()
        {
            string message = $"*{Title}*\n";

            int count = 0;
            if (Times.Count() == 0)
            {
                message += "На сегодня сеансов больше нет";
            }
            foreach (var time in Times)
            {
                if (count >= 3)
                {
                    message += '\n';
                    count = 0;
                }
                message += $"{time} ";
                count++;
            }
            return message + "\n\n";
        }
    }
}
