using System;
using System.IO;
using NLog;

using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using ImageResizer;
using Telegram.Bot.Types;

namespace TimelistBot
{
    class DramaTheatreTimelist : ITimelist
    {
        const string imgPath = "https://bresttheatre.Trace/assets/teatr/img/%D1%80%D0%B5%D0%BF%D0%B5%D1%80%D1%82%D1%83%D0%B0%D1%80%D0%BD%D0%B0%D1%8F%20%D0%B0%D1%84%D0%B8%D1%88%D0%B0/%D1%80%D0%B5%D0%BF%D0%B5%D1%80%D1%82%D1%83%D0%B0%D1%80%20%D0%A4%D0%95%D0%92%D0%A0%D0%90%D0%9B%D0%AC%202021.jpg";
        static Logger logger = LogManager.GetCurrentClassLogger();
        static string savePath = Environment.CurrentDirectory + "\\img\\DramaTheatreTimelist\\timelist.jpg";
        static string resizerPath = Environment.CurrentDirectory + "\\img\\DramaTheatreTimelist\\compressedTimelist.jpg";
        public async void SendTimelist(CallbackQueryEventArgs callback)
        {
            long chatId = callback.CallbackQuery.Message.Chat.Id;
            try
            {
                Bot.SendMessageAsync(chatId, "Это может занять некоторое время...");

                DownloadTimelist();
                ResizeImage();
                var inlineKeyboard = new InlineKeyboardMarkup(
                    new InlineKeyboardButton()
                    {
                        Text = "Обновить",
                        CallbackData = "Update"
                    }
                     );

                var photo = new InputOnlineFile(new FileStream(resizerPath, FileMode.Open));
                string caption = "Актуально на " + DateTime.Now;
                Bot.SendPhotoAsync(chatId, photo, caption, inlineKeyboard);
            }
            catch (Exception exc)
            {
                logger.Error("Ошибка отправки расписания");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
                SendErrorMessage(callback);
            }

        }

        public async void UpdateTimelist(CallbackQueryEventArgs callback)
        {
            long chatId = callback.CallbackQuery.Message.Chat.Id;
            int messageId = callback.CallbackQuery.Message.MessageId;
            try
            {
                logger.Trace("Update has begun");
                DownloadTimelist();
                ResizeImage();

                var inlineKeyboard = new InlineKeyboardMarkup(
                    new InlineKeyboardButton()
                    {
                        Text = "Обновить",
                        CallbackData = "Update"
                    }
                     );

                var media = new InputMediaPhoto(new InputMedia(new FileStream(resizerPath, FileMode.Open), "compressedTimelist"));
                Bot.EditMessageMediaAsync(chatId, messageId, media);

                logger.Trace("Updating is done");
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }

        }

        static void DownloadTimelist()
        {
            logger.Trace("Downloading a picture began");
            try
            {
                Web.webClient.DownloadFile(imgPath, savePath);
                logger.Trace("Download is succeful.");
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        static void ResizeImage()
        {
            try
            {
                logger.Trace("Resizing began");
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
                logger.Trace("Resize is succeful");
            }
            catch (Exception exc)
            {
                logger.Error("Resize error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
                throw new Exception(exc.Message);
            }
        }

        static async void SendErrorMessage(CallbackQueryEventArgs callback)
        {
            Bot.SendMessageAsync(callback.CallbackQuery.Message.Chat.Id, "Что-то пошло не так. Попробуйте снова");
        }
    }
}
