using System.Globalization;
using System.Text;
using TaskFlow.API.Exceptions;
using TaskFlow.API.Models.DTOs;
using TaskFlow.API.Models.Entities;
using TaskFlow.API.Models.Enums;
using TaskFlow.API.Repositories.Interfaces;
using TaskFlow.API.Services.Interfaces;

namespace TaskFlow.API.Services;

/// <summary>Regras de negócio e validações.</summary>
public class TarefaService : ITarefaService
{
    private readonly ITarefaRepository _repository;

    public TarefaService(ITarefaRepository repository) => _repository = repository;

    public async Task<List<TarefaResponseDto>> ListarAsync(string? status)
    {
        StatusTarefa? filtro = null;

        if (!string.IsNullOrWhiteSpace(status))
        {
            // aceita "Concluida" e "Concluída", ignorando maiúsculas/minúsculas
            var normalizado = RemoverAcentos(status.Trim());
            if (!Enum.TryParse<StatusTarefa>(normalizado, ignoreCase: true, out var parsed)
                || !Enum.IsDefined(parsed))
            {
                throw new BusinessException(
                    "Status inválido. Valores aceitos: Pendente, EmAndamento, Concluida.");
            }
            filtro = parsed;
        }

        var tarefas = await _repository.GetAllAsync(filtro);
        return tarefas.Select(ToDto).ToList();
    }

    public async Task<TarefaResponseDto> ObterPorIdAsync(int id) =>
        ToDto(await BuscarOuFalharAsync(id));

    public async Task<TarefaResponseDto> CriarAsync(TarefaCreateDto dto)
    {
        ValidarTexto(dto.Titulo, dto.Descricao);

        if (dto.CategoriaId.HasValue && !await _repository.CategoriaExisteAsync(dto.CategoriaId.Value))
            throw new BusinessException($"Categoria {dto.CategoriaId} não existe.");

        var tarefa = new Tarefa
        {
            Titulo = dto.Titulo.Trim(),
            Descricao = dto.Descricao?.Trim(),
            CategoriaId = dto.CategoriaId,
            Status = StatusTarefa.Pendente,   // regra RF02
            DataCriacao = DateTime.Now        // regra RF02
        };

        await _repository.AddAsync(tarefa);

        // recarrega para trazer a Categoria no retorno
        return ToDto(await BuscarOuFalharAsync(tarefa.Id));
    }

    public async Task<TarefaResponseDto> AtualizarAsync(int id, TarefaUpdateDto dto)
    {
        ValidarTexto(dto.Titulo, dto.Descricao);

        var tarefa = await BuscarOuFalharAsync(id);
        tarefa.Titulo = dto.Titulo.Trim();
        tarefa.Descricao = dto.Descricao?.Trim();
        // DataCriacao e DataConclusao nunca são alteradas aqui (RF04)

        await _repository.UpdateAsync(tarefa);
        return ToDto(tarefa);
    }

    public async Task ExcluirAsync(int id)
    {
        var tarefa = await BuscarOuFalharAsync(id);
        await _repository.DeleteAsync(tarefa);
    }

    public async Task<TarefaResponseDto> IniciarAsync(int id)
    {
        var tarefa = await BuscarOuFalharAsync(id);

        if (tarefa.Status != StatusTarefa.Pendente)
            throw new BusinessException(
                $"Só é possível iniciar tarefas Pendentes. Status atual: {tarefa.Status}.");

        tarefa.Status = StatusTarefa.EmAndamento;
        await _repository.UpdateAsync(tarefa);
        return ToDto(tarefa);
    }

    public async Task<TarefaResponseDto> ConcluirAsync(int id)
    {
        var tarefa = await BuscarOuFalharAsync(id);

        if (tarefa.Status != StatusTarefa.EmAndamento)
            throw new BusinessException(
                $"Só é possível concluir tarefas EmAndamento. Status atual: {tarefa.Status}.");

        tarefa.Status = StatusTarefa.Concluida;
        tarefa.DataConclusao = DateTime.Now;   // regra RF07
        await _repository.UpdateAsync(tarefa);
        return ToDto(tarefa);
    }

    // ---------- helpers ----------
    private async Task<Tarefa> BuscarOuFalharAsync(int id) =>
        await _repository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Tarefa com id {id} não encontrada.");

    private static void ValidarTexto(string titulo, string? descricao)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new BusinessException("O título é obrigatório.");
        if (titulo.Trim().Length > 150)
            throw new BusinessException("O título deve ter no máximo 150 caracteres.");
        if (descricao is { Length: > 500 })
            throw new BusinessException("A descrição deve ter no máximo 500 caracteres.");
    }

    private static string RemoverAcentos(string texto)
    {
        var sb = new StringBuilder();
        foreach (var c in texto.Normalize(NormalizationForm.FormD))
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    private static TarefaResponseDto ToDto(Tarefa t) => new()
    {
        Id = t.Id,
        Titulo = t.Titulo,
        Descricao = t.Descricao,
        Status = t.Status.ToString(),
        DataCriacao = t.DataCriacao,
        DataConclusao = t.DataConclusao,
        CategoriaId = t.CategoriaId,
        CategoriaNome = t.Categoria?.Nome
    };
}
