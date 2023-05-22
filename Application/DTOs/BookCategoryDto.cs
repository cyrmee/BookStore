namespace Application.DTOs;

public record BookCategoryDto
(
    Guid Id,
    Guid BookId,
    Guid CategoryId
);

// Read DTO
public record BookCategoryReadDto(Guid Id, Guid BookId, Guid CategoryId);

// Write DTO
public record BookCategoryWriteDto(Guid BookId, Guid CategoryId);
