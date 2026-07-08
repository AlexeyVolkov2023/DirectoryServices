using DirectoryServices.Application.Abstractions;

namespace DirectoryServices.Application.Managements.Departments.Link.AddingLink;

public record AddLinkCommand(Guid DepartmentId, Guid LocationId) : ICommand;