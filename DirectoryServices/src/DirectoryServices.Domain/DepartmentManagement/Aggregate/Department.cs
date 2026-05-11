using CSharpFunctionalExtensions;
using DirectoryServices.Domain.DepartmentManagement.Id;
using DirectoryServices.Domain.DepartmentManagement.Supporting;
using DirectoryServices.Domain.DepartmentManagement.ValueObjects;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.PositionManagement.Id;
using DirectoryServices.Domain.Shared;
using Path = DirectoryServices.Domain.DepartmentManagement.ValueObjects.Path;

namespace DirectoryServices.Domain.DepartmentManagement.Aggregate;

public class Department
{
    private readonly List<Department> _children = [];

    private readonly List<DepartmentLocation> _departmentLocations = [];

    private readonly List<DepartmentPosition> _departmentPositions = [];

    public Department()
    {
    }

    private Department(
        DepartmentName departmentName,
        Identifier identifier,
        Path path,
        Department? parent,
        IEnumerable<Guid> locationIds)
    {
        Id = DepartmentId.NewDepartmentId();
        DepartmentName = departmentName;
        Identifier = identifier;
        Path = path;
        Parent = parent;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        if (parent == null)
        {
            Depth = Depth.RootDepth();
        }
        else
        {
            var calculatedDepthResult = parent.Depth.Increment();

            Depth = calculatedDepthResult.Value;
        }


        var locations = locationIds.Select(locationId =>
                new DepartmentLocation(
                    DepartmentLocationId.NewDepartmentLocationId(),
                    this,
                    LocationId.Create(locationId)))
            .ToList();

        _departmentLocations = locations;

        _departmentPositions = new List<DepartmentPosition>();
    }

    public DepartmentId Id { get; private set; }

    public DepartmentName DepartmentName { get; private set; }

    public Identifier Identifier { get; private set; }

    public Path Path { get; private set; }

    public Depth Depth { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Department? Parent { get; private set; }

    public IReadOnlyList<Department> Children => _children;

    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;

    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;

    public static Result<Department, Error> Create(
        DepartmentName departmentName,
        Identifier identifier,
        Department? parent,
        IEnumerable<Guid> locationIds)
    {
        var path = BuildPath(parent, identifier).Value;

        return new Department(
            departmentName,
            identifier,
            path,
            parent,
            locationIds);
    }

    private static Result<Path, Error> BuildPath(Department? parent, Identifier identifier)
    {
        string pathStr = parent?.Path.Value ?? string.Empty;
        if (!string.IsNullOrEmpty(pathStr))
            pathStr += ".";

        pathStr += identifier.Value;

        return Path.Create(pathStr);
    }

    public Result<Guid, Error> AddPosition(PositionId positionId)
    {
        if (_departmentPositions.Exists(dp => dp.PositionId == positionId))
        {
            return GeneralErrors.Failure(
                $"Position {positionId.Value} is already assigned to this department.");
        }

        var departmentPositionToAdded = new DepartmentPosition(
            DepartmentPositionId.NewDepartmentPositionId(),
            this,
            positionId);

        _departmentPositions.Add(departmentPositionToAdded);

        UpdatedAt = DateTime.UtcNow;

        return positionId.Value;
    }
}