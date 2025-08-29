using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuthSampleApp.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = "Basic")]
public class SecureController : ControllerBase
{
    public SecureController()
    {
    }

    [HttpGet]
    public string Get()
    {
        return "Welcome to the basic auth secured area.";
    }
}
