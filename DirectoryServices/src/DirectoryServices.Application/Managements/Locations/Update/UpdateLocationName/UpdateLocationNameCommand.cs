using DirectoryServices.Application.Abstractions;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateLocationName;

public record UpdateLocationNameCommand(Guid LocationId, string LocationName) : ICommand;