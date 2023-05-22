namespace Application.DTOs;

public record CategoryDto
(
    Guid Id,
    string Name
);

// Read DTO
public record CategoryReadDto(Guid Id, string Name);

// Write DTO
public record CategoryWriteDto(string Name);
