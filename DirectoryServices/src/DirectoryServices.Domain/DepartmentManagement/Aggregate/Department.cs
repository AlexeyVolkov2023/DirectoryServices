using CSharpFunctionalExtensions;
using DirectoryServices.Domain.DepartmentManagement.Id;
using DirectoryServices.Domain.DepartmentManagement.Supporting;
using DirectoryServices.Domain.DepartmentManagement.ValueObjects;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.PositionManagement.Id;
using Path = DirectoryServices.Domain.DepartmentManagement.ValueObjects.Path;

namespace DirectoryServices.Domain.DepartmentManagement.Aggregate;

public class Department
{
    private readonly List<Department> _children = [];

    private readonly List<DepartmentLocation> _departmentLocations;

    private readonly List<DepartmentPosition> _departmentPositions;

    public Department()
    {
    }

    private Department(
        DepartmentName departmentName,
        Identifier identifier,
        Path path,
        Department? parent,
        Depth depth,
        bool isActive,
        IEnumerable<Guid> locationIds,
        IEnumerable<Guid> positionIds)
    {
        Id = DepartmentId.NewDepartmentId();
        DepartmentName = departmentName;
        Identifier = identifier;
        Path = path;
        Parent = parent;
        Depth = depth;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        var locations = locationIds.Select(locationId =>
            new DepartmentLocation(
                DepartmentLocationId.NewDepartmentLocationId(),
                this,
                LocationId.Create(locationId)))
            .ToList();

        var positions = positionIds.Select(positionId =>
            new DepartmentPosition(
                DepartmentPositionId.NewDepartmentPositionId(),
                this,
                PositionId.Create(positionId)))
            .ToList();

        _departmentLocations = locations;

        _departmentPositions = positions;
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


    public static Result<Department> Create(
        DepartmentName departmentName,
        Identifier identifier,
        Department? parent,
        Depth depth,
        bool isActive,
        IEnumerable<Guid> locationIds,
        IEnumerable<Guid> positionIds)
    {
        var path = BuildPath(parent, identifier).Value;

        return new Department(
            departmentName,
            identifier,
            path,
            parent,
            depth,
            isActive,
            locationIds,
            positionIds);
    }

    private static Result<Path> BuildPath(Department? parent, Identifier identifier)
    {
        string pathStr = parent?.Path.Value ?? string.Empty;
        if (!string.IsNullOrEmpty(pathStr))
            pathStr += ".";

        pathStr += identifier.Value;

        return Path.Create(pathStr);
    }

    public void UpdateName(DepartmentName newName)
    {
        DepartmentName = newName;
        UpdatedAt = DateTime.UtcNow;
    }


    public Result UpdateIdentifier(Identifier newIdentifier)
    {
        var pathResult = BuildPath(Parent, newIdentifier);
        if (pathResult.IsFailure)
            return pathResult;

        Identifier = newIdentifier;
        Path = pathResult.Value;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void UpdateDepth(Depth newDepth)
    {
        Depth = newDepth;
        UpdatedAt = DateTime.UtcNow;
    }

}