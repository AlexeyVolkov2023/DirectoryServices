using DirectoryServices.Application.Exceptions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Positions.Fails.Exceptions;

public class PositionValidationException : BadRequestException
{
    public PositionValidationException(Error[] errors)
        : base(errors)
    {
    }
}