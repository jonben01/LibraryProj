using api.DTOs;
using api.DTOs.Requests;

namespace api.Services;

public class GenreService : IGenreService
{
    public Task<List<GenreDto>> GetGenres()
    {
        throw new NotImplementedException();
    }

    public Task<GenreDto> CreateGenre(CreateGenreRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<GenreDto> UpdateGenre(UpdateGenreRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGenre(string genreId)
    {
        throw new NotImplementedException();
    }
}