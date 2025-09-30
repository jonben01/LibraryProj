using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests;

public record UpdateBookRequestDto
{
    [Required]
    [MinLength(1)]
    public string BookIdForUpdate { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Pages { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Title { get; set; }

    [Required] public List<string> AuthorIds { get; set; } = [];
    
    public string? GenreId { get; set; }
}