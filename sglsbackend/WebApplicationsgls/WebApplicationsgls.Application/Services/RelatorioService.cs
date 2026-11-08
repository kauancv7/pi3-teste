using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Domain.Enums;
using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Application.Services;

public class RelatorioService
{
    private const int LimiteEstoqueBaixo = 5;
    private static readonly string[] DiasSemana = ["Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"];

    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public RelatorioService(
        IPedidoRepository pedidoRepository,
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository)
    {
        _pedidoRepository = pedidoRepository;
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<RelatorioVendasPeriodoDTO> ObterVendasPorPeriodoAsync(
        DateTime dataInicio,
        DateTime dataFim,
        Guid usuarioId,
        bool isAdmin)
    {
        var inicio = dataInicio.Date;
        var fim = dataFim.Date.AddDays(1).AddTicks(-1);

        if (inicio > fim)
            throw new DomainException("A data inicial deve ser menor ou igual à data final");

        var pedidos = await ListarPedidosPorPerfilAsync(usuarioId, isAdmin);
        var pedidosPeriodo = pedidos
            .Where(p => p.DataPedido >= inicio && p.DataPedido <= fim)
            .OrderByDescending(p => p.DataPedido)
            .ToList();

        var vendasConcluidas = pedidosPeriodo
            .Where(PedidoConcluido)
            .ToList();

        var resumo = new ResumoVendasPeriodoDTO(
            vendasConcluidas.Sum(p => p.ValorTotal),
            pedidosPeriodo.Count,
            vendasConcluidas.Count,
            pedidosPeriodo.Count(p => p.Status == StatusPedido.Cancelado));

        return new RelatorioVendasPeriodoDTO(
            inicio,
            fim,
            resumo,
            pedidosPeriodo.Select(MapearPedido).ToList());
    }

    public async Task<DashboardRelatorioDTO> ObterDashboardAsync(Guid usuarioId, bool isAdmin)
    {
        var pedidos = await ListarPedidosPorPerfilAsync(usuarioId, isAdmin);
        var produtos = await _produtoRepository.ListarPorPerfilAsync(usuarioId, isAdmin);
        var categorias = await _categoriaRepository.ListarAsync();

        var hoje = DateTime.UtcNow;
        var inicioSemana = ObterInicioSemana(hoje);
        var fimSemana = inicioSemana.AddDays(7).AddTicks(-1);

        var vendasDaSemana = DiasSemana
            .Select((dia, indice) =>
            {
                var data = inicioSemana.AddDays(indice).Date;
                var valor = pedidos
                    .Where(p => PedidoConcluido(p) && p.DataPedido.Date == data)
                    .Sum(p => p.ValorTotal);

                return new VendaSemanaDTO(dia, data, valor);
            })
            .ToList();

        var produtosAtivos = produtos.Where(p => p.Ativo).ToList();
        var totalProdutosAtivos = produtosAtivos.Count;

        var distribuicao = categorias
            .Select(categoria =>
            {
                var quantidade = produtosAtivos.Count(p => p.CategoriaId == categoria.Id);
                var percentual = totalProdutosAtivos == 0
                    ? 0
                    : Math.Round((decimal)quantidade / totalProdutosAtivos * 100, 2);

                return new DistribuicaoCategoriaDTO(categoria.Id, categoria.Nome, quantidade, percentual);
            })
            .Where(c => c.QuantidadeProdutos > 0)
            .OrderByDescending(c => c.QuantidadeProdutos)
            .ToList();

        var estoqueBaixo = produtosAtivos
            .Where(p => p.Estoque <= LimiteEstoqueBaixo)
            .OrderBy(p => p.Estoque)
            .ThenBy(p => p.Nome)
            .Select(p => new ProdutoEstoqueBaixoDTO(
                p.Id,
                p.Nome,
                p.Estoque,
                p.CategoriaId,
                p.Categoria?.Nome ?? "Sem categoria"))
            .ToList();

        return new DashboardRelatorioDTO(
            produtos.Count,
            categorias.Count,
            pedidos.Count,
            estoqueBaixo.Count,
            inicioSemana,
            fimSemana,
            vendasDaSemana,
            distribuicao,
            estoqueBaixo,
            pedidos.OrderByDescending(p => p.DataPedido).Take(4).Select(MapearPedido).ToList());
    }

    private async Task<List<Pedido>> ListarPedidosPorPerfilAsync(Guid usuarioId, bool isAdmin)
    {
        return isAdmin
            ? await _pedidoRepository.ListarAsync()
            : await _pedidoRepository.ListarPorUsuarioAsync(usuarioId);
    }

    private static bool PedidoConcluido(Pedido pedido)
    {
        return pedido.Status is StatusPedido.Finalizado or StatusPedido.Pago or StatusPedido.Entregue;
    }

    private static DateTime ObterInicioSemana(DateTime data)
    {
        var inicio = data.Date;
        var diasDesdeSegunda = ((int)inicio.DayOfWeek + 6) % 7;
        return inicio.AddDays(-diasDesdeSegunda);
    }

    private static PedidoRelatorioDTO MapearPedido(Pedido pedido)
    {
        return new PedidoRelatorioDTO(
            pedido.Id,
            pedido.DataPedido,
            pedido.Status.ToString(),
            pedido.ValorTotal);
    }
}
