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
            var expectedLeftData = SetupDiffData("c2FtZSB0ZXh0");
            var expectedRightData = SetupDiffData("c2FtZSB0ZXh0");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.ExecuteAsync(expectedLeftData, expectedRightData);

            // Assert
            Assert.True(actualResult.Handled);
            Assert.Equal("Left equals right.", actualResult.Message);
        }

        [Fact]
        public async Task ExecutingChain_WithDifferentData_ShouldReturnNoMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("YmFzZSB0ZXh0");
            var expectedRightData = SetupDiffData("YW5vdGhlciBiYXNlIHRleHQ");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.ExecuteAsync(expectedLeftData, expectedRightData);

            // Assert
            Assert.False(actualResult.Handled);
            Assert.Equal("Left and right differs.", actualResult.Message);
        }

        private static IDiffProcessor SetupProcessor()
        {
            return new EqualityCheckDiffProcessor();
        }
    }
}
