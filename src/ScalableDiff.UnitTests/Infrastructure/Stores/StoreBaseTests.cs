using Moq;
using ScalableDiff.Domain.Stores;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.UnitTests.Infrastructure.Stores
{
    public abstract class StoreBaseTests<TData> where TData : class
    {
        [Fact]
        public async Task Writing_WithValidIdAndData_ShouldReturnSuccededResult()
        {
            // Arrange
            var store = SetupStore();
            var expectedId = Guid.Parse("E0BF5C98-E3F5-4B5B-B4E0-8CF87C060658");
            var expectedData = Mock.Of<TData>();

            // Act
            var result = await store.WriteAsync(expectedId, expectedData);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Writing_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var store = SetupStore();
            var expectedId = Guid.Empty;
            var expectedData = Mock.Of<TData>();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => store.WriteAsync(expectedId, expectedData));
        }

        [Fact]
        public async Task Writing_WithInvalidData_ShouldThrowArgumentNullException()
        {
            // Arrange
            var store = SetupStore();
            var expectedId = Guid.Parse("55538B3E-B751-4D97-B3B9-B73ED5C2F5FC");
            TData expectedData = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => store.WriteAsync(expectedId, expectedData));
        }

        [Fact]
        public async Task Reading_WithValidId_ShouldReturnDiffSession()
        {
            // Arrange
            var store = SetupStore();
            var expectedId = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6");
            var expectedData = Mock.Of<TData>();

            // Act
            await store.WriteAsync(expectedId, expectedData);
            var actualData = await store.ReadAsync(expectedId);

            // Assert
            Assert.NotNull(actualData);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task Reading_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var store = SetupStore();
            var expectedId = Guid.Empty;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => store.ReadAsync(expectedId));
        }

        [Fact]
        public async Task Reading_WithNotFoundId_ShouldReturnNull()
        {
            // Arrange
            var store = SetupStore();
            var expectedId = Guid.Parse("CF9984F0-5E5E-4051-9737-33A633987645");
            var invalidId = Guid.Parse("A273071F-9DEF-4691-B528-3082EA798431");
            var expectedData = Mock.Of<TData>();

            // Act
            await store.WriteAsync(expectedId, expectedData);
            var actualData = await store.ReadAsync(invalidId);

            // Assert
            Assert.Null(actualData);
        }

        protected abstract IStore<TData> SetupStore();
    }
}
