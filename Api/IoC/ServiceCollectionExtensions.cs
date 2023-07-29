namespace Api.CoronaVirusStatistics.IoC
{
    using Api.CoronaVirusStatistics.Application.DTO;
    using Api.CoronaVirusStatistics.Application.Mapping;
    using Api.CoronaVirusStatistics.Application.Services;
    using Api.CoronaVirusStatistics.Application.Validation;
    using Api.CoronaVirusStatistics.Domain.Repositories;
    using Api.CoronaVirusStatistics.Domain.Settings;
    using Api.CoronaVirusStatistics.InfraStructure.Context;
    using Api.CoronaVirusStatistics.InfraStructure.Repositories;
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using System;
    using System.IO;

    public static class ServiceCollectionExtensions
    {
        private static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("config.json", false, true);
            return builder;
        }

        private static WebApplicationBuilder AddKestrelConfiguration(this WebApplicationBuilder builder)
        {
            builder.WebHost.UseKestrel(options => {
                options.Limits.MaxConcurrentConnections = 100;
                options.Limits.MaxConcurrentUpgradedConnections = 100;
                options.Limits.MaxRequestBodySize = 52428800;
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
                options.AddServerHeader = false;
            });
            return builder;
        }

        private static WebApplicationBuilder AddLoggerConfigurations(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: Path.Combine(Directory.GetCurrentDirectory(), "logs", "webapi-.log"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} {Level:u3}] {Username} {Message:lj}{NewLine}{Exception}",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();
            builder.Logging.AddSerilog(logger);
            return builder;
        }

        private static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            var settings = new MongoDBSettings(connectionURI: builder.Configuration["mongodb:uri"],
                                               databaseName: builder.Configuration["mongodb:database"],
                                               port: Convert.ToInt32(builder.Configuration["mongodb:port"]),
                                               userName: builder.Configuration["mongodb:username"],
                                               password: builder.Configuration["mongodb:password"],
                                               useAtlas: Convert.ToBoolean(builder.Configuration["mongodb:useAtlas"]));
            builder.Services.AddSingleton(settings);
            builder.Services.AddSingleton<ApplicationDBContext>();
            return builder;
        }

        private static WebApplicationBuilder AddJsonOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.IncludeFields = true;
                options.SerializerOptions.WriteIndented = true;
            });
            return builder;
        }

        private static WebApplicationBuilder AddRepositoriesMapping(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IInfectadoRepository, InfectadoRepository>();
            return builder;
        }

        private static WebApplicationBuilder AddServicesMapping(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IInfectadoService, InfectadoService>();
            return builder;
        }

        private static WebApplicationBuilder AddValidationsMapping(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IValidator<InfectadoDto>, InfectadoValidation>();
            return builder;
        }

        private static WebApplicationBuilder AddExtensionsMapping(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(DomainToDtoMappingProfile));
            builder.Services.AddCors();
            builder.Services.AddAuthorization();
            return builder;
        }

        public static WebApplicationBuilder AddInfraStructure(this WebApplicationBuilder builder)
        {
            //configuration
            builder.AddConfigurations();
            //kestrel configuration
            builder.AddKestrelConfiguration();
            //database configuration
            builder.AddPersistence();
            //json configuration
            builder.AddJsonOptions();
            //repositories configuration
            builder.AddRepositoriesMapping();
            //services configuration
            builder.AddServicesMapping();
            //validations configuration
            builder.AddValidationsMapping();
            //extensions configuration
            builder.AddExtensionsMapping();
            //logger configuration
            builder.AddLoggerConfigurations();
            return builder;
        }
    }
}
