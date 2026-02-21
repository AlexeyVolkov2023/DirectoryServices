namespace DirectoryServices.Contracts;

public record AddressDto(
    string Country,
    string Region,
    string City,
    string Street,
    string HouseNumber);