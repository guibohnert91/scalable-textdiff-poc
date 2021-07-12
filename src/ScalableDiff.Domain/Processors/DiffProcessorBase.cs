using ScalableDiff.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Processors
{
    /// <summary>
    /// Abstract implementation of a chain of responsibility pattern.
    /// </summary>
    public abstract class DiffProcessorBase : IDiffProcessor
    {
        private readonly IDiffProcessor nextProcessor;

        public DiffProcessorBase(IDiffProcessor processor = null)
        {
            this.nextProcessor = processor;
        }

        public async Task<DiffProcessorResult> Execute(DiffData left, DiffData right)
        {
            var processResult = await Process(left, right);
            
            if (!processResult.Match && nextProcessor != null)
                return await nextProcessor.Execute(left, right);

            return processResult;
        }

        protected abstract Task<DiffProcessorResult> Process(DiffData left, DiffData right);
    }
}
