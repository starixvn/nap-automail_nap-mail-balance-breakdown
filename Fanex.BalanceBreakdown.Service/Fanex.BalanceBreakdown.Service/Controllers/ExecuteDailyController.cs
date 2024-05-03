using Fanex.BalanceBreakdown.Business.Services;
using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.BalanceBreakdown.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class ExecuteDailyController : ControllerBase
{
    private readonly MainService service;

    public ExecuteDailyController(MainService service)
    {
        this.service = service;
    }

    [HttpGet]
    public JobMessage Get()
    {
        return service.Execute(true);
    }
}