using Application.DTOs;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface IBookCategoryService
{
    public Task<List<BookCategoryDto>> GetAllAsync();
    public Task<BookCategoryDto> GetBookCategory(Guid id);
    public Task Add(BookCategoryWriteDto bookCategory);
    public Task AddRange(List<BookCategoryWriteDto> bookCategories);
    public Task Update(BookCategoryDto bookCategory);
    public Task Delete(Guid id);
    public Task DeleteRange(List<BookCategoryDto> bookCategories);

    public Task<int> Count();
    public Task<List<BookCategoryDto>> GetPaginated(int page, int pageSize);
}

public class BookCategoryService : IBookCategoryService
{
    public BookCategoryService(IRepository repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public async Task<List<BookCategoryDto>> GetAllAsync()
    {
        var bookCategories = await _repository.BookCategory!.GetAllAsync();
        return _mapper.Map<List<BookCategoryDto>>(bookCategories);
    }

    public async Task<BookCategoryDto> GetBookCategory(Guid id)
    {
        var bookCategory = await _repository.BookCategory!.FindByCondition(m => m.Id == id).FirstOrDefaultAsync();
        return _mapper.Map<BookCategoryDto>(bookCategory);
    }

    public async Task Add(BookCategoryWriteDto bookCategoryDto)
    {
        _repository.BookCategory!.Add(_mapper.Map<BookCategory>(bookCategoryDto));
        await _repository.SaveAsync();
    }

    public async Task AddRange(List<BookCategoryWriteDto> bookCategoryDtos)
    {
        _repository.BookCategory!.AddRange(_mapper.Map<List<BookCategory>>(bookCategoryDtos));
        await _repository.SaveAsync();
    }

    public async Task Update(BookCategoryDto bookCategoryDto)
    {
        _repository.BookCategory!.Update(_mapper.Map<BookCategory>(bookCategoryDto));
        await _repository.SaveAsync();
    }

    public async Task Delete(Guid id)
    {
        var bookCategoryDto = await GetBookCategory(id);
        _repository.BookCategory!.Delete(_mapper.Map<BookCategory>(bookCategoryDto));
        await _repository.SaveAsync();
    }

    public async Task DeleteRange(List<BookCategoryDto> bookCategoryDtos)
    {
        _repository.BookCategory!.DeleteRange(_mapper.Map<List<BookCategory>>(bookCategoryDtos));
        await _repository.SaveAsync();
    }

    public async Task<int> Count() => await _repository.BookCategory!.Count();

    public async Task<List<BookCategoryDto>> GetPaginated(int page, int pageSize)
    {
        var bookCategories = await _repository.BookCategory!.GetPaginated(page, pageSize).ToListAsync();
        return _mapper.Map<List<BookCategoryDto>>(bookCategories);
    }
}
