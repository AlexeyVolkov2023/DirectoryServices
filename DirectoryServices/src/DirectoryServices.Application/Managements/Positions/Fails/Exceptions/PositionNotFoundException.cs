using DirectoryServices.Application.Exceptions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Positions.Fails.Exceptions;

public class PositionNotFoundException : NotFoundException
{
    public PositionNotFoundException(Error[] errors)
        : base(errors)
    {
    }
}