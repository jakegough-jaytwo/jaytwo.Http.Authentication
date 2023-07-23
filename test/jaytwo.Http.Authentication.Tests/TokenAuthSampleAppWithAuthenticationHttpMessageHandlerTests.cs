using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace jaytwo.Http.Authentication.Tests;

public class TokenAuthSampleAppWithAuthenticationHttpMessageHandlerTests : IClassFixture<TokenAuthSampleAppWebApplicationFactory>
{
    private readonly TokenAuthSampleAppWebApplicationFactory _fixture;

    public TokenAuthSampleAppWithAuthenticationHttpMessageHandlerTests(TokenAuthSampleAppWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetSecure_ReturnsUnauthorizedWithIncorrectCredentials()
    {
        // Arrange
        var auth = new TokenAuthenticationProvider("noway");
        using var handler = new AuthenticationHttpMessageHandler(auth, _fixture.Server.CreateHandler());
        using var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSecure_ReturnsOkWithTokenAuthCredentials__token_as_string()
    {
        // Arrange
        var auth = new TokenAuthenticationProvider("helloworld");
        using var handler = new AuthenticationHttpMessageHandler(auth, _fixture.Server.CreateHandler());
        using var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the token auth secured area.", content);
    }

    [Fact]
    public async Task GetSecure_ReturnsOkWithTokenAuthCredentials__token_as_delegate()
    {
        // Arrange
        var auth = new TokenAuthenticationProvider("helloworld");
        var handler = new AuthenticationHttpMessageHandler(auth, _fixture.Server.CreateHandler());
        using var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/secure", UriKind.Relative));

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Welcome to the token auth secured area.", content);
    }

    [Fact]
    public async Task GetSecure_ReturnsOkWithTokenAuthCredentials__token_from_provider()
    {
        // Arrange
        var mockTokenProvider = new Mock<ITokenProvider>();
        mockTokenProvider.Setup(x => x.GetTokenAsync()).ReturnsAsync("helloworld");

        using var handler = new AuthenticationHttpMessageHandler(
            new TokenAuthenticationProvider(mockTokenProvider.Object),
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
