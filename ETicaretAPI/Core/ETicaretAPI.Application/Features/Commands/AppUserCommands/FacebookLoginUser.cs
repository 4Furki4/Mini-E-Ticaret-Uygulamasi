using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.ViewModels.Identity;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUserCommands
{
    public class FacebookLoginCommand : IRequest<FacebookLoginCommandResponse>
    {
        public FacebookLoginViewModel ViewModel { get; set; }

        public FacebookLoginCommand(FacebookLoginViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }

    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommand, FacebookLoginCommandResponse>
    {
        readonly IExternalAuth externalAuth;

        public FacebookLoginCommandHandler(IExternalAuth externalAuth)
        {
            this.externalAuth = externalAuth;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommand request, CancellationToken cancellationToken)
        {
            Token token = await externalAuth.FacebookLoginAsync(request.ViewModel.AuthToken);
            return new()
            {
                Token = token
            };
        }
    }

    public class FacebookLoginCommandResponse
    {
        public Token Token { get; set; }
    }
}
