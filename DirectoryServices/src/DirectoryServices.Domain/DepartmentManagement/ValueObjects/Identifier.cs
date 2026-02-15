using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.DepartmentManagement.ValueObjects;

public record Identifier
{
    private Identifier(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Identifier> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Identifier>("Identifier cannot be null or empty.");

        if (value.Length < LengthConstants.Length3 || value.Length > LengthConstants.Length150)
        {
            return Result.Failure<Identifier>(
                $"Identifier must be between {LengthConstants.Length3}" +
                $" and {LengthConstants.Length150} characters long.");
        }

        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9-]+$"))
            return Result.Failure<Identifier>("Identifier must contain only Latin letters, digits, and hyphens.");

        return Result.Success(new Identifier(value));
    }
}