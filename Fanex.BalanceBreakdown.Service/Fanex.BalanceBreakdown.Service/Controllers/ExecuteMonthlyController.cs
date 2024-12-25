using Fanex.BalanceBreakdown.Business.Services;
using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.BalanceBreakdown.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class ExecuteMonthlyController(MainService service) : ControllerBase
{
    [HttpGet]
    public JobMessage Get()
    {
        return service.Execute(false);
    }
}