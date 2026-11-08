using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Services;
using WebApplicationsgls.API.Authorization;


namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/estoque")]
[Authorize(Policy = AuthorizationPolicies.InternalUser, Roles = "Admin,Vendedor")]
public class EstoqueController : ControllerBase
{
    private readonly EstoqueService _service;

    public EstoqueController(EstoqueService service)
    {
        _service = service;
    }

    [HttpPost("entrada")]
    public async Task<IActionResult> Entrada(EntradaEstoqueDTO dto)
    {
        var usuarioId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        await _service.EntradaAsync(dto, usuarioId);
        return Ok();
    }

    [HttpPost("saida")]
    public async Task<IActionResult> Saida(SaidaEstoqueDTO dto)
    {
        var usuarioId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        await _service.SaidaAsync(dto, usuarioId);
        return Ok();
    }
}
