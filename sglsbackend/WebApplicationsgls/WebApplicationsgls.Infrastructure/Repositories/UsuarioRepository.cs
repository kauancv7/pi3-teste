using Microsoft.EntityFrameworkCore;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Infrastructure.Data;

namespace WebApplicationsgls.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(usuario =>
                usuario.Id == id
            );
    }

    public async Task<Usuario?> ObterPorEmailAsync(
        string email)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario =>
                usuario.Email == email
            );
    }

    public async Task<List<Usuario>> ListarAsync()
    {

        return await _context.Usuarios
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);

        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios
            .FirstOrDefaultAsync(usuarioBanco =>
                usuarioBanco.Id == usuario.Id
            );
        if (usuarioExistente is null) throw new KeyNotFoundException("Usuário não encontrado.");
        var entrada = _context.Entry(usuarioExistente);
        await _context.SaveChangesAsync();
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);

        await _context.SaveChangesAsync();
    }
}