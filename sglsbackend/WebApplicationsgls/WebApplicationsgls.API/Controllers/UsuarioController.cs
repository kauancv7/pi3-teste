using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationsgls.API.Authorization;
using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Services;
using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.API.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize(Policy = AuthorizationPolicies.Admin)]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _service.ListarAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Obter(Guid id)
    {
        var usuario = await _service.ObterPorIdAsync(id);

        if (usuario is null)
        {
            return NotFound(new
            {
                message = "Usuário não encontrado."
            });
        }

        return Ok(usuario);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Criar(
        [FromBody] CriarUsuarioDTO dto)
    {
        var usuario = await _service.CriarAsync(dto);

        return CreatedAtAction(
            nameof(Obter),
            new { id = usuario.Id },
            new
            {
                id = usuario.Id,
                message = "Usuário criado com sucesso!"
            }
        );
    }

    [HttpPut]
    public async Task<IActionResult> Atualizar(
        [FromBody] Usuario usuario)
    {
        try
        {
            await _service.AtualizarAsync(usuario);

            return Ok(new
            {
                message = "Usuário atualizado com sucesso!"
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> AlterarStatus(
        Guid id,
        [FromBody] AlterarStatusUsuarioDTO dto)
    {
        try
        {
            await _service.AlterarStatusAsync(
                id,
                dto.Ativo
            );

            return Ok(new
            {
                id,
                ativo = dto.Ativo,
                message = dto.Ativo
                    ? "Usuário reativado com sucesso!"
                    : "Usuário desativado com sucesso!"
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        try
        {
            await _service.RemoverAsync(id);

            return Ok(new
            {
                message = "Usuário removido com sucesso!"
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
    }
}
