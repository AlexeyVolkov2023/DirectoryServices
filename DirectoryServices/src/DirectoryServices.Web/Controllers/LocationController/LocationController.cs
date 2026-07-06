using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Managements.Locations.CreateLocation;
using DirectoryServices.Application.Managements.Locations.Delete.SoftDelete;
using DirectoryServices.Application.Managements.Locations.Update.UpdateDetailsLocation;
using DirectoryServices.Application.Managements.Locations.Update.UpdateLocationAddress;
using DirectoryServices.Application.Managements.Locations.Update.UpdateLocationName;
using DirectoryServices.Contracts.DepartmentDto;
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

    [HttpPatch("PATCH /locations/{locationId}")]
    public async Task<IActionResult> UpdateDetails(
        [FromRoute] Guid locationId,
        [FromServices] ICommandHandler<Guid, UpdateLocationDetailsCommand> handler,
        [FromBody] LocationDetailsDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLocationDetailsCommand(locationId, request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPatch("PATCH /locations/{locationId}/address")]
    public async Task<IActionResult> UpdateLocationAddress(
        [FromRoute] Guid locationId,
        [FromServices] ICommandHandler<Guid, UpdateLocationAddressCommand> handler,
        [FromBody] UpdateAddressDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLocationAddressCommand(locationId, request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPatch("PATCH /locations/{locationId}/locationName")]
    public async Task<IActionResult> UpdateLocationName(
        [FromRoute] Guid locationId,
        [FromServices] ICommandHandler<Guid, UpdateLocationNameCommand> handler,
        [FromBody] string locationName,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLocationNameCommand(locationId, locationName);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("DELETE /locations/{locationId}")]
    public async Task<IActionResult> SoftDeleteLocation(
        [FromRoute] Guid locationId,
        [FromServices] ICommandHandler<Guid, SoftDeleteLocationCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new SoftDeleteLocationCommand(locationId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}