namespace Application.DTOs;

public record BookDto
(
    Guid Id,
    string Title,
    string Author,
    string Publisher,
    DateTime PublicationDate,
    string ISBN,
    string Description,
    int Quantity,
    double Price);

public record BookWriteDto(
    string Title,
    string Author,
    string Publisher,
    DateTime PublicationDate,
    string ISBN,
    string Description,
    int Quantity,
    double Price);
