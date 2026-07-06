using FluentValidation;

namespace DirectoryServices.Application.Managements.Locations.Delete.SoftDelete;

public class SoftDeleteLocationCommandValidator : AbstractValidator<SoftDeleteLocationCommand>
{
    public SoftDeleteLocationCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty();
    }
}