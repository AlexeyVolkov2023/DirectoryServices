using CSharpFunctionalExtensions;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Locations;

public interface ILocationRepository
{
    Task<Result<Guid>> AddAsync(Location location, CancellationToken cancellationToken);

    Task<Result<Location, Error>> GetByLocationNameAsync(
        LocationName locationName,
        CancellationToken cancellationToken = default);
}