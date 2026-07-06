using DirectoryServices.Application.Extensions;
using DirectoryServices.Application.Managements.Locations.Validation;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using FluentValidation;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateDetailsLocation;

public class UpdateLocationDetailsCommandValidator : AbstractValidator<UpdateLocationDetailsCommand>
{
    public UpdateLocationDetailsCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty();

        RuleFor(x => x.UpdateLocationDetailsDto.LocationName)
            .MustBeValueObject(LocationName.Create);

        RuleFor(x => x.UpdateLocationDetailsDto.AddressDto)
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.UpdateLocationDetailsDto.Timezone)
            .MustBeValueObject(Timezone.Create);
    }
}