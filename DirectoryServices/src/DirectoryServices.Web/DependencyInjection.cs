using DirectoryServices.Application;
using DirectoryServices.Infrastructure;
using Serilog;
using Serilog.Exceptions;

namespace DirectoryServices.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection
            .AddApplicationDependencies()
            .AddInfrastructureDependencies(configuration)
            .AddWebDependencies(configuration);

        return serviceCollection;
    }

    private static IServiceCollection AddWebDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddControllers();
        serviceCollection.AddOpenApi();
        serviceCollection.AddSerilogLogger(configuration);

        return serviceCollection;
    }

    private static IServiceCollection AddSerilogLogger(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ServiceName", "DirectoryServices"));

        return serviceCollection;
    }
}