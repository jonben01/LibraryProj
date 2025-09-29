using efscaffold.Entities;

namespace api.DTOs;

public class AuthorDto
{
    public AuthorDto(Author author)
    {
        Id = author.Id;
        Name = author.Name;
        CreatedAt = author.Createdat;
        Books = author.Books?.Select(book => book.Id).ToList() ?? [];
        
    }

    public DateTime? CreatedAt { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }
    public List<string> Books { get; set; }
}