using CSharpFunctionalExtensions;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.LocationManagement.Aggregate;

public class Location
{
    public Location()
    {
    }

    private Location(
        LocationName locationName,
        Address address,
        Timezone timezone)
    {
        Id = LocationId.NewLocationId();
        LocationName = locationName;
        Address = address;
        Timezone = timezone;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public LocationId Id { get; private set; }

    public LocationName LocationName { get; private set; }

    public Address Address { get; private set; }

    public Timezone Timezone { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Result<Location, Error> Create(
        LocationName locationName,
        Address address,
        Timezone timezone)
    {
        return new Location(locationName, address, timezone);
    }

    public Result UpdateName(LocationName newLocationName)
    {
        LocationName = newLocationName;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result UpdateAddress(Address newAddress, Timezone newTimezone)
    {
        Address = newAddress;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result SetActive(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result UpdateDetails(LocationName newLocationName, Address newAddress, Timezone newTimezone)
    {
        LocationName = newLocationName;
        Address = newAddress;
        Timezone = newTimezone;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}