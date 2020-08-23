using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using jaytwo.FluentHttp;
using Xunit;

namespace jaytwo.Http.Authentication.Tests
{
    public class TokenAuthSampleAppWithAuthenticationDelegatingHandlerTests : IClassFixture<TokenAuthSampleAppWebApplicationFactory>
    {
        private readonly TokenAuthSampleAppWebApplicationFactory _fixture;

        public TokenAuthSampleAppWithAuthenticationDelegatingHandlerTests(TokenAuthSampleAppWebApplicationFactory fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetSecure_ReturnsUnauthorizedWithIncorrectCredentials()
        {
            // Arrange
            var handler = new AuthenticationDelegatingHandler(new TokenAuthenticationProvider("noway"));
            var client = _fixture.CreateDefaultClient(handler);

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

        [Fact]
        public async Task GetSecure_ReturnsOkWithTokenAuthCredentials()
        {
            // Arrange
            var handler = new AuthenticationDelegatingHandler(new TokenAuthenticationProvider("helloworld"));
            var client = _fixture.CreateDefaultClient(handler);

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
