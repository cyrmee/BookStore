using Application.DTOs;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface IOrderService
{
    public Task<List<OrderDto>> GetAllAsync();
    public Task<OrderDto> GetOrder(Guid id);
    public Task Add(OrderDto order);
    public Task AddRange(List<OrderDto> orders);
    public Task Update(OrderDto order);
    public Task Delete(Guid id);
    public Task DeleteRange(List<OrderDto> orders);

    public Task<int> Count();
    public Task<List<OrderDto>> GetPaginated(int page, int pageSize);
}

public class OrderService : IOrderService
{
    public OrderService(IRepository repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public async Task<List<OrderDto>> GetAllAsync()
    {
        var orders = await _repository.Order!.GetAllAsync();
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<OrderDto> GetOrder(Guid id)
    {
        var order = await _repository.Order!.FindByCondition(m => m.Id == id, false).FirstOrDefaultAsync();
        return _mapper.Map<OrderDto>(order);
    }

    public async Task Add(OrderDto orderDto)
    {
        _repository.Order!.Add(_mapper.Map<Order>(orderDto));
        await _repository.SaveAsync();
    }

    public async Task AddRange(List<OrderDto> orderDtos)
    {
        _repository.Order!.AddRange(_mapper.Map<List<Order>>(orderDtos));
        await _repository.SaveAsync();
    }

    public async Task Update(OrderDto orderDto)
    {
        _repository.Order!.Update(_mapper.Map<Order>(orderDto));
        await _repository.SaveAsync();
    }

    public async Task Delete(Guid id)
    {
        var orderDto = await GetOrder(id);
        _repository.Order!.Delete(_mapper.Map<Order>(orderDto));
        await _repository.SaveAsync();
    }

    public async Task DeleteRange(List<OrderDto> orderDtos)
    {
        _repository.Order!.DeleteRange(_mapper.Map<List<Order>>(orderDtos));
        await _repository.SaveAsync();
    }

    public async Task<int> Count() => await _repository.Order!.Count();

    public async Task<List<OrderDto>> GetPaginated(int page, int pageSize)
    {
        var orders = await _repository.Order!.GetPaginated(page, pageSize).ToListAsync();
        return _mapper.Map<List<OrderDto>>(orders);
    }
}
