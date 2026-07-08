using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts.LocationDtos;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateDetailsLocation;

public record UpdateLocationDetailsCommand(Guid LocationId, LocationDetailsDto UpdateLocationDetailsDto) : ICommand;

