using CSharpFunctionalExtensions;
using DirectoryServices.Application.Managements.Locations;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DirectoryServices.Infrastructure.Repositiories;

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
    
    public async Task<Result<Location, Error>> GetByLocationNameAsync(
        LocationName locationName,
        CancellationToken cancellationToken = default)
    {
        var location = await _dbContext.Locations
            .FirstOrDefaultAsync(l => l.LocationName.Value == locationName.Value, cancellationToken);

        if (location is null)
        {
            return Error.NotFound();
        }

        return location;
    }
}