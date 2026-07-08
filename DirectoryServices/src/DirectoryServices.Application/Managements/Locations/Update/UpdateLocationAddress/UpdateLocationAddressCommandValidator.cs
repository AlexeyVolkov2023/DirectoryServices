using DirectoryServices.Application.Extensions;
using DirectoryServices.Application.Managements.Locations.Validation;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using FluentValidation;

namespace DirectoryServices.Application.Managements.Locations.Update.UpdateLocationAddress;

public class UpdateLocationAddressCommandValidator : AbstractValidator<UpdateLocationAddressCommand>
{
    public UpdateLocationAddressCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty();

        RuleFor(x => x.UpdateAddressDto.AddressDto)
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.UpdateAddressDto.Timezone)
            .MustBeValueObject(Timezone.Create);
    }
}