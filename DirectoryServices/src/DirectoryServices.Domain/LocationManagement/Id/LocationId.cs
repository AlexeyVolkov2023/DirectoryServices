namespace DirectoryServices.Domain.LocationManagement.Id;

public record LocationId
{
    private LocationId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static LocationId NewLocationId() => new(Guid.NewGuid());

    public static LocationId Empty() => new(Guid.Empty);

    public static LocationId Create(Guid id) => new(id);

    public static implicit operator LocationId(Guid id) => new LocationId(id);

    public static implicit operator Guid(LocationId locationId)
    {
        ArgumentNullException.ThrowIfNull(locationId);

        return locationId.Value;
    }
}