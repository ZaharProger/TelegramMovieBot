using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBot
{
    class Actor:Modal<string>
    {
        private string name;
        private List<string> category;

        Actor(string id, string name, List<string> category)
            :base(id)
        {
            this.id = id;
            this.name = name;
            this.category.AddRange(category);
        }

        protected override string ToString()
        {
            return name + "(" + category.ToString() + ")";
        }
    }
}
