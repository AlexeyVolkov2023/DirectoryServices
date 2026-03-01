using DirectoryServices.Application.Exceptions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Locations.Fails.Exceptions;

public class LocationValidationException : BadRequestException
{
    public LocationValidationException(Error[] errors)
        : base(errors)
    {
    }
}