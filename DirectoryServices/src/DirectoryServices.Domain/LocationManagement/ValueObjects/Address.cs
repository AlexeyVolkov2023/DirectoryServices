using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.LocationManagement.ValueObjects;

public record Address
{
    private Address(
        string country,
        string region,
        string city,
        string street,
        string houseNumber)
    {
        Country = country;
        Region = region;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
    }

    public string Street { get; }

    public string HouseNumber { get; }

    public string City { get; }

    public string Region { get; }

    public string Country { get; }


    public static Result<Address, Error> Create(
        string country,
        string region,
        string city,
        string street,
        string houseNumber)
    {
        if (string.IsNullOrWhiteSpace(country))
            return GeneralErrors.ValueIsInvalid(country);

        if (string.IsNullOrWhiteSpace(region))
            return GeneralErrors.ValueIsInvalid(region);

        if (string.IsNullOrWhiteSpace(city))
            return GeneralErrors.ValueIsInvalid(city);

        if (string.IsNullOrWhiteSpace(street))
            return GeneralErrors.ValueIsInvalid(street);

        if (string.IsNullOrWhiteSpace(houseNumber))
            return GeneralErrors.ValueIsInvalid(houseNumber);


        return new Address(
            country,
            region,
            city,
            street,
            houseNumber);
    }
}