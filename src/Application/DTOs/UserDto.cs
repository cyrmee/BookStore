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

public record UserLockDto(
    string UserName,
    double LockoutInDays
);

public record UserUnlockDto(
    string UserName
);

public record UserChangePasswordDto(
    string UserName,
    string OldPassword,
    string NewPassword
);

public record UserInfoDto(
    string UserName,
    string Email,
    string PhoneNumber
);