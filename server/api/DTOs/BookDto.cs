using efscaffold.Entities;

namespace api.DTOs;

public class BookDto
{
    public BookDto(Book book)
    {
        Id = book.Id;
        Title = book.Title;
        Pages = book.Pages;
        CreatedAt = book.Createdat;
        GenreId = book.Genreid;
        AuthorsIds = book.Authors?.Select(author => author.Id).ToList() ?? new List<string>();
    }
    
    public string Id { get; set; }
    public string Title { get; set; }
    public int Pages { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<string> AuthorsIds { get; set; }
    public string? GenreId { get; set; }
    
    
}