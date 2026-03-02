using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts;
using DirectoryServices.Contracts.LocationDtos;

namespace DirectoryServices.Application.Locations.CreateLocation;

public record CreateLocationCommand(CreateLocationDto CreateLocationDto) : ICommand;