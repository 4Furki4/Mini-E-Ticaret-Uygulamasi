using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.ViewModels.Identity;
using MediatR;

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
        readonly IInternalAuth internalAuth;

        public LoginCommandHandler(IInternalAuth internalAuth)
        {
            this.internalAuth = internalAuth;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            Token token = await internalAuth.LoginAsync(request.LoginViewModel.UserNameOrEmail, request.LoginViewModel.Password);

            return new()
            {
                Token = token,
                Message = "Giriş Yapıldı !"
            };
        }
    }

    public class LoginCommandResponse
    {
        public Token Token { get; set; }
        public string Message { get; set; }
    }
}
