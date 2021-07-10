using NLog;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();
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
                Bot.SendMessageAsync(chatId, text, replyMarkup);
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

        public static async void SendMessageAsync(long chatId, string text, ParseMode parseMode = ParseMode.Markdown)
        {
            if (text == "")
            {
                logger.Warn("Empty message rejected");
                return;
            }
            logger.Trace("Message sending");
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
                logger.Error("Message sending error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Message sent");
        }
        public static async void SendMessageAsync(long chatId, string text, InlineKeyboardMarkup replyMarkup, ParseMode parseMode = ParseMode.Markdown)
        {
            if (text == "")
            {
                logger.Warn("Empty message rejected");
                return;
            }
            logger.Trace("Message sending");
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
                logger.Error("Message sending error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);

            }
            logger.Trace("Message sent");
        }

        public static void SendMessage(long chatId, string text, InlineKeyboardMarkup replyMarkup, ParseMode parseMode = ParseMode.Markdown)
        {
            if (text == "")
            {
                logger.Warn("Empty message rejected");
                return;
            }
            logger.Trace("Message sending");
            try
            {
                botClient.SendTextMessageAsync(
                chatId: chatId,
                 text: text,
                 replyMarkup: replyMarkup,
                 parseMode: parseMode
                 );
            }
            catch (Exception exc)
            {
                logger.Error("Message sending failed");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Message sent");
        }

        public static async void SendPhotoAsync(long chatId, InputOnlineFile photo)
        {
            logger.Trace("Photo sending");
            try
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: photo
                    );
            }
            catch (Exception exc)
            {
                logger.Error("Photo sending failed");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Photo sent");
        }
        public static async void SendPhotoAsync(long chatId, InputOnlineFile photo, InlineKeyboardMarkup replyMarkup)
        {
            logger.Trace("Photo sending");
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
                logger.Error("Photo sending failed");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Photo sending");
        }
        public static async void SendPhotoAsync(long chatId, InputOnlineFile photo, string caption, InlineKeyboardMarkup replyMarkup)
        {
            logger.Trace("Photo sending");
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
                logger.Error("Photo sending error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);

            }
            logger.Trace("Photo sent");
        }
        public static async void SendPhotoAsync(long chatId, InputOnlineFile photo, string caption)
        {
            logger.Trace("Photo sending");
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
                logger.Error("Photo sending failed");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Photo sent");
        }

        public static async void EditMessageMediaAsync(long chatId, int messageId, InputMediaBase media)
        {
            logger.Trace("Editing message media");
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
                logger.Error("Editing message error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Media edited");
        }
        public static async void EditMessageMediaAsync(long chatId, int messageId, InputMediaBase media, InlineKeyboardMarkup replyMarkup)
        {
            logger.Trace("Editing message media");
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
                logger.Error("Editing message error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Media edited");
        }
        public static async void EditMessageTextAsync(long chatId, int messageId, string text, ParseMode parseMode = ParseMode.Markdown)
        {
            logger.Trace("Editing message text");
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
                logger.Error("Editing message text error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Message text edited");
        }
        public static async void EditMessageTextAsync(long chatId, int messageId, string text, InlineKeyboardMarkup replyMarkup, ParseMode parseMode = ParseMode.Markdown)
        {
            logger.Trace("Editing message text");
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
                logger.Error("Editing message text error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Text edited");
        }
        public static async void EditMessageCaptionAsync(long chatId, int messageId, string caption)
        {
            logger.Trace("Editing message caption");
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
                logger.Error("Editing message caption error");
                logger.Error(exc.Message);
                logger.Error(exc.StackTrace);
            }
            logger.Trace("Caption edited");
        }
    }
}
