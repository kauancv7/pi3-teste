using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationsgls.API.Authorization;
using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Services;
using WebApplicationsgls.Domain.Exceptions;

namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize(Policy = AuthorizationPolicies.Client)]
public class ClienteController : ControllerBase
{
    private readonly ClienteService _service;
    private readonly PedidoService _pedidoService;
    public ClienteController(ClienteService service, PedidoService pedidoService) { _service = service; _pedidoService = pedidoService; }

    private Guid ClienteId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("me")]
    public async Task<IActionResult> ObterMeuPerfil()
    {
        var cliente = await _service.ObterPorIdAsync(ClienteId);
        return cliente is null ? NotFound(new { message = "Cliente não encontrado." }) : Ok(cliente);
    }

    [HttpPost("cadastro")]
    [AllowAnonymous]
    public async Task<IActionResult> Cadastrar(CriarClienteDTO dto)
    {
        try
        {
            var cliente = await _service.CriaClienteAsync(dto);
            return CreatedAtAction(nameof(ObterMeuPerfil), new { id = cliente.Id }, new { id = cliente.Id, nome = cliente.Nome, email = cliente.Email, telefone = cliente.Telefone, message = "Cliente cadastrado com sucesso." });
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPut("me")]
    public async Task<IActionResult> AtualizarMeuPerfil(AtualizarClienteDTO dto)
    {
        try
        {
            var cliente = await _service.AtualizarAsync(ClienteId, dto);
            return Ok(new { id = cliente.Id, nome = cliente.Nome, email = cliente.Email, telefone = cliente.Telefone, message = "Dados do cliente atualizados com sucesso." });
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpDelete("me")]
    public async Task<IActionResult> ExcluirMinhaConta()
    {
        try { await _service.RemoverAsync(ClienteId); return Ok(new { message = "Conta do cliente removida com sucesso." }); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPost("pedidos")]
    public async Task<IActionResult> CriarPedido(CriarPedidoDTO dto)
    {
        try { return Ok(new { id = await _pedidoService.CriarPedidoClienteAsync(dto, ClienteId), message = "Pedido do cliente criado com sucesso." }); }
        catch (DomainException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpGet("pedidos")]
    public async Task<IActionResult> ListarMeusPedidos() => Ok(await _pedidoService.ListarPorClienteAsync(ClienteId));

    [HttpGet("pedidos/{id:guid}")]
    public async Task<IActionResult> ObterMeuPedido(Guid id)
    {
        try { return Ok(await _pedidoService.ObterPorClienteAsync(id, ClienteId)); }
        catch (DomainException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPost("pedidos/{id:guid}/finalizar")]
    public async Task<IActionResult> FinalizarMeuPedido(Guid id)
    {
        try { await _pedidoService.FinalizarPedidoClienteAsync(id, ClienteId); return Ok(new { message = "Pedido finalizado com sucesso." }); }
        catch (DomainException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("pedidos/{id:guid}/cancelar")]
    public async Task<IActionResult> CancelarMeuPedido(Guid id)
    {
        try { await _pedidoService.CancelarPedidoClienteAsync(id, ClienteId); return Ok(new { message = "Pedido cancelado com sucesso." }); }
        catch (DomainException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
