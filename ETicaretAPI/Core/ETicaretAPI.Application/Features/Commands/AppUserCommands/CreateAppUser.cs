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
    public class CreateAppUserCommandRequest : IRequest<CreateAppUserCommandResponse>
    {
        public CreateAppUserViewModel ViewModel { get; set; }
        public CreateAppUserCommandRequest(CreateAppUserViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }

    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommandRequest, CreateAppUserCommandResponse>
    {
        readonly UserManager<AppUser> userManager;

        public CreateAppUserCommandHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<CreateAppUserCommandResponse> Handle(CreateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            AppUser appUser = new()
            {
                FullName = request.ViewModel.FullName,
                Email = request.ViewModel.Email,
                UserName = request.ViewModel.UserName,
            };
            string password = request.ViewModel.Password;
            IdentityResult result = await userManager.CreateAsync(appUser, password);
            if (result.Succeeded)
                return new() { IsSuccessfull = true, Message = "User created successfully!" };
            else
                throw new UserCreateFailedException();
        }
    }

    public class CreateAppUserCommandResponse
    {
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }
    }
}
