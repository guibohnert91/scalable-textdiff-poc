using Microsoft.Extensions.DependencyInjection;
using ScalableDiff.Application.Profiles;
using ScalableDiff.Application.Services;
using ScalableDiff.Domain;
using ScalableDiff.Domain.Factories;
using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Processors;
using ScalableDiff.Domain.Stores;
using ScalableDiff.Infrastructure.Stores;

namespace ScalableDiff.Configuration
{
    /// <summary>
    /// Contains easy to access services extensions to register dependencies.
    /// </summary>
    public static class DependencyConfiguration
    {
        /// <summary>
        /// Adds the default application dependencies.
        /// </summary>
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DiffSummaryProfile));
            services.AddScoped<IDiffAppService, DiffAppService>();
        }

        /// <summary>
        /// Adds the default domain dependencies.
        /// </summary>
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddScoped<IDiffFactory, DiffFactory>();
            services.AddScoped<IDiffAppService, DiffAppService>();

            /*
             * There are better elegant ways to register 
             * a chain of responsability and it's dependencies.
             * 
             * I'll take as not needed for this poc.
             *
             * Could have used "scrutor" or another DI container like Autofac, 
             * just keeping it simple for now.
            */
            services.AddScoped<IDiffProcessor>((provider) =>
            {
                return new EqualityCheckDiffProcessor(new DifferentSizeCheckDiffProcessor(
                                                      new OffsetDiffProcessor()));
            });
        }

        /// <summary>
        /// Adds the default infrastructure dependencies.
        /// </summary>
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IStore<Diff>), typeof(DiffStore));
        }
    }
}
