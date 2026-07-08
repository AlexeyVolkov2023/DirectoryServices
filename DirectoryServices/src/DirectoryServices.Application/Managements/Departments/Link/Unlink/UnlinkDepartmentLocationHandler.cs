using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Application.Managements.Locations;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Departments.Link.Unlink;

public class UnlinkDepartmentLocationHandler : ICommandHandler<Guid, UnlinkDepartmentLocationCommand>
{
    private readonly ILogger<UnlinkDepartmentLocationHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<UnlinkDepartmentLocationCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UnlinkDepartmentLocationHandler(
        ILogger<UnlinkDepartmentLocationHandler> logger,
        IDepartmentRepository departmentRepository,
        ILocationRepository locationRepository,
        IValidator<UnlinkDepartmentLocationCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _locationRepository = locationRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UnlinkDepartmentLocationCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var departmentResult =
            await _departmentRepository.GetByIdWithIncludeAsync(command.DepartmentId, cancellationToken);
        if (departmentResult.IsFailure)
        {
            return departmentResult.Error;
        }

        var locationResult = await _locationRepository.GetByIdAsync(command.LocationId, cancellationToken);
        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var department = departmentResult.Value;

        var locationId = LocationId.Create(command.LocationId);

        var existingLink = department.DepartmentLocations.FirstOrDefault(dl => dl.LocationId == locationId);
        if (existingLink == null)
        {
            return Error.NotFound(
                "department.location.link.not.found",
                $"No link found between department and location {locationId.Value}.");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var removeResult = department.RemoveLocationFromDepartment(existingLink.DepartmentLocationId);
        if (removeResult.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return removeResult.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        _logger.LogInformation("Unlinked location {LocationId} from department {DepartmentId}", locationId,
            department.Id);

        return existingLink.DepartmentLocationId.Value;
    }
}