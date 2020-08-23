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
            var handler = _fixture.Server.CreateHandler().WithBasicAuthentication("noUser", "noPassword");
            var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);

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
        public async Task GetSecure_ReturnsOkWithBasicAuthCredentials()
        {
            // Arrange
            var handler = _fixture.Server.CreateHandler().WithBasicAuthentication("hello", "world");
            var client = WebApplicationFactoryHelpers.CreateHttpClient(_fixture, handler);

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
                Assert.Equal("Welcome to the basic auth secured area.", content);
            }
        }
    }
}
