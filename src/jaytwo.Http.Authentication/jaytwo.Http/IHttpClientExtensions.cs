using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.Http.Authentication;

namespace jaytwo.Http;

public static class IHttpClientExtensions
{
    public static IHttpClient WithAuthentication(this IHttpClient httpClient, IAuthenticationProvider authenticationProvider)
        => new AuthenticationWrapper(httpClient, authenticationProvider);

    public static IHttpClient WithBasicAuthentication(this IHttpClient httpClient, string user, string pass)
        => httpClient.WithAuthentication(new BasicAuthenticationProvider(user, pass));

    public static IHttpClient WithBearerAuthentication(this IHttpClient httpClient, string token)
        => httpClient.WithAuthentication(new BearerAuthenticationProvider(token));

    public static IHttpClient WithBearerAuthentication(this IHttpClient httpClient, Func<string> tokenDelegate)
        => httpClient.WithAuthentication(new BearerAuthenticationProvider(tokenDelegate));

    public static IHttpClient WithBearerAuthentication(this IHttpClient httpClient, IBearerTokenProvider tokenProvider)
       => httpClient.WithAuthentication(new BearerAuthenticationProvider(tokenProvider));
}
