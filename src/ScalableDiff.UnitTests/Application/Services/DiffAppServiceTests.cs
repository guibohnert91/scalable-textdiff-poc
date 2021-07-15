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
        public async Task SettingLeftDiffContent_WithValidContent_ShouldUpdateDiffAndReturnTrueResult()
        {
            // Arrange
            var expectedId = Guid.Parse("D3177120-A79A-43B9-9562-599973857A4B");
            var expectedDiff = SetupDiff(expectedId);
            var expectedDiffContent = DiffContent.Create(expectedId, "left content");

            var diffServiceMock = new Mock<IDiffService>();
            diffServiceMock.Setup(m => m.GetAsync(expectedId)).ReturnsAsync(expectedDiff);

            var service = SetupService(diffService: diffServiceMock.Object);

            // Act
            var actualResult = await service.SetLeftDiffContent(expectedDiffContent);

            // Assert
            Assert.True(actualResult);
        }

        [Fact]
        public async Task SettingRightDiffContent_WithValidContent_ShouldUpdateDiffAndReturnTrueResult()
        {
            // Arrange
            var expectedId = Guid.Parse("D3177120-A79A-43B9-9562-599973857A4B");
            var expectedDiff = SetupDiff(expectedId);
            var expectedDiffContent = DiffContent.Create(expectedId, "right content");

            var diffServiceMock = new Mock<IDiffService>();
            diffServiceMock.Setup(m => m.GetAsync(expectedId)).ReturnsAsync(expectedDiff);

            var service = SetupService(diffService: diffServiceMock.Object);

            // Act
            var actualResult = await service.SetRightDiffContent(expectedDiffContent);

            // Assert
            Assert.True(actualResult);
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
            var expectedId = Guid.Parse("D3177120-A79A-43B9-9562-599973857A4B");
            var expectedDiff = SetupDiff(expectedId);
            var expectedSummaryMessage = "Expected message";
            var expectedDiffProcessorResult = DiffProcessorResult.Create(true, expectedSummaryMessage);

            var diffServiceMock = new Mock<IDiffService>();
            diffServiceMock.Setup(m => m.ExecuteAsync(expectedDiff)).ReturnsAsync(expectedDiffProcessorResult);
            diffServiceMock.Setup(m => m.GetAsync(expectedId)).ReturnsAsync(expectedDiff);

            var service = SetupService(diffService: diffServiceMock.Object);

            // Act & Assert
            var actualSummary = await service.ExecuteDiff(expectedId);

            // Assert
            Assert.NotNull(actualSummary);
            Assert.NotEmpty(actualSummary.Message);
            Assert.Equal(expectedSummaryMessage, actualSummary.Message);
        }

        [Fact]
        public async Task ExecutingDiff_WithEmptyDiffId_ShouldThrowArgumentException()
        {
            // Arrange
            var service = SetupService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.ExecuteDiff(Guid.Empty));
        }

        private static IDiffAppService SetupService(IDiffFactory diffFactory = null,
                                                    IDiffService diffService = null,
                                                    IStore<Diff> diffStore = null)
        {
            return new DiffAppService(diffService ?? Mock.Of<IDiffService>(),
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
