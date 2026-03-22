using DirectoryServices.Application.Extensions;
using DirectoryServices.Application.Locations.Validation;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using FluentValidation;

namespace DirectoryServices.Application.Locations.CreateLocation;

public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(x => x.CreateLocationDto.LocationName)
            .MustBeValueObject(LocationName.Create);

        RuleFor(x => x.CreateLocationDto.AddressDto)
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.CreateLocationDto.Timezone)
            .MustBeValueObject(Timezone.Create);
    }
}