using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Locations.CreateLocation;
using DirectoryServices.Web.Controllers.LocationControllers.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryServices.Web.Controllers.LocationControllers;

[ApiController]
[Route("[controller]")]
public class LocationControllers : ControllerBase
{
    [HttpPost("/api/locations")]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateLocationCommand> handler,
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLocationCommand(request.LocationName, request.AddressDto, request.Timezone);

        var result = await handler.Handle(command, cancellationToken);

        return Ok(result.Value);
    }
}