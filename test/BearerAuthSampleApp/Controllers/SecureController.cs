using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BearerAuthSampleApp.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = "Token")]
public class SecureController : ControllerBase
{
    public SecureController()
    {
    }

    [HttpGet]
    public string Get()
    {
        return "Welcome to the token auth secured area.";
    }
}
