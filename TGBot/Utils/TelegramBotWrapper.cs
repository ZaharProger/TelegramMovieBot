using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using TGBot.Models;

namespace TGBot.Utils
{
    public class TelegramBotWrapper
    {
        private static TelegramBotWrapper instance = null;
        private ITelegramBotClient Bot { get; set; }
        private bool isCanRequest;
        private static readonly object syncLocker = new object();
        private readonly InlineKeyboardMarkup inlineKeyboard;

        private TelegramBotWrapper(string token, ReceiverOptions options)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            Bot = new TelegramBotClient(token);
            Bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                options,
                cancellationToken
            );

            isCanRequest = true;

            inlineKeyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            text: BotConfig.buttons.RATING,
                            callbackData: BotConfig.buttons.RATING
                        ),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            text: BotConfig.buttons.ACTORS,
                            callbackData: BotConfig.buttons.ACTORS
                        ),
                    },
                }
            );
        }

        public static TelegramBotWrapper GetInstance(string token, ReceiverOptions options)
        {
            if (instance == null)
            {
                lock(syncLocker)
                {
                    if (instance == null)
                    {
                        instance = new TelegramBotWrapper(token, options);
                    }
                }
            }

            return instance;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (isCanRequest)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

                if (update.Type == UpdateType.Message)
                {
                    var message = update.Message;
                    var answer = "";

                    if (message.Text.ToLower().Equals(BotConfig.START))
                    {
                        answer = BotConfig.START;
                    }
                    else if (message.Text.ToLower().Equals(BotConfig.WATCH))
                    {
                        answer = BotConfig.WATCH;
                    }
                    else if (message.Text.ToLower().Equals(BotConfig.DMITRI))
                    {
                        answer = BotConfig.DMITRI;
                    }
                    else
                    {
                        isCanRequest = false;
                        answer = BotConfig.WAIT;
                    }

                    await botClient.SendTextMessageAsync(message.Chat, BotConfig.QApairs[answer]);

                    if (!isCanRequest)
                    {
                        string movieTitle = await FindTitleCinema(message.Text);

                        if (movieTitle.Equals(BotConfig.ERROR))
                        {
                            await botClient.SendTextMessageAsync(message.Chat, movieTitle);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: $"Фильм: {movieTitle}",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: cancellationToken
                            );
                        }

                        isCanRequest = true;
                    }
                }

                if (update.Type == UpdateType.CallbackQuery)
                {
                    string codeOfButton = update.CallbackQuery.Data;
                    var message = update.Message;

                    if (codeOfButton.Equals(BotConfig.buttons.RATING) || codeOfButton.Equals(BotConfig.buttons.ACTORS))
                    {
                        isCanRequest = false;

                        if (codeOfButton.Equals(BotConfig.buttons.ACTORS))
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: update.CallbackQuery.Message.Chat.Id,
                                BotConfig.QApairs[BotConfig.ACTORS_BUTTON]
                            );
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: update.CallbackQuery.Message.Chat.Id,
                                BotConfig.QApairs[BotConfig.RATING_BUTTON]
                            );
                        }      
                        
                        isCanRequest = true;
                    }
                }
            }

        }

        private async Task<string> FindTitleCinema(string attributes)
        {
            var isEmpty = attributes == null;
            var titleMovie = BotConfig.ERROR;

            if (!isEmpty)
            {
                List<string> listAttributes = new List<string>(attributes
                    .Trim()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                );

                if (listAttributes.Count != 0)
                {
                    var parser = new Parser();
                    titleMovie = await parser.Parse(30, listAttributes);
                }               
            }          
      
            return titleMovie;
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
