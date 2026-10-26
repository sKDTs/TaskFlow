using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Models.Entities;
using TaskFlow.API.Models.Enums;

namespace TaskFlow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(e =>
        {
            e.ToTable("Categorias");
            e.HasKey(c => c.Id);
            e.Property(c => c.Nome).IsRequired().HasMaxLength(100);

            e.HasData(
                new Categoria { Id = 1, Nome = "Desenvolvimento" },
                new Categoria { Id = 2, Nome = "Design" },
                new Categoria { Id = 3, Nome = "Infraestrutura" });
        });

        modelBuilder.Entity<Tarefa>(e =>
        {
            e.ToTable("Tarefas");
            e.HasKey(t => t.Id);
            e.Property(t => t.Titulo).IsRequired().HasMaxLength(150);
            e.Property(t => t.Descricao).HasMaxLength(500);
            e.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue(StatusTarefa.Pendente);
            e.Property(t => t.DataCriacao).IsRequired();

            // Relacionamento 1:N (Categoria 1 -> N Tarefas)
            e.HasOne(t => t.Categoria)
                .WithMany(c => c.Tarefas)
                .HasForeignKey(t => t.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
