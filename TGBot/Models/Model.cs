namespace TGBot.Models
{
    abstract class Model<T>
    {
        protected T id;
        protected string regex;


        protected Model(T id)
        {
            this.id = id;
        }

        protected new abstract string ToString();


    }
}
