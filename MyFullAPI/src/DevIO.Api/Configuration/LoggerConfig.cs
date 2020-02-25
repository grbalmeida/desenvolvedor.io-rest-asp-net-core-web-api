using DevIO.Api.Extensions;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(configureOptions =>
            {
                configureOptions.ApiKey = "API_KEY";
                configureOptions.LogId = new Guid("LOG_ID");
            });

            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "API_KEY";
            //        o.LogId = new Guid("LOG_ID");
            //    });

            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning); // Will not log trace, debug and information
            //});

            services.AddHealthChecks()
                .AddElmahIoPublisher("API_KEY", new Guid("LOG_ID"), "API Suppliers")
                .AddCheck("Products", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection"), "products"))
                .AddCheck("Suppliers", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection"), "suppliers"))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "SQLDatabase");

            services.AddHealthChecksUI();

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            app.UseHealthChecks("/api/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/api/hc-ui";
            });

            return app;
        }
    }
}