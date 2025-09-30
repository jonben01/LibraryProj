using api.DTOs;
using api.DTOs.Requests;

namespace api.Services;

public interface IGenreService
{
    Task<List<GenreDto>> GetGenres();
    Task<GenreDto> CreateGenre(CreateGenreRequestDto dto);
    Task<GenreDto> UpdateGenre(UpdateGenreRequestDto dto);
    Task DeleteGenre(string genreId);
}