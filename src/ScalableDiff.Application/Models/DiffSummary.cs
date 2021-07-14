namespace ScalableDiff.Application.Models
{
    public class DiffSummary
    {
        public string Message { get; init; }

        /// <summary>
        /// Creates a new diff summary.
        /// </summary>
        /// <param name="id">The diff summary message.</param>
        /// <returns>A new diff summary.</returns>
        public static DiffSummary Create(string message)
        {
            return new DiffSummary
            {
                Message = message
            };
        }
    }
}
