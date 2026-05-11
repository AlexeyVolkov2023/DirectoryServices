using DirectoryServices.Application.Extensions;
using DirectoryServices.Domain.PositionManagement.ValueObjects;
using FluentValidation;

namespace DirectoryServices.Application.Managements.Positions.CreatePosition;

public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
{
    public CreatePositionCommandValidator()
    {
        RuleFor(x => x.CreatePositionDto.PositionName)
            .MustBeValueObject(PositionName.Create);

        RuleFor(x => x.CreatePositionDto.Description)
            .MustBeValueObject(Description.Create);
    }
}