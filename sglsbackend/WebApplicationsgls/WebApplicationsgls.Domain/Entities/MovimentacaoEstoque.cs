using WebApplicationsgls.Domain.Enums;
using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Domain.Entities;

public class MovimentacaoEstoque
{
    public Guid Id { get; private set; }
    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; private set; }
    public TipoMovimentacaoEstoque Tipo { get; private set; }
    public int Quantidade { get; private set; }
    public Guid? UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; }
    public DateTime Data { get; private set; }
    public string? Observacao { get; private set; }

    protected MovimentacaoEstoque() { }

    private MovimentacaoEstoque(Guid produtoId, Guid? usuarioId, TipoMovimentacaoEstoque tipo, int quantidade, string? observacao)
    {
        if (quantidade <= 0) throw new DomainException("Quantidade inválida");
        Id = Guid.NewGuid(); ProdutoId = produtoId; UsuarioId = usuarioId; Tipo = tipo; Quantidade = quantidade; Observacao = observacao; Data = DateTime.UtcNow;
    }

    public static MovimentacaoEstoque CriarEntrada(Guid produtoId, Guid? usuarioId, int quantidade, string? observacao) => new(produtoId, usuarioId, TipoMovimentacaoEstoque.Entrada, quantidade, observacao);
    public static MovimentacaoEstoque CriarSaida(Guid produtoId, Guid? usuarioId, int quantidade, string? observacao) => new(produtoId, usuarioId, TipoMovimentacaoEstoque.Saida, quantidade, observacao);
}
