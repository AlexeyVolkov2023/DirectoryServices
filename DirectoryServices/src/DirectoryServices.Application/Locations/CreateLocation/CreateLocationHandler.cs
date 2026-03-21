using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Locations.CreateLocation;

public class CreateLocationHandler : ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILogger<CreateLocationHandler> _logger;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<CreateLocationCommand> _validator;

    public CreateLocationHandler(
        ILogger<CreateLocationHandler> logger,
        ILocationRepository locationRepository,
        IValidator<CreateLocationCommand> validator)
    {
        _logger = logger;
        _locationRepository = locationRepository;
        _validator = validator;
    }

    /// <summary>
    /// Создает Location
    /// </summary>
    public async Task<Result<Guid, Error>> Handle(
        CreateLocationCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var locationName = LocationName.Create(command.CreateLocationDto.LocationName).Value;

        var address = Address.Create(
            command.CreateLocationDto.AddressDto.Country,
            command.CreateLocationDto.AddressDto.Region,
            command.CreateLocationDto.AddressDto.City,
            command.CreateLocationDto.AddressDto.Street,
            command.CreateLocationDto.AddressDto.HouseNumber).Value;

        var timezone = Timezone.Create(command.CreateLocationDto.Timezone).Value;

        var location = Location.Create(locationName, address, timezone).Value;

        var result = await _locationRepository.AddAsync(location, cancellationToken);

        _logger.LogInformation("Created location with Id {location.Id}", location.Id);

        return location.Id.Value;
    }
}