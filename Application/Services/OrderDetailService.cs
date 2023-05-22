using Application.DTOs;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface IOrderDetailService
{
    public Task<List<OrderDetailDto>> GetAllAsync();
    public Task<OrderDetailDto> GetOrderDetail(Guid id);
    public Task Add(OrderDetailWriteDto orderDetail);
    public Task AddRange(List<OrderDetailWriteDto> orderDetails);
    public Task Update(OrderDetailDto orderDetail);
    public Task Delete(Guid id);
    public Task DeleteRange(List<OrderDetailDto> orderDetails);

    public Task<int> Count();
    public Task<List<OrderDetailDto>> GetPaginated(int page, int pageSize);
}

public class OrderDetailService : IOrderDetailService
{
    public OrderDetailService(IRepository repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public async Task<List<OrderDetailDto>> GetAllAsync()
    {
        var orderDetails = await _repository.OrderDetail!.GetAllAsync();
        return _mapper.Map<List<OrderDetailDto>>(orderDetails);
    }

    public async Task<OrderDetailDto> GetOrderDetail(Guid id)
    {
        var orderDetail = await _repository.OrderDetail!.FindByCondition(m => m.Id == id, false).FirstOrDefaultAsync();
        return _mapper.Map<OrderDetailDto>(orderDetail);
    }

    public async Task Add(OrderDetailWriteDto orderDetailDto)
    {
        _repository.OrderDetail!.Add(_mapper.Map<OrderDetail>(orderDetailDto));
        await _repository.SaveAsync();
    }

    public async Task AddRange(List<OrderDetailWriteDto> orderDetailDtos)
    {
        _repository.OrderDetail!.AddRange(_mapper.Map<List<OrderDetail>>(orderDetailDtos));
        await _repository.SaveAsync();
    }

    public async Task Update(OrderDetailDto orderDetailDto)
    {
        _repository.OrderDetail!.Update(_mapper.Map<OrderDetail>(orderDetailDto));
        await _repository.SaveAsync();
    }

    public async Task Delete(Guid id)
    {
        var orderDetailDto = await GetOrderDetail(id);
        _repository.OrderDetail!.Delete(_mapper.Map<OrderDetail>(orderDetailDto));
        await _repository.SaveAsync();
    }

    public async Task DeleteRange(List<OrderDetailDto> orderDetailDtos)
    {
        _repository.OrderDetail!.DeleteRange(_mapper.Map<List<OrderDetail>>(orderDetailDtos));
        await _repository.SaveAsync();
    }

    public async Task<int> Count() => await _repository.OrderDetail!.Count();

    public async Task<List<OrderDetailDto>> GetPaginated(int page, int pageSize)
    {
        var orderDetails = await _repository.OrderDetail!.GetPaginated(page, pageSize).ToListAsync();
        return _mapper.Map<List<OrderDetailDto>>(orderDetails);
    }
}

