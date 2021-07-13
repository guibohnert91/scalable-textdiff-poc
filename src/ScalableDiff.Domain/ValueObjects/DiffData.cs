namespace ScalableDiff.Domain.ValueObjects
{
    public class DiffData
    {
        public string Content { get; init; }

        protected DiffData() { }

        /// <summary>
        /// Creates a new diff data.
        /// </summary>
        /// <param name="content">The content of the diff data.</param>
        /// <returns>A diff data.</returns>
        public static DiffData Create(string content)
        {
            return new DiffData
            {
                Content = content
            };
        }
    }
}
