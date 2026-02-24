using DirectoryServices.Domain.LocationManagement.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace DirectoryServices.Application.Database;

public interface IDirectoryServiceDbContext
{
    DbSet<Location> Locations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}