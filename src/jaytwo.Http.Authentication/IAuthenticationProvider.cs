using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace jaytwo.Http.Authentication;

public interface IAuthenticationProvider
{
    Task AuthenticateAsync(IHttpClient httpClient, HttpRequestMessage request);
}
