using ScalableDiff.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Processors
{
    public class OffsetDiffProcessor : DiffProcessorBase
    {
        public OffsetDiffProcessor(IDiffProcessor diffProcessor = null) : base(diffProcessor) { }

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

            return Task.FromResult(DiffProcessorResult.Create(false, "The left and right match."));
        }
    }
}
