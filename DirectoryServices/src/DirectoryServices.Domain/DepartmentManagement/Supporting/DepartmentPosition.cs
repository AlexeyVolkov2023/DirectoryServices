using DirectoryServices.Domain.DepartmentManagement.Aggregate;
using DirectoryServices.Domain.PositionManagement.Id;

namespace DirectoryServices.Domain.DepartmentManagement.Supporting;

public record DepartmentPosition
{
    public DepartmentPosition()
    {
    }

    public DepartmentPosition(DepartmentPositionId departmentPositionId, Department department, PositionId positionId)
    {
        DepartmentPositionId = departmentPositionId;
        Department = department;
        PositionId = positionId;
        CreatedAt = DateTime.UtcNow;
    }

    public DepartmentPositionId DepartmentPositionId { get; }

    public Department Department { get; }

    public PositionId PositionId { get; }

    public DateTime CreatedAt { get; }
}