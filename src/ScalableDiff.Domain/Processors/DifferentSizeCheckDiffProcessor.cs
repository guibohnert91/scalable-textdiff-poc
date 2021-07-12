using ScalableDiff.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Processors
{
    public class DifferentSizeCheckDiffProcessor : DiffProcessorBase
    {
        public DifferentSizeCheckDiffProcessor(IDiffProcessor diffProcessor = null) : base(diffProcessor) { }

        protected override Task<DiffProcessorResult> Process(DiffData left, DiffData right)
        {
            if (left.Content.Length != right.Content.Length)
                return Task.FromResult(DiffProcessorResult.Create(true, "The sizes differs."));

            return Task.FromResult(DiffProcessorResult.Create(false, "The sizes equals."));
        }
    }
}
