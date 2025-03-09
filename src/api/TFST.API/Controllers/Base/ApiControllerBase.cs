using Microsoft.AspNetCore.Mvc;

namespace TFST.API.Controllers.Base;

[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
}
