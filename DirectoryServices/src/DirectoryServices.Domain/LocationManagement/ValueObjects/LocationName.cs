using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.LocationManagement.ValueObjects;

public record LocationName
{
    private const int MAX_LENGTH = 120;
    private const int MIN_LENGTH = 3;

    private LocationName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<LocationName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<LocationName>("LocationName cannot be null or empty.");

        if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
            return Result.Failure<LocationName>($"LocationName must be between {MIN_LENGTH} and {MAX_LENGTH} characters long.");


        return Result.Success(new LocationName(value));
    }
}