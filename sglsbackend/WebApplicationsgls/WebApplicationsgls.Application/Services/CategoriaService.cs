using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Application.Services;

public class CategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoriaResponseDTO>> ListarAsync(
        bool somenteAtivas)
    {
        var categorias = await _repository.ListarAsync();

        if (somenteAtivas)
            categorias = categorias
                .Where(c => c.Ativa)
                .ToList();

        return categorias.Select(c => new CategoriaResponseDTO
        {
            Id = c.Id,
            Nome = c.Nome,
            Ativa = c.Ativa
        }).ToList();
    }

    public async Task<CategoriaResponseDTO> CriarAsync(
        CriarCategoriaDTO dto)
    {
        if (await _repository.ExisteNomeAsync(dto.Nome))
            throw new DomainException(
                "Já existe uma categoria com esse nome"
            );

        var categoria = new TipoCategoria(dto.Nome);

        await _repository.AdicionarAsync(categoria);

        return new CategoriaResponseDTO
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Ativa = categoria.Ativa
        };
    }

    public async Task AtualizarAsync(
        AtualizarCategoriaDTO dto)
    {
        var categoria = await _repository
            .ObterPorIdAsync(dto.Id);

        if (categoria == null)
            throw new DomainException(
                "Categoria não encontrada"
            );

        if (await _repository.ExisteNomeAsync(
                dto.Nome,
                dto.Id))
        {
            throw new DomainException(
                "Já existe outra categoria com esse nome"
            );
        }

        categoria.AlterarNome(dto.Nome);

        if (dto.Ativa && !categoria.Ativa)
            categoria.Ativar();
        else if (!dto.Ativa && categoria.Ativa)
            categoria.Desativar();

        await _repository.AtualizarAsync(categoria);
    }

    public async Task DesativarAsync(Guid id)
    {
        var categoria = await _repository
            .ObterPorIdAsync(id);

        if (categoria == null)
            throw new DomainException(
                "Categoria não encontrada"
            );

        categoria.Desativar();

        await _repository.AtualizarAsync(categoria);
    }
}