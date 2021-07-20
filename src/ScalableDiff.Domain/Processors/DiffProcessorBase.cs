using ScalableDiff.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Processors
{
    /// <summary>
    /// Abstract implementation of a diff processor using a chain of responsibility pattern.
    /// </summary>
    public abstract class DiffProcessorBase : IDiffProcessor
    {
        private readonly IDiffProcessor nextProcessor;

        public DiffProcessorBase(IDiffProcessor processor = null)
        {
            this.nextProcessor = processor;
        }

        /// <summary>
        /// Executes the diffing process using a chain of processors until a processor is match.
        /// </summary>
        /// <param name="left">The left diff data to compare.</param>
        /// <param name="right">The right diff data to compare.</param>
        /// <returns>The diffing chain process result.</returns>
        public virtual async Task<DiffProcessorResult> ExecuteAsync(DiffData left, DiffData right)
        {
            var processResult = await Process(left, right);
            
            if (!processResult.Handled && nextProcessor != null)
                return await nextProcessor.ExecuteAsync(left, right);

            return processResult;
        }

        protected abstract Task<DiffProcessorResult> Process(DiffData left, DiffData right);
    }
}
