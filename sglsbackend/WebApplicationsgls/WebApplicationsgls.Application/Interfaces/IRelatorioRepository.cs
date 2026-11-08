using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Interfaces
{
    public interface IRelatorioRepository
    {
        Task<List<Relatorio>> ListarAsync();
        Task<Relatorio?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Relatorio relatorio);
        Task AtualizarAsync(Relatorio relatorio);
        Task RemoverAsync(Relatorio relatorio);
    }
}
