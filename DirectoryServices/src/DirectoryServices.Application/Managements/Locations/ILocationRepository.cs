using CSharpFunctionalExtensions;
using DirectoryServices.Domain.LocationManagement.Aggregate;

namespace DirectoryServices.Application.Managements.Locations;

public interface ILocationRepository
{
    Task<Result<Guid>> AddAsync(Location location, CancellationToken cancellationToken);
}