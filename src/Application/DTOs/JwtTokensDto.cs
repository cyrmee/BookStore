namespace Application.DTOs;

public record JwtTokensDto
(
    Guid Id,
    string UserName,
    string TokenValue,
    DateTime ExpirationDate,
    DateTime RevocationDate,
    bool IsRevoked
);