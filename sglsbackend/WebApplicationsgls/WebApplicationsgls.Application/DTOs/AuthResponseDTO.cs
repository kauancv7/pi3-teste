namespace WebApplicationsgls.Application.DTOs;

public class AuthResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracao { get; set; }
    public string TipoAcesso { get; set; } = string.Empty;
    public object? Perfil { get; set; }
}
