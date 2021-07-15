using ScalableDiff.Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScalableDiff.UnitTests.Domain.Factories
{
    public class DiffFactoryTests
    {
        [Fact]
        public void CreatingDiff_WithValidId_ReturnsCreatedDiffWithValidIdAndEmptyData()
        {
            // Arrange
            var factory = SetupFactory();
            var expectedId = Guid.Parse("F7AC7780-4D05-4F0E-8ED4-A712DF73130D");

            // Act
            var actualDiff = factory.Create(expectedId);

            // Assert
            Assert.Equal(expectedId, actualDiff.Id);
            Assert.Empty(actualDiff.Left.Content);
            Assert.Empty(actualDiff.Right.Content);
        }

        [Fact]
        public void CreatingDiff_WithEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var factory = SetupFactory();
            var invalidId = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => factory.Create(invalidId));
        }

        private static IDiffFactory SetupFactory()
        {
            return new DiffFactory();
        }
    }
}
