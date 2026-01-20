using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Department.Id;
using DirectoryServices.Domain.Department.Supporting;
using DirectoryServices.Domain.Department.ValueObjects;
using Path = DirectoryServices.Domain.Department.ValueObjects.Path;

namespace DirectoryServices.Domain.Department.Aggregate;

public class Department
{
    private readonly List<Department> _children = [];

    private Department(
        DepartmentName departmentName,
        Identifier identifier,
        Path path,
        Department? parent,
        Depth depth,
        bool isActive,
        IEnumerable<DepartmentLocation> departmentLocations,
        IEnumerable<DepartmentPosition> departmentPositions)
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
        DepartmentLocations = departmentLocations.ToList();
        DepartmentPositions = departmentPositions.ToList();
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

    public IReadOnlyList<DepartmentLocation> DepartmentLocations { get; private set; }

    public IReadOnlyList<DepartmentPosition> DepartmentPositions { get; private set; }


    public static Result<Department> Create(
        DepartmentName departmentName,
        Identifier identifier,
        Department? parent,
        Depth depth,
        bool isActive,
        IEnumerable<DepartmentLocation> departmentLocations,
        IEnumerable<DepartmentPosition> departmentPositions)
    {
        var locationsList = departmentLocations?.ToList() ?? new List<DepartmentLocation>();
        var positionsList = departmentPositions?.ToList() ?? new List<DepartmentPosition>();

        if (locationsList.Count == 0)
            return Result.Failure<Department>("DepartmentLocations must contain at least one item.");

        if (positionsList.Count == 0)
            return Result.Failure<Department>("DepartmentPositions must contain at least one item.");

        var path = BuildPath(parent, identifier).Value;

        return new Department(
            departmentName,
            identifier,
            path,
            parent,
            depth,
            isActive,
            locationsList,
            positionsList);
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