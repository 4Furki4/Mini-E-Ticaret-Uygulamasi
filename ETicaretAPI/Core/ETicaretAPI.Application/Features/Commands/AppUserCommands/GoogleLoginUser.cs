using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.ViewModels.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        readonly IExternalAuth externalAuth;
        public GoogleLoginUserCommandHandler(IExternalAuth externalAuth)
        {
            this.externalAuth = externalAuth;
        }
        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginUserCommand request, CancellationToken cancellationToken)
        {
            Token token = await externalAuth.GoogleLoginAsync(request.ViewModel.idToken, 60);
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
