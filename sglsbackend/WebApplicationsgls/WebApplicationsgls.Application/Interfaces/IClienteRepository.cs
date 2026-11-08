using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Interfaces
{
    public interface IClienteRepository
    {
        Task AdicionarAsync(Cliente usuario);
        Task<Cliente?> ObterPorIdAsync(Guid id);
        Task<Cliente?> ObterPorEmailAsync(string email);
        Task AtualizarAsync(Cliente usuario);
        Task RemoverAsync(Cliente usuario);
        Task SalvarAlteracoesAsync();
    }
}
