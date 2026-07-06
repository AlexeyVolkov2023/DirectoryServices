using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Locations.Delete.SoftDelete;

public class SoftDeleteLocationHandler : ICommandHandler<Guid, SoftDeleteLocationCommand>
{
    private readonly ILogger<SoftDeleteLocationHandler> _logger;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<SoftDeleteLocationCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public SoftDeleteLocationHandler(
        ILogger<SoftDeleteLocationHandler> logger,
        ILocationRepository locationRepository,
        IValidator<SoftDeleteLocationCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _locationRepository = locationRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        SoftDeleteLocationCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var locationResult = await _locationRepository.GetByIdAsync(
            command.LocationId,
            cancellationToken);
        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var locationToDelete = locationResult.Value;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var result = locationToDelete.SetActive(false);
        if (result.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure(
                "location.not.soft.deleted",
                "Location not soft deleted");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Soft delete location with Id {LocationId}", command.LocationId);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return command.LocationId;
    }
}