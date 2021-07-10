using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            bool dateSended = false;
            string curDate = "";

            foreach (var item in timelist)
            {
                if (item.Date != curDate)
                {
                    dateSended = false;
                }
                if (!dateSended)
                {
                    Bot.SendMessage(chatId, $"*{item.Date}*");
                    dateSended = true;
                    curDate = item.Date;
                }
                Bot.SendMessage(chatId, item.ToString(), item.InlineKeyboard);
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
            // var scheduleItem = parser.ParseDocument(html).QuerySelectorAll("div.schedule__item");
            Regex regex = new Regex("\\S.*?\\s");
            MatchCollection collection;
            foreach (var scheduleList in scheduleLists)
            {
                var scheduleItem = scheduleList.QuerySelectorAll("div.schedule__item");
                var dateContent = scheduleList.QuerySelector("h5.h5").TextContent;
                collection = regex.Matches(dateContent);
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
        public InlineKeyboardMarkup InlineKeyboard { get; set; }

        public TimelistItem(string date, string title, List<string> times, string href)
        {
            Date = date;
            Title = title;
            Times = times;
            InlineKeyboard = new InlineKeyboardMarkup(
                new InlineKeyboardButton()
                {
                    Text = "Подробнее",
                    Url = href
                }) ;
        }

        public override string ToString()
        {
            string message = $"*{Title}*\n";

            int count = 0;
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
            return message;
        }
    }
}
