using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Extensions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;

namespace TelegramBotExperiments
{

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("6225019057:AAHtVqsMloi8EA6SSe8KajUe68OXJSacA6c");

        static private bool isCanRequest;

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (isCanRequest)
            {

                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    var message = update.Message;

                    if (message.Text.ToLower() == "/watch")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Hey Boy \U0001F60A!\nЧто хотите посмотреть сегодня \U0001F3A5?\n(Введите аттрибуты):");
                    }

                    else if (message.Text.ToLower() == "/dmitri")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Спасибо за курс \U0001F499");
                    }

                    else
                    {
                        isCanRequest = false;
                        await botClient.SendTextMessageAsync(message.Chat, "Сейчас найду!");

                        string movieTitle = await FindTitleCinema(message.Text);

                        if (movieTitle.Equals("error"))
                        {
                            await bot.SendTextMessageAsync(message.Chat, "Хммм...\nНе могу понять сообщение!");
                            return;
                        }

                        SendInline(botClient: botClient, chatId: message.Chat.Id, cancellationToken: cancellationToken, movieTitle);
                        isCanRequest = true;
                    }
                }

                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                {
                    string codeOfButton = update.CallbackQuery.Data;
                    var message = update.Message;

                    if (codeOfButton.Equals("rating"))
                    {
                        isCanRequest = false;
                        await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, "Нажата Кнопка рейтинг");
                        isCanRequest = true;
                    }

                    else if (codeOfButton.Equals("actors"))
                    {
                        isCanRequest = false;
                        await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, "Нажата Кнопка актеры");
                        isCanRequest = true;
                    }
                }
            }
            
        }

        private static async Task<string> FindTitleCinema(string attributes)
        {
            if (attributes == null)
                return "error";

            List<string> listAttributes = new List<string>(attributes.Trim().Split(" "));

            if (listAttributes.Count == 0)
                return "error";

            //await ;

            string titleMovie = "Hey";
            return titleMovie;
        }

        public static async void SendInline(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken, string model)
        {
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                new[]
                {

                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Рейтинг", callbackData: "rating"),

                    },

                    new[]
                    {

                        InlineKeyboardButton.WithCallbackData(text: "Актеры", callbackData: "actors"),

                    },

                });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Фильм: " + model,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
            
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, 
            };

            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            isCanRequest = true;
            Console.ReadLine();
        }
    }
}