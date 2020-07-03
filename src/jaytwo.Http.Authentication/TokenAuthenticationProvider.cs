using System;
using System.Net.Http;
using System.Threading.Tasks;
using jaytwo.FluentHttp;

namespace jaytwo.Http.Authentication
{
    public class TokenAuthenticationProvider : AuthenticationProviderBase, IAuthenticationProvider
    {
        private readonly Func<Task<string>> _tokenProvider;

        public TokenAuthenticationProvider(string token)
            : this(() => token)
        {
        }

        public TokenAuthenticationProvider(Func<string> tokenDelegate)
            : this(() => Task.FromResult(tokenDelegate.Invoke()))
        {
        }

        public TokenAuthenticationProvider(Func<Task<string>> tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public TokenAuthenticationProvider(ITokenProvider tokenProvider)
            : this(tokenProvider.GetTokenAsync)
        {
        }

        public override async Task AuthenticateAsync(HttpRequestMessage request)
        {
            var token = await _tokenProvider.Invoke();
            SetAuthenticationHeader(request, "Bearer", token);
        }
    }
}
