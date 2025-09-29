using api.DTOs;
using api.DTOs.Requests;

namespace api.Services;

public interface IBookService
{
    Task<List<BookDto>> GetBooks();
    Task<BookDto> CreateBook(CreateBookRequestDto book);
    Task<BookDto> UpdateBook(UpdateBookRequestDto book);
    Task<BookDto> DeleteBook(string id);
    
}