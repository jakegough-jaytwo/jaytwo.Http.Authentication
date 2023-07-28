using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace jaytwo.Http.Authentication.Tests;

public class BearerAuthSampleAppWithAuthenticationHttpMessageHandlerTests : IClassFixture<AuthenticationTestFixture>
{
    private readonly IHttpClient _client;

    public BearerAuthSampleAppWithAuthenticationHttpMessageHandlerTests(AuthenticationTestFixture fixture)
    {
        _client = fixture.CreateBearerAuthSampleAppHttpClient();
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
    public async Task GetSecure_ReturnsOkWithBearerAuthCredentials__token_as_string()
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

    [Fact]
    public async Task GetSecure_ReturnsOkWithBearerAuthCredentials__token_as_delegate()
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

    [Fact]
    public async Task GetSecure_ReturnsOkWithBearerAuthCredentials__token_from_provider()
    {
        // Arrange
        var mockTokenProvider = new Mock<IBearerTokenProvider>();
        mockTokenProvider.Setup(x => x.GetTokenAsync()).ReturnsAsync("helloworld");

        var auth = new BearerAuthenticationProvider(mockTokenProvider.Object);
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
