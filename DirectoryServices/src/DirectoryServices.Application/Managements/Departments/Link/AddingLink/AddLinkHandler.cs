using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Application.Managements.Locations;
using DirectoryServices.Domain.LocationManagement.Id;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Departments.Link.AddingLink;

public class AddLinkHandler : ICommandHandler<Guid, AddLinkCommand>
{
    private readonly ILogger<AddLinkHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<AddLinkCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddLinkHandler(
        ILogger<AddLinkHandler> logger,
        IDepartmentRepository departmentRepository,
        ILocationRepository locationRepository,
        IValidator<AddLinkCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _locationRepository = locationRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddLinkCommand command,
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
            return departmentResult.Error;
        }

        var locationResult = await _locationRepository.GetByIdAsync(
            command.LocationId,
            cancellationToken);
        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var department = departmentResult.Value;

        var locationId = LocationId.Create(command.LocationId);

        if (department.DepartmentLocations.Any(dl => dl.LocationId == locationId))
        {
            return Error.Conflict(
                "department.location.link.already.exists",
                "A link between the specified department and location already exists.");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var addLinkResult = department.AddLocationToDepartment(locationId);
        if (addLinkResult.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return addLinkResult.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        _logger.LogInformation("Linked department {DepartmentId} with location {LocationId}", department.Id,
            locationResult.Value.Id);

        return addLinkResult.Value;
    }
}