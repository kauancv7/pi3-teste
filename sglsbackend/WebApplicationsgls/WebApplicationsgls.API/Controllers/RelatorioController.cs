using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationsgls.Application.Services;
using WebApplicationsgls.API.Authorization;

namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/relatorios")]
[Authorize(Policy = AuthorizationPolicies.InternalUser, Roles = "Admin,Vendedor")]
public class RelatorioController : ControllerBase
{
    private readonly RelatorioService _service;

    public RelatorioController(RelatorioService service)
    {
        _service = service;
    }

    [HttpGet("vendas-periodo")]
    public async Task<IActionResult> ObterVendasPorPeriodo(
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim)
    {
        var usuarioId = ObterUsuarioId();
        var isAdmin = User.IsInRole("Admin");
        var hoje = DateTime.UtcNow.Date;
        var inicio = dataInicio?.Date ?? new DateTime(hoje.Year, hoje.Month, 1);
        var fim = dataFim?.Date ?? hoje;

        var relatorio = await _service.ObterVendasPorPeriodoAsync(
            inicio,
            fim,
            usuarioId,
            isAdmin);

        return Ok(relatorio);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> ObterDashboard()
    {
        var usuarioId = ObterUsuarioId();
        var isAdmin = User.IsInRole("Admin");

        var relatorio = await _service.ObterDashboardAsync(usuarioId, isAdmin);

        return Ok(relatorio);
    }

    private Guid ObterUsuarioId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
