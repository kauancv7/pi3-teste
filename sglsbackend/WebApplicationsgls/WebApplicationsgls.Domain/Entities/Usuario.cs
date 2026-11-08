using WebApplicationsgls.Domain.Enums;
using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; }

    public string Email { get; private set; }

    public string SenhaHash { get; private set; }

    public PerfilUsuarioEnum Perfil { get; private set; }

    public bool Ativo { get; private set; }

    public DateTime DataCriacao { get; private set; }

    protected Usuario() { }

    public Usuario(string nome, string email, string senhaHash, PerfilUsuarioEnum perfil)
    {
        Validar(nome, email, senhaHash, perfil);

        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Perfil = perfil;
        Ativo = true;
        DataCriacao = DateTime.UtcNow;
    }
    public void AdicionarPerfil(PerfilUsuarioEnum perfil)
    {
        Perfil = perfil;
    }

    public void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome inválido");
        Nome = nome;
    }

    public void AlterarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email inválido");

        Email = email;
    }

    public void AlterarSenha(string novaSenhaHash)
    {
        if (string.IsNullOrWhiteSpace(novaSenhaHash))
            throw new DomainException("Senha inválida");

        SenhaHash = novaSenhaHash;
    }

    public void Desativar()
    {
        if (!Ativo)
            throw new DomainException("Usuário já está desativado");

        Ativo = false;
    }

    public void Ativar()
    {
        if (Ativo)
            throw new DomainException("Usuário já ativo");

        Ativo = true;
    }

    public void AlterarStatus(bool ativo)
    {
        Ativo = ativo;
    }

    private void Validar(string nome, string email, string senhaHash, PerfilUsuarioEnum perfil)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome obrigatório");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email obrigatório");

        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new DomainException("Senha obrigatória");

        if (!Enum.IsDefined(perfil))
            throw new DomainException("Perfil inválido");
    }
}