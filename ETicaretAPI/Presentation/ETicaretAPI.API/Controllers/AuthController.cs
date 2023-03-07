using ETicaretAPI.Application.Features.Commands.AppUserCommands;
using ETicaretAPI.Application.ViewModels.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator mediator;
        readonly IConfiguration configuration;
        public AuthController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            LoginCommand loginCommand = new(loginViewModel);
            var result = await mediator.Send(loginCommand);
            return Ok(result);
        }

        [HttpPost("google-login")]

        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginViewModel googleLoginVM)
        {
            GoogleLoginUserCommand googleLoginUserCommand = new(googleLoginVM);
            GoogleLoginCommandResponse response = await mediator.Send(googleLoginUserCommand);
            return Ok(response);
        }

        [HttpPost("facebook-login")]

        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginViewModel facebookLoginVM)
        {
            FacebookLoginCommand facebookLoginCommand = new(facebookLoginVM);
            FacebookLoginCommandResponse response = await mediator.Send(facebookLoginCommand);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult getConntectionString()
        {
            
            return Ok(configuration["ConnectionStrings:MySql"]);
        }
    }
}
