using DirectoryServices.Application;
using DirectoryServices.Application.Managements.Departments;
using DirectoryServices.Application.Managements.Locations;
using DirectoryServices.Application.Managements.Positions;
using DirectoryServices.Infrastructure.Repositiories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddScoped<ILocationRepository, LocationRepository>();
        serviceCollection.AddScoped<IPositionRepository, PositionRepository>();
        serviceCollection.AddScoped<IDepartmentRepository, DepartmentRepository>();
        serviceCollection.AddScoped<DirectoryServiceDbContext>(_ =>
            new DirectoryServiceDbContext(configuration.GetConnectionString("DirectoryServiceDb")!));
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        return serviceCollection;
    }
}