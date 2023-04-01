using System.Collections.Generic;

namespace TGBot.Models
{
    public class Movie : Model<string>
    {
        public string Title { get; private set; }
        public string Region { get; private set; }
        public Rating Rating { get; private set; }
        public List<Tag> Tags { get; private set; }
        public List<Actor> Actors { get; private set; }

        public Movie(string id, string title, string region, Rating rating = null, List<Tag> tags = null, List<Actor> actors = null)
            : base(id)
        {
            Title = title;
            Region = region;
            Rating = rating;
            Tags = tags;
            Actors = actors;
        }

        public override string ToString()
        {
            return id + "\t|\t" + Title + "\t|\t" + Region + "\t|\t" +
                   Rating.ToString() + "\t|\t" +
                   Tags.ToString() + "\t|\t" +
                   Actors.ToString();
        }
    }
}
