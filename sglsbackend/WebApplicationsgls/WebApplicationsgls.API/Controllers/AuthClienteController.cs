using Microsoft.AspNetCore.Mvc;
using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Services;

namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/v1/auth/cliente")]
public class AuthClienteController : ControllerBase
{
    private readonly AuthClienteService _authClienteService;

    public AuthClienteController(AuthClienteService authClienteService)
    {
        _authClienteService = authClienteService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            return Ok(await _authClienteService.LoginAsync(dto));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Erro: Usuário ou senha invpalidos" });
        }
    }
}
