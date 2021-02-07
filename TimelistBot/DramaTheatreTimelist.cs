using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AngleSharp.Html.Parser;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TimelistBot
{
    class DramaTheatreTimelist : ITimelist
    {
        const string imgPath = "https://bresttheatre.info/assets/teatr/img/%D1%80%D0%B5%D0%BF%D0%B5%D1%80%D1%82%D1%83%D0%B0%D1%80%D0%BD%D0%B0%D1%8F%20%D0%B0%D1%84%D0%B8%D1%88%D0%B0/%D1%80%D0%B5%D0%BF%D0%B5%D1%80%D1%82%D1%83%D0%B0%D1%80%20%D0%A4%D0%95%D0%92%D0%A0%D0%90%D0%9B%D0%AC%202021.jpg";
        const string savePath = @"D:\Lessons\А\TimelistBot\TimelistBot\ImgTimelists\DramaTheatre\1.jpg";
        public async void SendTimelist(CallbackQueryEventArgs callback)
        {
            //GetTimelist();
            var inlineKeyboard = new InlineKeyboardMarkup(
                new InlineKeyboardButton()
                {
                    Text = "Обновить",
                    CallbackData = "Update"
                }
                 );

            await Clients.botClient.SendPhotoAsync(
                chatId: callback.CallbackQuery.Message.Chat.Id,
                photo: new InputOnlineFile(new FileStream(savePath, FileMode.Open)),
                caption: "Актуально на: " + DateTime.Now.Day);
        }

        public void UpdateTimelist(CallbackQueryEventArgs callback)
        {
            throw new NotImplementedException();
        }

        static void GetTimelist()
        {
            Console.WriteLine("Downloading began... ");
            try
            {
                Clients.webClient.DownloadFile(imgPath, savePath);
                Console.WriteLine("Download is succeful.");
            }
            catch
            {
                Console.WriteLine("Download is failed.");
            }
        }
    }
}
