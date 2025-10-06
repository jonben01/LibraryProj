using System.ComponentModel.DataAnnotations;
using api.DTOs;
using api.DTOs.Requests;
using efscaffold.Entities;
using efscaffold;
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
        
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);
        
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
    
    public async Task<BookDto> UpdateBook(UpdateBookRequestDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);

        var bookToUpdate = context.Books.First(book => book.Id == dto.BookIdForUpdate);
        
        await context.Entry(bookToUpdate).Collection(book => book.Authors).LoadAsync();
        
        bookToUpdate.Title = dto.Title;
        bookToUpdate.Pages = dto.Pages;
        
        //can't decide if I prefer using operators over verbose, but readable, code.
        if (dto.GenreId is not null)
        {
            bookToUpdate.Genre = context.Genres.FirstOrDefault(genre => genre.Id == dto.GenreId) ??
                                 throw new ArgumentException("Genre id not found");
        }

        bookToUpdate.Authors.Clear();
        dto.AuthorIds.ForEach(id => bookToUpdate.Authors.Add(context.Authors.First(author => author.Id == id)));

        await context.SaveChangesAsync();
        return new BookDto(bookToUpdate);
    }

    public async Task DeleteBook(string bookId)
    {
        var bookToDelete = context.Books.FirstOrDefault(book => book.Id == bookId);
        if (bookToDelete is null)
        {
            throw new InvalidOperationException($"Book with id {bookId} doesn't exist");
        }
        context.Books.Remove(bookToDelete);
        await context.SaveChangesAsync();
    }
}