using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using Sharing.Application.CQRS.ShareAsset;
using Sharing.Application.CQRS.UpdateSharedUser;
using Sharing.Application.DTOs;
using Sharing.Domain.Entities;

namespace Sharing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShareAssetController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ShareAssetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("share")]
        public async Task<IActionResult> Share([FromBody] ShareAssetDto dto)
        {

            var userId = User.GetUserId(); 
            if (userId != dto.OwnerUserId)
                return Forbid(); 

            var id = await _mediator.Send(new ShareAssetCommand(dto));
            return Ok(id);
        }

        [HttpPost("unshare")]
        public async Task<IActionResult> UnShare([FromBody] UnShareAssetDto dto)
        {
            var userId = User.GetUserId();

            await _mediator.Send(
                new UnShareAssetCommand(dto.SharedAssetId, userId)
            );

            return NoContent();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditSharedUser([FromBody] ShareAssetDto dto)
        {
            var userId = User.GetUserId();

            var assetId = await _mediator.Send(new UpdateSharedUserCommand(dto));

            return Ok("assetId");
        }

    }
}
