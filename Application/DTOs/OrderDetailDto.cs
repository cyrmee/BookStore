namespace Application.DTOs;

public record OrderDetailDto
(
    Guid Id,
    Guid OrderId,
    Guid BookId,
    int Quantity,
    double Price
);

// Read DTO
public record OrderDetailReadDto(Guid Id, Guid OrderId, Guid BookId, int Quantity, double Price);

// Write DTO
public record OrderDetailWriteDto(Guid OrderId, Guid BookId, int Quantity, double Price);
