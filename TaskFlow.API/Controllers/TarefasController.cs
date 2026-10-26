using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Models.DTOs;
using TaskFlow.API.Services.Interfaces;

namespace TaskFlow.API.Controllers;

/// <summary>Expõe HTTP, rotas e status codes. Sem regras de negócio.</summary>
[ApiController]
[Route("api/tarefas")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaService _service;

    public TarefasController(ITarefaService service) => _service = service;

    // GET /api/tarefas  |  GET /api/tarefas?status=Pendente
    [HttpGet]
    [ProducesResponseType(typeof(List<TarefaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] string? status) =>
        Ok(await _service.ListarAsync(status));

    // GET /api/tarefas/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id) =>
        Ok(await _service.ObterPorIdAsync(id));

    // POST /api/tarefas
    [HttpPost]
    [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Criar([FromBody] TarefaCreateDto dto)
    {
        var criada = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
    }

    // PUT /api/tarefas/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] TarefaUpdateDto dto) =>
        Ok(await _service.AtualizarAsync(id, dto));

    // DELETE /api/tarefas/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Excluir(int id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }

    // PATCH /api/tarefas/{id}/iniciar
    [HttpPatch("{id:int}/iniciar")]
    public async Task<IActionResult> Iniciar(int id) =>
        Ok(await _service.IniciarAsync(id));

    // PATCH /api/tarefas/{id}/concluir
    [HttpPatch("{id:int}/concluir")]
    public async Task<IActionResult> Concluir(int id) =>
        Ok(await _service.ConcluirAsync(id));
}
