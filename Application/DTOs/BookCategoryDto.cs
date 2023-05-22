namespace Application.DTOs;

public record BookCategoryDto
(
    Guid Id,
    Guid BookId,
    Guid CategoryId
);

public record BookCategoryWriteDto(Guid BookId, Guid CategoryId);
