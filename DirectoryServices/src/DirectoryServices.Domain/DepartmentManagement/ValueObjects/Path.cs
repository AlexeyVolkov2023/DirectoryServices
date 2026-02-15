using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.DepartmentManagement.ValueObjects;

public record Path
{
    private Path(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<Path> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Path>("Path cannot be null or empty.");

        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9.-]+$"))
            return Result.Failure<Path>("Path can only contain Latin letters, digits, hyphens, and dots.");

        return Result.Success(new Path(value));
    }
}