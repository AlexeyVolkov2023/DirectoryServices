namespace DirectoryServices.Contracts.LocationDtos;

public record LocationDetailsDto(string LocationName, AddressDto AddressDto, string Timezone);