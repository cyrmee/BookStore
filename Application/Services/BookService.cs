using AutoMapper;
using Application.DTOs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface IBookService
{
    public Task<List<BookDto>> GetAllAsync();
    public Task<BookDto> GetBook(Guid id);
    public Task Add(BookWriteDto book);
    public Task AddRange(List<BookWriteDto> books);
    public Task Update(BookDto book);
    public Task Delete(Guid id);
    public Task DeleteRange(List<BookDto> books);

    public Task<int> Count();
    public Task<List<BookDto>> GetPaginated(int page, int pageSize);
}

public class BookService : IBookService
{
    public BookService(IRepository repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public async Task<List<BookDto>> GetAllAsync()
    {
        var books = await _repository.Book!.GetAllAsync();
        return _mapper.Map<List<BookDto>>(books);
    }

    public async Task<BookDto> GetBook(Guid id)
    {
        var book = await _repository.Book!.FindByCondition(m => m.Id == id).FirstOrDefaultAsync();
        return _mapper.Map<BookDto>(book);
    }

    public async Task Add(BookWriteDto bookDto)
    {
        _repository.Book!.Add(_mapper.Map<Book>(bookDto));
        await _repository.SaveAsync();
    }

    public async Task AddRange(List<BookWriteDto> bookDtos)
    {
        _repository.Book!.AddRange(_mapper.Map<List<Book>>(bookDtos));
        await _repository.SaveAsync();
    }

    public async Task Update(BookDto bookDto)
    {
        _repository.Book!.Update(_mapper.Map<Book>(bookDto));
        await _repository.SaveAsync();
    }

    public async Task Delete(Guid id)
    {
        var bookDto = await GetBook(id);
        _repository.Book!.Delete(_mapper.Map<Book>(bookDto));
        await _repository.SaveAsync();
    }

    public async Task DeleteRange(List<BookDto> bookDtos)
    {
        _repository.Book!.DeleteRange(_mapper.Map<List<Book>>(bookDtos));
        await _repository.SaveAsync();
    }

    public async Task<int> Count() => await _repository.Book!.Count();

    public async Task<List<BookDto>> GetPaginated(int page, int pageSize)
    {
        var books = await _repository.Book!.GetPaginated(page, pageSize).ToListAsync();
        return _mapper.Map<List<BookDto>>(books);
    }
}

