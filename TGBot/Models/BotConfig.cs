using System.Collections.Generic;

namespace TGBot.Models
{
    public class BotConfig
    {
        public static readonly string START = "/start";
        public static readonly string WATCH = "/watch";
        public static readonly string DMITRI = "/dmitri";
        public static readonly string ERROR = "error";
        public static readonly string MISSING = "missing";
        public static readonly string WAIT = "wait";
        public static readonly string ACTORS_BUTTON = "actors";
        public static readonly string RATING_BUTTON = "rating";
        public static readonly Buttons buttons = new Buttons();

        public static readonly Dictionary<string, string> QApairs = new Dictionary<string, string>()
        {
            { START, $"Привет! Я помогу найти фильмы для наиприятнейшего просмотра\U0001F60A!\nВведите {WATCH} для переходу к поиску фильмов" },
            { WATCH, "Hey Boy \U0001F60A!\nЧто хотите посмотреть сегодня \U0001F3A5?\n(Введите аттрибуты):" },
            { DMITRI, "Спасибо за курс \U0001F499" },
            { ERROR, "Хммм...\nНе могу понять сообщение!" },
            { WAIT, "Сейчас найду!" },
            { MISSING, "К сожалению по этим атрибутам я ничего не нашел" },
            { ACTORS_BUTTON, "Нажата Кнопка актеры" },
            { RATING_BUTTON, "Нажата Кнопка рейтинг" }
        };

        public class Buttons
        {
            public readonly string RATING = "Рейтинг";
            public readonly string ACTORS = "Актеры";
        }
    }
}
