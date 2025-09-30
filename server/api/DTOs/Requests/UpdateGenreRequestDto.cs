using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests;

public record UpdateGenreRequestDto
{
    [Required]
    [MinLength(1)]
    public string GenreIdForUpdate { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
}