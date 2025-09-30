using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests;

public record CreateGenreRequestDto
{
    [Required]
    [MinLength(1)]
    public string Name { get; init; }
}