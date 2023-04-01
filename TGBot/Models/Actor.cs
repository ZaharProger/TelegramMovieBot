using System.Collections.Generic;

namespace TGBot.Models
{
    class Actor : Model<string>
    {
        private string name;
        private List<string> category;

        Actor(string id, string name, List<string> category)
            : base(id)
        {
            this.name = name;
            this.category.AddRange(category);
        }

        protected override string ToString()
        {
            return name + "(" + category.ToString() + ")";
        }
    }
}
