using System;
using System.IO;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using ImageResizer;
using Telegram.Bot.Types;

namespace TimelistBot
{
    class DramaTheatreTimelist : ITimelist
    {
        const string imgPath = "https://bresttheatre.info/assets/teatr/img/%D1%80%D0%B5%D0%BF%D0%B5%D1%80%D1%82%D1%83%D0%B0%D1%80%D0%BD%D0%B0%D1%8F%20%D0%B0%D1%84%D0%B8%D1%88%D0%B0/%D1%80%D0%B5%D0%BF%D0%B5%D1%80%D1%82%D1%83%D0%B0%D1%80%20%D0%A4%D0%95%D0%92%D0%A0%D0%90%D0%9B%D0%AC%202021.jpg";
        static string savePath = Environment.CurrentDirectory + "\\img\\DramaTheatreTimelist\\timelist.jpg";
        static string resizerPath = Environment.CurrentDirectory + "\\img\\DramaTheatreTimelist\\compressedTimelist.jpg";
        public async void SendTimelist(CallbackQueryEventArgs callback)
        {
            try
            {
                await Clients.botClient.SendTextMessageAsync(
                chatId: callback.CallbackQuery.Message.Chat.Id,
                text: "Это может занять некоторое время..."
                );
                GetTimelist();
                Resize();
                var inlineKeyboard = new InlineKeyboardMarkup(
                    new InlineKeyboardButton()
                    {
                        Text = "Обновить",
                        CallbackData = "Update"
                    }
                     );

                await Clients.botClient.SendPhotoAsync(
                    chatId: callback.CallbackQuery.Message.Chat.Id,
                    photo: new InputOnlineFile(new FileStream(resizerPath, FileMode.Open)),
                    caption: "Актуально на: " + DateTime.Now,
                    replyMarkup: inlineKeyboard);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Somthing is wrong...");
                Console.WriteLine(exc.Message);
                SendErrorMessage(callback);
            }
            
        }

        public async void UpdateTimelist(CallbackQueryEventArgs callback)
        {
            try
            {
                Console.WriteLine("Start updating...");
                GetTimelist();
                Resize();
                var inlineKeyboard = new InlineKeyboardMarkup(
                    new InlineKeyboardButton()
                    {
                        Text = "Обновить",
                        CallbackData = "Update"
                    }
                     );



                await Clients.botClient.EditMessageMediaAsync(
                    callback.CallbackQuery.Message.Chat.Id,
                    callback.CallbackQuery.Message.MessageId,
                    media: new InputMediaPhoto(new InputMedia(new FileStream(resizerPath, FileMode.Open), "compressedTimelist"))
                    );

                await Clients.botClient.EditMessageCaptionAsync(
                    callback.CallbackQuery.Message.Chat.Id,
                    callback.CallbackQuery.Message.MessageId,
                    caption: "Актуально на: " + DateTime.Now,
                    replyMarkup: inlineKeyboard);

                Console.WriteLine("Updating is done.");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Updating error.");
                throw new Exception(exc.Message);
            }

        }

        static void GetTimelist()
        {
            Console.WriteLine("Downloading a picture began... ");
            try
            {
                Clients.webClient.DownloadFile(imgPath, savePath);
                Console.WriteLine("Download is succeful.");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Download error.");
                throw new Exception(exc.Message);
            }
        }


        static void Resize()
        {
            try
            {
                Console.WriteLine("Resizing began...");
                using (FileStream startStream = new FileStream(savePath, FileMode.Open), destStream = new FileStream(resizerPath, FileMode.OpenOrCreate))
                {
                    ImageBuilder.Current.Build(
                        new ImageJob(
                            startStream,
                            destStream,
                            new Instructions("maxwidth=1000&maxheight=1000"),
                            false,
                            false
                            )
                        );
                }
                Console.WriteLine("Resize is succeful.");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Resize error.");
                throw new Exception(exc.Message);
            }
        }

        static async void SendErrorMessage(CallbackQueryEventArgs callback)
        {
            await Clients.botClient.SendTextMessageAsync(
                callback.CallbackQuery.Message.Chat.Id,
                text: "Что-то пошло не так. Попробуйте снова ");
        }
    }
}
