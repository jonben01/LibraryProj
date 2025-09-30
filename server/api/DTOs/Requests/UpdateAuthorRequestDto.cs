using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests;

public record UpdateAuthorRequestDto
{
    [Required]
    [MinLength(1)]
    public string Name { get; init; }
    
    [Required]
    [MinLength(1)]
    public string AuthorIdForUpdate { get; init; }
    
    [Required]
    public List<string> BooksIds { get; init; }
}