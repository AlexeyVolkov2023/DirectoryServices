using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.DepartmentManagement.ValueObjects;
using FluentValidation;

namespace DirectoryServices.Application.Managements.Departments.Update.UpdateDetails;

public class UpdateDepartmentDetailsCommandValidator : AbstractValidator<UpdateDepartmentDetailsCommand>
{
    public UpdateDepartmentDetailsCommandValidator()
    {
        RuleFor(x => x.UpdateDepartmentDto.DepartmentName)
            .MustBeValueObject(DepartmentName.Create);

        RuleFor(x => x.UpdateDepartmentDto.Identifier)
            .MustBeValueObject(Identifier.Create);

        RuleFor(x => x.DepartmentId)
            .NotEmpty();
    }
}