using Domain.Types;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record OrderDto
(
    Guid Id,
    string UserName,
    DateTime OrderDate,
    double TotalAmount,
    OrderStatus Status
)
{
    public DateTime OrderDate { get; set; } = OrderDate;
}

public record OrderReadDto(Guid Id, string UserName, DateTime OrderDate, double TotalAmount, OrderStatus Status);

public record OrderWriteDto(string UserName, DateTime OrderDate, double TotalAmount, OrderStatus Status);