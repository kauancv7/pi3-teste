
﻿using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        Task RegistrarAsync(MovimentacaoEstoque movimentacao);
        Task<List<MovimentacaoEstoque>> ListarPorProdutoAsync(Guid produtoId);
    }
}
