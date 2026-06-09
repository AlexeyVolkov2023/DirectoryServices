using CSharpFunctionalExtensions;
using DirectoryServices.Domain.DepartmentManagement.Id;
using DirectoryServices.Domain.PositionManagement.Aggregate;
using DirectoryServices.Domain.PositionManagement.Id;
using DirectoryServices.Domain.PositionManagement.ValueObjects;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Positions;

public interface IPositionRepository
{
    Task<Result<Guid>> AddAsync(Position position, CancellationToken cancellationToken);

    Task<bool> DoAllDepartmentsExistAndActiveAsync(
        IEnumerable<DepartmentId> departmentIds,
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> AddPositionToDepartments(
        PositionId positionId,
        IEnumerable<Guid> departmentIds,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsActivePositionWithNameAsync(
        PositionName positionName,
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> DeleteAsync(
        PositionId positionId,
        CancellationToken cancellationToken = default);
}