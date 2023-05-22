using AutoMapper;
using Application.DTOs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface ICategoryService
{
    public Task<List<CategoryDto>> GetAllAsync();
    public Task<CategoryDto> GetCategory(Guid id);
    public Task Add(CategoryDto category);
    public Task AddRange(List<CategoryDto> categories);
    public Task Update(CategoryDto category);
    public Task Delete(Guid id);
    public Task DeleteRange(List<CategoryDto> categories);

    public Task<int> Count();
    public Task<List<CategoryDto>> GetPaginated(int page, int pageSize);
}

public class CategoryService : ICategoryService
{
    public CategoryService(IRepository repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _repository.Category!.GetAllAsync();
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> GetCategory(Guid id)
    {
        var category = await _repository.Category!.FindByCondition(m => m.Id == id, false).FirstOrDefaultAsync();
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task Add(CategoryDto categoryDto)
    {
        _repository.Category!.Add(_mapper.Map<Category>(categoryDto));
        await _repository.SaveAsync();
    }

    public async Task AddRange(List<CategoryDto> categoryDtos)
    {
        _repository.Category!.AddRange(_mapper.Map<List<Category>>(categoryDtos));
        await _repository.SaveAsync();
    }

    public async Task Update(CategoryDto categoryDto)
    {
        _repository.Category!.Update(_mapper.Map<Category>(categoryDto));
        await _repository.SaveAsync();
    }

    public async Task Delete(Guid id)
    {
        var categoryDto = await GetCategory(id);
        _repository.Category!.Delete(_mapper.Map<Category>(categoryDto));
        await _repository.SaveAsync();
    }

    public async Task DeleteRange(List<CategoryDto> categoryDtos)
    {
        _repository.Category!.DeleteRange(_mapper.Map<List<Category>>(categoryDtos));
        await _repository.SaveAsync();
    }

    public async Task<int> Count() => await _repository.Category!.Count();

    public async Task<List<CategoryDto>> GetPaginated(int page, int pageSize)
    {
        var categories = await _repository.Category!.GetPaginated(page, pageSize).ToListAsync();
        return _mapper.Map<List<CategoryDto>>(categories);
    }
}
