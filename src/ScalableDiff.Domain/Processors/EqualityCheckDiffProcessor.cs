using ScalableDiff.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Processors
{
    /// <summary>
    /// Check if the content of both left and right data are equal.
    /// </summary>
    public class EqualityCheckDiffProcessor : DiffProcessorBase
    {
        public EqualityCheckDiffProcessor(IDiffProcessor diffProcessor = null) : base(diffProcessor) { }

        /// <summary>
        /// Executes the diffing process between the left and right data comparing its content.
        /// </summary>
        /// <param name="left">The left diff data to compare.</param>
        /// <param name="right">The right diff data to compare.</param>
        /// <returns>If the content differs or equals.</returns>
        protected override Task<DiffProcessorResult> Process(DiffData left, DiffData right)
        {
            if (left.Content.Equals(right.Content, StringComparison.InvariantCulture))
                return Task.FromResult(DiffProcessorResult.Create(true, "Left equals right."));

            return Task.FromResult(DiffProcessorResult.Create(false, "Left and right differs."));
        }
    }
}
