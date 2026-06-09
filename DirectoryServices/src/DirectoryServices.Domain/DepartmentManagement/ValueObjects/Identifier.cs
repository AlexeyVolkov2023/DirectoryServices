using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.DepartmentManagement.ValueObjects;

public record Identifier
{
    private Identifier(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Identifier, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsInvalid("identifier");

        if (value.Length < LengthConstants.Length3 || value.Length > LengthConstants.Length150)
            return GeneralErrors.ValueIsInvalid("identifier");

        if (!Regex.IsMatch(value, @"^[a-zA-Z][a-zA-Z .-]*$"))
            return GeneralErrors.ValueIsInvalid("identifier");

        return new Identifier(value);
    }
}