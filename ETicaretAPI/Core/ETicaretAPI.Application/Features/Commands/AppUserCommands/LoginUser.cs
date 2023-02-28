using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.ViewModels.Identity;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUserCommands
{
    public class LoginCommand : IRequest<LoginCommandResponse>
    {
        public LoginViewModel LoginViewModel { get; set; }

        public LoginCommand(LoginViewModel loginViewModel)
        {
            LoginViewModel = loginViewModel;
        }
    }


    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
    {
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        readonly ITokenHandler tokenHandler;

        public LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenHandler = tokenHandler;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByNameAsync(request.LoginViewModel.UserNameOrEmail);

            if (appUser is null)
                appUser = await userManager.FindByEmailAsync(request.LoginViewModel.UserNameOrEmail);
            if(appUser is null)
            {
                throw new LoginFailedException();
            }

            SignInResult result = await signInManager.CheckPasswordSignInAsync(appUser, request.LoginViewModel.Password, false);
            if (result.Succeeded)
            {
                Token token = tokenHandler.CreateAccessToken(5);
                return new LoginCommandResponse()
                {
                    Token = token,
                    Message = "Giriş başarılı."
                };
            }
            else
                throw new LoginFailedException();
        }
    }

    public class LoginCommandResponse
    {
        public Token Token { get; set; }
        public string Message { get; set; }
    }
}
