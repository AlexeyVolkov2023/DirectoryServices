using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.PositionManagement.ValueObjects;

public record PositionName
{
    private PositionName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<PositionName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsInvalid("positionName");

        if (value.Length < LengthConstants.Length3 || value.Length > LengthConstants.Length100)
        {
            return GeneralErrors.ValueIsInvalid("positionName");
        }

        return new PositionName(value);
    }
}