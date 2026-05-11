using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Locations.CreateLocation;

public class CreateLocationHandler : ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILogger<CreateLocationHandler> _logger;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<CreateLocationCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLocationHandler(
        ILogger<CreateLocationHandler> logger,
        ILocationRepository locationRepository,
        IValidator<CreateLocationCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _locationRepository = locationRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

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

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _locationRepository.AddAsync(location, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created location with Id {LocationId}", location.Id.Value);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return location.Id.Value;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "An error occurred while creating location {LocationName}. Transaction rolled back.",
                locationName.ToString());

            return Error.Failure(
                "location.creation.failed",
                "An unexpected error occurred during location creation.");
        }
    }
}