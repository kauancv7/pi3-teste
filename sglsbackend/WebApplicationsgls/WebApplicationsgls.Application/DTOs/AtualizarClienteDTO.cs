namespace WebApplicationsgls.Application.DTOs;

public class AtualizarClienteDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Senha { get; set; }
}
