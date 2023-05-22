namespace Application.DTOs;

public record OrderDetailDto
(
    Guid Id,
    Guid OrderId,
    Guid BookId,
    int Quantity,
    double Price
);

public record OrderDetailWriteDto(Guid OrderId, Guid BookId, int Quantity, double Price);
