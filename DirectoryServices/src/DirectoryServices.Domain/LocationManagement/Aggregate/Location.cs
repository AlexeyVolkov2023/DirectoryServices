using CSharpFunctionalExtensions;
using DirectoryServices.Domain.DepartmentManagement.Supporting;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.LocationManagement.ValueObjects;

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

    public static Result<Location> Create(
        LocationName locationName,
        Address address,
        Timezone timezone)
    {
        return Result.Success(new Location(
            locationName,
            address,
            timezone));
    }

    public void UpdateName(LocationName newLocationName)
    {
        LocationName = newLocationName;
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