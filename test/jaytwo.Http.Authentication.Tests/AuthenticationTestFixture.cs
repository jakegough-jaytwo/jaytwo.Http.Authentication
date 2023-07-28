using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
