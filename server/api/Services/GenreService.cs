using System.ComponentModel.DataAnnotations;
using api.DTOs;
using api.DTOs.Requests;
using efscaffold.Entities;
using efscaffold;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class GenreService(MyDbContext context) : IGenreService
{
    public Task<List<GenreDto>> GetGenres()
    {
        return context.Genres
            .Include(genre => genre.Books)
            .Select(genre => new GenreDto(genre))
            .ToListAsync();
    }

    public async Task<GenreDto> CreateGenre(CreateGenreRequestDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);

        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.Name,
            Createdat = DateTime.UtcNow,
        };
        
        context.Genres.Add(genre);
        await context.SaveChangesAsync();
        return new  GenreDto(genre);
    }

    public async Task<GenreDto> UpdateGenre(UpdateGenreRequestDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);
        
        var genreToUpdate = context.Genres.First(genre => genre.Id == dto.GenreIdForUpdate);
     
        genreToUpdate.Name = dto.Name;
        
        await context.SaveChangesAsync();
        return new  GenreDto(genreToUpdate);
    }

    public async Task DeleteGenre(string genreId)
    {
        var genreToDelete = context.Genres.FirstOrDefault(genre => genre.Id == genreId)
                            ?? throw new ArgumentException("Genre not found");
        
        context.Genres.Remove(genreToDelete);
        await context.SaveChangesAsync();
    }
}