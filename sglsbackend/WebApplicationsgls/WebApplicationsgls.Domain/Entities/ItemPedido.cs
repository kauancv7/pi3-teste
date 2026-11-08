using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Domain.Entities;

public class ItemPedido
{
    public Guid Id { get; private set; }

    public Guid PedidoId { get; private set; }

    public Pedido Pedido { get; private set; }

    public Guid ProdutoId { get; private set; }

    public Produto Produto { get; private set; }

    public int Quantidade { get; private set; }

    public decimal PrecoUnitario { get; private set; }

    public decimal Subtotal => Quantidade * PrecoUnitario;

    protected ItemPedido() { }

    public ItemPedido(
        Guid produtoId,
        decimal precoUnitario,
        int quantidade)
    {
        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido");

        if (precoUnitario <= 0)
            throw new DomainException("Preço unitário inválido");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida");

        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        PrecoUnitario = precoUnitario;
        Quantidade = quantidade;
    }

    public void AdicionarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida");

        Quantidade += quantidade;
    }

    public void AtualizarQuantidade(int novaQuantidade)
    {
        if (novaQuantidade <= 0)
            throw new DomainException("Quantidade inválida");

        Quantidade = novaQuantidade;
    }

    public void AtualizarPrecoUnitario(decimal novoPreco)
    {
        if (novoPreco <= 0)
            throw new DomainException("Preço unitário inválido");

        PrecoUnitario = novoPreco;
    }
}