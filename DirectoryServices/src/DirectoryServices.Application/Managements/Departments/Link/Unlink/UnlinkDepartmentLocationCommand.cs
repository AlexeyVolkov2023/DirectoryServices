using DirectoryServices.Application.Abstractions;

namespace DirectoryServices.Application.Managements.Departments.Link.Unlink;

public record UnlinkDepartmentLocationCommand(Guid DepartmentId, Guid LocationId) : ICommand;