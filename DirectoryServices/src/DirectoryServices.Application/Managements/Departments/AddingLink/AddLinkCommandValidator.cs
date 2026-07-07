using FluentValidation;

namespace DirectoryServices.Application.Managements.Departments.AddingLink;

public class AddLinkCommandValidator : AbstractValidator<AddLinkCommand>
{
    public AddLinkCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty();

        RuleFor(x => x.LocationId)
            .NotEmpty();
    }
}