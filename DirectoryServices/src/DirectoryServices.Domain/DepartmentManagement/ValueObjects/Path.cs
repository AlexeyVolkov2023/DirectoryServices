using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.DepartmentManagement.ValueObjects;

public record Path
{
    private Path(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<Path, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsInvalid("path");

        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9.-]+$"))
            return GeneralErrors.ValueIsInvalid("path");

        return new Path(value);
    }
}