using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace jaytwo.Http.Authentication.Tests;

public class BasicAuthSampleAppTests : IClassFixture<AuthenticationTestFixture>
{
    private readonly IHttpClient _client;

    public BasicAuthSampleAppTests(AuthenticationTestFixture fixture)
    {
        _client = fixture.CreateBasicAuthSampleAppHttpClient();
    }

    [Fact]
    public async Task GetHome_ReturnsOkWithoutBasicAuthenticationProvider()
    {
        // Arrange
        var client = _client;
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/home", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the public insecure area.", content);
    }

    [Fact]
    public async Task GetSecure_ReturnsUnauthorizedWithoutBasicAuth()
    {
        // Arrange
        var client = _client;
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSecure_ReturnsUnauthorizedWithIncorrectCredentials()
    {
        // Arrange
        var auth = new BasicAuthenticationProvider("noUser", "noPassword");
        using var client = _client.WithAuthentication(auth);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSecure_ReturnsOkWithBasicAuthCredentials()
    {
        // Arrange
        var auth = new BasicAuthenticationProvider("hello", "world");
        using var client = _client.WithAuthentication(auth);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the basic auth secured area.", content);
    }
}
