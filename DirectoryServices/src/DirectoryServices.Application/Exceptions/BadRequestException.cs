using System.Text.Json;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Exceptions;

public class BadRequestException : Exception
{
    protected BadRequestException(Error[] errors)
        : base(JsonSerializer.Serialize(errors))
    {
    }
}