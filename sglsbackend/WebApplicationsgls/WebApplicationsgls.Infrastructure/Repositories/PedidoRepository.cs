using Microsoft.EntityFrameworkCore;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Infrastructure.Data;

namespace WebApplicationsgls.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;
    public PedidoRepository(AppDbContext context) => _context = context;
    private IQueryable<Pedido> ComItens() => _context.Pedidos.Include(p => p.Itens).ThenInclude(i => i.Produto);
    public Task<Pedido?> ObterPorIdAsync(Guid id) => ComItens().FirstOrDefaultAsync(p => p.Id == id);
    public Task<List<Pedido>> ListarAsync() => ComItens().ToListAsync();
    public Task<List<Pedido>> ListarPorUsuarioAsync(Guid usuarioId) => ComItens().Where(p => p.UsuarioId == usuarioId).ToListAsync();
    public Task<List<Pedido>> ListarPorClienteAsync(Guid clienteId) => ComItens().Where(p => p.ClienteId == clienteId).OrderByDescending(p => p.DataPedido).ToListAsync();
    public async Task AdicionarAsync(Pedido pedido) { _context.Pedidos.Add(pedido); await _context.SaveChangesAsync(); }
    public async Task AtualizarAsync(Pedido pedido) { _context.Pedidos.Update(pedido); await _context.SaveChangesAsync(); }
}
