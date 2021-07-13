using ScalableDiff.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Processors
{
    /// <summary>
    /// Verifies if the left and right sides of the diff have different sizes.
    /// </summary>
    public class DifferentSizeCheckDiffProcessor : DiffProcessorBase
    {
        public DifferentSizeCheckDiffProcessor(IDiffProcessor diffProcessor = null) : base(diffProcessor) { }

        /// <summary>
        /// Executes the diffing process between the left and right data comparing it's sizes.
        /// </summary>
        /// <param name="left">The left diff data to compare.</param>
        /// <param name="right">The right diff data to compare.</param>
        /// <returns>If the sizes differs or equals.</returns>
        protected override Task<DiffProcessorResult> Process(DiffData left, DiffData right)
        {
            if (left.Content.Length != right.Content.Length)
                return Task.FromResult(DiffProcessorResult.Create(true, "The sizes differs."));

            return Task.FromResult(DiffProcessorResult.Create(false, "The sizes equals."));
        }
    }
}
