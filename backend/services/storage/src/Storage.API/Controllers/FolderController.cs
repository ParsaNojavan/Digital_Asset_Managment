using Folders.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using Storage.Application.CQRS.Command.Folders;
using Storage.Application.CQRS.Command.Folders.DeleteFolderCommand;
using Storage.Application.CQRS.Command.Folders.RenameFolderCommand;
using Storage.Application.CQRS.Command.Folders.UploadFolderCommand;
using Storage.Application.CQRS.Query;
using Storage.Application.DTOs;
using Storage.Domain.Repositories;
using System.Security.Claims;

namespace Storage.API.Controllers
{
    [Route("api/Folder")]
    [ApiController]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFolderRepository _folderRepository;

        public FolderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto dto)
        {
            var userId = User.GetUserId();
            var command = new CreateFolderCommand(userId, dto.Name, dto.ParentFolderId);

            var folderId = await _mediator.Send(command);
            return Ok(new { FolderId = folderId });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFolder([FromQuery]Guid Id)
        {
            var userId = User.GetUserId(); 

            await _mediator.Send(new DeleteFolderCommand(Id, userId));

            return NoContent();

        }

        [HttpPost("rename")]
        public async Task<IActionResult> RenameFolder([FromBody]RenameFolderDto renameDto)
        {
            var userId = User.GetUserId();
            var command = new RenameFolderCommand(renameDto.FolderId, userId,renameDto.Name);
            var newPath = await _mediator.Send(command);
            return Ok(new { Path = newPath });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyFolders()
        {
            var userId = User.GetUserId();
            var query = new GetFoldersByUserIdQuery(userId);

            var folders = await _mediator.Send(query);
            return Ok(folders);
        }
    }
}
