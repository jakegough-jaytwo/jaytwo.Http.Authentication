using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jaytwo.SolutionResolution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace jaytwo.Http.Authentication.Tests;

public class AuthenticationTestFixture
{
    private readonly BasicAuthSampleAppWebApplicationFactory _basicAuthSampleAppWebApplicationFactory = new BasicAuthSampleAppWebApplicationFactory();
    private readonly BearerAuthSampleAppWebApplicationFactory _bearerAuthSampleAppWebApplicationFactory = new BearerAuthSampleAppWebApplicationFactory();

    public AuthenticationTestFixture()
    {
    }

    public IHttpClient CreateBasicAuthSampleAppHttpClient()
        => _basicAuthSampleAppWebApplicationFactory.CreateClient().Wrap();

    public IHttpClient CreateBearerAuthSampleAppHttpClient()
        => _bearerAuthSampleAppWebApplicationFactory.CreateClient().Wrap();

    private class BasicAuthSampleAppWebApplicationFactory
        : WebApplicationFactory<BasicAuthSampleApp.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var contentRoot = new SlnFileResolver().ResolvePathRelativeToSln("test/BasicAuthSampleApp");
            builder.UseContentRoot(contentRoot);
        }
    }

    private class BearerAuthSampleAppWebApplicationFactory
        : WebApplicationFactory<BearerAuthSampleApp.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var contentRoot = new SlnFileResolver().ResolvePathRelativeToSln("test/BearerAuthSampleApp");
            builder.UseContentRoot(contentRoot);
        }
    }
}
