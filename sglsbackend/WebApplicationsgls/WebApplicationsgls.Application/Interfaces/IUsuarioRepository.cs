using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Interfaces;

    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorIdAsync(Guid id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<List<Usuario>> ListarAsync();
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task RemoverAsync(Usuario usuario);
        Task SalvarAlteracoesAsync();
    }
