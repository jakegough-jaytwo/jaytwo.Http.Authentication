using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(HttpBinUrl + $"/basic-auth/{user}/{pass}"));

        // act
        using var response = await client.SendAsync(request);

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TokenAuth_Works()
    {
        // arrange
        var token = "hello";
        using var handler = new AuthenticationHttpMessageHandler(
            new TokenAuthenticationProvider(token),
            new HttpClientHandler());

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(HttpBinUrl + "/bearer"));

        // act
        using var response = await client.SendAsync(request);

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
