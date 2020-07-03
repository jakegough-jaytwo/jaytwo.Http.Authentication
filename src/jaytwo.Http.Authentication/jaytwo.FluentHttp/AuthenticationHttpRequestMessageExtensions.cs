using System;
using System.Net.Http;
using jaytwo.Http.Authentication;

namespace jaytwo.FluentHttp
{
    public static class AuthenticationHttpRequestMessageExtensions
    {
        public static HttpRequestMessage WithAuthentication(this HttpRequestMessage httpRequestMessage, IAuthenticationProvider authenticationProvider)
        {
            authenticationProvider.Authenticate(httpRequestMessage);
            return httpRequestMessage;
        }

        public static HttpRequestMessage WithBasicAuthentication(this HttpRequestMessage httpRequestMessage, string user, string pass)
        {
            return httpRequestMessage.WithAuthentication(new BasicAuthenticationProvider(user, pass));
        }

        public static HttpRequestMessage WithTokenAuthentication(this HttpRequestMessage httpRequestMessage, string token)
        {
            return httpRequestMessage.WithAuthentication(new TokenAuthenticationProvider(token));
        }

        public static HttpRequestMessage WithTokenAuthentication(this HttpRequestMessage httpRequestMessage, Func<string> tokenDelegate)
        {
            return httpRequestMessage.WithAuthentication(new TokenAuthenticationProvider(tokenDelegate));
        }

        public static HttpRequestMessage WithTokenAuthentication(this HttpRequestMessage httpRequestMessage, ITokenProvider tokenProvider)
        {
            return httpRequestMessage.WithAuthentication(new TokenAuthenticationProvider(tokenProvider));
        }
    }
}
