using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Locations.CreateLocation;
using DirectoryServices.Contracts.LocationDtos;
using DirectoryServices.Domain.Shared;
using DirectoryServices.Web.ResponseExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryServices.Web.Controllers.LocationControllers;

[ApiController]
[Route("[controller]")]
public class LocationControllers : ControllerBase
{
    [HttpPost("/api/locations")]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateLocationCommand> handler,
        [FromBody] CreateLocationDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLocationCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure ? BadRequest((Envelope.Fail(result.Error))) : Ok(Envelope.Ok(result.Value));
    }
}