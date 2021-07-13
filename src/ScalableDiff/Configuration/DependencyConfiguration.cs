using Microsoft.Extensions.DependencyInjection;
using ScalableDiff.Application.Profiles;
using ScalableDiff.Application.Services;
using ScalableDiff.Domain;
using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Processors;
using ScalableDiff.Domain.Stores;
using ScalableDiff.Infrastructure.Stores;

namespace ScalableDiff.Configuration
{
    public static class DependencyConfiguration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DiffSummaryProfile));
            services.AddScoped<IDiffAppService, DiffAppService>();
        }

        public static void AddDomain(this IServiceCollection services)
        {
            services.AddScoped<IDiffService, DiffService>();
            services.AddScoped<IDiffAppService, DiffAppService>();

            //TODO Register chain and another iocs here in a better manner.
            services.AddScoped<IDiffProcessor>((provider) =>
            {
                return new EqualityCheckDiffProcessor(new DifferentSizeCheckDiffProcessor(new OffsetDiffProcessor()));
            });
        }

        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IStore<Diff>), typeof(DiffStore));

        }
    }
}
