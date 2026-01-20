using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.Department.ValueObjects;

public record Identifier
{
    private const int MAX_LENGTH = 150;
    private const int MIN_LENGTH = 3;

    private Identifier(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Identifier> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Identifier>("Identifier cannot be null or empty.");

        if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
        {
            return Result.Failure<Identifier>(
                $"Identifier must be between {MIN_LENGTH} and {MAX_LENGTH} characters long.");
        }

        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9-]+$"))
            return Result.Failure<Identifier>("Identifier must contain only Latin letters, digits, and hyphens.");

        return Result.Success(new Identifier(value));
    }
}