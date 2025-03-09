﻿using Microsoft.AspNetCore.Mvc;
using TFST.API.Controllers.Base;

namespace TFST.API.Controllers.Identity;

[Tags("Identity")]
public class RolesController : ApiControllerBase
{
    [HttpGet]
    public IActionResult GetProfiles() => Ok(new { Message = "Profiles endpoint" });
}