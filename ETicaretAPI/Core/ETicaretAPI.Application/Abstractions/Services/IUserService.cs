using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponseDTO> CreateAsync(CreateUserDTO createUser);

        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int secondsToAdd);
    }
}
