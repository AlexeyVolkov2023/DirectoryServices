using CSharpFunctionalExtensions;
using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.DepartmentManagement.ValueObjects;
using DirectoryServices.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryServices.Application.Managements.Departments.Update.UpdateDetails;

public class UpdateDepartmentDetailsHandler : ICommandHandler<Guid, UpdateDepartmentDetailsCommand>
{
    private readonly ILogger<UpdateDepartmentDetailsHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IValidator<UpdateDepartmentDetailsCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDepartmentDetailsHandler(
        ILogger<UpdateDepartmentDetailsHandler> logger,
        IDepartmentRepository departmentRepository,
        IValidator<UpdateDepartmentDetailsCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateDepartmentDetailsCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToError();
        }

        var departmentResult = await _departmentRepository.GetByIdAsync(
            command.DepartmentId,
            cancellationToken);
        if (departmentResult.IsFailure)
        {
            return departmentResult.Error;
        }

        var identifier = Identifier.Create(command.UpdateDepartmentDto.Identifier).Value;

        var departmentExist = await _departmentRepository.GetByIdentifierAsync(
            identifier, cancellationToken);
        if (departmentExist.IsSuccess)
            return GeneralErrors.AlreadyExist();
        // Path  не пересчитываю

        var departmentName = DepartmentName.Create(command.UpdateDepartmentDto.DepartmentName).Value;

        var departmentToUpdate = departmentResult.Value;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var result = departmentToUpdate.UpdateDetails(departmentName, identifier);
        if (result.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Error.Failure(
                "department.details.not.updated",
                "Department details was not updated");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        _logger.LogInformation("Updated department with Id {departmentId}", departmentToUpdate.Id);

        return departmentToUpdate.Id.Value;
    }
}