using Microsoft.AspNetCore.Mvc;
using TFST.API.Controllers.Base;

namespace TFST.API.Controllers.Admin;

[Area("Admin")]
[Tags("Admin")]
[Route("admin/v1/[controller]")]
public class DashboardController : ApiControllerBase
{
    [HttpGet]
    public IActionResult GetAdminDashboard() => Ok(new { Message = "Admin dashboard" });
}
