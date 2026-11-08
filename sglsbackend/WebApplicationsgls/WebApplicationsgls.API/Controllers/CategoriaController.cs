using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Services;
using WebApplicationsgls.API.Authorization;


namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/categorias")]
[Authorize(Policy = AuthorizationPolicies.InternalUser)]
public class CategoriaController : ControllerBase
{
    private readonly CategoriaService _service;

    public CategoriaController(CategoriaService service)
    {
        _service = service;
    }

    // Admin vê tudo | Vendedor vê apenas ativas | Marketing não vê nada
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var perfil = User.FindFirstValue(ClaimTypes.Role);
        var somenteAtivas = perfil == "Vendedor";

        return Ok(await _service.ListarAsync(somenteAtivas));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Criar(CriarCategoriaDTO dto)
    {
        var categoria = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(Listar), categoria);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(AtualizarCategoriaDTO dto)
    {
        await _service.AtualizarAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Desativar(Guid id)
    {
        await _service.DesativarAsync(id);
        return NoContent();
    }
}

