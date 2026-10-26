using TaskFlow.API.Models.Entities;
using TaskFlow.API.Models.Enums;

namespace TaskFlow.API.Repositories.Interfaces;

public interface ITarefaRepository
{
    Task<List<Tarefa>> GetAllAsync(StatusTarefa? status = null);
    Task<Tarefa?> GetByIdAsync(int id);
    Task<bool> CategoriaExisteAsync(int categoriaId);
    Task AddAsync(Tarefa tarefa);
    Task UpdateAsync(Tarefa tarefa);
    Task DeleteAsync(Tarefa tarefa);
}
