using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace jaytwo.Http.Authentication.Tests;

public class BasicAuthSampleAppWithAuthenticationHttpMessageHandlerTests : IClassFixture<BasicAuthSampleAppWebApplicationFactory>
{
    private readonly BasicAuthSampleAppWebApplicationFactory _fixture;

    public BasicAuthSampleAppWithAuthenticationHttpMessageHandlerTests(BasicAuthSampleAppWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetSecure_ReturnsUnauthorizedWithIncorrectCredentials()
    {
        // Arrange
        using var handler = new AuthenticationHttpMessageHandler(
            new BasicAuthenticationProvider("noUser", "noPassword"),
            _fixture.Server.CreateHandler());

        using var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);
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
        using var handler = new AuthenticationHttpMessageHandler(
            new BasicAuthenticationProvider("hello", "world"),
            _fixture.Server.CreateHandler());

        using var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the basic auth secured area.", content);
    }
}
