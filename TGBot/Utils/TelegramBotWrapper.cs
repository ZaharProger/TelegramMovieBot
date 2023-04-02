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
        private ITelegramBotClient Bot { get; set; }

        private static TelegramBotWrapper instance = null;
        private readonly InlineKeyboardMarkup inlineKeyboard;
        private Movie movie;

        private static readonly object syncLocker = new object();
        private bool isCanRequest;

        private TelegramBotWrapper(string token, ReceiverOptions options)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            Bot = new TelegramBotClient(token);

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

            Bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                options,
                cancellationToken
            );

            isCanRequest = false;
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
                    answer = BotConfig.WAIT;
                    isCanRequest = true;
                }

                await botClient.SendTextMessageAsync(message.Chat, BotConfig.QApairs[answer]);

                if (isCanRequest)
                {
                    isCanRequest = false;
                    string movieTitle = await FindTitleCinema(message.Text);

                    if (movieTitle.Equals(BotConfig.ERROR))
                    {
                        await botClient.SendTextMessageAsync(message.Chat, movieTitle);
                    }

                    else if (movieTitle.Equals(BotConfig.MISSING))
                    {
                        await botClient.SendTextMessageAsync(message.Chat, BotConfig.QApairs[BotConfig.MISSING]);
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

                }
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                isCanRequest = false;

                string codeOfButton = update.CallbackQuery.Data;
                var message = update.Message;

                if (codeOfButton.Equals(BotConfig.buttons.RATING))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        BotConfig.QApairs[BotConfig.RATING_BUTTON]
                    );    
                }

                else if (codeOfButton.Equals(BotConfig.buttons.ACTORS))
                {
                    await botClient.SendTextMessageAsync(
                            chatId: update.CallbackQuery.Message.Chat.Id,
                            BotConfig.QApairs[BotConfig.ACTORS_BUTTON]
                        );
                }

                isCanRequest = true;
            }
        }

        private async Task<string> FindTitleCinema(string attributes)
        {
            string answer = BotConfig.ERROR;

            if (!string.IsNullOrWhiteSpace(attributes))
            {
                List<string> listAttributes = new List<string>(attributes
                    .Trim()
                    .ToLower()
                    .Split(new char[0], StringSplitOptions.RemoveEmptyEntries)
                );

                if (listAttributes.Count != 0)
                {
                    //List<Movie> movies = (await Parser.Parse(listAttributes));
                    List<string> tags = (await Parser.GetTagIdByTag(listAttributes));
                    if (tags.Count == 0)
                        return BotConfig.MISSING;
                    List<string> moviesID = (await Parser.GetMovieIdByTagId(tags));
                    List<string> imdbID = (await Parser.GetImdbIdByMoviceId(moviesID));

                    List<Movie> movies = (await Parser.GetMoviebByID(imdbID));

                    if (movies.Count != 0)
                        answer = movies[0].Title;

                    else
                        answer = BotConfig.MISSING;
                }
            }
            
            return answer;
        }

        private async Task<string> FindDescription(string description)
        {
            string answer = BotConfig.ERROR;

            if (!string.IsNullOrWhiteSpace(description))
            {
                if (description.Equals(BotConfig.ACTORS_BUTTON))
                {
                    //movie = await Parser.Parse(30, movie.Actors);
                    answer = movie.Actors.ToString();
                }

                else if (description.Equals(BotConfig.RATING_BUTTON))
                {
                    //movie = await Parser.Parse(30, movie.Rating);
                    answer = movie.Rating.ToString();
                }

            }

            return answer;
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
