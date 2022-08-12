namespace MMOGAScraper
{
    public class Requirements
    {
        public Requirements(bool available, string minimum, string maximum, bool uncomplete, string text)
        {
            Available = available;
            Minimum = minimum;
            Maximum = maximum;
            Uncomplete = uncomplete;
            Text = text;
        }

        public bool Available { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }
        public bool Uncomplete { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Available)}={Available}, {nameof(Minimum)}={Minimum}, {nameof(Maximum)}={Maximum}, {nameof(Uncomplete)}={Uncomplete}, {nameof(Text)}={Text}}}";
        }
    }
}