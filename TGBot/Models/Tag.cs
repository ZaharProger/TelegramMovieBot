namespace TGBot.Models
{
    class Tag : Model<int>
    {
        private string name;
        private double relevance;

        Tag(int id, string name, double relevance)
            : base(id)
        {
            this.name = name;
            this.relevance = relevance;
        }

        protected override string ToString()
        {
            return name + "(" + relevance.ToString() + ")";
        }
    }
}
