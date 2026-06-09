using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Managements.Positions.CreatePosition;
using DirectoryServices.Contracts.PositionDto;
using DirectoryServices.Web.ResponseExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryServices.Web.Controllers.PositionController;

[ApiController]
[Route("[controller]")]
public class PositionController : ControllerBase
{
    [HttpPost("/api/positions")]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreatePositionCommand> handler,
        [FromBody] CreatePositionDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePositionCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}