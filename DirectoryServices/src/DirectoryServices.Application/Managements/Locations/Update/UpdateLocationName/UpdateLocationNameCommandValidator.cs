using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using FluentValidation;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateLocationName;

public class UpdateLocationNameCommandValidator : AbstractValidator<UpdateLocationNameCommand>
{
    public UpdateLocationNameCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty();

        RuleFor(x => x.LocationName)
            .MustBeValueObject(LocationName.Create);
    }
}