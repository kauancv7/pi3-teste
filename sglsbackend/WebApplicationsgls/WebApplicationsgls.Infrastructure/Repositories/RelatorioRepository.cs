using Microsoft.EntityFrameworkCore;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Infrastructure.Data;

namespace WebApplicationsgls.Infrastructure.Repositories;

public class RelatorioRepository : IRelatorioRepository
{
    private readonly AppDbContext _context;

    public RelatorioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Relatorio relatorio)
    {
        await _context.Relatorios.AddAsync(relatorio);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Relatorio relatorio)
    {
        _context.Relatorios.Update(relatorio);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Relatorio>> ListarAsync()
    {
        return await _context.Relatorios
            .Include(r => r.Usuario)
            .Include(r => r.Produto)
            .ToListAsync();
    }

    public async Task<Relatorio?> ObterPorIdAsync(int id)
    {
        return await _context.Relatorios
            .Include(r => r.Usuario)
            .Include(r => r.Produto)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task RemoverAsync(Relatorio relatorio)
    {
        _context.Relatorios.Remove(relatorio);
        await _context.SaveChangesAsync();
    }
}
