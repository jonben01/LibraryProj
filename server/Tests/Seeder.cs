using efscaffold;
using efscaffold.Entities;
using Microsoft.EntityFrameworkCore;


namespace Tests;

public interface ISeeder
{
    Task Seed();
    Task SeedWithRelations();
}

public class Seeder(MyDbContext context) : ISeeder
{

    private async Task ClearDataFromDb()
    {
        await context.Books.ExecuteDeleteAsync();
        await context.Authors.ExecuteDeleteAsync();
        await context.Genres.ExecuteDeleteAsync();
    }
    
    public async Task Seed()
    {
        await ClearDataFromDb();
        
        var author = new Author
        {
            Createdat = DateTime.UtcNow,
            Name = "John Doe",
            Id = "1"
        };
        
        var genre = new Genre
        {
            Createdat = DateTime.UtcNow,
            Name = "Action",
            Id = "1"
        };

        var book = new Book
        {
            Createdat = DateTime.UtcNow,
            Title = "Book",
            Pages = 12,
            Id = "1"
        };
        context.AddRange(author, genre, book);
        
        var author2 = new Author
        {
            Createdat = DateTime.UtcNow,
            Name = "Author 2",
            Id = "2"
        };
        
        var genre2 = new Genre
        {
            Createdat = DateTime.UtcNow,
            Name = "Action 2", // :)
            Id = "2"
        };

        var book2 = new Book
        {
            Createdat = DateTime.UtcNow,
            Title = "Book 2",
            Pages = 23,
            Id = "2"
        };
        context.AddRange(author2, genre2, book2);
        await context.SaveChangesAsync();
    }

    public async Task SeedWithRelations()
    {
        await ClearDataFromDb();
        
        var author = new Author
        {
            Createdat = DateTime.UtcNow,
            Name = "Jane Doe",
            Id = "1"
        };
        
        var genre = new Genre
        {
            Createdat = DateTime.UtcNow,
            Name = "Horror",
            Id = "1"
        };

        var book = new Book
        {
            Createdat = DateTime.UtcNow,
            Title = "Koob",
            Pages = 21,
            Id = "1",
            Authors  = new List<Author> { author },
            Genre = genre
        };
        
        context.AddRange(author, genre, book);

        var genre2 = new Genre
        {
            Createdat = DateTime.UtcNow,
            Name = "Comedy",
            Id = "2"
        };

        var author2 = new Author
        {
            Createdat = DateTime.UtcNow,
            Name = "Ronald",
            Id = "2"
        };

        var book2 = new Book
        {
            Createdat = DateTime.UtcNow,
            Title = "book2",
            Pages = 200,
            Id = "2",
            Authors = new List<Author> { author2 },
            Genre = genre2
        };
        
        context.AddRange(genre2, author2);
        await context.SaveChangesAsync();
    }
}