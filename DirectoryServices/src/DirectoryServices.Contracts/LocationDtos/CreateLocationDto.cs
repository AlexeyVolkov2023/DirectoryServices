namespace DirectoryServices.Contracts.LocationDtos;

public record CreateLocationDto(string LocationName, AddressDto AddressDto, string Timezone);