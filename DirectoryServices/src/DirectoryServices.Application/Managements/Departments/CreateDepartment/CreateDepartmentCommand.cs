using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts.DepartmentDto;

namespace DirectoryServices.Application.Managements.Departments.CreateDepartment;

public record CreateDepartmentCommand(CreateDepartmentDto CreateDepartmentDto) : ICommand;