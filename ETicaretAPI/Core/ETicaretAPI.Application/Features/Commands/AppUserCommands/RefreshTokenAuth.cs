using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUserCommands
{
    public class RefreshTokenAuthCommand : IRequest<RefreshTokenAuthCommandResponse>
    {
        public string RefreshToken { get; set; }
        public RefreshTokenAuthCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }

    public class RefreshTokenAuthCommandHandler : IRequestHandler<RefreshTokenAuthCommand, RefreshTokenAuthCommandResponse>
    {
        readonly IInternalAuth internalAuth;

        public RefreshTokenAuthCommandHandler(IInternalAuth internalAuth)
        {
            this.internalAuth = internalAuth;
        }

        public async Task<RefreshTokenAuthCommandResponse> Handle(RefreshTokenAuthCommand request, CancellationToken cancellationToken)
        {
            Token token = await internalAuth.LoginWithRefreshToken(request.RefreshToken);
            return new()
            {
                Token = token,
            };
        }
    }

    public class RefreshTokenAuthCommandResponse
    {
        public Token Token { get; set; }
    }
}
