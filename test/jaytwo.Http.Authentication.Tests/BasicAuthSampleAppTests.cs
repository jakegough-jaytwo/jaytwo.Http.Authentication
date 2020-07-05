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
    public class BasicAuthSampleAppTests : IClassFixture<BasicAuthSampleAppWebApplicationFactory>
    {
        private readonly BasicAuthSampleAppWebApplicationFactory _fixture;

        public BasicAuthSampleAppTests(BasicAuthSampleAppWebApplicationFactory fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetHome_ReturnsOkWithoutBasicAuthenticationProvider()
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
        public async Task GetSecure_ReturnsUnauthorizedWithoutBasicAuth()
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
                    .WithBasicAuthentication("noUser", "noPassword");
            });

            // Assert
            using (response)
            {
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetSecure_ReturnsOkWithBasicAuthCredentials()
        {
            // Arrange
            var client = _fixture.CreateClient();

            // Act
            var response = await client.SendAsync(request =>
            {
                request
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath("/secure")
                    .WithBasicAuthentication("hello", "world");
            });

            // Assert
            using (response)
            {
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Assert.Equal("Welcome to the basic auth secured area.", content);
            }
        }
    }
}
