using AutoMapper;
using Moq;
using ScalableDiff.Application.Services;
using ScalableDiff.Domain;
using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Stores;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.UnitTests.Application.Services
{
    public class DiffAppServiceTests
    {
        [Fact]
        public async Task SettingLeftDiffContent_WithValidContent_ShouldUpdateDiffSessionAndReturnTrueResult()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task SettingRightDiffContent_WithValidContent_ShouldUpdateDiffSessionAndReturnTrueResult()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task SettingLeftDiffContent_WithInvalidContent_ShouldNotUpdateDiffSessionAndReturnFalseResult()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task SettingRightDiffContent_WithInvalidContent_ShouldNotUpdateDiffSessionAndReturnFalseResult()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task SettingLeftDiffContent_WithNullContent_ShouldThrowArgumentNullException()
        {
            // Arrange
            var service = SetupService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.SetLeftDiffContent(null));
        }

        [Fact]
        public async Task SettingRightDiffContent_WithNullContent_ShouldThrowArgumentNullException()
        {
            // Arrange
            var service = SetupService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.SetRightDiffContent(null));
        }

        [Fact]
        public async Task ExecutingDiff_WithNonExistentSessionId_ShouldReturnNullSessionSummary()
        {
            // Arrange
            var expectedId = Guid.Parse("D3177120-A79A-43B9-9562-599973857A4B");
            var service = SetupService();

            // Act & Assert
            var actualSummary = await service.ExecuteDiff(expectedId);

            // Assert
            Assert.Null(actualSummary);
        }

        [Fact]
        public async Task ExecutingDiff_WithExistentSessionId_ShouldReturnValidSessionSummary()
        {
            // Arrange
            var expectedId = Guid.Parse("D3177120-A79A-43B9-9562-599973857A4B");
            var expectedDiff = Diff.Create(expectedId);
            
            var diffStoreMock = new Mock<IStore<Diff>>();
            diffStoreMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedDiff);

            var service = SetupService(diffStore: diffStoreMock.Object);

            // Act & Assert
            var actualSummary = await service.ExecuteDiff(expectedId);

            // Assert
            Assert.Null(actualSummary);
        }

        [Fact]
        public async Task ExecutingDiff_WithEmptySessionId_ShouldThrowArgumentException()
        {
            // Arrange
            var service = SetupService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.ExecuteDiff(Guid.Empty));
        }

        private static IDiffAppService SetupService(IDiffService diffService = null,
                                                    IStore<Diff> diffStore = null,
                                                    IMapper mapper = null)
        {
            return new DiffAppService(diffService ?? Mock.Of<IDiffService>(),
                                      diffStore ?? Mock.Of<IStore<Diff>>(),
                                      mapper ?? Mock.Of<IMapper>());
        }
    }
}
