using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.LocationManagement.ValueObjects;

public record Timezone
{
    private Timezone(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Timezone, Error> Create(string timezone)
    {
        if (string.IsNullOrWhiteSpace(timezone))
        {
            return GeneralErrors.ValueIsInvalid(timezone);
        }

        return new Timezone(timezone);
    }
}