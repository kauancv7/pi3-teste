using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Domain.Enums;

namespace WebApplicationsgls.Application.Services;

public class ProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IImageRepository _imageRepository;

    public ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IImageRepository imageRepository)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _imageRepository = imageRepository;
    }
    public async Task<List<ProdutoResponseDTO>> ListarAsync(Guid usuarioId,bool isAdmin)
    {

        var produtos = await _produtoRepository.ListarPorPerfilAsync(usuarioId, true);

        if (!isAdmin)
        {
            produtos = produtos
                .Where(produto => produto.Ativo)
                .ToList();
        }

        var produtosDto = new List<ProdutoResponseDTO>();

        foreach (var produto in produtos)
        {
            var imagens = await _imageRepository
                .ListarPorVinculoAsync(
                    (int)TipoImagemEnum.Produto,
                    produto.Id
                );

            var imagem = imagens.FirstOrDefault();

            produtosDto.Add(new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Estoque = produto.Estoque,
                Ativo = produto.Ativo,
                CategoriaId = produto.CategoriaId,
                CategoriaNome =
                    produto.Categoria?.Nome ?? "Sem categoria",
                ImagemId = imagem?.Id
            });
        }

        return produtosDto;
    }

    public async Task<ProdutoResponseDTO?> ObterPorIdAsync(Guid id, Guid usuarioId, bool isAdmin)
    {
        var produto = await _produtoRepository
            .ObterPorIdAsync(id);

        if (produto == null)
            return null;

        if (!isAdmin && !produto.Ativo)
        {
            throw new Exception(
                "Produto indisponível para venda"
            );
        }

        var imagens = await _imageRepository
            .ListarPorVinculoAsync(
                (int)TipoImagemEnum.Produto,
                produto.Id
            );

        var imagem = imagens.FirstOrDefault();

        return new ProdutoResponseDTO
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            Ativo = produto.Ativo,
            CategoriaId = produto.CategoriaId,
            CategoriaNome =
                produto.Categoria?.Nome ?? "Sem categoria",
            ImagemId = imagem?.Id
        };
    }

    public async Task<ProdutoResponseDTO> CriarAsync(CriarProdutoDTO dto, Guid usuarioId)
    {
        var categoria = await _categoriaRepository
            .ObterPorIdAsync(dto.CategoriaId);

        if (categoria == null || !categoria.Ativa)
        {
            throw new Exception(
                "Categoria inválida ou inativa"
            );
        }

        if (dto.Estoque < 0)
        {
            throw new Exception("Estoque inválido");
        }

        var produto = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            Estoque = dto.Estoque,
            CategoriaId = dto.CategoriaId,
            VendedorId = usuarioId,
            Ativo = true,
            DataCadastro = DateTime.UtcNow
        };

        var produtoCriado =
            await _produtoRepository.AdicionarAsync(produto);

        return new ProdutoResponseDTO
        {
            Id = produtoCriado.Id,
            Nome = produtoCriado.Nome,
            Descricao = produtoCriado.Descricao,
            Preco = produtoCriado.Preco,
            Estoque = produtoCriado.Estoque,
            Ativo = produtoCriado.Ativo,
            CategoriaId = produtoCriado.CategoriaId,
            CategoriaNome = categoria.Nome,
            ImagemId = null
        };
    }

    public async Task AtualizarAsync( AtualizarProdutoDTO dto, Guid usuarioId, bool isAdmin)
    {
        var produto = await _produtoRepository
            .ObterPorIdAsync(dto.Id);

        if (produto == null)
        {
            throw new Exception(
                "Produto não encontrado"
            );
        }

        if (!isAdmin && produto.VendedorId != usuarioId)
        {
            throw new Exception(
                "Você não pode editar este produto"
            );
        }

        var categoria = await _categoriaRepository
            .ObterPorIdAsync(dto.CategoriaId);

        if (categoria == null || !categoria.Ativa)
        {
            throw new Exception(
                "Categoria inválida ou inativa"
            );
        }

        if (dto.Estoque < 0)
        {
            throw new Exception("Estoque inválido");
        }

        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.Preco;
        produto.Estoque = dto.Estoque;
        produto.Ativo = dto.Ativo;
        produto.CategoriaId = dto.CategoriaId;

        await _produtoRepository
            .AtualizarAsync(produto);
    }

    public async Task RemoverAsync(Guid id)
    {
        var produto = await _produtoRepository
            .ObterPorIdAsync(id);

        if (produto == null)
        {
            throw new Exception(
                "Produto não encontrado"
            );
        }

        produto.Ativo = false;

        await _produtoRepository
            .AtualizarAsync(produto);
    }
    public async Task<List<ProdutoResponseDTO>> ListarProdutosPublicosAsync() 
    {
        var produtos = await _produtoRepository.ListarProdutosPublicosAsync(); //pega a query do banco
        
        var produtosDto = new List<ProdutoResponseDTO>(); // inicaliza objeto do DTO 
        foreach (var produto in produtos)
        {
            var imagens = await _imageRepository.ListarPorVinculoAsync((int)TipoImagemEnum.Produto,produto.Id);
            var imagem = imagens.FirstOrDefault();
            produtosDto.Add(new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Estoque = produto.Estoque,
                Ativo = produto.Ativo,
                CategoriaId = produto.CategoriaId,
                CategoriaNome =
                produto.Categoria?.Nome ?? "Sem categoria",
                ImagemId = imagem?.Id
            });
        }
        return produtosDto;
    }
}