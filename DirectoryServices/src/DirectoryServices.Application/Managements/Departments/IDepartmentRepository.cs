using CSharpFunctionalExtensions;
using DirectoryServices.Domain.DepartmentManagement.Aggregate;
using DirectoryServices.Domain.DepartmentManagement.Id;
using DirectoryServices.Domain.DepartmentManagement.ValueObjects;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Departments;

public interface IDepartmentRepository
{
    Task<Result<Guid>> AddAsync(Department department, CancellationToken cancellationToken);

    Task<Result<Department, Error>> GetByIdentifierAsync(Identifier identifier, CancellationToken cancellationToken);

    Task<Result<Department, Error>> GetByIdAsync(DepartmentId departmentId, CancellationToken cancellationToken);

    Task<bool> LocationsExistAsync(IEnumerable<Guid> locationIds, CancellationToken cancellationToken);
}