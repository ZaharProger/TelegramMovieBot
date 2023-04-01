namespace TGBot.Models
{
    class Rating : Model<string>
    {
        private float value;

        Rating(string id, float value, string tags)
            : base(id)
        {
            this.value = value;
        }

        protected override string ToString()
        {
            return value.ToString();
        }
    }
}
