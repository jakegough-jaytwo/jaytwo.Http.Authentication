using System;
using System.Collections.Generic;
using System.Text;
using jaytwo.SolutionResolution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace jaytwo.Http.Authentication.Tests
{
    public class BasicAuthSampleAppWebApplicationFactory
        : WebApplicationFactory<BasicAuthSampleApp.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var contentRoot = new SlnFileResolver().ResolvePathRelativeToSln("test/BasicAuthSampleApp");
            builder.UseContentRoot(contentRoot);
        }
    }
}
