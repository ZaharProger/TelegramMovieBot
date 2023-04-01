using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBot
{
    abstract class Modal<T>
    {
        protected T id;
        protected string regex;


        protected Modal(T id)
        {
            this.id = id;
        }

        protected new abstract string ToString();

         
    }
}
