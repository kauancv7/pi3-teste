using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Services;
using WebApplicationsgls.API.Authorization;

namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/produtos")]
[Authorize(Policy = AuthorizationPolicies.InternalUser)]
public class ProdutoController : ControllerBase
{
    private readonly ProdutoService _service;

    public ProdutoController(ProdutoService service)
    {
        _service = service;
    }

    private Guid? ObterUsuarioIdDoToken()
    {
        var claimId = User.Claims.FirstOrDefault(c => 
            c.Type.EndsWith("nameidentifier", StringComparison.OrdinalIgnoreCase) || 
            c.Type.EndsWith("sub", StringComparison.OrdinalIgnoreCase) || 
            c.Type.EndsWith("id", StringComparison.OrdinalIgnoreCase))?.Value;

        if (string.IsNullOrEmpty(claimId) || !Guid.TryParse(claimId, out var id))
        {
            return null;
        }

        return id;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Listar()
    {
        var usuarioId = ObterUsuarioIdDoToken();
        if (usuarioId == null)
        {
            return Unauthorized(new { message = "O token não contém um identificador de usuário válido." });
        }

        var isAdmin = User.IsInRole("Admin");
        var produtos = await _service.ListarAsync(usuarioId.Value, isAdmin);

        return Ok(produtos);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Obter(Guid id)
    {
        var usuarioId = ObterUsuarioIdDoToken();
        if (usuarioId == null)
        {
            return Unauthorized(new { message = "O token não contém um identificador de usuário válido." });
        }

        var isAdmin = User.IsInRole("Admin");
        var produto = await _service.ObterPorIdAsync(id, usuarioId.Value, isAdmin);

        if (produto == null)
            return NotFound();

        return Ok(produto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Criar(CriarProdutoDTO dto)
    {
        var usuarioId = ObterUsuarioIdDoToken();
        if (usuarioId == null)
        {
            return Unauthorized(new { message = "O token não contém um identificador de usuário válido." });
        }
        
        var produtoCriado = await _service.CriarAsync(dto, usuarioId.Value);

        return CreatedAtAction(nameof(Obter), new { id = produtoCriado.Id }, produtoCriado);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Atualizar(AtualizarProdutoDTO dto)
    {
        var usuarioId = ObterUsuarioIdDoToken();
        if (usuarioId == null)
        {
            return Unauthorized(new { message = "O token não contém um identificador de usuário válido." });
        }

        var isAdmin = User.IsInRole("Admin");

        await _service.AtualizarAsync(dto, usuarioId.Value, isAdmin);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Remover(Guid id)
    {
        await _service.RemoverAsync(id);
        return Ok();
    }

    [HttpGet("publicos")]
    [AllowAnonymous]
    public async Task<IActionResult> ListarProdutosPublicos()
    {
        var produtos = await _service.ListarProdutosPublicosAsync();
        return Ok(produtos);
    }
}