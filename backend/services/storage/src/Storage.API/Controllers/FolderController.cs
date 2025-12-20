using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Storage.Application.CQRS.Command.Folders;
using Storage.Application.CQRS.Query;
using Storage.Application.DTOs;
using System.Security.Claims;

namespace Storage.API.Controllers
{
    [Route("api/Folder")]
    [ApiController]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FolderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var command = new CreateFolderCommand(userId, dto.Name, dto.ParentFolderId);

            var folderId = await _mediator.Send(command);
            return Ok(new { FolderId = folderId });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyFolders()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var query = new GetFoldersByUserIdQuery(userId);

            var folders = await _mediator.Send(query);
            return Ok(folders);
        }
    }
}
