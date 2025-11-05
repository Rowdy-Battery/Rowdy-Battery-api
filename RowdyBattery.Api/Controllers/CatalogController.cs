using Microsoft.AspNetCore.Mvc;

namespace RowdyBattery.Api.Controllers;

[ApiController]
[Route("[controller]")] // => /catalog
public class CatalogController : ControllerBase
{
    // Part 1: start with a simple "hello world"
    [HttpGet]
    public IActionResult Get() => Ok("hello world.");
}
