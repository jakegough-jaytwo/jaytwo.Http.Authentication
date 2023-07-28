using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(HttpBinUrl);
    }

    [Fact]
    public async Task BasicAuth_Works()
    {
        // arrange
        var user = "hello";
        var pass = "world";

        var auth = new BasicAuthenticationProvider(user, pass);
        using var client = new HttpClient().Wrap().WithAuthentication(auth);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(HttpBinUrl + $"basic-auth/{user}/{pass}"));

        // act
        using var response = await client.SendAsync(request);

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HiddenBasicAuth_Works()
    {
        // arrange
        var user = "hello";
        var pass = "world";

        var auth = new BasicAuthenticationProvider(user, pass);
        using var client = new HttpClient().Wrap().WithAuthentication(auth);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(HttpBinUrl + $"hidden-basic-auth/{user}/{pass}"));

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
        var auth = new BearerAuthenticationProvider(token);
        using var client = new HttpClient().Wrap().WithAuthentication(auth);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(HttpBinUrl + $"bearer"));

        // act
        using var response = await client.SendAsync(request);

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
