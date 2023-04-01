using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBot
{
    class Rating:Modal<string>
    {
        private string id;
        private float value;

        Rating(string id, float value, string tags)
            :base(id)
        {
            this.id = id;
            this.value = value;
        }

        protected override string ToString()
        {
            return value.ToString();
        }
    }
}
