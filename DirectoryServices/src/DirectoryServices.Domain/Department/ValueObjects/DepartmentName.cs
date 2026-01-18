using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.Department.ValueObjects;

public record DepartmentName
{
    private const int MAX_LENGTH = 150;
    private const int MIN_LENGTH = 3;

    private DepartmentName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<DepartmentName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<DepartmentName>("DepartmentName cannot be null or empty.");

        if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
            return Result.Failure<DepartmentName>($"DepartmentName must be between {MIN_LENGTH} and {MAX_LENGTH} characters long.");

        return Result.Success(new DepartmentName(value));
    }
}