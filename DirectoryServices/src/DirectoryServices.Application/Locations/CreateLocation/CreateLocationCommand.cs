using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts;

namespace DirectoryServices.Application.Locations.CreateLocation;

public record CreateLocationCommand(string LocationName, AddressDto AddressDto, string Timezone) : ICommand;