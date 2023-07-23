using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace jaytwo.Http.Authentication.Tests;

public static class WebApplicationFactoryHelpers
{
    public static HttpClient CreateHttpClient<T>(WebApplicationFactory<T> webApplicationFactory, HttpMessageHandler handler)
        where T : class
    {
        var asDelegatingHandler = handler as DelegatingHandler;
        if (asDelegatingHandler != null && asDelegatingHandler.InnerHandler == null)
        {
            asDelegatingHandler.InnerHandler = new HttpClientHandler();
        }

        return new HttpClient(handler) { BaseAddress = webApplicationFactory.Server.BaseAddress };
    }
}
