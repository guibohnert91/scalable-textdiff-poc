using ScalableDiff.IntegrationTests.Factories;
using System;
using System.Net.Http;

namespace ScalableDiff.IntegrationTests.Fixtures
{
    public class DiffApiFixture : IDisposable
    {
        private readonly ScalableDiffApiFactory factory;
        public readonly HttpClient Client;

        public DiffApiFixture()
        {
            factory = new ScalableDiffApiFactory();
            this.Client = factory.CreateClient();
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }
    }
}
