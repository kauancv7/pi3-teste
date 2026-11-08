using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Services;

public class UsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Usuario>> ListarAsync()
    {
        return await _repository.ListarAsync();
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<Usuario> CriarAsync(
        CriarUsuarioDTO dto)
    {
        var novoUsuario = new Usuario(
            dto.Nome,
            dto.Email,
            BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            dto.Perfil
        );

        await _repository.AdicionarAsync(novoUsuario);

        return novoUsuario;
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        if (usuario.Id == Guid.Empty)
        {
            throw new ArgumentException(
                "O ID do usuário é obrigatório."
            );
        }

        var usuarioExistente =
            await _repository.ObterPorIdAsync(usuario.Id);

        if (usuarioExistente is null)
        {
            throw new KeyNotFoundException(
                "Usuário não encontrado."
            );
        }
        
        await _repository.AtualizarAsync(usuario);
    }

    public async Task AlterarStatusAsync(
        Guid usuarioId,
        bool ativo)
    {
        if (usuarioId == Guid.Empty)
        {
            throw new ArgumentException(
                "O ID do usuário é obrigatório."
            );
        }

        var usuario =
            await _repository.ObterPorIdAsync(usuarioId);

        if (usuario is null)
        {
            throw new KeyNotFoundException(
                "Usuário não encontrado."
            );
        }

        usuario.AlterarStatus(ativo);

        await _repository.SalvarAlteracoesAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "O ID do usuário é obrigatório."
            );
        }

        var usuario =
            await _repository.ObterPorIdAsync(id);

        if (usuario is null)
        {
            throw new KeyNotFoundException(
                "Usuário não encontrado."
            );
        }

        await _repository.RemoverAsync(usuario);
    }
}