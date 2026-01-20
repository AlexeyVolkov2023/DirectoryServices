using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.LocationManagement.ValueObjects;

public record Timezone
{
    private Timezone(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Timezone> Create(string timezone)
    {
        if (string.IsNullOrWhiteSpace(timezone))
            return Result.Failure<Timezone>("Timezone cannot be null or empty.");

        return Result.Success(new Timezone(timezone));
    }
}