using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.PositionManagement.ValueObjects;

public record Description
{
    private Description(string? value)
    {
        Value = value;
    }

    public string? Value { get; }

    public static Result<Description> Create(string? value)
    {
        if (value != null && value.Length > LengthConstants.Length1000)
        {
            return Result.Failure<Description>(
                $"Description must be no more than {LengthConstants.Length1000} characters long.");
        }

        return Result.Success(new Description(value));
    }
};