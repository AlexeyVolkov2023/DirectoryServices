using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Department.Id;
using DirectoryServices.Domain.LocationManagement.Id;

namespace DirectoryServices.Domain.Department.Supporting;

public class DepartmentLocation
{
    private DepartmentLocation(DepartmentId departmentId, LocationId locationId)
    {
        Id = Guid.NewGuid();
        DepartmentId = departmentId;
        LocationId = locationId;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public DepartmentId DepartmentId { get; }

    public LocationId LocationId { get; }

    public DateTime CreatedAt { get; }

    public static Result<DepartmentLocation> Create(DepartmentId departmentId, LocationId locationId)
    {
        return Result.Success(new DepartmentLocation(departmentId, locationId));
    }
}