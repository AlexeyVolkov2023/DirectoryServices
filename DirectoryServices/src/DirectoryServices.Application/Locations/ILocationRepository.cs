using CSharpFunctionalExtensions;
using DirectoryServices.Domain.LocationManagement.Aggregate;

namespace DirectoryServices.Application.Locations;

public interface ILocationRepository
{
    Task<Result<Guid>> AddAsync(Location location, CancellationToken cancellationToken);
}