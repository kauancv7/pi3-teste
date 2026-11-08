using Microsoft.EntityFrameworkCore;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Infrastructure.Data;

namespace WebApplicationsgls.Infrastructure.Repositories;

public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
{
    private readonly AppDbContext _context;

    public MovimentacaoEstoqueRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(MovimentacaoEstoque movimentacao)
    {
        _context.MovimentacoesEstoque.Add(movimentacao);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MovimentacaoEstoque>> ListarPorProdutoAsync(Guid produtoId)
    {
        return await _context.MovimentacoesEstoque
            .Include(m => m.Usuario)
            .Where(m => m.ProdutoId == produtoId)
            .OrderByDescending(m => m.Data)
            .ToListAsync();
    }
}
