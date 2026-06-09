using DirectoryServices.Application.Exceptions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Departments.Fails.Exceptions;

public class DepartmentNotFoundException : NotFoundException
{
    public DepartmentNotFoundException(Error[] errors)
        : base(errors)
    {
    }
}