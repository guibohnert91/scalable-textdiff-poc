using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ScalableDiff.IntegrationTests.Factories
{
    public class ScalableDiffApiFactory : WebApplicationFactory<Startup>
	{
		protected override IWebHostBuilder CreateWebHostBuilder()
		{
			return WebHost.CreateDefaultBuilder()
					      .UseStartup<Startup>();
		}
	}
}
