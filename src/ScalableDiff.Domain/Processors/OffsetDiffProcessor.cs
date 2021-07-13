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
            /* 
             * I will assume this is a good logic to implement to compare the left and right sides.        
             * Since "actual diffs are not needed ", this is doing only an offset check 
             * to determine if the left data precedes or follows the right data.
             * It uses the default string compare method, calculating the offsets based in the same position in the sort order.
            */
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
