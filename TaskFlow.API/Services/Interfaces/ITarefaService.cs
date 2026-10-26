using TaskFlow.API.Models.DTOs;

namespace TaskFlow.API.Services.Interfaces;

public interface ITarefaService
{
    Task<List<TarefaResponseDto>> ListarAsync(string? status);
    Task<TarefaResponseDto> ObterPorIdAsync(int id);
    Task<TarefaResponseDto> CriarAsync(TarefaCreateDto dto);
    Task<TarefaResponseDto> AtualizarAsync(int id, TarefaUpdateDto dto);
    Task ExcluirAsync(int id);
    Task<TarefaResponseDto> IniciarAsync(int id);
    Task<TarefaResponseDto> ConcluirAsync(int id);
}
