using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.PositionManagement.ValueObjects;

public record PositionName
{
    private PositionName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<PositionName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<PositionName>("PositionName cannot be null or empty.");

        if (value.Length < LengthConstants.Length3 || value.Length > LengthConstants.Length100)
        {
            return Result.Failure<PositionName>(
                $"PositionName must be between {LengthConstants.Length3}" +
                $" and {LengthConstants.Length100} characters long.");
        }

        return Result.Success(new PositionName(value));
    }
}