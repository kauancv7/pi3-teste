using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Domain.Entities;

public class Produto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; }
    public Guid CategoriaId { get; set; }
    public TipoCategoria Categoria { get; set; }
    public Guid VendedorId { get; set; }
    public Usuario Vendedor { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public void AdicionarProduto(string nome, string descricao, decimal preco, int estoque, Guid categoriaId, Guid vendedorId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome inválido");
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição inválida");
        if (preco <= 0)
            throw new DomainException("Preço inválido");
        if (estoque < 0)
            throw new DomainException("Estoque inválido");
        if (categoriaId == Guid.Empty)
            throw new DomainException("Categoria inválida");
        if (vendedorId == Guid.Empty)
            throw new DomainException("Vendedor inválido");
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Estoque = estoque;
        Ativo = true;
        CategoriaId = categoriaId;
        VendedorId = vendedorId;
    }
    public void AlterarPreco(decimal preco)
    {
        if (preco <= 0)
            throw new DomainException("Preço inválido");
        Preco = preco;
    }
    public void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome inválido");
        Nome = nome;
    }
    public void Desativar()
    {
        if (!Ativo)
            throw new DomainException("Produto já está desativado");
        Ativo = false;
    }   
    public void Ativar()
    {
        if (Ativo)
            throw new DomainException("Produto já está ativo");
        Ativo = true;
    }
    public void AdicionarDescricao(string descricao)
    {
        if(string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição inválida");
    }   
    public void AdicionarNome(string nome)
    {
        if(string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome inválido");
    }
    public void AdicionarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida");

        Estoque += quantidade;
    }

    public void RemoverEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida");

        if (Estoque < quantidade)
            throw new EstoqueException("Estoque insuficiente");

        Estoque -= quantidade;
    }

}
