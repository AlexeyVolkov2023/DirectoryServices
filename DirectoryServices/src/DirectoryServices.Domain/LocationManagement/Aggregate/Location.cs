using CSharpFunctionalExtensions;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.LocationManagement.ValueObjects;

namespace DirectoryServices.Domain.LocationManagement.Aggregate;

public class Location
{
    private Location(
        LocationName name,
        Address address,
        Timezone timezone,
        bool isActive)
    {
        Id = LocationId.NewLocationId();
        Name = name;
        Address = address;
        Timezone = timezone;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public LocationId Id { get; private set; }

    public LocationName Name { get; private set; }

    public Address Address { get; private set; }

    public Timezone Timezone { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Result<Location> Create(
        LocationName name,
        Address address,
        Timezone timezone,
        bool isActive)
    {
        return Result.Success(new Location(
            name: name,
            address: address,
            timezone: timezone,
            isActive: isActive));
    }

    public void UpdateName(LocationName newName)
    {
        Name = newName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(Address newAddress)
    {
        Address = newAddress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTimezone(Timezone newTimezone)
    {
        Timezone = newTimezone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }
}