using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts.LocationDtos;

namespace DirectoryServices.Application.Managements.Locations.CreateLocation;

public record CreateLocationCommand(CreateLocationDto CreateLocationDto) : ICommand;