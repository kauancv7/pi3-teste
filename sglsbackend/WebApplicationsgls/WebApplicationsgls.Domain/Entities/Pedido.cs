using WebApplicationsgls.Domain.Enums;
using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Domain.Entities;

public class Pedido
{
    private readonly List<ItemPedido> _itens = new();

    public Guid Id { get; private set; }

    public Guid? UsuarioId { get; private set; }

    public Usuario Usuario { get; private set; }

    public Guid? ClienteId { get; private set; }

    public Cliente Cliente { get; private set; }

    public DateTime DataPedido { get; private set; }

    public StatusPedido Status { get; private set; }

    public decimal ValorTotal { get; private set; }

    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    protected Pedido() { }

    public Pedido(Guid usuarioId)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        DataPedido = DateTime.UtcNow;
        Status = StatusPedido.Pendente;
    }

    private Pedido(Guid clienteId, bool pedidoDeCliente) {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
        DataPedido = DateTime.UtcNow;
        Status = StatusPedido.Pendente;
    }

    public static Pedido CriarParaCliente(Guid clienteId) {
        if (clienteId == Guid.Empty) throw new DomainException("Cliente inválido");
        return new Pedido(clienteId, true);
    }

    public void AdicionarItem(Produto produto, int quantidade)
    {
        if (Status != StatusPedido.Pendente)
            throw new DomainException("Não é possível alterar um pedido finalizado");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida");

        var itemExistente = _itens
            .FirstOrDefault(i => i.ProdutoId == produto.Id);

        if (itemExistente != null)
        {
            itemExistente.AdicionarQuantidade(quantidade);
        }
        else
        {
            _itens.Add(
                new ItemPedido(
                    produto.Id,
                    produto.Preco,
                    quantidade
                )
            );
        }

        RecalcularTotal();
    }

    public void Cancelar()
    {
        if (Status == StatusPedido.Cancelado)
            throw new DomainException("Pedido já cancelado");

        if (Status == StatusPedido.Entregue)
            throw new DomainException("Pedido entregue não pode ser cancelado");

        Status = StatusPedido.Cancelado;
    }

    public void Finalizar()
    {
        if (!_itens.Any())
            throw new DomainException("Pedido sem itens");

        Status = StatusPedido.Finalizado;
    }

    private void RecalcularTotal()
    {
        ValorTotal = _itens.Sum(i => i.Subtotal);
    }
}