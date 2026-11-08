using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;

namespace WebApplicationsgls.Application.Services
{
    public class AuthClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IConfiguration _configuration;

        public AuthClienteService(IClienteRepository clienteRepository, IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var cliente = await _clienteRepository.ObterPorEmailAsync(dto.Email);

            if (cliente is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, cliente.SenhaHash))
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new(ClaimTypes.Email, cliente.Email),
                new(TokenClaims.Roles, "Cliente"),
                new(TokenClaims.AccessType, TokenClaims.Client)
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);

            return new AuthResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                Expiracao = descriptor.Expires.Value,
                TipoAcesso = TokenClaims.Client
            };
        }
    }
}
