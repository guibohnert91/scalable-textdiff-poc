using Xunit;

namespace ScalableDiff.IntegrationTests.Fixtures
{
    [CollectionDefinition("Diff Api Collection")]
    public class DiffApiCollection : ICollectionFixture<DiffApiFixture> { }
}
