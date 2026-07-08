using CSharpFunctionalExtensions;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Locations;

public interface ILocationRepository
{
    Task<Result<Guid>> AddAsync(Location location, CancellationToken cancellationToken);

    Task<Result<Location, Error>> GetByLocationNameAsync(
        LocationName locationName,
        CancellationToken cancellationToken = default);

    Task<Result<Location, Error>> GetByIdAsync(
        LocationId locationId,
        CancellationToken cancellationToken = default);

    Task<bool> DoesLocationNameExistExcludingIdAsync(
        LocationName locationName,
        LocationId excludedId,
        CancellationToken cancellationToken = default);
}