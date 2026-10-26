namespace TaskFlow.API.Models.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    // Relacionamento 1:N -> uma categoria possui várias tarefas
    public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
