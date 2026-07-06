using DirectoryServices.Application.Abstractions;

namespace DirectoryServices.Application.Managements.Locations.Delete.SoftDelete;

public record SoftDeleteLocationCommand(Guid LocationId) : ICommand;