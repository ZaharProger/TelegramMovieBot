namespace TGBot.Models
{
    public class Rating : Model<string>
    {
        private float value;

        Rating(string id, float value, string tags)
            : base(id)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
