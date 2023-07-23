using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using jaytwo.FluentHttp;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.Http.Authentication.Tests;

public class HttpClientTests
{
    public const string HttpBinUrl = "http://httpbin.jaytwo.com/";

    private readonly HttpClient _httpClient;
    private readonly ITestOutputHelper _output;

    public HttpClientTests(ITestOutputHelper output)
    {
        _output = output;
        _httpClient = new HttpClient().WithBaseAddress(HttpBinUrl);
    }

    [Fact]
    public async Task BasicAuth_Works()
    {
        // arrange
        var user = "hello";
        var pass = "world";

        var auth = new BasicAuthenticationProvider(user, pass);
        using var handler = new AuthenticationHttpMessageHandler(auth, new SocketsHttpHandler());
        using var client = new HttpClient(handler).WithBaseAddress(HttpBinUrl);

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

    [Fact]
    public async Task HiddenBasicAuth_Works()
    {
        // arrange
        var user = "hello";
        var pass = "world";

        var auth = new BasicAuthenticationProvider(user, pass);
        using var handler = new AuthenticationHttpMessageHandler(auth, new SocketsHttpHandler());
        using var client = new HttpClient(handler).WithBaseAddress(HttpBinUrl);

        // act
        var response = await client.SendAsync(request =>
        {
            request
                .WithMethod(HttpMethod.Get)
                .WithUriPath($"/hidden-basic-auth/{user}/{pass}");
        });

        using (response)
        {
            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

    [Fact]
    public async Task TokenAuth_Works()
    {
        // arrange
        var token = "hello";
        var auth = new TokenAuthenticationProvider(token);
        using var handler = new AuthenticationHttpMessageHandler(auth, new SocketsHttpHandler());
        using var client = new HttpClient(handler).WithBaseAddress(HttpBinUrl);

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
