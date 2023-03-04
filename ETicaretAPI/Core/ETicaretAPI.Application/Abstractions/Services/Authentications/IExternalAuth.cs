using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuth
    {
        Task<DTOs.Token> GoogleLoginAsync(string idToken, int tokenLifeTimeSec);
        Task<DTOs.Token> FacebookLoginAsync(string authToken , int tokenLifeTimeSec);
    }
}
