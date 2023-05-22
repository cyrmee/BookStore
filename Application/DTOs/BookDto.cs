using System.ComponentModel.DataAnnotations;

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
    double Price
)
{
    public DateTime PublicationDate { get; set; } = PublicationDate;
}

// Read DTO
public record BookReadDto(Guid Id, string Title, string Author, string Publisher, DateTime PublicationDate, string ISBN, string Description, int Quantity, double Price);

// Write DTO
public record BookWriteDto(string Title, string Author, string Publisher, DateTime PublicationDate, string ISBN, string Description, int Quantity, double Price);
