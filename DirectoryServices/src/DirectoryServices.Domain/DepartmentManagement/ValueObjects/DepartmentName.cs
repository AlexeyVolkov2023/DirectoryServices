using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.DepartmentManagement.ValueObjects;

public record DepartmentName
{
    private DepartmentName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<DepartmentName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<DepartmentName>("DepartmentName cannot be null or empty.");

        if (value.Length < LengthConstants.Length3 || value.Length > LengthConstants.Length150)
        {
            return Result.Failure<DepartmentName>(
                $"DepartmentName must be between {LengthConstants.Length3} " +
                $"and {LengthConstants.Length150} characters long.");
        }

        return Result.Success(new DepartmentName(value));
    }
}