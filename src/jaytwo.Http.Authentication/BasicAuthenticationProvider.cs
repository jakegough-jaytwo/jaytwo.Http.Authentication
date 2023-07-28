using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.Http.Authentication;

public class BasicAuthenticationProvider : AuthenticationProviderBase, IAuthenticationProvider
{
    public BasicAuthenticationProvider(string user, string pass)
    {
        User = user;
        Password = pass;
    }

    protected internal string User { get; private set; }

    protected internal string Password { get; private set; }

    public override Task AuthenticateAsync(IHttpClient httpClient, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var combined = $"{User}:{Password}";
        var bytes = Encoding.UTF8.GetBytes(combined);
        var base64 = Convert.ToBase64String(bytes);

        SetRequestAuthenticationHeader(request, "Basic", base64);
        return Task.CompletedTask;
    }
}
