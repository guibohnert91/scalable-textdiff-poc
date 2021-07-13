using ScalableDiff.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Processors
{
    /// <summary>
    /// Verifies the left and right data contents offsets.
    /// </summary>
    public class OffsetDiffProcessor : DiffProcessorBase
    {
        public OffsetDiffProcessor(IDiffProcessor diffProcessor = null) : base(diffProcessor) { }

        /// <summary>
        /// Executes the diffing process between the left and right data verifying the offset of its contents.
        /// </summary>
        /// <param name="left">The left diff data to compare.</param>
        /// <param name="right">The right diff data to compare.</param>
        /// <returns>Left side precedes, follow or has no offsets.</returns>
        protected override Task<DiffProcessorResult> Process(DiffData left, DiffData right)
        {
            if (left.Content.Length == right.Content.Length)
            {
                var offset = left.Content.CompareTo(right.Content);
                if (offset < 0)
                    return Task.FromResult(DiffProcessorResult.Create(true, "Left precedes right."));
                else if(offset > 0)
                    return Task.FromResult(DiffProcessorResult.Create(true, "Left follows right."));
            }

            return Task.FromResult(DiffProcessorResult.Create(false, "The left and right has no offsets."));
        }
    }
}
