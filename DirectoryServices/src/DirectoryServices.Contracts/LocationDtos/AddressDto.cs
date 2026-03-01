namespace DirectoryServices.Contracts.LocationDtos;

public record AddressDto(
    string Country,
    string Region,
    string City,
    string Street,
    string HouseNumber);