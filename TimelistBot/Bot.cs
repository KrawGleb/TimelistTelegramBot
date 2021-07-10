using System;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TimelistBot
{
    static class Bot
    {
        private const string TOKEN = "1302268641:AAF4QtLhE78WpTGSN5BgKU_GRv-U_p2tPaA";
        private static ITelegramBotClient botClient = new TelegramBotClient(TOKEN);
        public static ITimelist targetTimelist;

        static Bot()
        {
            botClient.OnMessage += TextMessageHandler;
            botClient.OnCallbackQuery += CallbackQueryHandler;
        }

        public static void Start()
        {
            botClient.StartReceiving();
        }
        public static void Stop()
        {
            botClient.StopReceiving();
        }

        private static void TextMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                long chatId = e.Message.Chat.Id;
                string text = "Меню";
                InlineKeyboardMarkup replyMarkup = new InlineKeyboardMarkup(
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
                            },

                            new InlineKeyboardButton()
                            {
                                Text = "Кино",
                                CallbackData = "Cinema"
                            }
                    }
                    );
                Bot.SendMessage(chatId, text, replyMarkup);
            }
        }
        static void CallbackQueryHandler(object sender, CallbackQueryEventArgs callback)
        {
            if (callback.CallbackQuery.Data == "IcePalaceTimelist")
            {
                targetTimelist = new IcePalaceTimelist();
                targetTimelist.SendTimelist(callback);
            }
            else if (callback.CallbackQuery.Data == "DramaTheatreTimelist")
            {
                targetTimelist = new DramaTheatreTimelist();
                targetTimelist.SendTimelist(callback);
            }
            else if (callback.CallbackQuery.Data == "Update")
            {
                targetTimelist?.UpdateTimelist(callback);
            }
            else if (callback.CallbackQuery.Data == "Cinema")
            {
                targetTimelist = new CinemaTimelist();
                targetTimelist.SendTimelist(callback);
            }
        }

        public static async void SendMessage(long chatId, string text, ParseMode parseMode = ParseMode.Markdown)
        {
            Console.WriteLine("Отправка сообщения...");
            try
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                parseMode: parseMode
                );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Отправка не удалась");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Сообщение отправленно");
        }
        public static async void SendMessage(long chatId, string text, InlineKeyboardMarkup replyMarkup, ParseMode parseMode = ParseMode.Markdown)
        {
            Console.WriteLine("Отправка сообщения...");
            try
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                 text: text,
                 replyMarkup: replyMarkup,
                 parseMode: parseMode
                 );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Отправка не удалась");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Сообщение отправленно");
        }

        public static async void SendPhoto(long chatId, InputOnlineFile photo)
        {
            Console.WriteLine("Отправка фото...");
            try
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: photo
                    );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Отправка не удалась");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Фото отправлено");
        }
        public static async void SendPhoto(long chatId, InputOnlineFile photo, InlineKeyboardMarkup replyMarkup)
        {
            Console.WriteLine("Отправка фото...");
            try
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: photo,
                    replyMarkup: replyMarkup
                    );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Отправка не удалась");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Фото отправлено");
        }
        public static async void SendPhoto(long chatId, InputOnlineFile photo, string caption, InlineKeyboardMarkup replyMarkup)
        {
            Console.WriteLine("Отправка фото...");
            try
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: photo,
                    caption: caption,
                    replyMarkup: replyMarkup
                    );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Отправка не удалась");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Фото отправлено");
        }
        public static async void SendPhoto(long chatId, InputOnlineFile photo, string caption)
        {
            Console.WriteLine("Отправка фото...");
            try
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: photo,
                    caption: caption
                    );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Отправка не удалась");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Фото отправлено");
        }

        public static async void EditMessageMedia(long chatId, int messageId, InputMediaBase media)
        {
            Console.WriteLine("Изменение медиа сообщения");
            try
            {
                await botClient.EditMessageMediaAsync(
                    chatId: chatId,
                    messageId: messageId,
                    media: media
                    );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Ошибка изменения медиа");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Медиа изменено");
        }
        public static async void EditMessageMedia(long chatId, int messageId, InputMediaBase media, InlineKeyboardMarkup replyMarkup)
        {
            Console.WriteLine("Изменение медиа сообщения");
            try
            {
                await botClient.EditMessageMediaAsync(
                    chatId: chatId,
                    messageId: messageId,
                    media: media,
                    replyMarkup: replyMarkup
                    );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Ошибка изменения медиа");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Медиа изменено");
        }
        public static async void EditMessageText(long chatId, int messageId, string text, ParseMode parseMode = ParseMode.Markdown)
        {
            Console.WriteLine("Изменение текста сообщения");
            try
            {
                await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: text,
                parseMode: parseMode);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Не удалось изменить текст сообщения");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Текст изменён");
        }
        public static async void EditMessageText(long chatId, int messageId, string text, InlineKeyboardMarkup replyMarkup, ParseMode parseMode = ParseMode.Markdown)
        {
            Console.WriteLine("Изменение текста сообщения");
            try
            {   
                await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: text,
                replyMarkup: replyMarkup,
                parseMode: parseMode);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Не удалось изменить текст сообщения");
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Текст изменён");
        }
        public static async void EditMessageCaption(long chatId, int messageId, string caption)
        {
            Console.WriteLine("Изменение подписи");
            try
            {
                await botClient.EditMessageCaptionAsync(
                    chatId: chatId,
                    messageId: messageId,
                    caption: caption
                    );
            }
            catch (Exception exc)
            {
                Console.WriteLine("Ошибка изменения");
                Console.WriteLine(exc.Message);
            }
        }
    }
}
