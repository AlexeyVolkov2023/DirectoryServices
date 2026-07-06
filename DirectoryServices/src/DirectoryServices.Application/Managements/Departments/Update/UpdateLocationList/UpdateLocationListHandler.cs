using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Departments.Update.UpdateLocationList;

public class UpdateLocationListHandler : ICommandHandler<Guid, UpdateLocationListCommand>
{
    private readonly ILogger<UpdateLocationListCommand> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IValidator<UpdateLocationListCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLocationListHandler(
        ILogger<UpdateLocationListCommand> logger,
        IDepartmentRepository departmentRepository,
        IValidator<UpdateLocationListCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateLocationListCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var departmentResult = await _departmentRepository.GetByIdWithIncludeAsync(
            command.DepartmentId,
            cancellationToken);
        if (departmentResult.IsFailure)
        {
            return GeneralErrors.NotFound();
        }

        var locationIds = command.UpdateLocationListDto.LocationIds.ToList();

        var locationsExistResult = await _departmentRepository.CheckLocationsExistAndActiveAsync(
            locationIds,
            cancellationToken);
        if (locationsExistResult.IsFailure)
        {
            return locationsExistResult.Error;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var departmentToUpdate = departmentResult.Value;

        var result = departmentToUpdate.UpdateLocations(locationIds);
        if (result.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure(
                "locations.not.updated",
                "Locations list is not updated");
        }

        await _departmentRepository.Save();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        _logger.LogInformation("Updated locations list in department with Id {departmentId}", departmentToUpdate.Id);

        return departmentToUpdate.Id.Value;
    }
}