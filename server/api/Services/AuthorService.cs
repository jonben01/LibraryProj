using System.ComponentModel.DataAnnotations;
using api.DTOs;
using api.DTOs.Requests;
using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class AuthorService(MyDbContext context) : IAuthorService
{
    public Task<List<AuthorDto>> GetAuthors()
    {
        return context.Authors
            .Include(author => author.Books)
            .ThenInclude(book => book.Genre)
            .Select(author => new AuthorDto(author))
            .ToListAsync();
    }

    public async Task<AuthorDto> CreateAuthor(CreateAuthorRequestDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);

        var author = new Author
        {
            Name = dto.Name,
            Createdat = DateTime.UtcNow,
            Id = Guid.NewGuid().ToString(),
        };
        
        context.Authors.Add(author);
        await context.SaveChangesAsync();
        return new AuthorDto(author);
    }

    public async Task<AuthorDto> UpdateAuthor(UpdateAuthorRequestDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);

        var authorToUpdate = context.Authors.First(author => author.Id == dto.AuthorIdForUpdate);
        
        await context
            .Entry(authorToUpdate)
            .Collection(author => author.Books)
            .LoadAsync();
        
        authorToUpdate.Books.Clear();
        
        dto.BookIds
            .ForEach(id => authorToUpdate.Books
            .Add(context.Books
                .First(book => book.Id == id)));
        
        authorToUpdate.Name = dto.Name;
        await context.SaveChangesAsync();
        return new AuthorDto(authorToUpdate);
    }

    public async Task DeleteAuthor(string authorId)
    {
        var authorToDelete = context.Authors
            .FirstOrDefault(author => author.Id == authorId);
        
        if (authorToDelete is null)
        {
            throw new ArgumentException($"Author with id {authorId} doesn't exist");
        }
        
        context.Authors.Remove(authorToDelete);
        await context.SaveChangesAsync();
    }
}