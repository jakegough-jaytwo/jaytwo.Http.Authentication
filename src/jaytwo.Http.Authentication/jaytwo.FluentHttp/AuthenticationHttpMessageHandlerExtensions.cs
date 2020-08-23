using System;
using System.Net.Http;
using jaytwo.Http.Authentication;

namespace jaytwo.FluentHttp
{
    public static class AuthenticationHttpMessageHandlerExtensions
    {
        public static HttpMessageHandler WithAuthentication(this HttpMessageHandler innerHandler, IAuthenticationProvider authenticationProvider)
        {
            return new AuthenticationHttpMessageHandler(authenticationProvider, innerHandler);
        }

        public static HttpMessageHandler WithBasicAuthentication(this HttpMessageHandler innerHandler, string user, string pass)
        {
            return innerHandler.WithAuthentication(new BasicAuthenticationProvider(user, pass));
        }

        public static HttpMessageHandler WithTokenAuthentication(this HttpMessageHandler innerHandler, string token)
        {
            return innerHandler.WithAuthentication(new TokenAuthenticationProvider(token));
        }

        public static HttpMessageHandler WithTokenAuthentication(this HttpMessageHandler innerHandler, Func<string> tokenDelegate)
        {
            return innerHandler.WithAuthentication(new TokenAuthenticationProvider(tokenDelegate));
        }

        public static HttpMessageHandler WithTokenAuthentication(this HttpMessageHandler innerHandler, ITokenProvider tokenProvider)
        {
            return innerHandler.WithAuthentication(new TokenAuthenticationProvider(tokenProvider));
        }
    }
}
