﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBot
{
    class Movie:Modal<string>
    {
        private string title;
        private string region;
        private Rating rating;
        private List<Tag> tags;
        private List<Actor> actors;

        Movie(string id, string title, string region, Rating rating, List<Tag> tags, List<Actor> actors)
            :base(id)
        {
            this.title = title;
            this.region = region;
            this.rating = rating;
            this.tags = tags;
            this.actors = actors;
        }

        protected override string ToString()
        {
            return base.id + "\t|\t" + title + "\t|\t" + region + "\t|\t" + 
                   rating.ToString() + "\t|\t" + 
                   tags.ToString() + "\t|\t" + 
                   actors.ToString()
                   ;
        }
    }
}
