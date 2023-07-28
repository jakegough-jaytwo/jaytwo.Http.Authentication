using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace jaytwo.Http.Authentication.Tests;

public class IHttpClientExtensionsTests : IClassFixture<AuthenticationTestFixture>
{
    public IHttpClientExtensionsTests()
    {
    }

    [Fact]
    public void WithAuthentication_Authentication_Provider()
    {
        // Arrange
        var auth = new BasicAuthenticationProvider("hello", "world");
        var mock = new Mock<IHttpClient>();

        // Act
        var wrapped = mock.Object.WithAuthentication(auth);

        // Assert
        var typed = Assert.IsType<AuthenticationWrapper>(wrapped);
        Assert.Same(auth, typed.AuthenticationProvider);
    }

    [Fact]
    public void WithBasicAuthentication_username_password()
    {
        // Arrange
        var user = "hello";
        var pass = "world";
        var mock = new Mock<IHttpClient>();

        // Act
        var wrapped = mock.Object.WithBasicAuthentication(user, pass);

        // Assert
        var typed = Assert.IsType<AuthenticationWrapper>(wrapped);
        var auth = Assert.IsType<BasicAuthenticationProvider>(typed.AuthenticationProvider);
        Assert.Equal(user, auth.User);
        Assert.Equal(pass, auth.Password);
    }

    [Fact]
    public async Task WithBearerAuthentication_token()
    {
        // Arrange
        var token = "hello";
        var mock = new Mock<IHttpClient>();

        // Act
        var wrapped = mock.Object.WithBearerAuthentication(token);

        // Assert
        var typed = Assert.IsType<AuthenticationWrapper>(wrapped);
        var auth = Assert.IsType<BearerAuthenticationProvider>(typed.AuthenticationProvider);
        Assert.Equal(token, await auth.TokenProvider.Invoke());
    }

    [Fact]
    public async Task WithBearerAuthentication_token_delegate()
    {
        // Arrange
        var token = "hello";
        var mock = new Mock<IHttpClient>();

        // Act
        var wrapped = mock.Object.WithBearerAuthentication(() => token);

        // Assert
        var typed = Assert.IsType<AuthenticationWrapper>(wrapped);
        var auth = Assert.IsType<BearerAuthenticationProvider>(typed.AuthenticationProvider);
        Assert.Equal(token, await auth.TokenProvider.Invoke());
    }

    [Fact]
    public async Task WithBearerAuthentication_token_provider()
    {
        // Arrange
        var token = "hello";
        var mockHttpClient = new Mock<IHttpClient>();
        var mockTokenProvider = new Mock<IBearerTokenProvider>();
        mockTokenProvider.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);

        // Act
        var wrapped = mockHttpClient.Object.WithBearerAuthentication(mockTokenProvider.Object);

        // Assert
        var typed = Assert.IsType<AuthenticationWrapper>(wrapped);
        var auth = Assert.IsType<BearerAuthenticationProvider>(typed.AuthenticationProvider);
        Assert.Equal(token, await auth.TokenProvider.Invoke());
    }
}
