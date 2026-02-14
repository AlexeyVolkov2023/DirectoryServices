using DirectoryServices.Domain.DepartmentManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.Id;

namespace DirectoryServices.Domain.DepartmentManagement.Supporting;

public record DepartmentLocation
{
    public DepartmentLocation()
    {
    }

    public DepartmentLocation(
        DepartmentLocationId departmentLocationId,
        Department department,
        LocationId locationId)
    {
        DepartmentLocationId = departmentLocationId;
        Department = department;
        LocationId = locationId;
        CreatedAt = DateTime.UtcNow;
    }

    public DepartmentLocationId DepartmentLocationId { get; }

    public Department Department { get; private set; }

    public LocationId LocationId { get; }

    public DateTime CreatedAt { get; }
}