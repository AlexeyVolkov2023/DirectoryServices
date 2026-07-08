using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts.LocationDtos;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateLocationAddress;

public record UpdateLocationAddressCommand(Guid LocationId, UpdateAddressDto UpdateAddressDto) : ICommand;

