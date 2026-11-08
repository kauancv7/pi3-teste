namespace WebApplicationsgls.Application.DTOs;

public record PedidoRelatorioDTO(
    Guid Id,
    DateTime DataPedido,
    string Status,
    decimal ValorTotal);

public record ResumoVendasPeriodoDTO(
    decimal FaturamentoTotal,
    int TotalPedidos,
    int VendasConcluidas,
    int VendasCanceladas);

public record RelatorioVendasPeriodoDTO(
    DateTime DataInicio,
    DateTime DataFim,
    ResumoVendasPeriodoDTO Resumo,
    IReadOnlyCollection<PedidoRelatorioDTO> Pedidos);

public record VendaSemanaDTO(
    string Dia,
    DateTime Data,
    decimal Valor);

public record DistribuicaoCategoriaDTO(
    Guid CategoriaId,
    string Categoria,
    int QuantidadeProdutos,
    decimal Percentual);

public record ProdutoEstoqueBaixoDTO(
    Guid Id,
    string Nome,
    int Estoque,
    Guid CategoriaId,
    string Categoria);

public record DashboardRelatorioDTO(
    int TotalProdutos,
    int TotalCategorias,
    int TotalPedidos,
    int EstoqueBaixo,
    DateTime InicioSemana,
    DateTime FimSemana,
    IReadOnlyCollection<VendaSemanaDTO> VendasDaSemana,
    IReadOnlyCollection<DistribuicaoCategoriaDTO> DistribuicaoPorCategoria,
    IReadOnlyCollection<ProdutoEstoqueBaixoDTO> ProdutosEstoqueBaixo,
    IReadOnlyCollection<PedidoRelatorioDTO> UltimosPedidos);
