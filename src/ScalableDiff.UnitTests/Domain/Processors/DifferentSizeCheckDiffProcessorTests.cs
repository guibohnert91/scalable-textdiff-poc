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
            var expectedLeftData = SetupDiffData("bWluIHRleHQ");
            var expectedRightData = SetupDiffData("bGFyZ2UgdGV4dA");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.ExecuteAsync(expectedLeftData, expectedRightData);

            // Assert
            Assert.True(actualResult.Handled);
            Assert.Equal("The sizes differs.", actualResult.Message);
        }

        [Fact]
        public async Task ExecutingChain_WithEqualsSizeData_ShouldReturnNoMatchProcessResult()
        {
            // Arrange
            var expectedLeftData = SetupDiffData("c2FtZSB0ZXh0IGRhdGE");
            var expectedRightData = SetupDiffData("c2FtZSB0ZXh0IGRhdGE");

            var processor = SetupProcessor();

            // Act
            var actualResult = await processor.ExecuteAsync(expectedLeftData, expectedRightData);

            // Assert
            Assert.False(actualResult.Handled);
            Assert.Equal("The sizes equals.", actualResult.Message);
        }


        private static IDiffProcessor SetupProcessor()
        {
            return new DifferentSizeCheckDiffProcessor();
        }
    }
}
