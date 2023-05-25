using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
    {
        var order = await _orderService.GetOrder(id);
        return order == null ? throw new NotFoundException($"Order with {id} is not found!") : (ActionResult<OrderDto>)Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(OrderWriteDto orderDto)
    {
        await _orderService.Add(orderDto);
        return CreatedAtAction(nameof(AddOrder), "Order added successfully!");
    }

    [HttpPost("range")]
    public async Task<IActionResult> AddOrderRange(List<OrderWriteDto> orderDtos)
    {
        await _orderService.AddRange(orderDtos);
        return CreatedAtAction(nameof(AddOrderRange), "Orders added successfully!");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder(OrderDto orderDto)
    {
        await _orderService.Update(orderDto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        _ = await _orderService.GetOrder(id) ?? throw new NotFoundException($"Order with {id} is not found!");
        await _orderService.Delete(id);
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteOrderRange(List<OrderDto> orderDtos)
    {
        foreach (var orderDto in orderDtos)
            _ = await _orderService.GetOrder(orderDto.Id)
                ?? throw new NotFoundException($"Order with ID {orderDto.Id} is not found!");

        await _orderService.DeleteRange(orderDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetOrderCount()
    {
        var count = await _orderService.Count();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<OrderDto>>> GetPaginatedOrders([FromQuery] int page, [FromQuery] int pageSize)
    {
        var orders = await _orderService.GetPaginated(page, pageSize);
        return Ok(orders);
    }
}
