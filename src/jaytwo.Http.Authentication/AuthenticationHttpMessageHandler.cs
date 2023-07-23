using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.Http.Authentication;

public class AuthenticationHttpMessageHandler : DelegatingHandler
{
    private readonly IAuthenticationProvider _authenticationProvider;

    public AuthenticationHttpMessageHandler(IAuthenticationProvider authenticationProvider, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        _authenticationProvider = authenticationProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await _authenticationProvider.AuthenticateAsync(request);
        return await base.SendAsync(request, cancellationToken);
    }
}
