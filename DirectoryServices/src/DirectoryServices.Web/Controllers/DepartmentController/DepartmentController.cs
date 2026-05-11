using DirectoryServices.Application.Abstractions;
using DirectoryServices.Application.Managements.Departments.CreateDepartment;
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
}