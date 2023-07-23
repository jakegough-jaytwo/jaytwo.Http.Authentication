using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using jaytwo.FluentHttp;
using Microsoft.AspNetCore.Mvc.Testing;

namespace jaytwo.Http.Authentication.Tests;

public static class WebApplicationFactoryHelpers
{
    public static HttpClient CreateHttpClient<T>(WebApplicationFactory<T> webApplicationFactory, HttpMessageHandler handler)
        where T : class
    {
        // TODO: use jaytwo.FluentHttp.HttpMessageHandlerExtensions.CreateClient() once it exists
        var asDelegatingHandler = handler as DelegatingHandler;
        if (asDelegatingHandler != null && asDelegatingHandler.InnerHandler == null)
        {
            asDelegatingHandler.InnerHandler = new HttpClientHandler();
        }

        return new HttpClient(handler).WithBaseAddress(webApplicationFactory.Server.BaseAddress);
    }
}
