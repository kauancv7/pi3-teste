using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationsgls.API.Authorization;
using WebApplicationsgls.Application.DTOs;
using UploadImageRequest = WebApplicationsgls.API.DTOs.UploadImageRequest;
using WebApplicationsgls.Application.Interfaces;

namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImagesController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] UploadImageRequest request, CancellationToken cancellationToken)
    {
        if (request.File == null)
            return BadRequest("Nenhuma imagem foi enviada.");

        try
        {
            await using var stream = request.File.OpenReadStream();
            var image = await _imageService.SalvarAsync(new UploadImageDTO
            {
                FileName = request.File.FileName,
                ContentType = request.File.ContentType,
                Length = request.File.Length,
                Content = stream,
                ImageTypeId = request.ImageTypeId,
                OwnerId = request.OwnerId
            }, cancellationToken);

            return CreatedAtAction(nameof(ObterPorId), new { id = image.Id }, image);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var images = await _imageService.ListarAsync();
        return Ok(images);
    }

    [HttpGet("vinculo/{imageTypeId:int}/{ownerId:guid}")]
    public async Task<IActionResult> ListarPorVinculo(int imageTypeId, Guid ownerId)
    {
        try
        {
            var images = await _imageService.ListarPorVinculoAsync(imageTypeId, ownerId);
            return Ok(images);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var image = await _imageService.ObterPorIdAsync(id);
        if (image == null)
            return NotFound();

        return Ok(image);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var image = await _imageService.BaixarAsync(id, cancellationToken);
            if (image == null)
                return NotFound();

            return File(image.Content, image.ContentType, image.FileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        var removed = await _imageService.RemoverAsync(id, cancellationToken);
        if (!removed)
            return NotFound();

        return NoContent();
    }
}
