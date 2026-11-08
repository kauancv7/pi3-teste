using WebApplicationsgls.Application.DTOs;

namespace WebApplicationsgls.Application.Interfaces;

public interface IImageStorageService
{
    Task<ImageStorageResultDTO> SalvarAsync(Stream content, string originalFileName, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> AbrirAsync(string relativeUrl, CancellationToken cancellationToken = default);
    Task RemoverAsync(string relativeUrl, CancellationToken cancellationToken = default);
}
