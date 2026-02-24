using DirectoryServices.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryServices.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection serviceCollection)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        serviceCollection.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return serviceCollection;
    }
}