namespace Application.DTOs;

public record UserLoginDto(
    string UserName,
    string Password
);

public record UserSignupDto(
    string Email,
    string UserName,
    string PhoneNumber,
    string Password
);