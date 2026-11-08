using Microsoft.EntityFrameworkCore;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Infrastructure.Data;

namespace WebApplicationsgls.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;
    public ClienteRepository(AppDbContext context) => _context = context;
    public async Task AdicionarAsync(Cliente cliente) { _context.Clientes.Add(cliente); await _context.SaveChangesAsync(); }
    public async Task AtualizarAsync(Cliente cliente)
    {
        var existente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == cliente.Id) ?? throw new KeyNotFoundException("Cliente não encontrado.");
        existente.Atualizar(cliente.Nome, cliente.Email, cliente.Telefone);
        if (!string.IsNullOrWhiteSpace(cliente.SenhaHash)) existente.SenhaHash = cliente.SenhaHash;
        await _context.SaveChangesAsync();
    }
    public Task<Cliente?> ObterPorEmailAsync(string email)
    {
        var normalizado = email.Trim().ToLowerInvariant();
        return _context.Clientes.FirstOrDefaultAsync(c => c.Email.ToLower() == normalizado);
    }
    public Task<Cliente?> ObterPorIdAsync(Guid id) => _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
    public async Task RemoverAsync(Cliente cliente) { _context.Clientes.Remove(cliente); await _context.SaveChangesAsync(); }
    public Task SalvarAlteracoesAsync() => _context.SaveChangesAsync();
}
