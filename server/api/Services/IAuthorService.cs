using api.DTOs;
using api.DTOs.Requests;
using efscaffold.Entities;

namespace api.Services;

public interface IAuthorService
{
    Task<List<AuthorDto>> GetAuthors();
    Task<AuthorDto> CreateAuthor(CreateAuthorRequestDto author);
    Task<AuthorDto> UpdateAuthor(UpdateAuthorRequestDto author);
    Task DeleteAuthor(string authorId);

}