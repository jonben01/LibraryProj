using api.DTOs;
using api.DTOs.Requests;

namespace api.Services;

public interface IGenreService
{
    Task<List<GenreDto>> GetGenres();
    Task<GenreDto> CreateGenre(CreateGenreRequestDto genre);
    Task<GenreDto> UpdateGenre(UpdateGenreRequestDto genre);
    Task DeleteGenre(string genreId);
}