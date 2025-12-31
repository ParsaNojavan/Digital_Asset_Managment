using AuthService.Application.CQRS.Command.Refresh;
using AuthService.Application.CQRS.Command.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using UserService.Application.CQRS.Command.DeleteUser;
using UserService.Application.CQRS.Command.Login;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(new { accessToken = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            try
            {
                var tokens = await _mediator.Send(command);
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMe(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            await _mediator.Send(new DeleteUserCommand { UserId = userId}, cancellationToken);
            return NoContent();
        }
    }
}
