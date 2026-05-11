using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Managements.Locations.CreateLocation;
using DirectoryServices.Contracts.LocationDtos;
using DirectoryServices.Web.ResponseExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryServices.Web.Controllers.LocationController;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{
    [HttpPost("/api/locations")]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateLocationCommand> handler,
        [FromBody] CreateLocationDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLocationCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}