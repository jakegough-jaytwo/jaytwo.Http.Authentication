using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using jaytwo.AsyncHelper;

namespace jaytwo.Http.Authentication
{
    public abstract class AuthenticationProviderBase : IAuthenticationProvider
    {
        // override either of these methods, but don't do both

        public virtual Task AuthenticateAsync(HttpRequestMessage request)
        {
            Authenticate(request);

#if NETFRAMEWORK || NETSTANDARD1_1
            return Task.FromResult(0);
#else
            return Task.CompletedTask;
#endif
        }

        public virtual void Authenticate(HttpRequestMessage request)
        {
            AuthenticateAsync(request).AwaitSynchronously();
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
}
