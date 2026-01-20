using CSharpFunctionalExtensions;

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


    public static Result<Address> Create(
        string country,
        string region,
        string city,
        string street,
        string houseNumber)
    {
        if (string.IsNullOrWhiteSpace(country))
            return Result.Failure<Address>("Country can not by empty");

        if (string.IsNullOrWhiteSpace(region))
            return Result.Failure<Address>("Region can not by empty");

        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>("City can not by empty");

        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>("Street can not by empty");

        if (string.IsNullOrWhiteSpace(houseNumber))
            return Result.Failure<Address>("House number can not by empty");


        return Result.Success(new Address(
            country,
            region,
            city,
            street,
            houseNumber));
    }
}