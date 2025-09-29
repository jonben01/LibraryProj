using efscaffold.Entities;

namespace api.DTOs;

public class GenreDto
{
    public GenreDto(Genre genre)
    {
        Id =  genre.Id;
        Name = genre.Name;
        CreatedAt = genre.Createdat;
        Books = genre.Books?.Select(book => book.Id).ToList() ?? [];
    }

    public List<string> Books { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Name { get; set; }

    public string Id { get; set; }
}