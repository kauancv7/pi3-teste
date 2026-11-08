using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Domain.Enums;
using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Application.Services;

public class PedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly EstoqueService _estoqueService;

    public PedidoService(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository, EstoqueService estoqueService)
    {
        _pedidoRepository = pedidoRepository;
        _produtoRepository = produtoRepository;
        _estoqueService = estoqueService;
    }

    public async Task<Guid> CriarPedidoAsync(CriarPedidoDTO dto, Guid usuarioId)
    {
        var pedido = PedidoInterno(dto, usuarioId);
        await AdicionarItensAsync(pedido, dto);
        await _pedidoRepository.AdicionarAsync(pedido);
        return pedido.Id;
    }

    public async Task<Guid> CriarPedidoClienteAsync(CriarPedidoDTO dto, Guid clienteId)
    {
        var pedido = Pedido.CriarParaCliente(clienteId);
        await AdicionarItensAsync(pedido, dto);
        await _pedidoRepository.AdicionarAsync(pedido);
        return pedido.Id;
    }

    private static Pedido PedidoInterno(CriarPedidoDTO dto, Guid usuarioId)
    {
        if (dto.Itens is null || dto.Itens.Count == 0)
            throw new DomainException("Pedido deve conter ao menos um item");
        return new Pedido(usuarioId);
    }

    private async Task AdicionarItensAsync(Pedido pedido, CriarPedidoDTO dto)
    {
        if (dto.Itens is null || dto.Itens.Count == 0)
            throw new DomainException("Pedido deve conter ao menos um item");

        foreach (var itemDto in dto.Itens)
        {
            if (itemDto.Quantidade <= 0)
                throw new DomainException("Quantidade inválida");

            var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoId)
                ?? throw new DomainException("Produto não encontrado");
            if (!produto.Ativo)
                throw new DomainException($"Produto {produto.Nome} está inativo");
            if (produto.Estoque < itemDto.Quantidade)
                throw new DomainException($"Estoque insuficiente para o produto {produto.Nome}");

            pedido.AdicionarItem(produto, itemDto.Quantidade);
        }
    }

    public async Task FinalizarPedidoAsync(Guid pedidoId, Guid usuarioId)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId)
            ?? throw new DomainException("Pedido não encontrado");
        foreach (var item in pedido.Itens)
            await _estoqueService.SaidaAsync(new SaidaEstoqueDTO { ProdutoId = item.ProdutoId, Quantidade = item.Quantidade, Observacao = $"Pedido #{pedido.Id}" }, usuarioId);
        pedido.Finalizar();
        await _pedidoRepository.AtualizarAsync(pedido);
    }

    public async Task FinalizarPedidoClienteAsync(Guid pedidoId, Guid clienteId)
    {
        var pedido = await ObterPorClienteAsync(pedidoId, clienteId);
        if (pedido.Status != StatusPedido.Pendente)
            throw new DomainException("Este pedido não está pendente");
        foreach (var item in pedido.Itens)
            await _estoqueService.SaidaAsync(new SaidaEstoqueDTO { ProdutoId = item.ProdutoId, Quantidade = item.Quantidade, Observacao = $"Pedido do cliente #{pedido.Id}" }, null);
        pedido.Finalizar();
        await _pedidoRepository.AtualizarAsync(pedido);
    }

    public async Task CancelarPedidoAsync(Guid pedidoId, Guid usuarioId)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId)
            ?? throw new DomainException("Pedido não encontrado");
        foreach (var item in pedido.Itens)
            await _estoqueService.EntradaAsync(new EntradaEstoqueDTO { ProdutoId = item.ProdutoId, Quantidade = item.Quantidade, Observacao = $"Cancelamento Pedido #{pedido.Id}" }, usuarioId);
        pedido.Cancelar();
        await _pedidoRepository.AtualizarAsync(pedido);
    }

    public async Task CancelarPedidoClienteAsync(Guid pedidoId, Guid clienteId)
    {
        var pedido = await ObterPorClienteAsync(pedidoId, clienteId);
        if (pedido.Status == StatusPedido.Finalizado)
            foreach (var item in pedido.Itens)
                await _estoqueService.EntradaAsync(new EntradaEstoqueDTO { ProdutoId = item.ProdutoId, Quantidade = item.Quantidade, Observacao = $"Cancelamento do cliente #{pedido.Id}" }, null);
        pedido.Cancelar();
        await _pedidoRepository.AtualizarAsync(pedido);
    }

    public Task<List<Pedido>> ListarAsync(Guid usuarioId, bool isAdmin) => isAdmin ? _pedidoRepository.ListarAsync() : _pedidoRepository.ListarPorUsuarioAsync(usuarioId);
    public Task<List<Pedido>> ListarPorClienteAsync(Guid clienteId) => _pedidoRepository.ListarPorClienteAsync(clienteId);

    public async Task<Pedido> ObterPorIdAsync(Guid pedidoId, Guid usuarioId, bool isAdmin)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId) ?? throw new DomainException("Pedido não encontrado");
        if (!isAdmin && pedido.UsuarioId != usuarioId) throw new DomainException("Você não possui acesso a este pedido");
        return pedido;
    }

    public async Task<Pedido> ObterPorClienteAsync(Guid pedidoId, Guid clienteId)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId) ?? throw new DomainException("Pedido não encontrado");
        if (pedido.ClienteId != clienteId) throw new DomainException("Você não possui acesso a este pedido");
        return pedido;
    }
}
