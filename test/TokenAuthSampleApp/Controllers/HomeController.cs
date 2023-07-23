using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TokenAuthSampleApp.Controllers;

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
