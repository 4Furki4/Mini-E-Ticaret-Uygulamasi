using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Features.Commands.AppUserCommands;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<CreateUserResponseDTO> CreateAsync(CreateUserDTO createUser)
        {
            AppUser appUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = createUser.FullName,
                Email = createUser.Email,
                UserName = createUser.UserName,
            };
            string password = createUser.Password;
            IdentityResult result = await userManager.CreateAsync(appUser, password);
            CreateUserResponseDTO response = new() { IsSuccessfull = result.Succeeded };
            if (result.Succeeded)
                response.Message = response.Message;
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}"; //<br> can be added
                }
            }
            return response;
        }
    }
}
