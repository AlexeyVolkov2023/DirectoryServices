using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.DepartmentManagement.Id;
using DirectoryServices.Domain.PositionManagement.Aggregate;
using DirectoryServices.Domain.PositionManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Positions.CreatePosition;

public class CreatePositionHandler : ICommandHandler<Guid, CreatePositionCommand>
{
    private readonly ILogger<CreatePositionHandler> _logger;
    private readonly IPositionRepository _positionRepository;
    private readonly IValidator<CreatePositionCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePositionHandler(
        ILogger<CreatePositionHandler> logger,
        IPositionRepository positionRepository,
        IValidator<CreatePositionCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _positionRepository = positionRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreatePositionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var departmentIds = command.CreatePositionDto.DepartmentIds.ToList();
        if (departmentIds.Count != 0)
        {
            bool allDepartmentsExist = await _positionRepository.DoAllDepartmentsExistAndActiveAsync(
                departmentIds.Select(DepartmentId.Create), cancellationToken);

            if (!allDepartmentsExist)
            {
                return Error.NotFound(
                    "departments.not.found.or.inactive",
                    "One or more departments specified do not exist or are not active.");
            }
        }

        var positionName = PositionName.Create(command.CreatePositionDto.PositionName).Value;

        bool uniqueName = await _positionRepository.ExistsActivePositionWithNameAsync(positionName, cancellationToken);
        if (uniqueName)
        {
            return GeneralErrors.ValueIsInvalid($"{positionName} must be unique");
        }

        var description = Description.Create(command.CreatePositionDto.Description).Value;

        var position = Position.Create(positionName, description).Value;

        var result = await _positionRepository.AddAsync(position, cancellationToken);
        if (result.IsFailure)
        {
            return Error.Failure("position.not.created", "Position was not created");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveResult <= 0)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure("position.not.saved", "Failed to save the position.");
        }

        _logger.LogInformation("Created position with Id {PositionId}", position.Id.Value);

        var addingResult =
            await _positionRepository.AddPositionToDepartments(position.Id, departmentIds, cancellationToken);
        if (addingResult.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure(
                "positionId.not.added.to.departments",
                "Position ID is not added to the departments");
        }

        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        return position.Id.Value;
    }
}