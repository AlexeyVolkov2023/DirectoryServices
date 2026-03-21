using DirectoryServices.Application.Extensions;
using DirectoryServices.Contracts.LocationDtos;
using DirectoryServices.Domain.LocationManagement.ValueObjects;
using FluentValidation;

namespace DirectoryServices.Application.Locations.Validation;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => new { x.Country, x.Region, x.City, x.Street, x.HouseNumber })
            .MustBeValueObject(x =>
                Address.Create(x.Country, x.Region, x.City, x.Street, x.HouseNumber));
    }
}