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

public class CreateDepartmentHandler
{
    public class CreateLocationHandler : ICommandHandler<Guid, CreateDepartmentCommand>
    {
        private readonly ILogger<CreateLocationHandler> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IValidator<CreateDepartmentCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLocationHandler(
            ILogger<CreateLocationHandler> logger,
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

            var department = await _departmentRepository.GetByIdentifierAsync(
                identifier, cancellationToken);

            if (department.IsSuccess)
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

            try
            {
                var result = await _departmentRepository.AddAsync(departmentToCreate, cancellationToken);

                if (result.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken); // <-- Откат при ошибке репозитория
                    return Error.Failure(
                        "department.not.created",
                        "Department was not created");
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Created department with Id {departmentId}", departmentToCreate.Id);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return departmentToCreate.Id.Value;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error occurred while creating department with identifier {Identifier}",
                    identifier.Value);
                return Error.Failure(
                    "department.creation.failed",
                    "An unexpected error occurred during department creation.");
            }
        }
    }
}