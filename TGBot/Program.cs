using System;
using Telegram.Bot.Polling;
using TGBot.Utils;

namespace TGBot
{

    class Program
    {
        static void Main(string[] args)
        {
            var bot = TelegramBotWrapper.GetInstance(
                "6225019057:AAHtVqsMloi8EA6SSe8KajUe68OXJSacA6c",
                new ReceiverOptions { 
                    AllowedUpdates = { }, 
                }
            );

            Console.WriteLine("Запущен бот");
            Console.ReadLine();
        }
    }
}