using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts.DepartmentDto;

namespace DirectoryServices.Application.Managements.Departments.Update.UpdateDetails;

public record UpdateDepartmentDetailsCommand(Guid DepartmentId, UpdateDepartmentDto UpdateDepartmentDto) : ICommand;