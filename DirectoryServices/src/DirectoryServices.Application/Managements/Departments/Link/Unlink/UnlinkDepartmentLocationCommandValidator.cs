using FluentValidation;

namespace DirectoryServices.Application.Managements.Departments.Link.Unlink;

public class UnlinkDepartmentLocationCommandValidator : AbstractValidator<UnlinkDepartmentLocationCommand>
{
    public UnlinkDepartmentLocationCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty();

        RuleFor(x => x.LocationId)
            .NotEmpty();
    }
}