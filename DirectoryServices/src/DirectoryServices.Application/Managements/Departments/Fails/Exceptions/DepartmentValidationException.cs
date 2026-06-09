using DirectoryServices.Application.Exceptions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Departments.Fails.Exceptions;

public class DepartmentValidationException : BadRequestException
{
    public DepartmentValidationException(Error[] errors)
        : base(errors)
    {
    }
}