using CSharpFunctionalExtensions;
using DirectoryServices.Domain.PositionManagement.Id;
using DirectoryServices.Domain.PositionManagement.ValueObjects;

namespace DirectoryServices.Domain.PositionManagement.Aggregate;

public class Position
{
    public Position()
    {
    }

    private Position(
        PositionName positionName,
        Description description)
    {
        Id = PositionId.NewPositionId();
        PositionName = positionName;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public PositionId Id { get; private set; }

    public PositionName PositionName { get; private set; }

    public Description Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Result<Position> Create(
        PositionName positionName,
        Description description)
    {
        return Result.Success(new Position(positionName, description));
    }

    public void UpdateName(PositionName newPositionName)
    {
        PositionName = newPositionName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(Description newDescription)
    {
        Description = newDescription;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }
}