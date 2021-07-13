using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace ScalableDiff.Configuration
{
    /// <summary>
    /// Contains easy to access services extensions to register the swagger documentation.
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Register the swagger dependencies.
        /// </summary>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Scalable Diff Api",
                    Version = "v1",
                    Description = "Provides basic diff funcionality.",
                });

                // This is needed since swagger doesn't load the xml docs by default.
                // Also must be enabled to generate at compile time.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        /// <summary>
        /// Enables swagger within the api.
        /// </summary>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            return app;
        }
    }
}
