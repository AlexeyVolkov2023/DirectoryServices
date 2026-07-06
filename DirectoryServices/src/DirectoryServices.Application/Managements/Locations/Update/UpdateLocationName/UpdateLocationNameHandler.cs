using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateLocationName;

public class UpdateLocationNameHandler : ICommandHandler<Guid, UpdateLocationNameCommand>
{
    private readonly ILogger<UpdateLocationNameHandler> _logger;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<UpdateLocationNameCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLocationNameHandler(
        ILogger<UpdateLocationNameHandler> logger,
        ILocationRepository locationRepository,
        IValidator<UpdateLocationNameCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _locationRepository = locationRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateLocationNameCommand detailsCommand,
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

        var locationName = LocationName.Create(detailsCommand.LocationName).Value;

        var duplicateExists = await _locationRepository.DoesLocationNameExistExcludingIdAsync(
            locationName,
            detailsCommand.LocationId,
            cancellationToken);
        if (duplicateExists)
        {
            return GeneralErrors.AlreadyExist();
        }

        var locationToUpdate = locationResult.Value;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var result = locationToUpdate.UpdateName(locationName);
        if (result.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure(
                "location.name.not.updated",
                "Location name is not updated");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated location name with Id {LocationId}", locationToUpdate.Id);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return locationToUpdate.Id.Value;
    }
}