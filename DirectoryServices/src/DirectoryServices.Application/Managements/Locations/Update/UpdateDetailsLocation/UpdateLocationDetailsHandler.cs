using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateDetailsLocation;

public class UpdateLocationDetailsHandler : ICommandHandler<Guid, UpdateLocationDetailsCommand>
{
    private readonly ILogger<UpdateLocationDetailsHandler> _logger;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<UpdateLocationDetailsCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLocationDetailsHandler(
        ILogger<UpdateLocationDetailsHandler> logger,
        ILocationRepository locationRepository,
        IValidator<UpdateLocationDetailsCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _locationRepository = locationRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateLocationDetailsCommand detailsCommand,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(detailsCommand, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var locationResult = await _locationRepository.GetByIdAsync(
            detailsCommand.LocationId,
            cancellationToken);
        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var locationName = LocationName.Create(detailsCommand.UpdateLocationDetailsDto.LocationName).Value;

        var duplicateExists = await _locationRepository.DoesLocationNameExistExcludingIdAsync(
            locationName,
            detailsCommand.LocationId,
            cancellationToken);
        if (duplicateExists)
        {
            return GeneralErrors.AlreadyExist();
        }

        var address = Address.Create(
            detailsCommand.UpdateLocationDetailsDto.AddressDto.Country,
            detailsCommand.UpdateLocationDetailsDto.AddressDto.Region,
            detailsCommand.UpdateLocationDetailsDto.AddressDto.City,
            detailsCommand.UpdateLocationDetailsDto.AddressDto.Street,
            detailsCommand.UpdateLocationDetailsDto.AddressDto.HouseNumber).Value;

        var timezone = Timezone.Create(detailsCommand.UpdateLocationDetailsDto.Timezone).Value;

        var locationToUpdate = locationResult.Value;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var result = locationToUpdate.UpdateDetails(locationName, address, timezone);
        if (result.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure(
                "location.details.not.updated",
                "Location details is not updated");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated location with Id {LocationId}", locationToUpdate.Id);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return locationToUpdate.Id.Value;
    }
}