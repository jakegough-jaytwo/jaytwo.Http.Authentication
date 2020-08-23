using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.FluentHttp;

namespace jaytwo.Http.Authentication
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly IAuthenticationProvider _authenticationProvider;

        public AuthenticationDelegatingHandler(IAuthenticationProvider authenticationProvider)
            : base()
        {
            _authenticationProvider = authenticationProvider;
        }

        public AuthenticationDelegatingHandler(IAuthenticationProvider authenticationProvider, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            _authenticationProvider = authenticationProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.WithAuthentication(_authenticationProvider);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
