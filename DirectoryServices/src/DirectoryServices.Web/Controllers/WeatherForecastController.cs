using Microsoft.AspNetCore.Mvc;

namespace DirectoryServices.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpPost]
    public void Create()
    { }
}