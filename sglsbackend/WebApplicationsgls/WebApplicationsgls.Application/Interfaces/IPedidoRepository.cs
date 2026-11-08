using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(Guid id);
    Task<List<Pedido>> ListarAsync();
    Task<List<Pedido>> ListarPorUsuarioAsync(Guid usuarioId);
    Task<List<Pedido>> ListarPorClienteAsync(Guid clienteId);
    Task AdicionarAsync(Pedido pedido);
    Task AtualizarAsync(Pedido pedido);
}
