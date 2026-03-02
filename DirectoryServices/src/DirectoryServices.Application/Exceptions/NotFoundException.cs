using System.Text.Json;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Exceptions;

public class NotFoundException : Exception
{
    protected NotFoundException(Error[] errors)
        : base(JsonSerializer.Serialize(errors))
    {
    }
}