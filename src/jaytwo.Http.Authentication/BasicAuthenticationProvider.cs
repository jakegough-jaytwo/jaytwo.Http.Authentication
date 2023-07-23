using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.Http.Authentication;

public class BasicAuthenticationProvider : AuthenticationProviderBase, IAuthenticationProvider
{
    private readonly string _user;
    private readonly string _pass;

    public BasicAuthenticationProvider(string user, string pass)
    {
        _user = user;
        _pass = pass;
    }

    public override Task AuthenticateAsync(HttpRequestMessage request)
    {
        var combined = $"{_user}:{_pass}";
        var bytes = Encoding.UTF8.GetBytes(combined);
        var base64 = Convert.ToBase64String(bytes);

        SetRequestAuthenticationHeader(request, "Basic", base64);
        return Task.CompletedTask;
    }
}
