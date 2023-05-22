using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAllBooks()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookDto>> GetBook(Guid id)
    {
        var book = await _bookService.GetBook(id);
        return book == null ? throw new NotFoundException($"Book with {id} is not found!") : (ActionResult<BookDto>)Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(BookWriteDto bookDto)
    {
        await _bookService.Add(bookDto);
        return CreatedAtAction(nameof(AddBook), "Book added successfully!");
    }

    [HttpPost("range")]
    public async Task<IActionResult> AddBookRange(List<BookWriteDto> bookDtos)
    {
        await _bookService.AddRange(bookDtos);
        return CreatedAtAction(nameof(AddBookRange), "Books added successfully!");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBook(BookDto bookDto)
    {
        await _bookService.Update(bookDto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        _ = await _bookService.GetBook(id) ?? throw new NotFoundException($"Book with ID {id} is not found!");
        await _bookService.Delete(id);
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteBookRange(List<BookDto> bookDtos)
    {
        foreach (var bookDto in bookDtos)
            _ = await _bookService.GetBook(bookDto.Id)
                ?? throw new NotFoundException($"Book with ID {bookDto.Id} is not found!");

        await _bookService.DeleteRange(bookDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetBookCount()
    {
        var count = await _bookService.Count();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<BookDto>>> GetPaginatedBooks([FromQuery] int page, [FromQuery] int pageSize)
    {
        var books = await _bookService.GetPaginated(page, pageSize);
        return Ok(books);
    }
}
