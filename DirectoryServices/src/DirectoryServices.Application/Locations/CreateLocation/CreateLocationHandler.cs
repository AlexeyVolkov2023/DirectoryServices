using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Locations.CreateLocation;

public class CreateLocationHandler : ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILogger<CreateLocationHandler> _logger;
    private readonly ILocationRepository _locationRepository;

    public CreateLocationHandler(
        ILogger<CreateLocationHandler> logger,
        ILocationRepository locationRepository)
    {
        _logger = logger;
        _locationRepository = locationRepository;
    }

    /// <summary>
    /// Создает Location
    /// </summary>
    public async Task<Result<Guid, Error>> Handle(
        CreateLocationCommand command,
        CancellationToken cancellationToken)
    {
        // validation
        var locationNameResult = LocationName.Create(command.CreateLocationDto.LocationName);
        if (locationNameResult.IsFailure)
        {
            return Error.Validation(new ErrorMessage(
                "locationNameResult.is.invalid",
                "LocationName is invalid"));
        }

        var locationName = locationNameResult.Value;

        var addressResult = Address.Create(
            command.CreateLocationDto.AddressDto.Country,
            command.CreateLocationDto.AddressDto.Region,
            command.CreateLocationDto.AddressDto.City,
            command.CreateLocationDto.AddressDto.Street,
            command.CreateLocationDto.AddressDto.HouseNumber);
        if (addressResult.IsFailure)
        {
            return Error.Validation(new ErrorMessage(
                "address.is.invalid",
                "LocationName is invalid"));
        }

        var address = addressResult.Value;

        var timezoneResult = Timezone.Create(command.CreateLocationDto.Timezone);
        if (timezoneResult.IsFailure)
        {
            return Error.Validation(new ErrorMessage(
                "timezone.is.invalid",
                "LocationName is invalid"));
        }

        var timezone = timezoneResult.Value;

        var locationResult = Location.Create(locationName, address, timezone);
        if (locationResult.IsFailure)
        {
            return Error.Validation(new ErrorMessage(
                "locationResult.is.invalid",
                "Location is invalid"));
        }

        var location = locationResult.Value;


        var result = await _locationRepository.AddAsync(location, cancellationToken);

        _logger.LogInformation("Created location with Id {location.Id}", location.Id);

        return location.Id.Value;
    }
}