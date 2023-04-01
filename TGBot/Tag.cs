using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBot
{
    class Tag:Modal<int>
    {
        private string name;
        private double relevance;

        Tag(int id, string name, double relevance)
            :base(id)
        {
            this.id = id;
            this.name = name;
            this.relevance = relevance;
        }

        protected override string ToString()
        {
            return name + "(" + relevance.ToString() + ")";
        }
    }
}
