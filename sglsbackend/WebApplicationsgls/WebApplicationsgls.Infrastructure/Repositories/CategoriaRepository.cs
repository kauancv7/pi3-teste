using Microsoft.EntityFrameworkCore;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Infrastructure.Data;

namespace WebApplicationsgls.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TipoCategoria>> ListarAsync()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<TipoCategoria?> ObterPorIdAsync(Guid id)
    {
        return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AdicionarAsync(TipoCategoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(TipoCategoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(TipoCategoria categoria)
    {
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteNomeAsync(string nome, Guid? ignorarId = null)
    {
        return await _context.Categorias
            .AnyAsync(c => c.Nome == nome && (!ignorarId.HasValue || c.Id != ignorarId.Value));
    }

    public async Task<bool> PossuiProdutosAtivosAsync(Guid categoriaId)
    {
        return await _context.Produtos
            .AnyAsync(p => p.CategoriaId == categoriaId && p.Ativo);
    }
}
