using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Application.Services;

public class ClienteService
{
    private readonly IClienteRepository _repository;
    public ClienteService(IClienteRepository repository) => _repository = repository;

    public async Task<Cliente> CriaClienteAsync(CriarClienteDTO dto)
    {
        var nome = dto.Nome?.Trim() ?? string.Empty;
        var email = NormalizarEmail(dto.Email);
        var telefone = dto.Telefone?.Trim() ?? string.Empty;
        Validar(nome, email, telefone, dto.Senha);
        if (await _repository.ObterPorEmailAsync(email) is not null)
            throw new InvalidOperationException("Já existe um cliente cadastrado com este e-mail.");
        var cliente = new Cliente(nome, email, telefone, BCrypt.Net.BCrypt.HashPassword(dto.Senha));
        await _repository.AdicionarAsync(cliente);
        return cliente;
    }

    public Task<Cliente?> ObtemPorEmailAsync(string email) => _repository.ObterPorEmailAsync(NormalizarEmail(email));
    public Task<Cliente?> ObterPorIdAsync(Guid id) => _repository.ObterPorIdAsync(id);

    public async Task<Cliente> AtualizarAsync(Guid id, AtualizarClienteDTO dto)
    {
        var cliente = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException("Cliente não encontrado.");
        var nome = dto.Nome?.Trim() ?? string.Empty;
        var email = NormalizarEmail(dto.Email);
        var telefone = dto.Telefone?.Trim() ?? string.Empty;
        Validar(nome, email, telefone, null);
        var outro = await _repository.ObterPorEmailAsync(email);
        if (outro is not null && outro.Id != id)
            throw new InvalidOperationException("Já existe um cliente cadastrado com este e-mail.");
        if (!string.IsNullOrWhiteSpace(dto.Senha) && dto.Senha.Length < 6)
            throw new ArgumentException("A senha deve ter pelo menos 6 caracteres.");
        cliente.Atualizar(nome, email, telefone);
        if (!string.IsNullOrWhiteSpace(dto.Senha)) cliente.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
        await _repository.AtualizarAsync(cliente);
        return cliente;
    }

    public async Task RemoverAsync(Guid id)
    {
        var cliente = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException("Cliente não encontrado.");
        await _repository.RemoverAsync(cliente);
    }

    private static string NormalizarEmail(string? email) => (email ?? string.Empty).Trim().ToLowerInvariant();
    private static void Validar(string nome, string email, string telefone, string? senha)
    {
        if (nome.Length < 2) throw new ArgumentException("Nome inválido.");
        if (!email.Contains('@') || email.Length < 5) throw new ArgumentException("E-mail inválido.");
        if (telefone.Length < 8) throw new ArgumentException("Telefone inválido.");
        if (senha is not null && senha.Length < 6) throw new ArgumentException("A senha deve ter pelo menos 6 caracteres.");
    }
}
