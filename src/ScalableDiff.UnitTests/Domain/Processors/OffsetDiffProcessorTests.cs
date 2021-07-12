using ScalableDiff.Domain;
using ScalableDiff.Domain.Processors;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.UnitTests.Domain.Processors
{
    public class OffsetDiffProcessorTests : DiffProcessorBaseTests
    {
        [Fact]
        public async Task Executing_WithLeftPrecedingData_ShouldReturnMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("abc text");
            var expectedRightData = SetupDiffData("bca text");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.Execute(expectedLeftData, expectedRightData);

            // Assert
            Assert.True(actualResult.Match);
            Assert.Equal("Left precedes right.", actualResult.Message);
        }

        [Fact]
        public async Task Executing_WithLeftFollowingData_ShouldReturnMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("bca text");
            var expectedRightData = SetupDiffData("abc text");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.Execute(expectedLeftData, expectedRightData);

            // Assert
            Assert.True(actualResult.Match);
            Assert.Equal("Left follows right.", actualResult.Message);
        }

        [Fact]
        public async Task ExecutingChain_WithNoOffsetData_ShouldReturnNoMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("one text");
            var expectedRightData = SetupDiffData("another text");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.Execute(expectedLeftData, expectedRightData);

            // Assert
            Assert.False(actualResult.Match);
            Assert.Equal("The left and right match.", actualResult.Message);
        }

        private static IDiffProcessor SetupProcessor()
        {
            return new OffsetDiffProcessor();
        }
    }
}
