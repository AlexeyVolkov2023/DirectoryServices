using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.PositionManagement.ValueObjects;

public record Description
{
    private Description(string? value)
    {
        Value = value;
    }

    public string? Value { get; }

    public static Result<Description, Error> Create(string? value)
    {
        if (value != null && value.Length > LengthConstants.Length1000)
        {
            return GeneralErrors.ValueIsInvalid("description");
        }

        return new Description(value);
    }
};