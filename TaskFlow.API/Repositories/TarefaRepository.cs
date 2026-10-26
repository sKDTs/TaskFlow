using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Data;
using TaskFlow.API.Models.Entities;
using TaskFlow.API.Models.Enums;
using TaskFlow.API.Repositories.Interfaces;

namespace TaskFlow.API.Repositories;

/// <summary>Responsável exclusivamente pelo acesso a dados (sem regras de negócio).</summary>
public class TarefaRepository : ITarefaRepository
{
    private readonly AppDbContext _context;

    public TarefaRepository(AppDbContext context) => _context = context;

    public async Task<List<Tarefa>> GetAllAsync(StatusTarefa? status = null)
    {
        var query = _context.Tarefas.Include(t => t.Categoria).AsNoTracking().AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        return await query.OrderBy(t => t.Id).ToListAsync();
    }

    public async Task<Tarefa?> GetByIdAsync(int id) =>
        await _context.Tarefas.Include(t => t.Categoria).FirstOrDefaultAsync(t => t.Id == id);

    public async Task<bool> CategoriaExisteAsync(int categoriaId) =>
        await _context.Categorias.AnyAsync(c => c.Id == categoriaId);

    public async Task AddAsync(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tarefa tarefa)
    {
        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync();
    }
}
