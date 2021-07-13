namespace ScalableDiff.Domain.ValueObjects
{
    public class DiffProcessorResult
    {
        public bool Handled { get; init; }

        public string Message { get; init; }

        protected DiffProcessorResult() { }

        /// <summary>
        /// Creates a new diff processor result.
        /// </summary>
        /// <param name="handled">if the result is handled.</param>
        /// <param name="message">The result message.</param>
        /// <returns>A diff processor result.</returns>
        public static DiffProcessorResult Create(bool handled, string message = null)
        {
            return new DiffProcessorResult
            {
                Handled = handled,
                Message = message
            };
        }
    }
}
