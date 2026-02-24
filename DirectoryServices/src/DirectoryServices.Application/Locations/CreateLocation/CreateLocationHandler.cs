using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Database;
using DirectoryServices.Contracts;
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
        var locationName = LocationName.Create(command.LocationName).Value;

        var address = Address.Create(
            command.AddressDto.Country,
            command.AddressDto.Region,
            command.AddressDto.City,
            command.AddressDto.Street,
            command.AddressDto.HouseNumber).Value;

        var timezone = Timezone.Create(command.Timezone).Value;

        var location = Location.Create(locationName, address, timezone).Value;

        var result = await _locationRepository.AddAsync(location, cancellationToken);

        return result;
    }
}