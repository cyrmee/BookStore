namespace Application.DTOs;

public record BookDto
(
    Guid Id,
    string Title,
    string Author,
    string Publisher,
    DateTime PublicationDate,
    string Isbn10,
    string Isbn13,
    string Description,
    int Quantity,
    double Price);

public record BookWriteDto(
    string Title,
    string Author,
    string Publisher,
    DateTime PublicationDate,
    string Isbn10,
    string Isbn13,
    string Description,
    int Quantity,
    double Price);
