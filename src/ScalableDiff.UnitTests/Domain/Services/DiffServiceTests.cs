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
        public async Task GettingSessionAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var service = SetupService();
            var invalidId = Guid.Empty;            

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetSessionAsync(invalidId));
        }

        [Fact]
        public async Task GettingSessionAsync_WithValidId_ShouldReturnBlankSessionWithId()
        {
            // Arrange
            var service = SetupService();
            var expectedId = Guid.Parse("92CB8471-9AC2-44C1-9D04-27AA60F6A194");

            // Act
            var actualSession = await service.GetSessionAsync(expectedId);

            // Assert
            Assert.NotNull(actualSession);
            Assert.Equal(expectedId, actualSession.Id);
        }

        [Fact]
        public async Task GettingSessionAsync_WithExistingValidId_ShouldReturnExistingSession()
        {
            // Arrange
            var expectedId = Guid.Parse("92CB8471-9AC2-44C1-9D04-27AA60F6A194");
            var expectedSession = SetupSession(expectedId, "left content", "right content");

            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedSession);

            var service = SetupService(storeMock.Object);

            // Act
            var actualSession = await service.GetSessionAsync(expectedId);

            // Assert
            Assert.NotNull(actualSession);
            Assert.Equal(expectedId, actualSession.Id);
            Assert.Equal(expectedSession, actualSession);
        }

        [Fact]
        public async Task ExecutingSessionAsync_WithNullDiffSession_ShouldThrowArgumentNullException()
        {
            // Arrange
            var service = SetupService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.ExecuteAsync(null));
        }

        [Fact]
        public async Task ExecutingSessionAsync_WithValidDiffSession_ShouldReturnValidDiffProcessorResult()
        {
            // Arrange
            var expectedId = Guid.Parse("CAEE5111-D6D9-4EA4-9FF9-3AFB65EAF258");
            var expectedSession = SetupSession(expectedId, "content", "content");
            
            var expectedProcessorResult = SetupProcessorResult(message: "Content match!");
            var processorMock = new Mock<IDiffProcessor>();
            processorMock.Setup(m => m.Execute(expectedSession.Left, expectedSession.Right))
                         .ReturnsAsync(expectedProcessorResult);

            var service = SetupService(diffProcessor: processorMock.Object);

            // Act
            var actualResult = await service.ExecuteAsync(expectedSession);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(expectedProcessorResult, actualResult);
        }

        [Fact]
        public async Task ExecutingSessionAsync_WithInvalidDiffSession_ShouldReturnInvalidDiffProcessorResult()
        {
            // Arrange
            var expectedId = Guid.Parse("C3802691-0BDF-4CC5-8CFF-F9C9034E4C1D");
            var expectedSession = SetupSession(expectedId, "different", "content");

            var expectedProcessorResult = SetupProcessorResult(match: false, message: "Content don't match!");
            var processorMock = new Mock<IDiffProcessor>();
            processorMock.Setup(m => m.Execute(expectedSession.Left, expectedSession.Right))
                         .ReturnsAsync(expectedProcessorResult);

            var service = SetupService(diffProcessor: processorMock.Object);

            // Act
            var actualResult = await service.ExecuteAsync(expectedSession);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(expectedProcessorResult, actualResult);
        }

        private static Diff SetupSession(Guid id, string leftContent = null, string rightContent = null)
        {
            var session = Diff.Create(id);
            session.SetLeftData(DiffData.Create(leftContent));
            session.SetRightData(DiffData.Create(rightContent));

            return session;
        }

        private static DiffProcessorResult SetupProcessorResult(bool match = true, string message = null)
        {
            return DiffProcessorResult.Create(match, message);
        }

        private static IDiffService SetupService(IStore<Diff> sessionStore = null, IDiffProcessor diffProcessor = null)
        {
            return new DiffService(sessionStore ?? Mock.Of<IStore<Diff>>(),
                                   diffProcessor ?? Mock.Of<IDiffProcessor>());
        }
    }
}
