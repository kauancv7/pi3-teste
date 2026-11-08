using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationsgls.Domain.Entities
{
    public class Relatorio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public Guid ProdutoId { get; set; }
        public Produto Produto { get; set; }


        // Navigation properties

        public void AdicionarNome()
        {
            if (Nome == null || Nome.Length < 3)
            {
                throw new ArgumentException("O nome do relatório deve ter pelo menos 3 caracteres.");
            }
        }
        public void AdicionarDescricao()
        {
            if (Descricao == null || Descricao.Length < 10)
            {
                throw new ArgumentException("A descrição do relatório deve ter pelo menos 10 caracteres.");
            }
        }

        public void Ativar()
        {
            Ativo = true;
        }
        public void Desativar()
        {
            Ativo = false;
        }
        public void Validar()
        {
            AdicionarNome();
            AdicionarDescricao();
        }
    }
}