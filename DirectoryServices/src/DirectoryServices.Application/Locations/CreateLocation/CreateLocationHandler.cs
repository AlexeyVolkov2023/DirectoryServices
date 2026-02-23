using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
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
    public async Task<Result<Guid>> Handle(
        CreateLocationCommand command,
        CancellationToken cancellationToken)
    {
        // validation
        var locationNameResult = LocationName.Create(command.LocationName);
        if (locationNameResult.IsFailure)
            return Result.Failure<Guid>(locationNameResult.Error);


        var locationName = locationNameResult.Value;

        var addressResult = Address.Create(
            command.AddressDto.Country,
            command.AddressDto.Region,
            command.AddressDto.City,
            command.AddressDto.Street,
            command.AddressDto.HouseNumber);
        if (addressResult.IsFailure)
            return Result.Failure<Guid>(addressResult.Error);

        var address = addressResult.Value;

        var timezoneResult = Timezone.Create(command.Timezone);
        if (timezoneResult.IsFailure)
            return Result.Failure<Guid>(timezoneResult.Error);

        var timezone = timezoneResult.Value;

        var locationResult = Location.Create(locationName, address, timezone);
        if (locationResult.IsFailure)
            return Result.Failure<Guid>(locationResult.Error);

        var location = locationResult.Value;


        var result = await _locationRepository.AddAsync(location, cancellationToken);

        _logger.LogInformation("Created location with Id {location.Id}", location.Id);

        return result;
    }
}