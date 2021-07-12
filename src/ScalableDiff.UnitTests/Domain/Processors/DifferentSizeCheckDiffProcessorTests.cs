using ScalableDiff.Domain;
using ScalableDiff.Domain.Processors;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.UnitTests.Domain.Processors
{
    public class DifferentSizeCheckDiffProcessorTests : DiffProcessorBaseTests
    {
        [Fact]
        public async Task Executing_WithDifferentSizeData_ShouldReturnMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("min text");
            var expectedRightData = SetupDiffData("large text");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.Execute(expectedLeftData, expectedRightData);

            // Assert
            Assert.True(actualResult.Match);
            Assert.Equal("The sizes differs.", actualResult.Message);
        }

        [Fact]
        public async Task ExecutingChain_WithEqualsSizeData_ShouldReturnNoMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("same text");
            var expectedRightData = SetupDiffData("same text");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.Execute(expectedLeftData, expectedRightData);

            // Assert
            Assert.False(actualResult.Match);
            Assert.Equal("The sizes equals.", actualResult.Message);
        }


        private static IDiffProcessor SetupProcessor()
        {
            return new DifferentSizeCheckDiffProcessor();
        }
    }
}
