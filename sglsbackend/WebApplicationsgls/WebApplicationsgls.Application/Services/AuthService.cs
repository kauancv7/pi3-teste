namespace WebApplicationsgls.Application.Services;

using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);

        if (usuario == null || !usuario.Ativo)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos");

        if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("Usuário ou senha inválidos");

        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            // The enum name is the stable authorization value; never serialize its underlying integer.
            new Claim(TokenClaims.Roles, usuario.Perfil.ToString()),
            new Claim(TokenClaims.AccessType, TokenClaims.InternalUser)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponseDTO
        {
            Token = tokenHandler.WriteToken(token),
            Expiracao = tokenDescriptor.Expires.Value,
            Perfil = usuario.Perfil,
            TipoAcesso = TokenClaims.InternalUser
        };
    }
}
