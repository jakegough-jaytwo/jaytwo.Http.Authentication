using System;
using System.Threading.Tasks;

namespace jaytwo.Http.Authentication;

public interface IBearerTokenProvider
{
    Task<string> GetTokenAsync();
}
