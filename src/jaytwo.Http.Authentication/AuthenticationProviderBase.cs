using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace jaytwo.Http.Authentication;

public abstract class AuthenticationProviderBase : IAuthenticationProvider
{
    // override either of these methods, but don't do both

    public virtual Task AuthenticateAsync(HttpRequestMessage request)
    {
        return Task.CompletedTask;
    }

    protected void SetRequestAuthenticationHeader(HttpRequestMessage request, string headerValue)
    {
        request.Headers.Authorization = AuthenticationHeaderValue.Parse(headerValue);
    }

    protected void SetRequestAuthenticationHeader(HttpRequestMessage request, string scheme, string parameter)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
    }
}
