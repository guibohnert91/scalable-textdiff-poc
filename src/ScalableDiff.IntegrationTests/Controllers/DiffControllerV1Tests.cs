using ScalableDiff.IntegrationTests.CustomOrderers;
using ScalableDiff.IntegrationTests.Fixtures;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.IntegrationTests.v1
{
    [Collection("Diff Api Collection")]
    [TestCaseOrderer("ScalableDiff.IntegrationTests.CustomOrderers.PriorityOrderer", "ScalableDiff.IntegrationTests")]
    public class DiffControllerV1Tests
    {
        private readonly DiffApiFixture fixture;
        private static Guid TestingId = Guid.Parse("201BBA35-6B11-4316-B07D-D479A2168609");

        public DiffControllerV1Tests(DiffApiFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task CreatingValidDiff_ShouldReturnOk()
        {
            // Arrange
            var expectedContent = JsonSerializer.Serialize(TestingId);
            var requestPayload = SetupContent(expectedContent);

            // Act
            var response = await fixture.Client.PostAsync($"/v1/Diff/", requestPayload);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(2)]
        public async Task Setting_LeftDiffContent_ShouldReturnOk()
        {
            // Arrange
            var expectedContent = JsonSerializer.Serialize("dGhpcyBpcyBteSBlbmNvZGVkIGRhdGE");
            var requestPayload = SetupContent(expectedContent);

            // Act
            var response = await fixture.Client.PostAsync($"/v1/Diff/{TestingId}/Left", requestPayload);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(3)]
        public async Task Setting_RightDiffContent_ShouldReturnOk()
        {
            // Arrange
            var expectedContent = JsonSerializer.Serialize("dGhpcyBpcyBteSBlbmNvZGVkIGRhdGEgZm9yIHRoZSByaWdodCBjb250ZW50");
            var requestPayload = SetupContent(expectedContent);

            // Act
            var response = await fixture.Client.PostAsync($"/v1/Diff/{TestingId}/Right", requestPayload);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(4)]
        public async Task ExecutingValidDiff_ShouldReturnOk()
        {
            // Arrange
            var expectedContent = JsonSerializer.Serialize("dGhpcyBpcyBteSBlbmNvZGVkIGRhdGEgZm9yIHRoZSByaWdodCBjb250ZW50");

            // Act
            var actualResponse = await fixture.Client.GetAsync($"/v1/Diff/{TestingId}");
            var actualResponseText = await actualResponse.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, actualResponse.StatusCode);
            Assert.NotNull(actualResponseText);
            Assert.NotEmpty(actualResponseText);
        }

        [Fact, TestPriority(5)]
        public async Task CreatingInvalidDiff_ShouldReturnBadRequest()
        {
            // Arrange
            var expectedContent = JsonSerializer.Serialize(Guid.Empty);
            var requestPayload = SetupContent(expectedContent);

            // Act
            var response = await fixture.Client.PostAsync($"/v1/Diff/", requestPayload);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(6)]
        public async Task Setting_InvalidRightDiff_ShouldReturnBadRequest()
        {
            // Act
            var response = await fixture.Client.PostAsync($"/v1/Diff/{Guid.Empty}/Right", SetupContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(7)]
        public async Task Setting_InvalidLeftDiff_ShouldReturnBadRequest()
        {
            // Act
            var response = await fixture.Client.PostAsync($"/v1/Diff/{Guid.Empty}/Right", SetupContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(8)]
        public async Task ExecutingInvalidDiff_ShouldReturnBadRequest()
        {
            // Act
            var invalidId = Guid.Parse("CE017581-3DF2-418E-B651-E5FFC03A5DF1");
            var actualResponse = await fixture.Client.GetAsync($"/v1/Diff/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, actualResponse.StatusCode);
        }

        private static HttpContent SetupContent(string data = null)
        {
            var stringContent = new StringContent(data ?? string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);

            return stringContent;
        }
    }
}
