using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.PositionManagement.ValueObjects;

public record Description
{
    private const int MAX_LENGTH = 1000;

    private Description(string? value)
    {
        Value = value;
    }

    public string? Value { get; }

    public static Result<Description> Create(string? value)
    {
        if (value != null && value.Length > MAX_LENGTH)
        {
            return Result.Failure<Description>($"Description must be no more than {MAX_LENGTH} characters long.");
        }

        return Result.Success(new Description(value));
    }
};