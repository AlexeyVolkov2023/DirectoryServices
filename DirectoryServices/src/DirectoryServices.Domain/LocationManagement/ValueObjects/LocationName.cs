using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.LocationManagement.ValueObjects;

public record LocationName
{
    private LocationName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<LocationName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return GeneralErrors.ValueIsInvalid(value);
        }

        if (value.Length < LengthConstants.Length3 || value.Length > LengthConstants.Length120)
        {
            return GeneralErrors.ValueIsInvalid(value);
        }

        return new LocationName(value);
    }
}