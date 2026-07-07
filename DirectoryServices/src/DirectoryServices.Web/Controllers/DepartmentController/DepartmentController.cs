using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Managements.Departments.CreateDepartment;
using DirectoryServices.Application.Managements.Departments.Link.AddingLink;
using DirectoryServices.Application.Managements.Departments.Link.Unlink;
using DirectoryServices.Application.Managements.Departments.Update.UpdateDetails;
using DirectoryServices.Application.Managements.Departments.Update.UpdateLocationList;
using DirectoryServices.Contracts.DepartmentDto;
using DirectoryServices.Web.ResponseExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryServices.Web.Controllers.DepartmentController;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    [HttpPost("/api/departments")]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateDepartmentCommand> handler,
        [FromBody] CreateDepartmentDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPatch("/api/departments/{departmentId}")]
    public async Task<IActionResult> UpdateDepartmentDetails(
        [FromRoute] Guid departmentId,
        [FromServices] ICommandHandler<Guid, UpdateDepartmentDetailsCommand> handler,
        [FromBody] UpdateDepartmentDetailsDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDepartmentDetailsCommand(departmentId, request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPatch("/api/departments/{departmentId}/locations")]
    public async Task<IActionResult> UpdateLocationList(
        [FromRoute] Guid departmentId,
        [FromServices] ICommandHandler<Guid, UpdateLocationListCommand> handler,
        [FromBody] UpdateLocationListDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLocationListCommand(departmentId, request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("/api/departments/{departmentId}/locations/{locationId}")]
    public async Task<IActionResult> AddLinkLocation(
        [FromRoute] Guid departmentId,
        [FromRoute] Guid locationId,
        [FromServices] ICommandHandler<Guid, AddLinkCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddLinkCommand(departmentId, locationId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("/api/departments/{departmentId}/locations/{locationId}")]
    public async Task<IActionResult> UnlinkLocation(
        [FromRoute] Guid departmentId,
        [FromRoute] Guid locationId,
        [FromServices] ICommandHandler<Guid, UnlinkDepartmentLocationCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UnlinkDepartmentLocationCommand(departmentId, locationId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}