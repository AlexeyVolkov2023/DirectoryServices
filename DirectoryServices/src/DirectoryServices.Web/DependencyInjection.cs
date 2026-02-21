using DirectoryServices.Application;
using DirectoryServices.Infrastructure;

namespace DirectoryServices.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddApplicationDependencies()
            .AddInfrastructureDependencies(configuration)
            .AddWebDependencies();

        return serviceCollection;
    }

    private static IServiceCollection AddWebDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddControllers();
        serviceCollection.AddOpenApi();

        return serviceCollection;
    }
}