using Assests.Domain.Entities;
using Folders.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using Storage.API.Extensions;
using Storage.Application.CQRS.Command.Files.RenameFileCommand;
using Storage.Application.CQRS.Command.Files.UploadFileCommand;
using Storage.Application.CQRS.Command.Folders.RenameFolderCommand;
using Storage.Application.DTOs;
using Storage.Application.Services;
using Storage.Domain.Repositories;

namespace Storage.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFolderRepository _folderRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IMediator _mediator;

        public FilesController(IFileStorageService fileStorageService, IAssetRepository assetRepository, IFolderRepository folderRepository, IMediator mediator)
        {
            _fileStorageService = fileStorageService;
            _assetRepository = assetRepository;
            _folderRepository = folderRepository;
            _mediator = mediator;
        }

        #region Upload
        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string path, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            byte[] fileBytes;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationToken);
                fileBytes = ms.ToArray();
            }

            var command = new UploadFileCommand
            {
                UserId = User.GetUserId(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileContent = fileBytes,
                Path = path
            };

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        #endregion


        #region Download File
        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string path, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(path)) return BadRequest("Path is required");

            var stream = await _fileStorageService.DownloadAsync(path, cancellationToken);

            return File(
                stream,
                "application/octet-stream",
                Path.GetFileName(path));
        }
        #endregion

        #region Delete File
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(
            [FromQuery] string path,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(path))
                return BadRequest("Path is required");

            await _fileStorageService.DeleteAsync(path, cancellationToken);

            return NoContent();
        }
        #endregion

        #region Rename
        [HttpPost("rename")]
        public async Task<IActionResult> Rename([FromBody]RenameFileDto fileDto)
        {
            var userId = User.GetUserId();
            var command = new RenameFileCommand(fileDto.FileId, userId, fileDto.Name);
            var newPath = await _mediator.Send(command);
            return Ok(new { Path = newPath });
        }
        #endregion
    }
}
