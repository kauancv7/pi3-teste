using WebApplicationsgls.Application.DTOs;

namespace WebApplicationsgls.Application.Interfaces;

public interface IImageService
{
    Task<ImageResponseDTO> SalvarAsync(UploadImageDTO dto, CancellationToken cancellationToken = default);
    Task<List<ImageResponseDTO>> ListarAsync();
    Task<List<ImageResponseDTO>> ListarPorVinculoAsync(int imageTypeId, Guid ownerId);
    Task<ImageResponseDTO?> ObterPorIdAsync(Guid id);
    Task<ImageDownloadDTO?> BaixarAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
