using api.DTOs;
using api.DTOs.Requests;
using efscaffold.Entities;

namespace api.Services;

public interface IAuthorService
{
    Task<List<AuthorDto>> GetAuthors();
    Task<AuthorDto> CreateAuthor(CreateAuthorRequestDto dto);
    Task<AuthorDto> UpdateAuthor(UpdateAuthorRequestDto dto);
    Task DeleteAuthor(string authorId);

}