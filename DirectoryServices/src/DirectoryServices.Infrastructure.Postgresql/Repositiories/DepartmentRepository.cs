using CSharpFunctionalExtensions;
using DirectoryServices.Application.Managements.Departments;
using DirectoryServices.Domain.DepartmentManagement.Aggregate;
using DirectoryServices.Domain.DepartmentManagement.Id;
using DirectoryServices.Domain.DepartmentManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DirectoryServices.Infrastructure.Repositiories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly DirectoryServiceDbContext _dbContext;

    public DepartmentRepository(DirectoryServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> AddAsync(Department department, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Departments.AddAsync(department, cancellationToken);
            return department.Id.Value;
        }
        catch (Exception e)
        {
            return Result.Failure<Guid>(e.Message);
        }
    }

    public async Task<Result<Department, Error>> GetByIdentifierAsync(
        Identifier identifier,
        CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments
            .FirstOrDefaultAsync(d => d.Identifier.Value == identifier.Value, cancellationToken);

        if (department is null)
        {
            return Error.NotFound();
        }

        return department;
    }

    public async Task<Result<Department, Error>> GetByIdAsync(
        DepartmentId departmentId,
        CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

        if (department is null)
        {
            return GeneralErrors.NotFound();
        }

        return department;
    }

    public async Task<bool> LocationsExistAsync(
        IEnumerable<Guid> locationIds,
        CancellationToken cancellationToken = default)
    {
        var locationIdsList = locationIds.ToList();

        if (!locationIdsList.Any())
        {
            return false;
        }

        var existingCount = await _dbContext.Locations
            .Where(l => locationIdsList.Contains(l.Id))
            .Select(l => l.Id)
            .Distinct()
            .CountAsync(cancellationToken);

        return existingCount == locationIdsList.Distinct().Count();
    }
}