using CSharpFunctionalExtensions;
using DirectoryServices.Application;
using DirectoryServices.Application.Managements.Positions;
using DirectoryServices.Domain.DepartmentManagement.Id;
using DirectoryServices.Domain.PositionManagement.Aggregate;
using DirectoryServices.Domain.PositionManagement.Id;
using DirectoryServices.Domain.PositionManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DirectoryServices.Infrastructure.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public PositionRepository(
        DirectoryServiceDbContext dbContext,
        IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> AddAsync(Position position, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Positions.AddAsync(position, cancellationToken);
            return position.Id.Value;
        }
        catch (Exception e)
        {
            return Result.Failure<Guid>(e.Message);
        }
    }

    public async Task<bool> DoAllDepartmentsExistAndActiveAsync(
        IEnumerable<DepartmentId> departmentIds,
        CancellationToken cancellationToken = default)
    {
        var departmentIdsList = departmentIds.ToList();

        if (departmentIdsList.Count == 0)
        {
            return false;
        }

        var existingActiveCount = await _dbContext.Departments
            .Where(d => departmentIdsList.Contains(d.Id) && d.IsActive)
            .Select(d => d.Id)
            .Distinct()
            .CountAsync(cancellationToken);

        return existingActiveCount == departmentIdsList.Distinct().Count();
    }

    public async Task<Result<Guid, Error>> AddPositionToDepartments(
        PositionId positionId,
        IEnumerable<Guid> departmentIds,
        CancellationToken cancellationToken = default)
    {
        var departmentIdsList = departmentIds.ToList();

        if (departmentIdsList.Count == 0)
        {
            return GeneralErrors.NotFound();
        }

        var departments = await _dbContext.Departments
            .Include(d => d.DepartmentPositions)
            .Where(d => departmentIdsList.Contains(d.Id))
            .ToListAsync(cancellationToken);

        if (departments.Count != departmentIdsList.Count)
        {
            return GeneralErrors.NotFound(
                null,
                $"Only {departments.Count} out of {departmentIdsList.Count} departments were found.");
        }

        foreach (var department in departments)
        {
            var result = department.AddPosition(positionId);

            if (result.IsFailure)
            {
                return result.Error;
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return positionId.Value;
    }

    public async Task<bool> ExistsActivePositionWithNameAsync(
        PositionName positionName,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Positions
            .AnyAsync(p => p.PositionName == positionName && p.IsActive, cancellationToken);
    }

    public async Task<Result<Guid, Error>> DeleteAsync(
        PositionId positionId,
        CancellationToken cancellationToken = default)
    {
        var position = await _dbContext.Positions
            .FirstOrDefaultAsync(p => p.Id == positionId, cancellationToken);

        if (position == null)
        {
            return GeneralErrors.NotFound(positionId.Value, "Position");
        }

        _dbContext.Positions.Remove(position);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return positionId.Value;
    }
}