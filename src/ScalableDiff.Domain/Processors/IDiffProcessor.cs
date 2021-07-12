using ScalableDiff.Domain.ValueObjects;
using System.Threading.Tasks;

namespace ScalableDiff.Domain
{
    public interface IDiffProcessor
    {
        Task<DiffProcessorResult> Execute(DiffData left, DiffData right);
    }
}
