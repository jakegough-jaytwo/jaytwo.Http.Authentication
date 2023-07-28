using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace jaytwo.Http.Authentication.Tests;

public class BearerAuthSampleAppTests : IClassFixture<AuthenticationTestFixture>
{
    private readonly IHttpClient _client;

    public BearerAuthSampleAppTests(AuthenticationTestFixture fixture)
    {
        _client = fixture.CreateBearerAuthSampleAppHttpClient();
    }

    [Fact]
    public async Task GetHome_ReturnsOkWithoutBearerAuthenticationProvider()
    {
        // Arrange
        using var client = _client;
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/home", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the public insecure area.", content);
    }

    [Fact]
    public async Task GetSecure_ReturnsUnauthorizedWithoutBearerAuth()
    {
        // Arrange
        using var client = _client;
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
        var auth = new BearerAuthenticationProvider("noway");
        using var client = _client.WithAuthentication(auth);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSecure_ReturnsOkWithBearerAuthCredentials()
    {
        // Arrange
        var auth = new BearerAuthenticationProvider("helloworld");
        using var client = _client.WithAuthentication(auth);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the token auth secured area.", content);
    }
}
