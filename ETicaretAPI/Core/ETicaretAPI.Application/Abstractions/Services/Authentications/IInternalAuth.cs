using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuth
    {
        Task<DTOs.Token> LoginAsync(string usernameOrEmail, string password, int tokenLifeTimeSec);
        Task<DTOs.Token> LoginWithRefreshToken(string refreshToken);
    }
}
