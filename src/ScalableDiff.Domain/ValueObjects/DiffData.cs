namespace ScalableDiff.Domain.ValueObjects
{
    public class DiffData
    {
        public string Content { get; init; }

        protected DiffData() { }

        public static DiffData Create(string content)
        {
            return new DiffData
            {
                Content = content
            };
        }
    }
}
