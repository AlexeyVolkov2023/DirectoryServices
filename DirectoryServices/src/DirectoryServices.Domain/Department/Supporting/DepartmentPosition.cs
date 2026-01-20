using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Department.Id;
using DirectoryServices.Domain.PositionManagement.Id;

namespace DirectoryServices.Domain.Department.Supporting;

public class DepartmentPosition
{
    private DepartmentPosition(DepartmentId departmentId, PositionId positionId)
    {
        Id = Guid.NewGuid();
        DepartmentId = departmentId;
        PositionId = positionId;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public DepartmentId DepartmentId { get; }

    public PositionId PositionId { get; }

    public DateTime CreatedAt { get; }

    public static Result<DepartmentPosition> Create(DepartmentId departmentId, PositionId positionId)
    {
        return Result.Success(new DepartmentPosition(departmentId, positionId));
    }
}