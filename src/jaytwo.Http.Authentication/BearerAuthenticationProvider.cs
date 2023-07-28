using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace jaytwo.Http.Authentication;

public class BearerAuthenticationProvider : AuthenticationProviderBase, IAuthenticationProvider
{
    public BearerAuthenticationProvider(string token)
        : this(() => token)
    {
    }

    public BearerAuthenticationProvider(Func<string> tokenDelegate)
        : this(() => Task.FromResult(tokenDelegate.Invoke()))
    {
    }

    public BearerAuthenticationProvider(Func<Task<string>> tokenProvider)
    {
        TokenProvider = tokenProvider;
    }

    public BearerAuthenticationProvider(IBearerTokenProvider tokenProvider)
        : this(tokenProvider.GetTokenAsync)
    {
    }

    protected internal Func<Task<string>> TokenProvider { get; private set; }

    public override async Task AuthenticateAsync(IHttpClient httpClient, HttpRequestMessage request)
    {
        var token = await TokenProvider.Invoke();
        SetRequestAuthenticationHeader(request, "Bearer", token);
    }
}
