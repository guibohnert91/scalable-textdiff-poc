namespace ScalableDiff.Application.Models
{
    public class DiffSummary
    {
        public string Message { get; init; }

        protected DiffSummary() { }

        public static DiffSummary Create(string message)
        {
            return new DiffSummary
            {
                Message = message
            };
        }
    }
}
