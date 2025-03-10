using Microsoft.AspNetCore.Mvc;

namespace TFST.SharedKernel.Presentation;

[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
}
