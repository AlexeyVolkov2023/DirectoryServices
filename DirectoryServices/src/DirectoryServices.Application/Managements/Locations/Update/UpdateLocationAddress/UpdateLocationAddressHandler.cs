using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateLocationAddress;

public class UpdateLocationAddressHandler : ICommandHandler<Guid, UpdateLocationAddressCommand>
{
    private readonly ILogger<UpdateLocationAddressHandler> _logger;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<UpdateLocationAddressCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLocationAddressHandler(
        ILogger<UpdateLocationAddressHandler> logger,
        ILocationRepository locationRepository,
        IValidator<UpdateLocationAddressCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _locationRepository = locationRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateLocationAddressCommand detailsCommand,
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

        var address = Address.Create(
            detailsCommand.UpdateAddressDto.AddressDto.Country,
            detailsCommand.UpdateAddressDto.AddressDto.Region,
            detailsCommand.UpdateAddressDto.AddressDto.City,
            detailsCommand.UpdateAddressDto.AddressDto.Street,
            detailsCommand.UpdateAddressDto.AddressDto.HouseNumber).Value;

        var timezone = Timezone.Create(detailsCommand.UpdateAddressDto.Timezone).Value;

        var locationToUpdate = locationResult.Value;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var result = locationToUpdate.UpdateAddress(address, timezone);
        if (result.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure(
                "location.address.not.updated",
                "Location address is not updated");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated location address and timezone with Id {LocationId}", locationToUpdate.Id);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return locationToUpdate.Id.Value;
    }
}