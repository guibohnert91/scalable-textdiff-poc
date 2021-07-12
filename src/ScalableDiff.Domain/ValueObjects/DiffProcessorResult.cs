namespace ScalableDiff.Domain.ValueObjects
{
    public class DiffProcessorResult
    {
        public bool Match { get; init; }

        public string Message { get; init; }

        protected DiffProcessorResult() { }

        public static DiffProcessorResult Create(bool match, string message = null)
        {
            return new DiffProcessorResult
            {
                Match = match,
                Message = message
            };
        }
    }
}
