using api.DTOs;
using api.DTOs.Requests;
using api.Services;
using efscaffold;
using efscaffold.Entities;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class BookTests(MyDbContext context, ISeeder seeder, IBookService bookService, ITestOutputHelper output)
{
    [Fact]
    public async Task CreateBook_Success()
    {
        await seeder.Seed();

        var requestDto = new CreateBookRequestDto
        {
            Title = "The Hitchhiker's Guide to the Galaxy",
            Pages = 520
        };
        
        var result = await bookService.CreateBook(requestDto);
        
        output.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));
        
        Assert.True(requestDto.Pages == result.Pages);
        Assert.True(requestDto.Title == result.Title);
        Assert.True(result.CreatedAt < DateTime.UtcNow);
        Assert.Null(result.GenreId);
        Assert.Empty(result.AuthorIds);
    }

    [Fact]
    public async Task GetBooks_Success()
    {
        await seeder.Seed();
        var books = await bookService.GetBooks();
        Assert.Equal(2, books.Count);
        Assert.Equal(books.First().Id, context.Books.First().Id);
    }

    [Fact]
    public async Task UpdateBook_CanAddGenreAndAuthor()
    {
        await seeder.Seed();
        
        var book = context.Books.First();
        var genreId = context.Genres.First().Id;
        var authorId = context.Authors.First().Id;

        var request = new UpdateBookRequestDto
        {
            Pages = 200,
            Title = "It",
            GenreId = genreId,
            AuthorIds = [authorId],
            BookIdForUpdate = book.Id
        };
        
        //check result DTO
        var result = await bookService.UpdateBook(request);
        
        Assert.NotNull(result);
        Assert.Equal(book.Id, result.Id);
        Assert.Equal(200, result.Pages);
        Assert.Equal("It", result.Title);
        Assert.NotNull(result.GenreId);
        Assert.Equal(genreId, result.GenreId);
        Assert.Contains(authorId, result.AuthorIds);
        
        //check persisted book entry
        var updatedBook = context.Books
            .Include(b => b.Genre)
            .Include(b => b.Authors)
            .First(b => b.Id == book.Id);
        
        Assert.Equal(book.Id, updatedBook.Id);
        Assert.Equal(200, updatedBook.Pages);
        Assert.Equal("It", updatedBook.Title);
        Assert.Equal(genreId, updatedBook.GenreId);
        Assert.Equal(authorId, updatedBook.Authors.First().Id); //only adding 1 author
    }
    
    //can delete,
    [Fact]
    public async Task DeleteBook_Success()
    {
        await seeder.Seed();
        
        var book = context.Books.First();
        
        await bookService.DeleteBook(book.Id);
        
        var deletedBook = await context.Books.FindAsync([book.Id], TestContext.Current.CancellationToken);
        Assert.Null(deletedBook);
        Assert.Equal(1, context.Books.Count());
        
    }

    [Fact]
    public async Task DeleteBook_FailureShouldThrowException()
    {
        await seeder.Seed();
        
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => bookService.DeleteBook("missing"));
        
        Assert.Contains("doesn't exist", exception.Message);
    }

    [Fact]
    public async Task UpdateBook_CanSwitchGenreAndAuthor()
    {
        await seeder.SeedWithRelations();
        //the book from the seeder has references to genre 1 and author 1
        
        var book = context.Books.First();

        var request = new UpdateBookRequestDto
        {
            AuthorIds = [context.Authors.First(a => a.Id == "2").Id],
            GenreId = context.Genres.First(g => g.Id == "2").Id,
            BookIdForUpdate = book.Id,
            Pages = 1823981,
            Title = "BOOOOOOOK"
        };
        
        var result = await bookService.UpdateBook(request);
        Assert.NotNull(result);
        Assert.Equal(book.Id, result.Id);
        Assert.Contains("2", result.AuthorIds);
        Assert.Equal("2", result.GenreId);
        
        var updatedBook = context.Books
            .Include(b => b.Genre)
            .Include(b => b.Authors)
            .First(b => b.Id == book.Id);
        
        Assert.Equal(book.Id, updatedBook.Id);
        Assert.Equal(1823981, updatedBook.Pages);
        Assert.NotNull(updatedBook.GenreId);
        Assert.Equal("2", updatedBook.GenreId);
        Assert.Contains("2", updatedBook.Authors.First().Id);

    }

    [Fact]
    public async Task UpdateBook_CanAddMultipleAuthors()
    {
        await seeder.Seed();
        
        var book = context.Books.First();
        var author1 = context.Authors.First(a => a.Id == "1");
        var author2 = context.Authors.First(a => a.Id == "2");

        var request = new UpdateBookRequestDto
        {
            AuthorIds = [author1.Id, author2.Id],
            GenreId = context.Genres.First(g => g.Id == "2").Id,
            BookIdForUpdate = book.Id,
            Pages = 2222,
            Title = "Book with multiple authors"
        };
        
        var result = await bookService.UpdateBook(request);
        Assert.NotNull(result);
        Assert.Equal(book.Id, result.Id);
        Assert.Equal("Book with multiple authors", result.Title);
        Assert.NotNull(result.GenreId);
        Assert.Equal(context.Genres.First(g => g.Id == "2").Id, result.GenreId);
        
        var updatedBook = context.Books
            .Include(b => b.Genre)
            .Include(b => b.Authors)
            .First(b => b.Id == book.Id);
        
        Assert.Equal("1", updatedBook.Id);
        Assert.Equal(2222, updatedBook.Pages);
        Assert.NotNull(updatedBook.GenreId);
        
        var authorIds = updatedBook.Authors.Select(a => a.Id).ToList();
        Assert.Contains(author1.Id, authorIds);
        Assert.Contains(author2.Id, authorIds);
    }
    
    
    //can remove author + genre
    
    //negative paths for author + genre
    
    //what happens if you try to add a genre or author already on the book?
    
    
    
    
}