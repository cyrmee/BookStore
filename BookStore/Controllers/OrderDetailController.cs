using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService;

    public OrderDetailController(IOrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDetailDto>>> GetAllOrderDetails()
    {
        var orderDetails = await _orderDetailService.GetAllAsync();
        return Ok(orderDetails);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDetailDto>> GetOrderDetail(Guid id)
    {
        var orderDetail = await _orderDetailService.GetOrderDetail(id);
        return orderDetail == null ? throw new NotFoundException($"OrderDetail with {id} is not found!") : (ActionResult<OrderDetailDto>)Ok(orderDetail);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrderDetail(OrderDetailDto orderDetailDto)
    {
        await _orderDetailService.Add(orderDetailDto);
        return CreatedAtAction(nameof(AddOrderDetail), "OrderDetail added successfully!");
    }

    [HttpPost("range")]
    public async Task<IActionResult> AddOrderDetailRange(List<OrderDetailDto> orderDetailDtos)
    {
        await _orderDetailService.AddRange(orderDetailDtos);
        return CreatedAtAction(nameof(AddOrderDetailRange), "OrderDetails added successfully!");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrderDetail(OrderDetailDto orderDetailDto)
    {
        await _orderDetailService.Update(orderDetailDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderDetail(Guid id)
    {
        _ = await _orderDetailService.GetOrderDetail(id) ?? throw new NotFoundException($"OrderDetail with {id} is not found!");
        await _orderDetailService.Delete(id);
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteOrderDetailRange(List<OrderDetailDto> orderDetailDtos)
    {
        foreach (var orderDetailDto in orderDetailDtos)
            _ = await _orderDetailService.GetOrderDetail(orderDetailDto.Id)
                ?? throw new NotFoundException($"OrderDetail with ID {orderDetailDto.Id} is not found!");

        await _orderDetailService.DeleteRange(orderDetailDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetOrderDetailCount()
    {
        var count = await _orderDetailService.Count();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<OrderDetailDto>>> GetPaginatedOrderDetails([FromQuery] int page, [FromQuery] int pageSize)
    {
        var orderDetails = await _orderDetailService.GetPaginated(page, pageSize);
        return Ok(orderDetails);
    }
}
