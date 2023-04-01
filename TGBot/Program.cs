using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using TGBot.Utils;

namespace TGBot
{

    class Program
    {
        static void Main(string[] args)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };

            var bot = TelegramBotWrapper.GetInstance(
                "6225019057:AAHtVqsMloi8EA6SSe8KajUe68OXJSacA6c",
                receiverOptions
            );

            Console.WriteLine("Запущен бот");
            Console.ReadLine();
        }
    }
}