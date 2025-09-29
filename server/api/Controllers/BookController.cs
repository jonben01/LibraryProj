using api.DTOs;
using api.DTOs.Requests;
using api.Services;
using efscaffold.Entities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;


[ApiController]
public class BookController(IBookService bookService) : ControllerBase
{
    [Route(nameof(GetAllBooks))]
    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAllBooks()
    {
        return await bookService.GetBooks();
    }

    [Route(nameof(CreateBook))]
    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookRequestDto request)
    {
        return await bookService.CreateBook(request);
    }

    [Route(nameof(UpdateBook))]
    [HttpPut]
    public async Task<ActionResult<BookDto>> UpdateBook([FromBody] UpdateBookRequestDto request)
    {
        return await bookService.UpdateBook(request);
    }

    [Route(nameof(DeleteBook))]
    [HttpDelete]
    public async Task<ActionResult<BookDto>> DeleteBook(string id)
    {
        return await bookService.DeleteBook(id);
    }
    
}