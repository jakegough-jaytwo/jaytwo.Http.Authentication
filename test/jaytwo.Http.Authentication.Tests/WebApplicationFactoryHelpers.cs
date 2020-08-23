using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using jaytwo.FluentHttp;
using Microsoft.AspNetCore.Mvc.Testing;

namespace jaytwo.Http.Authentication.Tests
{
    public static class WebApplicationFactoryHelpers
    {
        public static HttpClient CreateHttpClient<T>(WebApplicationFactory<T> webApplicationFactory, HttpMessageHandler handler)
            where T : class
        {
            return new HttpClient(handler).WithBaseAddress(webApplicationFactory.Server.BaseAddress);
        }
    }
}
