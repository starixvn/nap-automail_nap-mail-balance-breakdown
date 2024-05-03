using Fanex.BalanceBreakdown.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Fanex.BalanceBreakdown.Service.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        if (AppConfigService.IsDone)
        {
            return Problem(
                detail: "Time to shutdown",
                instance: Environment.MachineName,
                statusCode: (int)HttpStatusCode.ServiceUnavailable);
        }

        return Ok($"{Environment.MachineName}");
    }
}