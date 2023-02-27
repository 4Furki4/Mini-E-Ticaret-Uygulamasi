using ETicaretAPI.Application.Features.Commands.AppUserCommands;
using ETicaretAPI.Application.ViewModels.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        readonly IMediator mediator;
        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AppUser([FromBody] CreateAppUserViewModel viewModel)
        {
            CreateAppUserCommandRequest request = new(viewModel);
            CreateAppUserCommandResponse result = await mediator.Send(request);
            return Ok(result);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            LoginCommand loginCommand = new(loginViewModel);
            await mediator.Send(loginCommand);
            return Ok();
        }

    }
}
