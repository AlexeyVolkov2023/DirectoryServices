using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.DepartmentManagement.Aggregate;
using DirectoryServices.Domain.DepartmentManagement.Id;
using DirectoryServices.Domain.DepartmentManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Departments.CreateDepartment;

public class CreateDepartmentHandler : ICommandHandler<Guid, CreateDepartmentCommand>
{
    private readonly ILogger<CreateDepartmentCommand> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IValidator<CreateDepartmentCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDepartmentHandler(
        ILogger<CreateDepartmentCommand> logger,
        IDepartmentRepository departmentRepository,
        IValidator<CreateDepartmentCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var identifier = Identifier.Create(command.CreateDepartmentDto.Identifier).Value;

        var departmentExist = await _departmentRepository.GetByIdentifierAsync(
            identifier, cancellationToken);

        if (departmentExist.IsSuccess)
            return GeneralErrors.AlreadyExist();

        Department? parent = null;
        if (command.CreateDepartmentDto.ParentId.HasValue)
        {
            var parentId = DepartmentId.Create(command.CreateDepartmentDto.ParentId.Value);
            var parentResult = await _departmentRepository.GetByIdAsync(parentId, cancellationToken);
            if (parentResult.IsFailure)
            {
                return GeneralErrors.NotFound(parentId, "Department");
            }

            parent = parentResult.Value;
        }

        var departmentName = DepartmentName.Create(command.CreateDepartmentDto.DepartmentName).Value;

        var locationIds = command.CreateDepartmentDto.LocationIds.ToList();

        var locationsExistResult = await _departmentRepository.LocationsExistAsync(
            locationIds,
            cancellationToken);

        if (locationsExistResult == false)
        {
            return GeneralErrors.NotFound(null, "LocationId");
        }

        var departmentToCreate = Department.Create(
            departmentName,
            identifier,
            parent,
            locationIds).Value;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var result = await _departmentRepository.AddAsync(departmentToCreate, cancellationToken);

        if (result.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure(
                "department.not.created",
                "Department was not created");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        _logger.LogInformation("Created department with Id {departmentId}", departmentToCreate.Id);

        return departmentToCreate.Id.Value;
    }
}


