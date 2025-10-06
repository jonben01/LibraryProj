using api.DTOs;
using api.DTOs.Requests;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
public class AuthorController(IAuthorService authorService)  : ControllerBase
{
    [Route(nameof(GetAllAuthors))]
    [HttpGet]
    public async Task<ActionResult<List<AuthorDto>>> GetAllAuthors()
    {
        return await authorService.GetAuthors();
    }

    [Route(nameof(CreateAuthor))]
    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor([FromBody] CreateAuthorRequestDto request)
    {
        return await authorService.CreateAuthor(request);
    }

    [Route(nameof(UpdateAuthor))]
    [HttpPut]
    public async Task<ActionResult<AuthorDto>> UpdateAuthor([FromBody] UpdateAuthorRequestDto request)
    {
        return await authorService.UpdateAuthor(request);
    }

    [Route(nameof(DeleteAuthor))]
    [HttpDelete]
    public async Task<ActionResult<AuthorDto>> DeleteAuthor(string id)
    {
        await authorService.DeleteAuthor(id);
        //return NoContent -- could be changed later if user should see a confirmation or similar.
        return NoContent();
    }
}