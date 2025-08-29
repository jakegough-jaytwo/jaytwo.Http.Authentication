using System;
using Microsoft.AspNetCore.Mvc;

namespace BearerAuthSampleApp.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    public HomeController()
    {
    }

    [HttpGet]
    public string Get()
    {
        return "Welcome to the public insecure area.";
    }
}
