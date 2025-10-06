using api.DTOs;
using api.DTOs.Requests;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public class GenreController(IGenreService genreService) : ControllerBase
{
    [Route(nameof(GetAllGenres))]
    [HttpGet]
    public async Task<ActionResult<List<GenreDto>>> GetAllGenres()
    {
        return await genreService.GetGenres();
    }

    [Route(nameof(CreateGenre))]
    [HttpPost]
    public async Task<ActionResult<GenreDto>> CreateGenre([FromBody]CreateGenreRequestDto request)
    {
        return await genreService.CreateGenre(request);
    }

    [Route(nameof(UpdateGenre))]
    [HttpPut]
    public async Task<ActionResult<GenreDto>> UpdateGenre([FromBody] UpdateGenreRequestDto request)
    {
        return await genreService.UpdateGenre(request);
    }

    [Route(nameof(DeleteGenre))]
    [HttpDelete]
    public async Task<ActionResult<GenreDto>> DeleteGenre(string id)
    {
        await genreService.DeleteGenre(id);
        return NoContent();
    }
    
}