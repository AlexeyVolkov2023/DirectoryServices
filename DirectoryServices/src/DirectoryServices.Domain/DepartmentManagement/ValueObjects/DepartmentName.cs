using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.DepartmentManagement.ValueObjects;

public record DepartmentName
{
    private DepartmentName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<DepartmentName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsInvalid("departmentName");

        if (value.Length < LengthConstants.Length3 || value.Length > LengthConstants.Length150)
            return GeneralErrors.ValueIsInvalid("departmentName");

        return new DepartmentName(value);
    }
}