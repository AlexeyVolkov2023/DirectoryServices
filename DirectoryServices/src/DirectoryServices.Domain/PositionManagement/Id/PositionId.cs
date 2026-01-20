namespace DirectoryServices.Domain.PositionManagement.Id;

public record PositionId
{
    private PositionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PositionId NewPositionId() => new(Guid.NewGuid());

    public static PositionId Empty() => new(Guid.Empty);

    public static PositionId Create(Guid id) => new(id);

    public static implicit operator PositionId(Guid id) => new PositionId(id);

    public static implicit operator Guid(PositionId positionId)
    {
        ArgumentNullException.ThrowIfNull(positionId);

        return positionId.Value;
    }
}