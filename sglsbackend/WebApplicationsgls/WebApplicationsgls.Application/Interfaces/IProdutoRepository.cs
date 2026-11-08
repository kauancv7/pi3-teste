
using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> ListarAsync();
        Task<List<Produto>> ListarProdutosPublicosAsync();
        Task<Produto?> ObterPorIdAsync(Guid id);
        Task<Produto> AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task RemoverAsync(Produto produto);
        Task<List<Produto>> ListarPorPerfilAsync(Guid usuarioId, bool isAdmin);

    }

}
