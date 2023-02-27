using ETicaretAPI.Application.ViewModels.Identity;
using MediatR;
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
        public Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class LoginCommandResponse
    {

    }
}
