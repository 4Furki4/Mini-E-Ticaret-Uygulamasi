using ETicaretAPI.Application.Features.Commands.AppUserCommands;
using ETicaretAPI.Application.ViewModels.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

    }
}
