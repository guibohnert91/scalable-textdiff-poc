using Microsoft.AspNetCore.Mvc.Testing;
using ScalableDiff.IntegrationTests.Factories;
using System.Net.Http;
using Xunit;

namespace ScalableDiff.IntegrationTests.v1
{
    public abstract class IntegrationTestBase : IClassFixture<ScalableDiffApiFactory>
    {
        private readonly ScalableDiffApiFactory factory;

        protected readonly HttpClient Client;

        public IntegrationTestBase(ScalableDiffApiFactory factory)
        {
            this.factory = factory;
            this.Client = factory.CreateClient();
        }        
    }
}
