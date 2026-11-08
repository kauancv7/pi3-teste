using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Domain.Entities;

public class TipoCategoria
{
    private readonly List<Produto> _produtos = new();

    public Guid Id { get; private set; }

    public string Nome { get; private set; }

    public bool Ativa { get; private set; }

    public IReadOnlyCollection<Produto> Produtos =>
        _produtos.AsReadOnly();

    protected TipoCategoria() { }

    public TipoCategoria(string nome)
    {
        ValidarNome(nome);

        Id = Guid.NewGuid();
        Nome = nome;
        Ativa = true;
    }

    public void AlterarNome(string nome)
    {
        ValidarNome(nome);

        Nome = nome;
    }

    public void Ativar()
    {
        if (Ativa)
            throw new DomainException(
                "Categoria já está ativa"
            );

        Ativa = true;
    }

    public void Desativar()
    {
        if (!Ativa)
            throw new DomainException(
                "Categoria já está desativada"
            );

        if (_produtos.Any())
            throw new DomainException(
                "Não é possível desativar categoria com produtos vinculados"
            );

        Ativa = false;
    }

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException(
                "Nome da categoria é obrigatório"
            );

        if (nome.Length > 100)
            throw new DomainException(
                "Nome da categoria deve possuir no máximo 100 caracteres"
            );
    }
}