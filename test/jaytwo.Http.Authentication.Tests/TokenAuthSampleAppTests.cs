using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace jaytwo.Http.Authentication.Tests;

public class TokenAuthSampleAppTests : IClassFixture<TokenAuthSampleAppWebApplicationFactory>
{
    private readonly TokenAuthSampleAppWebApplicationFactory _fixture;

    public TokenAuthSampleAppTests(TokenAuthSampleAppWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetHome_ReturnsOkWithoutTokenAuthenticationProvider()
    {
        // Arrange
        using var client = _fixture.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/home", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the public insecure area.", content);
    }

    [Fact]
    public async Task GetSecure_ReturnsUnauthorizedWithoutTokenAuth()
    {
        // Arrange
        using var client = _fixture.CreateClient();
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
        using var handler = new AuthenticationHttpMessageHandler(
            new TokenAuthenticationProvider("noway"),
            _fixture.Server.CreateHandler());

        using var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSecure_ReturnsOkWithTokenAuthCredentials()
    {
        // Arrange
        using var handler = new AuthenticationHttpMessageHandler(
            new TokenAuthenticationProvider("helloworld"),
            _fixture.Server.CreateHandler());

        using var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the token auth secured area.", content);
    }
}
