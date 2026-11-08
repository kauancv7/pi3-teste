using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Services;
using WebApplicationsgls.API.Authorization;


namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/pedidos")]
[Authorize(Policy = AuthorizationPolicies.InternalUser)]
public class PedidoController : ControllerBase
{
    private readonly PedidoService _service;

    public PedidoController(PedidoService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Criar(CriarPedidoDTO dto)
    {
        var usuarioId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        var pedidoId = await _service.CriarPedidoAsync(dto, usuarioId);
        return Ok(new { id = pedidoId, message = "Pedido criado com sucesso." });
    }

    [HttpPost("{id}/finalizar")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Finalizar(Guid id)
    {
        var usuarioId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        await _service.FinalizarPedidoAsync(id, usuarioId);
        return Ok(new { message = "Pedido finalizado com sucesso." });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Listar()
    {
        var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");

        var pedidos = await _service.ListarAsync(usuarioId, isAdmin);
        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");

        var pedido = await _service.ObterPorIdAsync(id, usuarioId, isAdmin);
        return Ok(pedido);
    }

    [HttpPost("{id}/cancelar")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancelar(Guid id)
    {
        var usuarioId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        await _service.CancelarPedidoAsync(id, usuarioId);
        return Ok(new { message = "Pedido cancelado com sucesso." });
    }
}
