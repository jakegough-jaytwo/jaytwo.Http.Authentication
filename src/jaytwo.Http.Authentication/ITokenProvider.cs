using System;
using System.Threading.Tasks;

namespace jaytwo.Http.Authentication;

public interface ITokenProvider
{
    Task<string> GetTokenAsync();
}
