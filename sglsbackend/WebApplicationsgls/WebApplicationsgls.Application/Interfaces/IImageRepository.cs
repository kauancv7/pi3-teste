using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Interfaces;

public interface IImageRepository
{
    Task<List<Image>> ListarAsync();
    Task<List<Image>> ListarPorVinculoAsync(int imageTypeId, Guid ownerId);
    Task<Image?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Image image);
    Task RemoverAsync(Image image);
}
