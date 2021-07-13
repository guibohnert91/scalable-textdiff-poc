using ScalableDiff.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ScalableDiff.Domain
{
    /// <summary>
    /// Diffing processor.
    /// </summary>
    public interface IDiffProcessor
    {
        /// <summary>
        /// Executes a diff processor
        /// </summary>
        /// <param name="left">The left diff data to compare.</param>
        /// <param name="right">The right diff data to compare.</param>
        /// <returns>The processor result.</returns>
        Task<DiffProcessorResult> Execute(DiffData left, DiffData right);
    }
}
