using ScalableDiff.Domain.ValueObjects;

namespace ScalableDiff.UnitTests.Domain.Processors
{
    public abstract class DiffProcessorBaseTests
    {
        protected static DiffData SetupDiffData(string content = null)
        {
            return DiffData.Create(content);
        }
    }
}
