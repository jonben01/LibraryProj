using api.DTOs;
using api.DTOs.Requests;
using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class BookService(MyDbContext context) : IBookService
{
    public Task<List<BookDto>> GetBooks()
    {
        return context.Books
            .Include(book => book.Genre)
            .Include(book => book.Authors)
            .Select(book => new BookDto(book))
            .ToListAsync();

    }

    public async Task<BookDto> CreateBook(CreateBookRequestDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Pages = dto.Pages,
            Createdat = DateTime.UtcNow,
            Id = Guid.NewGuid().ToString(),
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return new BookDto(book);
    }

    
    public Task<BookDto> UpdateBook(UpdateBookRequestDto book)
    {
        throw new NotImplementedException();
    }

    public Task<BookDto> DeleteBook(string id)
    {
        throw new NotImplementedException();
    }
}