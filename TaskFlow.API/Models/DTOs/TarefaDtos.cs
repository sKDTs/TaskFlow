using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.Models.DTOs;

public class TarefaCreateDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título deve ter no máximo 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string? Descricao { get; set; }

    public int? CategoriaId { get; set; }
}

// Somente Titulo e Descricao podem ser alterados (RF04)
public class TarefaUpdateDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título deve ter no máximo 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string? Descricao { get; set; }
}

public class TarefaResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public int? CategoriaId { get; set; }
    public string? CategoriaNome { get; set; }
}
