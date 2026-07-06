using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts.DepartmentDto;

namespace DirectoryServices.Application.Managements.Departments.Update.UpdateLocationList;

public record UpdateLocationListCommand(Guid DepartmentId, UpdateLocationListDto UpdateLocationListDto) : ICommand;