using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;
using Application.Services;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        var category = await _categoryService.GetCategory(id);
        return category == null ? throw new NotFoundException($"Category with {id} is not found!") : (ActionResult<CategoryDto>)Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryWriteDto categoryDto)
    {
        await _categoryService.Add(categoryDto);
        return CreatedAtAction(nameof(AddCategory), "Category added successfully!");
    }

    [HttpPost("range")]
    public async Task<IActionResult> AddCategoryRange(List<CategoryWriteDto> categoryDtos)
    {
        await _categoryService.AddRange(categoryDtos);
        return CreatedAtAction(nameof(AddCategoryRange), "Categories added successfully!");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
    {
        await _categoryService.Update(categoryDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        _ = await _categoryService.GetCategory(id) ?? throw new NotFoundException($"Category with {id} is not found!");
        await _categoryService.Delete(id);
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteCategoryRange(List<CategoryDto> categoryDtos)
    {
        foreach (var categoryDto in categoryDtos)
        {
            _ = await _categoryService.GetCategory(categoryDto.Id)
                ?? throw new NotFoundException($"Category with ID {categoryDto.Id} is not found!");
        }

        await _categoryService.DeleteRange(categoryDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCategoryCount()
    {
        var count = await _categoryService.Count();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<CategoryDto>>> GetPaginatedCategories([FromQuery] int page, [FromQuery] int pageSize)
    {
        var categories = await _categoryService.GetPaginated(page, pageSize);
        return Ok(categories);
    }
}
