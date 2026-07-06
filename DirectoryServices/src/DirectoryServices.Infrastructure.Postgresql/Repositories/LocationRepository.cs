using CSharpFunctionalExtensions;
using DirectoryServices.Application.Managements.Locations;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DirectoryServices.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly DirectoryServiceDbContext _dbContext;

    public LocationRepository(DirectoryServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> AddAsync(Location location, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Locations.AddAsync(location, cancellationToken);
            return location.Id.Value;
        }
        catch (Exception e)
        {
            return Result.Failure<Guid>(e.Message);
        }
    }

    public async Task<Result<LocationName, Error>> GetByLocationNameAsync(
        LocationName locationName,
        CancellationToken cancellationToken = default)
    {
        var location = await _dbContext.Locations
            .FirstOrDefaultAsync(l => l.LocationName.Value == locationName.Value, cancellationToken);

        if (location is null)
        {
            return Error.NotFound();
        }

        return locationName;
    }

    public async Task<Result<Location, Error>> GetByIdAsync(
        LocationId locationId,
        CancellationToken cancellationToken = default)
    {
        var location = await _dbContext.Locations
            .FirstOrDefaultAsync(l => l.Id == locationId && l.IsActive, cancellationToken);

        if (location is null)
        {
            return GeneralErrors.NotFound();
        }

        return location;
    }

    public async Task<bool> DoesLocationNameExistExcludingIdAsync(
        LocationName locationName,
        LocationId excludedId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Locations
            .AnyAsync(
                l =>
                    l.LocationName == locationName
                    && l.Id != excludedId
                    && l.IsActive,
                cancellationToken);
    }
}