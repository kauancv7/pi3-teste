
using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<TipoCategoria>> ListarAsync();
        Task<TipoCategoria?> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(TipoCategoria categoria);
        Task AtualizarAsync(TipoCategoria categoria);
        Task RemoverAsync(TipoCategoria categoria);
        Task<bool> ExisteNomeAsync(string nome, Guid? ignorarId = null);
        Task<bool> PossuiProdutosAtivosAsync(Guid categoriaId);

    }

}
