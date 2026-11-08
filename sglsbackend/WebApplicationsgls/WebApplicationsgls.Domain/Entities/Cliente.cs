using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public string Telefone { get; set; }

        protected Cliente() { }

        public Cliente( string nome, string email, string telefone, string senhaHash)
        {
            Validar(nome, email, telefone, senhaHash);
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            Telefone = telefone;
            SenhaHash = senhaHash;
        }

        private void Validar(string nome, string email, string telefone, string senhaHash)
        {

            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome obrigatório");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email obrigatório");

            if (string.IsNullOrWhiteSpace(senhaHash))
                throw new DomainException("Senha obrigatória");
        }

        public void Atualizar(string nome, string email, string telefone)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone;
        }
        public void AtualizarTelefone(string telefone)
        {
            Telefone = telefone;
        }
        public void AtualizarEmail(string email)
        {
            Email = email;
        }
        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }

    }
}
