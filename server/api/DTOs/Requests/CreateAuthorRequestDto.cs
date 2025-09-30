using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests;

public record CreateAuthorRequestDto
{
    [Required]
    [MinLength(1)]
    public string Name { get; init; }
}