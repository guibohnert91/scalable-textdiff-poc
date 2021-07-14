using ScalableDiff.IntegrationTests.Factories;
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
    public class DiffControllerTests : IntegrationTestBase
    {
        private static Guid TestingId = Guid.Parse("201BBA35-6B11-4316-B07D-D479A2168609");

        public DiffControllerTests(ScalableDiffApiFactory factory) : base(factory) { }

        [Fact]
        public async Task Setting_LeftDiffContent_ShouldReturnOk()
        {   
            // Arrange
            var expectedContent = JsonSerializer.Serialize("dGhpcyBpcyBteSBlbmNvZGVkIGRhdGE");
            var requestPayload = SetupContent(expectedContent);

            // Act
            var response = await Client.PostAsync($"/v1/Diff/{TestingId}/Left", requestPayload);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Setting_InvalidLeftDiff_ShouldReturnBadRequest()
        {
            // Act
            var response = await Client.PostAsync($"/v1/Diff/{Guid.Empty}/Right", SetupContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Setting_RightDiffContent_ShouldReturnOk()
        {
            // Arrange
            var expectedContent = JsonSerializer.Serialize("dGhpcyBpcyBteSBlbmNvZGVkIGRhdGEgZm9yIHRoZSByaWdodCBjb250ZW50");
            var requestPayload = SetupContent(expectedContent);

            // Act
            var response = await Client.PostAsync($"/v1/Diff/{TestingId}/Right", requestPayload);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Setting_InvalidRightDiff_ShouldReturnBadRequest()
        {
            // Act
            var response = await Client.PostAsync($"/v1/Diff/{Guid.Empty}/Right", SetupContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ExecutingValidDiff_ShouldReturnOk()
        {
            // Arrange
            var expectedContent = JsonSerializer.Serialize("dGhpcyBpcyBteSBlbmNvZGVkIGRhdGEgZm9yIHRoZSByaWdodCBjb250ZW50");
            var requestPayload = SetupContent(expectedContent);

            // Act
            await Client.PostAsync($"/v1/Diff/{TestingId}/Right", requestPayload);
            var actualResponse = await Client.GetAsync($"/v1/Diff/{TestingId}");
            
            var actualResponseText = await actualResponse.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, actualResponse.StatusCode);
            Assert.NotNull(actualResponseText);
            Assert.NotEmpty(actualResponseText);            
        }

        [Fact]
        public async Task ExecutingInvalidDiff_ShouldReturnNotFound()
        {
            // Act
            var actualResponse = await Client.GetAsync($"/v1/Diff/CE017581-3DF2-418E-B651-E5FFC03A5DF1");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, actualResponse.StatusCode);
        }

        private static HttpContent SetupContent(string data = null)
        {
            var stringContent = new StringContent(data ?? string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);

            return stringContent;
        }
    }
}
