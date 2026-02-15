using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.LocationManagement.ValueObjects;

public record LocationName
{
    private LocationName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<LocationName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<LocationName>("LocationName cannot be null or empty.");

        if (value.Length < LengthConstants.Length3 || value.Length > LengthConstants.Length120)
        {
            return Result.Failure<LocationName>(
                $"LocationName must be between {LengthConstants.Length3}" +
                $" and {LengthConstants.Length120} characters long.");
        }

        return Result.Success(new LocationName(value));
    }
}