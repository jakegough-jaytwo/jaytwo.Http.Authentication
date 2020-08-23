using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp;
using Moq;
using Xunit;

namespace jaytwo.Http.Authentication.Tests
{
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
            var handler = _fixture.Server.CreateHandler().WithTokenAuthentication("noway");

            using (var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler))
            {
                // Act
                var response = await client.SendAsync(request =>
                {
                    request
                        .WithMethod(HttpMethod.Get)
                        .WithUriPath("/secure");
                });

                // Assert
                using (response)
                {
                    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                }
            }
        }

        [Fact]
        public async Task GetSecure_ReturnsOkWithTokenAuthCredentials__token_as_string()
        {
            // Arrange
            var handler = _fixture.Server.CreateHandler().WithTokenAuthentication("helloworld");

            using (var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler))
            {
                // Act
                var response = await client.SendAsync(request =>
                {
                    request
                        .WithMethod(HttpMethod.Get)
                        .WithUriPath("/secure");
                });

                // Assert
                using (response)
                {
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    Assert.Equal("Welcome to the token auth secured area.", content);
                }
            }
        }

        [Fact]
        public async Task GetSecure_ReturnsOkWithTokenAuthCredentials__token_as_delegate()
        {
            // Arrange
            var handler = _fixture.Server.CreateHandler().WithTokenAuthentication(() => "helloworld");

            using (var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler))
            {
                // Act
                var response = await client.SendAsync(request =>
                {
                    request
                        .WithMethod(HttpMethod.Get)
                        .WithUriPath("/secure");
                });

                // Assert
                using (response)
                {
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    Assert.Equal("Welcome to the token auth secured area.", content);
                }
            }
        }

        [Fact]
        public async Task GetSecure_ReturnsOkWithTokenAuthCredentials__token_from_provider()
        {
            // Arrange
            var mockTokenProvider = new Mock<ITokenProvider>();
            mockTokenProvider.Setup(x => x.GetTokenAsync()).ReturnsAsync("helloworld");

            var handler = _fixture.Server.CreateHandler().WithTokenAuthentication(mockTokenProvider.Object);

            using (var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler))
            {
                // Act
                var response = await client.SendAsync(request =>
                {
                    request
                        .WithMethod(HttpMethod.Get)
                        .WithUriPath("/secure");
                });

                // Assert
                using (response)
                {
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    Assert.Equal("Welcome to the token auth secured area.", content);
                }
            }
        }
    }
}
