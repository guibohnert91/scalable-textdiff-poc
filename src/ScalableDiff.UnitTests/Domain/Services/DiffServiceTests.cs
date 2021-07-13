using Moq;
using ScalableDiff.Domain;
using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Stores;
using ScalableDiff.Domain.ValueObjects;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.UnitTests.Domain.Services
{
    public class DiffServiceTests
    {
        [Fact]
        public async Task GettingDiffAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var service = SetupService();
            var invalidId = Guid.Empty;            

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetAsync(invalidId));
        }

        [Fact]
        public async Task GettingDiffAsync_WithValidId_ShouldReturnBlankDiffWithId()
        {
            // Arrange
            var service = SetupService();
            var expectedId = Guid.Parse("92CB8471-9AC2-44C1-9D04-27AA60F6A194");

            // Act
            var actualDiff = await service.GetAsync(expectedId);

            // Assert
            Assert.NotNull(actualDiff);
            Assert.Equal(expectedId, actualDiff.Id);
        }

        [Fact]
        public async Task GettingDiffAsync_WithExistingValidId_ShouldReturnExistingDiff()
        {
            // Arrange
            var expectedId = Guid.Parse("92CB8471-9AC2-44C1-9D04-27AA60F6A194");
            var expectedDiff = SetupDiff(expectedId, "left content", "right content");

            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedDiff);

            var service = SetupService(storeMock.Object);

            // Act
            var actualDiff = await service.GetAsync(expectedId);

            // Assert
            Assert.NotNull(actualDiff);
            Assert.Equal(expectedId, actualDiff.Id);
            Assert.Equal(expectedDiff, actualDiff);
        }

        [Fact]
        public async Task ExecutingDiffAsync_WithNullDiffDiff_ShouldThrowArgumentNullException()
        {
            // Arrange
            var service = SetupService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.ExecuteAsync(null));
        }

        [Fact]
        public async Task ExecutingDiffAsync_WithValidDiffDiff_ShouldReturnValidDiffProcessorResult()
        {
            // Arrange
            var expectedId = Guid.Parse("CAEE5111-D6D9-4EA4-9FF9-3AFB65EAF258");
            var expectedDiff = SetupDiff(expectedId, "content", "content");
            
            var expectedProcessorResult = SetupProcessorResult(message: "Content match!");
            var processorMock = new Mock<IDiffProcessor>();
            processorMock.Setup(m => m.Execute(expectedDiff.Left, expectedDiff.Right))
                         .ReturnsAsync(expectedProcessorResult);

            var service = SetupService(diffProcessor: processorMock.Object);

            // Act
            var actualResult = await service.ExecuteAsync(expectedDiff);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(expectedProcessorResult, actualResult);
        }

        [Fact]
        public async Task ExecutingDiffAsync_WithInvalidDiffDiff_ShouldReturnInvalidDiffProcessorResult()
        {
            // Arrange
            var expectedId = Guid.Parse("C3802691-0BDF-4CC5-8CFF-F9C9034E4C1D");
            var expectedDiff = SetupDiff(expectedId, "different", "content");

            var expectedProcessorResult = SetupProcessorResult(match: false, message: "Content don't match!");
            var processorMock = new Mock<IDiffProcessor>();
            processorMock.Setup(m => m.Execute(expectedDiff.Left, expectedDiff.Right))
                         .ReturnsAsync(expectedProcessorResult);

            var service = SetupService(diffProcessor: processorMock.Object);

            // Act
            var actualResult = await service.ExecuteAsync(expectedDiff);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(expectedProcessorResult, actualResult);
        }

        private static Diff SetupDiff(Guid id, string leftContent = null, string rightContent = null)
        {
            var diff = Diff.Create(id);
            diff.SetLeftData(DiffData.Create(leftContent));
            diff.SetRightData(DiffData.Create(rightContent));

            return diff;
        }

        private static DiffProcessorResult SetupProcessorResult(bool match = true, string message = null)
        {
            return DiffProcessorResult.Create(match, message);
        }

        private static IDiffService SetupService(IStore<Diff> Store = null, IDiffProcessor diffProcessor = null)
        {
            return new DiffService(Store ?? Mock.Of<IStore<Diff>>(),
                                   diffProcessor ?? Mock.Of<IDiffProcessor>());
        }
    }
}
