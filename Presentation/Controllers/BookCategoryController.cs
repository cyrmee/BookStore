using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookCategoryController : ControllerBase
{
    private readonly IBookCategoryService _bookCategoryService;

    public BookCategoryController(IBookCategoryService bookCategoryService)
    {
        _bookCategoryService = bookCategoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookCategoryDto>>> GetAllBookCategories()
    {
        var bookCategories = await _bookCategoryService.GetAllAsync();
        return Ok(bookCategories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookCategoryDto>> GetBookCategory(Guid id)
    {
        var bookCategory = await _bookCategoryService.GetBookCategory(id);
        return bookCategory == null ? throw new NotFoundException($"Book category with ID {id} is not found!") : (ActionResult<BookCategoryDto>)Ok(bookCategory);
    }

    [HttpPost]
    public async Task<IActionResult> AddBookCategory(BookCategoryWriteDto bookCategoryDto)
    {
        await _bookCategoryService.Add(bookCategoryDto);
        return CreatedAtAction(nameof(AddBookCategory), "Book category added successfully!");
    }

    [HttpPost("range")]
    public async Task<IActionResult> AddBookCategoryRange(List<BookCategoryWriteDto> bookCategoryDtos)
    {
        await _bookCategoryService.AddRange(bookCategoryDtos);
        return CreatedAtAction(nameof(AddBookCategoryRange), "Book categories added successfully!");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBookCategory(BookCategoryDto bookCategoryDto)
    {
        await _bookCategoryService.Update(bookCategoryDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBookCategory(Guid id)
    {
        _ = await _bookCategoryService.GetBookCategory(id) ?? throw new NotFoundException($"Book category with ID {id} is not found!");
        await _bookCategoryService.Delete(id);
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteBookCategoryRange(List<BookCategoryDto> bookCategoryDtos)
    {
        foreach (var bookCategoryDto in bookCategoryDtos)
            _ = await _bookCategoryService.GetBookCategory(bookCategoryDto.Id)
                ?? throw new NotFoundException($"Book category with ID {bookCategoryDto.Id} is not found!");

        await _bookCategoryService.DeleteRange(bookCategoryDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetBookCategoryCount()
    {
        var count = await _bookCategoryService.Count();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<BookCategoryDto>>> GetPaginatedBookCategories([FromQuery] int page, [FromQuery] int pageSize)
    {
        var bookCategories = await _bookCategoryService.GetPaginated(page, pageSize);
        return Ok(bookCategories);
    }
}
