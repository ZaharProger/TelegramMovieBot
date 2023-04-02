namespace TGBot.Models
{
    public class Tag : Model<int>
    {
        private string name;
        private double relevance;

        public Tag(int id, string name, double relevance)
            : base(id)
        {
            this.name = name;
            this.relevance = relevance;
        }

        public override string ToString()
        {
            return name + "(" + relevance.ToString() + ")";
        }
    }
}
