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
            var expectedLeftData = SetupDiffData("dGV4dCB0d28");
            var expectedRightData = SetupDiffData("dGV4dCBvbmU");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.ExecuteAsync(expectedLeftData, expectedRightData);

            // Assert
            Assert.True(actualResult.Handled);
            Assert.Equal("Left precedes right.", actualResult.Message);
        }

        [Fact]
        public async Task Executing_WithLeftFollowingData_ShouldReturnMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("dGV4dCBvbmUgYWdhaW4");
            var expectedRightData = SetupDiffData("dGV4dCB0d28gYWdhaW4");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.ExecuteAsync(expectedLeftData, expectedRightData);

            // Assert
            Assert.True(actualResult.Handled);
            Assert.Equal("Left follows right.", actualResult.Message);
        }

        [Fact]
        public async Task ExecutingChain_WithNoOffsetData_ShouldReturnNoMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("c29tZSBlbmMgdGV4dA");
            var expectedRightData = SetupDiffData("ZGlmZmVyZW50IGVuYyB0ZXh0");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.ExecuteAsync(expectedLeftData, expectedRightData);

            // Assert
            Assert.False(actualResult.Handled);
            Assert.Equal("The left and right has no offsets.", actualResult.Message);
        }

        private static IDiffProcessor SetupProcessor()
        {
            return new OffsetDiffProcessor();
        }
    }
}
