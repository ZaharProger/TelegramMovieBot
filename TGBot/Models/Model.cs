namespace TGBot.Models
{
    public abstract class Model<T>
    {
        protected T id;
        protected string regex;


        protected Model(T id)
        {
            this.id = id;
        }

        public override abstract string ToString();
    }
}
