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
            var client = _fixture.CreateClient();

            // Act
            var response = await client.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath("/home");
            });

            // Assert
            using (response)
            {
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Assert.Equal("Welcome to the public insecure area.", content);
            }
        }

        [Fact]
        public async Task GetSecure_ReturnsUnauthorizedWithoutTokenAuth()
        {
            // Arrange
            var client = _fixture.CreateClient();

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
        public async Task GetSecure_ReturnsUnauthorizedWithIncorrectCredentials()
        {
            // Arrange
            var client = _fixture.CreateClient();

            // Act
            var response = await client.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath("/secure")
                    .WithTokenAuthentication("noway");
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
            var client = _fixture.CreateClient();

            // Act
            var response = await client.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath("/secure")
                    .WithTokenAuthentication("helloworld");
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
