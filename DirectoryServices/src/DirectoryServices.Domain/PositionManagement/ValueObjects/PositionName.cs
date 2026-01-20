using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.PositionManagement.ValueObjects;

public record PositionName
{
    private const int MAX_LENGTH = 100;
    private const int MIN_LENGTH = 3;

    private PositionName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<PositionName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<PositionName>("PositionName cannot be null or empty.");

        if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
            return Result.Failure<PositionName>($"PositionName must be between {MIN_LENGTH} and {MAX_LENGTH} characters long.");

        return Result.Success(new PositionName(value));
    }
}