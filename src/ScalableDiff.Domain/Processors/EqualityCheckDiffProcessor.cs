using ScalableDiff.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Processors
{
    public class EqualityCheckDiffProcessor : DiffProcessorBase
    {
        public EqualityCheckDiffProcessor(IDiffProcessor diffProcessor = null) : base(diffProcessor) { }

        protected override Task<DiffProcessorResult> Process(DiffData left, DiffData right)
        {
            if (left.Content.Equals(right.Content, StringComparison.InvariantCulture))
                return Task.FromResult(DiffProcessorResult.Create(true, "Left equals right."));

            return Task.FromResult(DiffProcessorResult.Create(false, "Left and right differs."));
        }
    }
}
