using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.ViewModels.Identity;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ETicaretAPI.Application.Features.Commands.AppUserCommands
{
    public class GoogleLoginUserCommand  :IRequest<GoogleLoginCommandResponse>
    {
        public GoogleLoginViewModel ViewModel { get; set; }

        public GoogleLoginUserCommand(GoogleLoginViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }

    public class GoogleLoginUserCommandHandler : IRequestHandler<GoogleLoginUserCommand, GoogleLoginCommandResponse>
    {
        readonly UserManager<AppUser> userManager;
        readonly ITokenHandler tokenHandler;
        public GoogleLoginUserCommandHandler(UserManager<AppUser> userManager, ITokenHandler tokenHandler)
        {
            this.userManager = userManager;
            this.tokenHandler = tokenHandler;
        }
        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginUserCommand request, CancellationToken cancellationToken)
        {
            var settings = new ValidationSettings()
            {
                Audience = new List<string> { "1016383062266-tj15o49nfq4bjg9vdgk7rvapb6qdbp0u.apps.googleusercontent.com" }
            };

            Payload payload = await ValidateAsync(request.ViewModel.idToken, settings);
            UserLoginInfo userInfo = new(request.ViewModel.provider, payload.Subject, request.ViewModel.provider);
            AppUser? user = await userManager.FindByLoginAsync(userInfo.LoginProvider, userInfo.ProviderKey);
            bool result = user != null;
            if(user is null)
            {
                user = await userManager.FindByEmailAsync(request.ViewModel.email);
                if(user is null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = request.ViewModel.email,
                        UserName = request.ViewModel.email,
                        FullName = request.ViewModel.name,
                    };
                    IdentityResult identityResult = await userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
                await userManager.AddLoginAsync(user, userInfo);
            else
                throw new Exception("Invalid external login!");

            Token token = tokenHandler.CreateAccessToken(15);
            return new()
            {
                Token = token
            };
        }
    }

    public class GoogleLoginCommandResponse
    {
        public Token Token { get; set; }
    }
}
