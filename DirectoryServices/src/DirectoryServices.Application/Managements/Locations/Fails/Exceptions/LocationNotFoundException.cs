using DirectoryServices.Application.Exceptions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Managements.Locations.Fails.Exceptions;

public class LocationNotFoundException : NotFoundException
{
    public LocationNotFoundException(Error[] errors)
        : base(errors)
    {
    }
}