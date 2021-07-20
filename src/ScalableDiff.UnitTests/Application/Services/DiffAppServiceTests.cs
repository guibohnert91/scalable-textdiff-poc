using AutoMapper;
using Moq;
using ScalableDiff.Application.Models;
using ScalableDiff.Application.Profiles;
using ScalableDiff.Application.Services;
using ScalableDiff.Domain;
using ScalableDiff.Domain.Factories;
using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Stores;
using ScalableDiff.Domain.ValueObjects;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.UnitTests.Application.Services
{
    public class DiffAppServiceTests
    {
        [Fact]
        public async Task CreatingDiffAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var expectedId = Guid.Parse("92CB8471-9AC2-44C1-9D04-27AA60F6A194");
            var expectedDiff = SetupDiff(expectedId);

            var factoryMock = new Mock<IDiffFactory>();
            factoryMock.Setup(m => m.Create(expectedId)).Returns(expectedDiff);

            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.WriteAsync(expectedId, expectedDiff)).ReturnsAsync(true);

            var service = SetupService(diffFactory: factoryMock.Object, 
                                       diffStore: storeMock.Object);
            
            // Act
            var actualResult = await service.CreateDiff(expectedId);

            // Assert
            Assert.True(actualResult);
            factoryMock.Verify(m => m.Create(expectedId), Times.Once);
            storeMock.Verify(m => m.WriteAsync(expectedId, expectedDiff), Times.Once);
        }

        [Fact]
        public async Task CreatingDiffAsync_WithExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var expectedId = Guid.Parse("CF08F726-0540-4979-8C65-41C343CF0E65");
            var expectedDiff = SetupDiff(expectedId);
            
            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedDiff);

            var service = SetupService(diffStore: storeMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateDiff(expectedId));
        }

        [Fact]
        public async Task SettingLeftDiffContent_WithValidContent_ShouldUpdateDiffAndReturnTrueResult()
        {
            // Arrange
            var expectedId = Guid.Parse("C3D35176-8555-4FDE-908F-5071D675C7F4");
            var expectedDiff = SetupDiff(expectedId);
            var expectedDiffContent = DiffContent.Create(expectedId, "left content");

            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedDiff);
            storeMock.Setup(m => m.WriteAsync(expectedId, expectedDiff)).ReturnsAsync(true);

            var service = SetupService(diffStore: storeMock.Object);

            // Act
            var actualResult = await service.SetLeftDiffContent(expectedDiffContent);

            // Assert
            Assert.True(actualResult);
            storeMock.Verify(m => m.WriteAsync(expectedId, expectedDiff), Times.Once);
        }

        [Fact]
        public async Task SettingRightDiffContent_WithValidContent_ShouldUpdateDiffAndReturnTrueResult()
        {
            // Arrange
            var expectedId = Guid.Parse("2ADC5309-33EE-4A08-B60B-404566A92687");
            var expectedDiff = SetupDiff(expectedId);
            var expectedDiffContent = DiffContent.Create(expectedId, "left content");

            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedDiff);
            storeMock.Setup(m => m.WriteAsync(expectedId, expectedDiff)).ReturnsAsync(true);

            var service = SetupService(diffStore: storeMock.Object);

            // Act
            var actualResult = await service.SetRightDiffContent(expectedDiffContent);

            // Assert
            Assert.True(actualResult);
            storeMock.Verify(m => m.WriteAsync(expectedId, expectedDiff), Times.Once);
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
        public async Task ExecutingDiff_WithNonExistentDiffId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var expectedId = Guid.Parse("D3177120-A79A-43B9-9562-599973857A4B");
            var service = SetupService();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.ExecuteDiff(expectedId));
        }

        [Fact]
        public async Task ExecutingDiff_WithExistentDiffId_ShouldReturnValidDiffSummary()
        {
            // Arrange
            var expectedId = Guid.Parse("DA546E14-1ACC-424E-A794-C7CF7381ED53");
            var expectedDiff = SetupDiff(expectedId);
            var expectedSummaryMessage = "Expected message";
            var expectedDiffProcessorResult = DiffProcessorResult.Create(true, expectedSummaryMessage);

            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedDiff);

            var processorMock = new Mock<IDiffProcessor>();
            processorMock.Setup(m => m.ExecuteAsync(expectedDiff.Left, expectedDiff.Right))
                         .ReturnsAsync(expectedDiffProcessorResult);

            var service = SetupService(diffProcessor: processorMock.Object,
                                       diffStore: storeMock.Object);

            // Act & Assert
            var actualSummary = await service.ExecuteDiff(expectedId);

            // Assert
            Assert.NotNull(actualSummary);
            Assert.NotEmpty(actualSummary.Message);
            Assert.Equal(expectedSummaryMessage, actualSummary.Message);
            storeMock.Verify(m => m.ReadAsync(expectedId), Times.Once);
        }

        [Fact]
        public async Task ExecutingDiff_WithEmptyDiffId_ShouldThrowArgumentException()
        {
            // Arrange
            var service = SetupService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.ExecuteDiff(Guid.Empty));
        }

        [Fact]
        public async Task ExecutingDiffAsync_WithValidDiff_ShouldReturnValidDiffProcessorResult()
        {
            // Arrange
            var expectedId = Guid.Parse("CAEE5111-D6D9-4EA4-9FF9-3AFB65EAF258");
            var expectedDiff = SetupDiff(expectedId, "content", "content");
            var expectedProcessorResult = SetupProcessorResult(message: "Content match!");

            var processorMock = new Mock<IDiffProcessor>();
            processorMock.Setup(m => m.ExecuteAsync(expectedDiff.Left, expectedDiff.Right))
                         .ReturnsAsync(expectedProcessorResult);
            
            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedDiff);

            var service = SetupService(diffProcessor: processorMock.Object,
                                       diffStore: storeMock.Object);

            // Act
            var actualResult = await service.ExecuteDiff(expectedId);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(expectedProcessorResult.Message, actualResult.Message);
            storeMock.Verify(m => m.ReadAsync(expectedId), Times.Once);
            processorMock.Verify(m => m.ExecuteAsync(expectedDiff.Left, expectedDiff.Right), Times.Once);
        }

        [Fact]
        public async Task ExecutingDiffAsync_WithInvalidDiff_ShouldReturnInvalidDiffProcessorResult()
        {
            // Arrange
            var expectedId = Guid.Parse("C3802691-0BDF-4CC5-8CFF-F9C9034E4C1D");
            var expectedDiff = SetupDiff(expectedId, "different", "content");

            var expectedProcessorResult = SetupProcessorResult(match: false, message: "Content don't match!");
            var processorMock = new Mock<IDiffProcessor>();
            processorMock.Setup(m => m.ExecuteAsync(expectedDiff.Left, expectedDiff.Right))
                         .ReturnsAsync(expectedProcessorResult);

            var storeMock = new Mock<IStore<Diff>>();
            storeMock.Setup(m => m.ReadAsync(expectedId)).ReturnsAsync(expectedDiff);

            var service = SetupService(diffProcessor: processorMock.Object,
                                       diffStore: storeMock.Object);

            // Act
            var actualResult = await service.ExecuteDiff(expectedId);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(expectedProcessorResult.Message, actualResult.Message);
            storeMock.Verify(m => m.ReadAsync(expectedId), Times.Once);
            processorMock.Verify(m => m.ExecuteAsync(expectedDiff.Left, expectedDiff.Right), Times.Once);
        }

        private static DiffProcessorResult SetupProcessorResult(bool match = true, string message = null)
        {
            return DiffProcessorResult.Create(match, message);
        }

        private static IDiffAppService SetupService(IDiffFactory diffFactory = null,
                                                    IDiffProcessor diffProcessor = null,
                                                    IStore<Diff> diffStore = null)
        {
            return new DiffAppService(diffFactory ?? Mock.Of<IDiffFactory>(),
                                      diffProcessor ?? Mock.Of<IDiffProcessor>(),
                                      diffStore ?? Mock.Of<IStore<Diff>>(),
                                      SetupMapper());
        }

        private static Diff SetupDiff(Guid id, string leftContent = null, string rightContent = null)
        {
            var diff = new DiffFactory().Create(id);
            diff.SetLeftData(DiffData.Create(leftContent));
            diff.SetRightData(DiffData.Create(rightContent));

            return diff;
        }

        private static IMapper SetupMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DiffContentProfile());
                cfg.AddProfile(new DiffSummaryProfile());
            });
            return config.CreateMapper();
        }
    }
}
