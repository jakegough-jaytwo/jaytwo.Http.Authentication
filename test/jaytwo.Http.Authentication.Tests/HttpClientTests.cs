using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using jaytwo.FluentHttp;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.Http.Tests
{
    public class HttpClientTests
    {
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;

        public HttpClientTests(ITestOutputHelper output)
        {
            _output = output;
            _httpClient = new HttpClient().WithBaseAddress("http://httpbin.org");
        }

        [Fact]
        public async Task BasicAuth_Works()
        {
            // arrange
            var user = "hello";
            var pass = "world";

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithBasicAuthentication(user, pass)
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath($"/basic-auth/{user}/{pass}");
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task HiddenBasicAuth_Works()
        {
            // arrange
            var user = "hello";
            var pass = "world";

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithBasicAuthentication(user, pass)
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath($"/hidden-basic-auth/{user}/{pass}");
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task TokenAuth_Works()
        {
            // arrange
            var token = "hello";

            // act
            var response = await _httpClient.SendAsync(request =>
            {
                request
                    .WithTokenAuthentication(token)
                    .WithMethod(HttpMethod.Get)
                    .WithUriPath($"/bearer");
            });

            using (response)
            {
                // assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
