using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.DepartmentManagement.ValueObjects;
using FluentValidation;

namespace DirectoryServices.Application.Managements.Departments.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.CreateDepartmentDto.DepartmentName)
            .MustBeValueObject(DepartmentName.Create);

        RuleFor(x => x.CreateDepartmentDto.Identifier)
            .MustBeValueObject(Identifier.Create);

        RuleFor(x => x.CreateDepartmentDto.LocationIds)
            .NotEmpty();
    }
}