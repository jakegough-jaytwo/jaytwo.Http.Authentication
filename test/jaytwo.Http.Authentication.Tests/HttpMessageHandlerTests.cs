using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using jaytwo.FluentHttp;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.Http.Authentication.Tests;

public class HttpMessageHandlerTests
{
    public const string HttpBinUrl = "http://httpbin.jaytwo.com/";

    private readonly ITestOutputHelper _output;

    public HttpMessageHandlerTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task BasicAuth_Works()
    {
        // arrange
        var user = "hello";
        var pass = "world";

        using var handler = new AuthenticationHttpMessageHandler(
            new BasicAuthenticationProvider(user, pass),
            new HttpClientHandler());

        using (var client = GetHttpClient(handler))
        {
            // act
            var response = await client.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath($"/basic-auth/{user}/{pass}");
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }

    [Fact]
    public async Task TokenAuth_Works()
    {
        // arrange
        var token = "hello";
        using var handler = new AuthenticationHttpMessageHandler(
            new TokenAuthenticationProvider(token),
            new HttpClientHandler());

        using (var client = GetHttpClient(handler))
        {
            // act
            var response = await client.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath($"/bearer");
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }

    private HttpClient GetHttpClient(HttpMessageHandler handler)
    {
        return new HttpClient(handler).WithBaseAddress(HttpBinUrl);
    }
}
