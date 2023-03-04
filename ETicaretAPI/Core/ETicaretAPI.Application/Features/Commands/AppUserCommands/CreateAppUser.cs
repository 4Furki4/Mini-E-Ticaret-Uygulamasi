using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.ViewModels.Identity;
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
        readonly IUserService userService;

        public CreateAppUserCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<CreateAppUserCommandResponse> Handle(CreateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserDTO createUserDTO = new()
            {
                UserName = request.ViewModel.UserName,
                Email = request.ViewModel.Email,
                FullName = request.ViewModel.FullName,
                Password = request.ViewModel.Password,
                PasswordConfirm = request.ViewModel.ConfirmPassword
            };
            CreateUserResponseDTO responseDTO = await userService.CreateAsync(createUserDTO);

            CreateAppUserCommandResponse response = new() { IsSuccessfull = responseDTO.IsSuccessfull, Message = responseDTO.Message };

            return response;
        }
    }

    public class CreateAppUserCommandResponse
    {
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }
    }
}
