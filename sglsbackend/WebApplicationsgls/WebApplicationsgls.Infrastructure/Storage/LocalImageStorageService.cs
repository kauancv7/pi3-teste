using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;

namespace WebApplicationsgls.Infrastructure.Storage;

public class LocalImageStorageService : IImageStorageService
{
    private readonly string _uploadsPath;
    private readonly string _requestPath;

    public LocalImageStorageService(string uploadsPath, string requestPath)
    {
        _uploadsPath = uploadsPath;
        _requestPath = NormalizeRequestPath(requestPath);
    }

    public async Task<ImageStorageResultDTO> SalvarAsync(Stream content, string originalFileName, string contentType, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_uploadsPath);

        var extension = Path.GetExtension(originalFileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var physicalPath = Path.Combine(_uploadsPath, fileName);

        await using var fileStream = new FileStream(physicalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fileStream, cancellationToken);

        return new ImageStorageResultDTO
        {
            FileName = fileName,
            RelativeUrl = $"{_requestPath}/{fileName}"
        };
    }

    public Task<Stream> AbrirAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        var physicalPath = GetPhysicalPath(relativeUrl);
        if (!File.Exists(physicalPath))
            throw new FileNotFoundException("Arquivo de imagem não encontrado.", physicalPath);

        Stream stream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }

    public Task RemoverAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        var physicalPath = GetPhysicalPath(relativeUrl);
        if (File.Exists(physicalPath))
            File.Delete(physicalPath);

        return Task.CompletedTask;
    }

    private string GetPhysicalPath(string relativeUrl)
    {
        var fileName = Path.GetFileName(relativeUrl);
        return Path.Combine(_uploadsPath, fileName);
    }

    private static string NormalizeRequestPath(string requestPath)
    {
        if (string.IsNullOrWhiteSpace(requestPath))
            return "/uploads";

        return requestPath.StartsWith('/') ? requestPath.TrimEnd('/') : $"/{requestPath.TrimEnd('/')}";
    }
}
