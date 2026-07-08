using FluentValidation;

namespace DirectoryServices.Application.Managements.Departments.Update.UpdateLocationList;

public class UpdateLocationListCommandValidator : AbstractValidator<UpdateLocationListCommand>
{
    public UpdateLocationListCommandValidator()
    {
        RuleFor(x => x.UpdateLocationListDto.LocationIds)
            .NotEmpty();

        RuleFor(x => x.UpdateLocationListDto.LocationIds)
            .NotNull().WithMessage("Location IDs list cannot be null.")
            .NotEmpty().WithMessage("Location IDs list cannot be empty.")
            .Custom((locationIds, context) =>
            {
                IEnumerable<Guid> enumerable = locationIds.ToList();
                if (locationIds != null && enumerable.Distinct().Count() != enumerable.Count())
                {
                    context.AddFailure("Location IDs list contains duplicates.");
                }
            });
    }
}