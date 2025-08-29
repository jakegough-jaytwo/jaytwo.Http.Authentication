using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
