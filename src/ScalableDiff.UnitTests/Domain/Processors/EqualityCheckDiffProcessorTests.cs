using ScalableDiff.Domain;
using ScalableDiff.Domain.Processors;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.UnitTests.Domain.Processors
{
    public class EqualityCheckDiffProcessorTests : DiffProcessorBaseTests
    {
        [Fact]
        public async Task Executing_WithEqualData_ShouldReturnMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("same text");
            var expectedRightData = SetupDiffData("same text");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.Execute(expectedLeftData, expectedRightData);

            // Assert
            Assert.True(actualResult.Match);
            Assert.Equal("Left equals right.", actualResult.Message);
        }

        [Fact]
        public async Task ExecutingChain_WithDifferentData_ShouldReturnNoMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("one text");
            var expectedRightData = SetupDiffData("another text");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.Execute(expectedLeftData, expectedRightData);

            // Assert
            Assert.False(actualResult.Match);
            Assert.Equal("Left and right differs.", actualResult.Message);
        }

        private static IDiffProcessor SetupProcessor()
        {
            return new EqualityCheckDiffProcessor();
        }
    }
}
