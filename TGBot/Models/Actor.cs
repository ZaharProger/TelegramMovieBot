using System.Collections.Generic;

namespace TGBot.Models
{
    public class Actor : Model<string>
    {
        private string name;
        private List<string> category;

        Actor(string id, string name, List<string> category)
            : base(id)
        {
            this.name = name;
            this.category.AddRange(category);
        }

        public override string ToString()
        {
            return name + "(" + category.ToString() + ")";
        }
    }
}
